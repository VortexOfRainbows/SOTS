using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}
		
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = true;
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.timeLeft = 7200;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
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
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			base.PostDraw(spriteBatch, drawColor);
		}
		public override void AI()
		{
			Vector2 origin = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			if(counter != -1)
				counter += 3;
			if (Projectile.timeLeft <= 7185)
			{
				if(counter != -1)
				{
					Projectile.extraUpdates = 20;
					counter = -1;
					Projectile.netUpdate = true;
					Terraria.Audio.SoundEngine.PlaySound(SoundID.Item71, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
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
			if(Projectile.timeLeft > 7185)
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + MathHelper.ToRadians(45);
				Projectile.position.Y = origin.Y - Projectile.height / 2;
				Projectile.position.X = origin.X - Projectile.width / 2;
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
				if(Projectile.timeLeft % 2 == 0)
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
		