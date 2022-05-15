using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;

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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Magic;
			Projectile.friendly = true;
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.timeLeft = 7200;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 3;
			base.OnHitNPC(target, damage, knockback, crit);
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 14;
			height = 14;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (bounceCounter >= modPlayer.frigidJavelinBoost + 1)
			{
				storeRot = Projectile.rotation;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				Projectile.friendly = false;
				Projectile.extraUpdates = 2;
				startAnim = true;
				for (int i = 0; i < 6; i++)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 3.4f;
					Main.dust[num1].scale = 1f;

					num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 2.2f;
					Main.dust[num1].scale = 2f;
				}
			}
			else
			{
				if (Projectile.velocity.X != oldVelocity.X)
				{
					Projectile.velocity.X = -oldVelocity.X;
				}
				if (Projectile.velocity.Y != oldVelocity.Y)
				{
					Projectile.velocity.Y = -oldVelocity.Y;
				}
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);
				storeRot = Projectile.rotation;
			}
			bounceCounter++;
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void AI()
		{
			if(counter != -1)
				counter += 3;
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Projectile.timeLeft <= 7170)
			{
				if(counter != -1)
				{
					Projectile.extraUpdates = 20;
					counter = -1;
					Projectile.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item71, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
					return;
				}
				Projectile.tileCollide = true;
				Projectile.position += Projectile.velocity;
				if(Projectile.velocity.X != 0 || Projectile.velocity.Y != 0)
				{
					int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 2f;
					Main.dust[num1].scale = 1.5f;

					num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 34, 34, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity *= 0.8f;
					Main.dust[num1].scale = 2.5f;
				}
			}
			if(Projectile.timeLeft > 7170)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);
				Projectile.position.Y = player.Center.Y - Projectile.height / 2;
				Projectile.position.X = player.Center.X - Projectile.width / 2;
				Vector2 rotater = new Vector2(0, counter * 2).RotatedBy(MathHelper.ToRadians(6 * counter));
				rotater = new Vector2(rotater.X, 0).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
				Projectile.position.X += rotater.X;
				Projectile.position.Y += rotater.Y;
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
				if(Projectile.timeLeft % 3 == 0)
				{
					Projectile.alpha++;
				}
				Projectile.rotation = storeRot + radians;
				if (Projectile.alpha > 250)
					Projectile.Kill();
			}
		}
	}
}
		