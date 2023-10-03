using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{
	public class InfernoPhaseBolt : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phase Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 18;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 30;
			Projectile.height = 18;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 720;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(255, 130, 100, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 20; i++)
			{
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = new Color(255, 130, 0, 0);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.75f;
				dust.velocity *= 2.5f;
				dust.velocity -= Projectile.velocity * 1.5f;
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
		bool runOnce = true;
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			Projectile.ai[0]++;
			Lighting.AddLight(Projectile.Center, 0.75f, 0.25f, 0.35f);
			if(runOnce)
			{
				Projectile.rotation = Projectile.velocity.ToRotation();
				runOnce = false;
			}
			Vector2 varyingVelocity = new Vector2(-4f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0] * 1.5f));
			Projectile.position += Projectile.velocity + new Vector2(varyingVelocity.X, varyingVelocity.Y * 0.15f).RotatedBy(Projectile.rotation);
		}
	}
}