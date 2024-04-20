using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
	public class BossBabyLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Red Laser");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.alpha = 80;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.OnFire, 360, false);
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
		int counter = 0;
        public override void AI()
		{
			counter++;
			//Projectile.Center = npc.Center;
			if (Projectile.alpha <= 100)
            {
				for (int i = 0; i < 20; i++)
				{
					int dust3 = Dust.NewDust(Projectile.Center - new Vector2(20, 20) - new Vector2(5), 40, 40, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.55f;
					dust4.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(8);
                    dust4.color = new Color(255, 69, 0, 0);
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.75f;
				}
			}
			Projectile.alpha += 7;
			if (Projectile.alpha > 120)
			{
				Projectile.hostile = false;
			}
			if (Projectile.alpha >= 255)
			{
				Projectile.Kill();
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (!Projectile.hostile)
				return false;
			float laserDist = 200;
			Vector2 center = Projectile.Center;
			for (int i = 0; i < laserDist; i++)
			{
				Rectangle rect = new Rectangle((int)center.X - 18, (int)center.Y - 18, 36, 36);
				center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * Projectile.width * 2.5f;
				if (rect.Intersects(targetHitbox))
					return true;
			}
			return false;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float laserDist = 100;
			Vector2 center = Projectile.Center;
			Color color = new Color(255, 69, 0, 0);
            float bonusAlphaMult = 1 - 1 * (counter / 28f);
            float alpha = ((255f - Projectile.alpha) / 255f);
            for (int i = 0; i < laserDist; i++)
            {
				center += Projectile.velocity.SafeNormalize(new Vector2(1, 0)) * Projectile.width * 6f;
				Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, color * alpha, Projectile.velocity.ToRotation(), origin, new Vector2(12, 3), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, Color.White * alpha * 0.75f * bonusAlphaMult, Projectile.velocity.ToRotation(), origin, new Vector2(6f, 2.5f), SpriteEffects.None, 0f);
                for (int j = 0; j < 2; j++)
				{
					float dir = j * 2 - 1;
					Vector2 offset = new Vector2(counter * 0.75f * dir, 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(90));
					Main.spriteBatch.Draw(texture, center - Main.screenPosition + offset, null, color * bonusAlphaMult * alpha, Projectile.velocity.ToRotation(), origin, new Vector2(6, 3), SpriteEffects.None, 0.0f);
				}
			}
			return false;
		}
	}
}