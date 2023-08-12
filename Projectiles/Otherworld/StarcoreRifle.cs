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
			// DisplayName.SetDefault("Starcore Rifle");
		}
		public override void SetDefaults() 
		{
			Projectile.width = 30;
			Projectile.height = 70;
            Projectile.aiStyle = 14;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 60;
            Projectile.hide = true;
            Projectile.alpha = 255;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Color color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
			for (int k = 0; k < 2; k++)
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect2");
            for (int k = 0; k < 2; k++)
                Main.spriteBatch.Draw(texture, drawPos, null, new Color(Main.DiscoG, Main.DiscoB, Main.DiscoR, 0), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleEffect3");
            for (int k = 0; k < 2; k++)
                Main.spriteBatch.Draw(texture, drawPos, null, new Color(Main.DiscoB, Main.DiscoR, Main.DiscoG, 0), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            color = Color.White;
            texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/StarcoreRifleGlow");
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
		public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (Projectile.timeLeft > Projectile.damage)
            {
                Projectile.timeLeft = (int)Projectile.damage;
            }
            Vector2 vector2_1 = Main.player[Projectile.owner].RotatedRelativePoint(Main.player[Projectile.owner].MountedCenter, true);
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
                Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
            }
            if (Projectile.hide == false)
            {
                Projectile.alpha = 0;
                player.ChangeDir(Projectile.direction);
                player.heldProj = Projectile.whoAmI;
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation + MathHelper.ToRadians(Projectile.direction == -1 ? 90 : -90));
            }
            Projectile.hide = false;
            Projectile.spriteDirection = Projectile.direction;
            Projectile.position.X = (vector2_1.X - (Projectile.width / 2));
            Projectile.position.Y = (vector2_1.Y - (Projectile.height / 2));
            Projectile.rotation = (float)(Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + 1.57000005245209);
            return false;
        }
	}
}