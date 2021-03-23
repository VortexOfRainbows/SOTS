using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{
    public class DoomstickHoldOut : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doomstick");
        }
        public override void SetDefaults()
        {
            projectile.width = 20;
            projectile.height = 58;
            projectile.aiStyle = 20;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.ranged = true;
            projectile.timeLeft = 20;
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
        bool ended = false;
        public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            if ((player.ownedProjectileCounts[ModContent.ProjectileType<Doomhook>()] < 1) && Main.myPlayer == projectile.owner && projectile.timeLeft <= 2)
            {
                ended = true;
                projectile.netUpdate = true;
            }
            if (!ended && projectile.timeLeft < 2)
                projectile.timeLeft = 2;
            Vector2 vector2_1 = player.RotatedRelativePoint(player.MountedCenter, true);
            if (Main.myPlayer == projectile.owner)
            {
                float num1 =  projectile.velocity.Length() * projectile.scale;
                Vector2 vector2_2 = vector2_1;
                float num2 = (float)((double)Main.mouseX + Main.screenPosition.X - vector2_2.X);
                float num3 = (float)((double)Main.mouseY + Main.screenPosition.Y - vector2_2.Y);
                if (player.gravDir == -1.0)
                    num3 = (float)((double)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector2_2.Y);
                float num5 = (float)Math.Sqrt(num2 * (double)num2 + num3 * (double)num3);
                float num6 = num1 / num5;
                float num7 = num2 * num6;
                float num8 = num3 * num6;

                if ((double)num7 != projectile.velocity.X || (double)num8 != projectile.velocity.Y)
                    projectile.netUpdate = true;
                projectile.velocity.X = num7;
                projectile.velocity.Y = num8;
                projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[0]);
            }
            if (projectile.hide == false)
            {
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = (projectile.velocity * projectile.direction).ToRotation();
                if (!ended && player.itemTime < 2)
                {
                    player.itemTime = 2;
                    player.itemAnimation = 2;
                }
                projectile.alpha = 0;
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.position += projectile.velocity;
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57000005245209);
            return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
    }
}