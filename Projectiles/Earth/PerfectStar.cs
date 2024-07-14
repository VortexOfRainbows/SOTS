using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Diagnostics.Metrics;
using System.IO;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
    public class PerfectStar : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 40;
            Projectile.hide = true;
            Projectile.alpha = 255;
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ended);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ended = reader.ReadBoolean();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.hide)
                return false;
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/PerfectStarGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            for (int i = 0; i < chargeLevel; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    Vector2 c = new Vector2(2 + i * 2, 0).RotatedBy(j * MathHelper.PiOver2 + MathHelper.ToRadians(SOTSWorld.GlobalCounter * (i * 2 - 1)) * Projectile.direction);
                    Main.spriteBatch.Draw(texture, drawPos + c, null, new Color(100, 100, 100, 0) * (1 / (i + 1f)), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
                }
            }
        }
        bool ended = false;
        int chargeLevel = 0;
        public override void OnKill(int timeLeft)
        {
            DoDust(0.7f + 0.1f * chargeLevel, 2 + chargeLevel);
            if (chargeLevel != 0)
                SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarShot" + chargeLevel), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, -0.1f);
            else
                SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarShot1"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, 0.4f);
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
                if(chargeLevel == 0)
                {
                    int size = 10;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 0.5f, ModContent.ProjectileType<PerfectStarLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, size);
                }
                else
                {
                    int size = 28 + chargeLevel * 24;
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), fireFrom, Projectile.velocity * 0.3f, ModContent.ProjectileType<PerfectStarLaser2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, size, -chargeLevel);
                }
            }
        }
        public override void AI()
        {
            int chargeTime = (int)Projectile.ai[0];
            if(chargeLevel < 3)
            {
                Projectile.ai[1]++;
                if (Projectile.ai[1] > chargeTime)
                {
                    if(chargeLevel != 2)
                        SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarCharge"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, 0.1f * chargeLevel);
                    else
                        SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/PerfectStarFull"), (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, 0);
                    chargeLevel++;
                    Projectile.ai[1] = -6 * chargeLevel;
                }
            }
        }
        public void DoDust(float scale, float numSpikes)
        {
            Vector2 dustCenter = Projectile.Center + new Vector2(37, 1 * Projectile.direction).RotatedBy(Projectile.rotation - MathHelper.PiOver2);
            Vector2 startingLocation = dustCenter;
            Vector2 presumedVelo = Projectile.position - Projectile.oldPosition;
            for (int j = 0; j < numSpikes; j++)
            {
                Vector2 offset = new Vector2(0, 14 * scale -(5 - numSpikes)).RotatedBy(MathHelper.ToRadians(j * 360f / numSpikes) + Projectile.rotation + MathHelper.PiOver2 * Projectile.direction);
                for (int i = -10; i < 10; i++)
                {
                    startingLocation = new Vector2(i * scale * 0.825f, (20 - Math.Abs(i) * 2) * scale).RotatedBy(MathHelper.ToRadians(j * 360f / numSpikes) + Projectile.rotation + MathHelper.PiOver2 * Projectile.direction);
                    Vector2 velo = offset + startingLocation;
                    Dust dust = Dust.NewDustPerfect(dustCenter + velo * 1f, ModContent.DustType<CopyDust4>());
                    dust.noGravity = true;
                    dust.scale = 1.1f;
                    dust.fadeIn = 0.1f;
                    dust.color = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
                    dust.velocity = velo * 0.06f * scale + presumedVelo * 0.25f;
                }
            }
        }
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.channel && (Projectile.ai[1] > 6 || chargeLevel > 0))
                ended = true;
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                Projectile.timeLeft = 2;
            }
            Vector2 center = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float offsetDist = 24 * Projectile.scale;
                Vector2 toMouse = Main.MouseWorld - center;
                toMouse = toMouse.SafeNormalize(Vector2.Zero) * offsetDist;
                if ((double)toMouse.X != Projectile.velocity.X || (double)toMouse.Y != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity = toMouse;
            }
            Projectile.rotation = (float)(Projectile.velocity.ToRotation() + MathHelper.PiOver2);
            if (Main.player[Projectile.owner].channel || Projectile.timeLeft > 50)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation + MathHelper.ToRadians(Projectile.direction == -1 ? 90 : -90));
            }
            Projectile.hide = false;
            Projectile.alpha = 0;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.Center = center;
            return true;
        }
    }
}