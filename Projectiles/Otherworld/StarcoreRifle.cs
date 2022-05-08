using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	
	public class StarcoreRifle : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcore Rifle");
		}
		public override void SetDefaults() 
		{
			projectile.width = 30;
			projectile.height = 70;
            projectile.aiStyle = 14;
			projectile.friendly = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.ranged = true;
            projectile.timeLeft = 60;
            projectile.hide = true;
            projectile.alpha = 255;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
			for (int k = 0; k < 2; k++)
                spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect2");
            for (int k = 0; k < 2; k++)
                spriteBatch.Draw(texture, drawPos, null, new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 0), projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect3");
            for (int k = 0; k < 2; k++)
                spriteBatch.Draw(texture, drawPos, null, new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 0), projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            color = Color.White;
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleGlow");
            spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
		public override bool PreAI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.timeLeft > projectile.damage)
            {
                projectile.timeLeft = (int)projectile.damage;
            }
            Vector2 vector2_1 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter, true);
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
                projectile.velocity = projectile.velocity.RotatedBy(projectile.ai[0]);
            }
            if (projectile.hide == false)
            {
                projectile.alpha = 0;
                player.ChangeDir(projectile.direction);
                player.heldProj = projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(projectile.rotation + MathHelper.ToRadians(projectile.direction == -1 ? 90 : -90));
            }
            projectile.hide = false;
            projectile.spriteDirection = projectile.direction;
            projectile.position.X = (vector2_1.X - (projectile.width / 2));
            projectile.position.Y = (vector2_1.Y - (projectile.height / 2));
            projectile.rotation = (float)(Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57000005245209);
            return false;
        }
	}
}