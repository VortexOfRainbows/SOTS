using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.IO;
using Terraria;
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
            Projectile.DamageType = DamageClass.Ranged;
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
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/PerfectStarGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        bool ended = false;
        int chargeLevel = 0;
        public override void Kill(int timeLeft)
        {
            DoDust(0.7f + 0.1f * chargeLevel, 2 + chargeLevel);
            if (chargeLevel != 0)
                SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarShot" + chargeLevel), 1.2f, -0.1f);
            else
                SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarShot1"), 1.2f, 0.4f);
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 fireFrom = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
                if(chargeLevel == 0)
                {
                    int size = 10;
                    Projectile.NewProjectileDirect(fireFrom, Projectile.velocity * 0.5f, ModContent.ProjectileType<PerfectStarLaser>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, size);
                }
                else
                {
                    int size = 28 + chargeLevel * 24;
                    Projectile.NewProjectileDirect(fireFrom, Projectile.velocity * 0.3f, ModContent.ProjectileType<PerfectStarLaser2>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, size, -chargeLevel);
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
                        SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarCharge"), 1.2f, 0.1f * chargeLevel);
                    else
                        SoundEngine.PlaySound(SoundLoader.customSoundType, (int)Projectile.Center.X, (int)Projectile.Center.Y, Mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarFull"), 1.2f, 0);
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
            Vector2 vector2_1 = player.RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
            if (Main.myPlayer == Projectile.owner)
            {
                float num1 = 20 * Projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[Projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != Projectile.velocity.X || (double)num8 != Projectile.velocity.Y)
                    Projectile.netUpdate = true;
                Projectile.velocity.X = num7;
                Projectile.velocity.Y = num8;
                Projectile.velocity = Projectile.velocity;
            }
            if (Projectile.hide == false)
            {
                Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
                Projectile.alpha = 0;
            }
            Projectile.rotation = (float)(Projectile.velocity.ToRotation() + MathHelper.PiOver2);
            if (Main.player[Projectile.owner].channel || Projectile.timeLeft > 50)
            {
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation + MathHelper.ToRadians(Projectile.direction == -1 ? 90 : -90));
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            return true;
        }
    }
}