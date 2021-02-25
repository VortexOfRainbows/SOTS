using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;

namespace SOTS.Projectiles.Laser
{    
    public class PutridEye : ModProjectile
	{
		private float dist
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}
		private float speed
		{
			get => projectile.ai[1];
			set => projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Eye");
		}
		
        public override void SetDefaults()
        {
			projectile.height = 46;
			projectile.width = 46;
			projectile.friendly = false;
			projectile.timeLeft = 6004;
			projectile.tileCollide = false;
			projectile.hostile = false;
		}
		public override bool PreAI()
        {
			if(projectile.active)
			return true;
		
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Vector2 drawOrigin = new Vector2(23, 23);
			double direction = Math.Atan2(player.Center.Y - projectile.Center.Y, player.Center.X - projectile.Center.X);
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Laser/PutridPupil");
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Laser/PutridEye");
			spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, lightColor, (float)direction + MathHelper.ToRadians(225), drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			float dist2 = dist > 14 ? 14 : dist;
			dist2 = dist2 <= 2 ? dist2 * ((12 + dist2)/14f) : dist2;
			spriteBatch.Draw(texture, projectile.Center - new Vector2(dist2, 0).RotatedBy(direction) - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(7, 7), 0.9f - (dist2/14f * 0.25f), SpriteEffects.None, 0f);
		}
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		int counter = 0;
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			if (projectile.hide == false)
			{
				if (projectile.Center.X < player.Center.X)
					projectile.direction = -1;
				else
					projectile.direction = 1;
				Main.player[projectile.owner].heldProj = projectile.whoAmI;
				projectile.alpha = 0;
                player.ChangeDir(projectile.direction);
                player.itemRotation = MathHelper.WrapAngle(projectile.rotation + MathHelper.ToRadians(projectile.direction == -1 ? 0 : -180));
			}
			Vector2 cursorArea = Main.MouseWorld;
			float shootToX = cursorArea.X - player.Center.X;
			float shootToY = cursorArea.Y - player.Center.Y;
			float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
			distance = 4f / distance;
			shootToX *= distance * 5f;
			shootToY *= distance * 5f;
			counter++;
			if(dist < 20)
			{
				dist += 0.2f;
			}
			else
			{
				dist = 20;
			}
			double startingDirection = Math.Atan2((double)-shootToY, (double)-shootToX);
			double direction = Math.Atan2(player.Center.Y - projectile.Center.Y, player.Center.X - projectile.Center.X);
			projectile.rotation = (float)direction;
			if (Main.myPlayer == player.whoAmI)
			{
				startingDirection *= 180 / Math.PI;
				double deg = startingDirection;
				double rad = deg * (Math.PI / 180);
				double dist2 = 20;
				projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist2) - projectile.width / 2;
				projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist2) - projectile.height / 2;
				if (player.channel || projectile.timeLeft > 6)
				{
					projectile.timeLeft = 6;
					projectile.alpha = 0;
					if (Main.myPlayer == projectile.owner && dist >= 20)
					{
						VoidItem.DrainMana(player);
						dist = -12;
						Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f, (255 - projectile.alpha) * 0.8f / 255f);
						Main.PlaySound(SoundID.Item94, (int)(projectile.Center.X), (int)(projectile.Center.Y));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("FriendlyPinkLaser"), projectile.damage, 1f, projectile.owner, projectile.Center.X + shootToX * 60, projectile.Center.Y + shootToY * 60);
					}
				}
				projectile.netUpdate = true;
			}
			if (dist >= 2 && counter % 3 == 0)
			{
				if(dist <= 3)
					Main.PlaySound(SoundID.Item15, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				for (int i = 0; i < 360; i += 10)
				{
					
					Vector2 circularLocation = new Vector2((6 + dist), 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X *= 0.6f;
					circularLocation = circularLocation.RotatedBy(direction);
					circularLocation += projectile.Center - new Vector2(60, 0).RotatedBy(direction);
					int num1 = Dust.NewDust(new Vector2(circularLocation.X - 4, circularLocation.Y - 4), 4, 4, 72);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
				}
			}
			if (dist >= 8 && counter % 3 == 0)
			{
				if (dist <= 9)
					Main.PlaySound(SoundID.Item15, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X *= 0.6f;
					circularLocation = circularLocation.RotatedBy(direction);
					circularLocation += projectile.Center - new Vector2(120, 0).RotatedBy(direction);
					int num1 = Dust.NewDust(new Vector2(circularLocation.X - 4, circularLocation.Y - 4), 4, 4, 72);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
				}
			}
			if (dist >= 14 && counter % 3 == 0)
			{
				if (dist <= 15)
					Main.PlaySound(SoundID.Item15, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				for (int i = 0; i < 360; i += 10)
				{
					Vector2 circularLocation = new Vector2(dist - 6, 0).RotatedBy(MathHelper.ToRadians(i));
					circularLocation.X *= 0.6f;
					circularLocation = circularLocation.RotatedBy(direction);
					circularLocation += projectile.Center - new Vector2(180, 0).RotatedBy(direction);
					int num1 = Dust.NewDust(new Vector2(circularLocation.X - 4, circularLocation.Y - 4), 4, 4, 72);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180));
				}
			}
		}
	}
}