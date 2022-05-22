using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Laser
{    
    public class PutridEye : ModProjectile
	{
		private float dist
		{
			get => Projectile.ai[0];
			set => Projectile.ai[0] = value;
		}
		private float speed
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putrid Eye");
		}
		
        public override void SetDefaults()
        {
			Projectile.height = 44;
			Projectile.width = 44;
			Projectile.friendly = false;
			Projectile.timeLeft = 6004;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
		}
		public override bool PreAI()
        {
			if(Projectile.active)
				return true;
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			double direction = (player.Center - Projectile.Center).ToRotation();
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/PutridPupil");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/PutridEye");
			Vector2 drawOrigin = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Vector2 drawOrigin2 = new Vector2(texture.Width / 2, texture.Height / 2);
			spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, lightColor, (float)direction + MathHelper.ToRadians(225), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			float dist2 = dist > 16 ? 16 : dist;
			dist2 = dist2 <= 2 ? dist2 * ((12 + dist2)/14f) : dist2;
			spriteBatch.Draw(texture, Projectile.Center - new Vector2(dist2, 0).RotatedBy(direction) - Main.screenPosition, null, lightColor, (float)direction, drawOrigin2, 1.1f - (dist2/14f * 0.18f), SpriteEffects.None, 0f);
			if (dist >= 2)
			{
				DrawCircle(true, 6 + dist, 60, 1);
			}
			if (dist >= 8)
			{
				DrawCircle(true, dist, 120, 1);
			}
			if (dist >= 14)
			{
				DrawCircle(true, -6 + dist, 180, 1);
			}
		}
		public void DrawCircle(bool spriteBatch, float size, float dist, float colorMod = 1)
		{
			Player player = Main.player[Projectile.owner];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Laser/PutridEyeDecor");
			Vector2 drawOrigin2 = new Vector2(texture.Width / 2, texture.Height / 2);
			Color lightColor = new Color(100, 100, 100, 0);
			double direction = (player.Center - Projectile.Center).ToRotation();
			for (int i = 0; i < 360; i += 5)
			{
				Vector2 circularLocation = new Vector2(size, 0).RotatedBy(MathHelper.ToRadians(i));
				circularLocation.X *= 0.6f;
				circularLocation = circularLocation.RotatedBy(direction);
				Vector2 circularVelo = circularLocation;
				circularLocation += Projectile.Center - new Vector2(dist, 0).RotatedBy(direction);
				if(spriteBatch)
					Main.spriteBatch.Draw(texture, circularLocation - Main.screenPosition, null, lightColor * colorMod, MathHelper.ToRadians(i), drawOrigin2, 0.4f, SpriteEffects.None, 0f);
				else
				{
					lightColor = new Color(255, 200, 230);
					Dust dust = Dust.NewDustDirect(new Vector2(circularLocation.X - 4, circularLocation.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 0.6f;
					dust.velocity += circularVelo.SafeNormalize(Vector2.Zero) * 2.5f;
					dust.fadeIn = 0.1f;
					dust.color = lightColor;
					dust.scale *= 1.4f;
					dust.alpha = 100;
				}
			}
		}
		public override bool ShouldUpdatePosition() 
		{
			return false;
		}
		int counter = 0;
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			if (Projectile.hide == false)
			{
				if (Projectile.Center.X < player.Center.X)
					Projectile.direction = -1;
				else
					Projectile.direction = 1;
				Main.player[Projectile.owner].heldProj = Projectile.whoAmI;
				Projectile.alpha = 0;
                player.ChangeDir(Projectile.direction);
                player.itemRotation = MathHelper.WrapAngle(Projectile.rotation + MathHelper.ToRadians(Projectile.direction == -1 ? 0 : -180));
				player.itemTime = 2;
				player.itemAnimation = 2;
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
			double direction = (player.Center - Projectile.Center).ToRotation();
			Projectile.rotation = (float)direction;
			if (Main.myPlayer == player.whoAmI)
			{
				startingDirection *= 180 / Math.PI;
				double deg = startingDirection;
				double rad = deg * (Math.PI / 180);
				double dist2 = 20;
				Projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist2) - Projectile.width / 2;
				Projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist2) - Projectile.height / 2;
				if (player.channel || Projectile.timeLeft > 6)
				{
					Projectile.timeLeft = 6;
					Projectile.alpha = 0;
				}
				Projectile.netUpdate = true;
			}
			if(dist >= 20)
			{
				DrawCircle(false, 6 + dist, 60, 1);
				DrawCircle(false, dist, 120, 1);
				DrawCircle(false, -6 + dist, 180, 1);
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<FriendlyPinkLaser>(), Projectile.damage, 1f, Projectile.owner, Projectile.Center.X + shootToX * 60, Projectile.Center.Y + shootToY * 60);
					Item item = player.HeldItem;
					VoidItem vItem = Item.modItem as VoidItem;
					if (vItem != null)
						vItem.DrainMana(player);
				}
				dist = -10;
				Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f, (255 - Projectile.alpha) * 0.8f / 255f);
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item94, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			}
			if(dist >= 2.8f && dist <= 3.2f)
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item15, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			if (dist >= 8.8f && dist <= 9.2f)
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item15, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			if (dist >= 14.8f && dist <= 15.2f)
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Item15, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
		}
	}
}