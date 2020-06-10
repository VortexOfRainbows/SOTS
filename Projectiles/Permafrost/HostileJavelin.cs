using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Permafrost
{    
    public class HostileJavelin : ModProjectile 
    {
		int counter = 0;
		int counter2 = 72;
		bool startAnim = false;
		float storeRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Spear");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}
		
        public override void SetDefaults()
        {
			projectile.magic = true;
			projectile.hostile = true;
			projectile.width = 34;
			projectile.height = 34;
			projectile.timeLeft = 7200;
			projectile.extraUpdates = 1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 14;
			height = 14;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[projectile.owner];
			storeRot = projectile.rotation;
			projectile.velocity *= 0;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.extraUpdates = 2;
			startAnim = true;
			for (int i = 0; i < 6; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, 67);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 3.4f;
				Main.dust[num1].scale = 1f;

				num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, 67);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 2.2f;
				Main.dust[num1].scale = 2f;
			}
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (projectile.alpha / 255f)), projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void AI()
		{
			Vector2 origin = new Vector2(projectile.ai[0], projectile.ai[1]);
			if(counter != -1)
				counter += 3;
			if (projectile.timeLeft <= 7185)
			{
				if(counter != -1)
				{
					projectile.extraUpdates = 20;
					counter = -1;
					projectile.netUpdate = true;
					Main.PlaySound(SoundID.Item71, (int)(projectile.Center.X), (int)(projectile.Center.Y));
					return;
				}
				projectile.tileCollide = true;
				projectile.position += projectile.velocity;
				if(projectile.velocity.X != 0 || projectile.velocity.Y != 0)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, 67);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 2f;
					Main.dust[num1].scale = 1.5f;

					num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, 67);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.8f;
					Main.dust[num1].scale = 2.5f;
				}
			}
			if(projectile.timeLeft > 7185)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.ToRadians(45);
				projectile.position.Y = origin.Y - projectile.height / 2;
				projectile.position.X = origin.X - projectile.width / 2;
				Vector2 rotater = new Vector2(0, counter * 2).RotatedBy(MathHelper.ToRadians(6 * counter));
				rotater = new Vector2(rotater.X, 0).RotatedBy(Math.Atan2(projectile.velocity.Y, projectile.velocity.X));
				projectile.position.X += rotater.X;
				projectile.position.Y += rotater.Y;
			}
			if(startAnim)
			{
				float radians = MathHelper.ToRadians(counter2/4);
				if (counter2 != 0)
				{
					if (counter2 < 0)
						counter2 += 1;
					if (counter2 > 0)
						counter2 -= 1;
					counter2 *= -1;
					
				}
				if(projectile.timeLeft % 2 == 0)
				{
					projectile.alpha++;
				}
				projectile.rotation = storeRot + radians;
				if (projectile.alpha > 250)
					projectile.Kill();
			}
		}
	}
}
		