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
            projectile.width = 30;
            projectile.height = 68;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 40;
            projectile.hide = true;
            projectile.alpha = 255;
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
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Earth/PerfectStarGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Color color = Color.White;
            spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        bool ended = false;
        int chargeLevel = 0;
        public override void Kill(int timeLeft)
        {
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 fireFrom = projectile.Center + projectile.velocity.SafeNormalize(Vector2.Zero) * 32;
                if(chargeLevel != 0)
                    Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarShot" + chargeLevel), 1.6f, -0.1f);
                else
                    Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarShot1"), 1.6f, 0.4f);
                if(chargeLevel == 0)
                {
                    int size = 10;
                    Projectile.NewProjectileDirect(fireFrom, projectile.velocity * 0.5f, ModContent.ProjectileType<PerfectStarLaser>(), projectile.damage, projectile.knockBack, Main.myPlayer, size);
                }
                else
                {
                    int size = 28 + chargeLevel * 24;
                    Projectile.NewProjectileDirect(fireFrom, projectile.velocity * 0.3f, ModContent.ProjectileType<PerfectStarLaser2>(), projectile.damage, projectile.knockBack, Main.myPlayer, size, -chargeLevel);
                }
            }
        }
        public override void AI()
        {
            int chargeTime = (int)projectile.ai[0];
            if(chargeLevel < 3)
            {
                projectile.ai[1]++;
                if (projectile.ai[1] > chargeTime)
                {
                    if(chargeLevel != 2)
                        Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarCharge"), 1.6f, 0.1f * chargeLevel);
                    else
                        Main.PlaySound(SoundLoader.customSoundType, (int)projectile.Center.X, (int)projectile.Center.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/PerfectStarFull"), 1.6f, 0);
                    chargeLevel++;
                    DoDust(0.7f + 0.1f * chargeLevel, 2 + chargeLevel);
                    projectile.ai[1] = -6 * chargeLevel;
                }
            }
        }
        public void DoDust(float scale, float numSpikes)
        {
            Vector2 dustCenter = projectile.Center + new Vector2(37, 1 * projectile.direction).RotatedBy(projectile.rotation - MathHelper.PiOver2);
            Vector2 startingLocation = dustCenter;
            Vector2 presumedVelo = projectile.position - projectile.oldPosition;
            for (int j = 0; j < numSpikes; j++)
            {
                Vector2 offset = new Vector2(0, 16 * scale -(5 - numSpikes)).RotatedBy(MathHelper.ToRadians(j * 360f / numSpikes) + projectile.rotation + MathHelper.PiOver2 * projectile.direction);
                for (int i = -10; i < 10; i++)
                {
                    startingLocation = new Vector2(i * scale, (20 - Math.Abs(i) * 2) * scale).RotatedBy(MathHelper.ToRadians(j * 360f / numSpikes) + projectile.rotation + MathHelper.PiOver2 * projectile.direction);
                    Vector2 velo = offset + startingLocation;
                    Dust dust = Dust.NewDustPerfect(dustCenter + velo * 1f, ModContent.DustType<CopyDust4>());
                    dust.noGravity = true;
                    dust.scale = 1.1f;
                    dust.fadeIn = 0.1f;
                    dust.color = Color.Lerp(new Color(175, 218, 118, 0), new Color(74, 186, 54, 0), Main.rand.NextFloat(1));
                    dust.velocity = -velo * 0.105f * scale + presumedVelo * 0.25f;
                }
            }
        }
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            if (!player.channel && (projectile.ai[1] > 6 || chargeLevel > 0))
                ended = true;
            if (!ended)
            {
                player.itemTime = 2;
                player.itemAnimation = 2;
                projectile.timeLeft = 2;
            }
            Vector2 vector2_1 = player.RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                float num1 = 20 * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if ((double)Main.player[projectile.owner].gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num4 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
                projectile.velocity = projectile.velocity;
            }
            if (projectile.hide == false)
            {
                Main.player[projectile.owner].heldProj = projectile.whoAmI;
                projectile.alpha = 0;
            }
            projectile.rotation = (float)(projectile.velocity.ToRotation() + MathHelper.PiOver2);
            if (Main.player[projectile.owner].channel || projectile.timeLeft > 50)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(projectile.rotation + MathHelper.ToRadians(projectile.direction == -1 ? 90 : -90));
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            return true;
        }
    }
}