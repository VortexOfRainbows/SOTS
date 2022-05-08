using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class FrigidJavelin : ModProjectile 
    {
		int bounceCounter = 0;
		int counter = 0;
		int counter2 = 72;
		bool startAnim = false;
		float storeRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Javelin");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;    
		}
        public override void SetDefaults()
        {
			projectile.magic = true;
			projectile.friendly = true;
			projectile.width = 38;
			projectile.height = 38;
			projectile.timeLeft = 7200;
			projectile.extraUpdates = 1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 3;
			base.OnHitNPC(target, damage, knockback, crit);
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
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (bounceCounter >= modPlayer.frigidJavelinBoost + 1)
			{
				storeRot = projectile.rotation;
				projectile.velocity *= 0;
				projectile.tileCollide = false;
				projectile.friendly = false;
				projectile.extraUpdates = 2;
				startAnim = true;
				for (int i = 0; i < 6; i++)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 3.4f;
					Main.dust[num1].scale = 1f;

					num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 2.2f;
					Main.dust[num1].scale = 2f;
				}
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.ToRadians(45);
				storeRot = projectile.rotation;
			}
			bounceCounter++;
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
			return true;
		}
		public override void AI()
		{
			if(counter != -1)
				counter += 3;
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (projectile.timeLeft <= 7170)
			{
				if(counter != -1)
				{
					projectile.extraUpdates = 20;
					counter = -1;
					projectile.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item71, (int)(projectile.Center.X), (int)(projectile.Center.Y));
					return;
				}
				projectile.tileCollide = true;
				projectile.position += projectile.velocity;
				if(projectile.velocity.X != 0 || projectile.velocity.Y != 0)
				{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 2f;
					Main.dust[num1].scale = 1.5f;

					num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.8f;
					Main.dust[num1].scale = 2.5f;
				}
			}
			if(projectile.timeLeft > 7170)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + MathHelper.ToRadians(45);
				projectile.position.Y = player.Center.Y - projectile.height / 2;
				projectile.position.X = player.Center.X - projectile.width / 2;
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
				if(projectile.timeLeft % 3 == 0)
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
		