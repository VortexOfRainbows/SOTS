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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 120;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; //also saves rotation and spritedriection  
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
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
				storeRot = Projectile.rotation;
			}
			bounceCounter++;
			return false;
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		int wobbleCounter = 0;
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            if (Projectile.ai[1] == -1)
            {
				texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Permafrost/FrigidJavelinAlt").Value;
			}
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.4f); //move javelin center towards top
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float alphaScale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = lightColor;
				if (Projectile.ai[1] == -1)
				{
					toChange = new Color(50, 70, 120, 0);
					alphaScale *= 0.8f;
				}
				else
					alphaScale *= 0.4f;
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * alphaScale;
				float rotation = Projectile.oldRot[k];
				if (startAnim && wobbleCounter > k)
					rotation = Projectile.rotation;
				Main.spriteBatch.Draw(texture, drawPos, null, color, rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void AI()
		{
			if(counter != -1)
				counter += 3;
			Player player = Main.player[Projectile.owner];
			//SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Projectile.timeLeft <= 7170)
			{
				if(counter != -1)
				{
					Projectile.extraUpdates = 20;
					counter = -1;
					Projectile.netUpdate = true;
					SoundEngine.PlaySound(SoundID.Item71, Projectile.Center);
					return;
				}
				Projectile.tileCollide = true;
				Projectile.position += Projectile.velocity;
				if(Projectile.velocity.X != 0 || Projectile.velocity.Y != 0)
				{
					if (Projectile.ai[1] != -1)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), 38, 38, ModContent.DustType<CopyIceDust>());
						dust.noGravity = true;
						dust.velocity *= 1.5f;
						dust.scale *= 1.1f;

						dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), 38, 38, ModContent.DustType<CopyIceDust>());
						dust.noGravity = true;
						dust.velocity *= 0.5f;
						dust.scale *= 2.2f;
					}
					else
					{
						for(float i = 0; i < 1; i += 0.5f)
						{
							Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + i * Projectile.velocity, 0, 0, ModContent.DustType<CopyDust4>());
							dust.noGravity = true;
							dust.velocity *= 0.4f;
							dust.scale = 1.4f;
							dust.fadeIn = 0.1f;
							dust.color = new Color(80, 150, 221, 0) * 0.6f;
						}
						Dust dust3 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyDust4>());
						dust3.noGravity = true;
						dust3.velocity *= 1.2f;
						dust3.scale = 1.2f;
						dust3.fadeIn = 0.1f;
						dust3.color = new Color(166, 201, 219, 0) * 0.8f;
					}
					Dust dust2 = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 0, 0, ModContent.DustType<CopyIceDust>());
					dust2.noGravity = true;
					dust2.velocity *= 0.5f;
					dust2.scale = 1.6f;
					dust2.color = new Color(60, 70, 160, 0) * 0.7f;
				}
			}
			if(Projectile.timeLeft > 7170)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
				Projectile.position.Y = player.Center.Y - Projectile.height / 2;
				Projectile.position.X = player.Center.X - Projectile.width / 2;
				Vector2 rotater = new Vector2(0, counter * 2).RotatedBy(MathHelper.ToRadians(6 * counter));
				rotater = new Vector2(rotater.X, 0).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
				Projectile.position.X += rotater.X;
				Projectile.position.Y += rotater.Y;
			}
			if(startAnim)
			{
				wobbleCounter++;
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
		