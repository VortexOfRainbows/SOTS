using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Dusts;
using SOTS.Projectiles.Celestial;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{
	public class LingeringFlame : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Fire Bolt");
		}
		public override void SetDefaults()
		{
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
		}
		List<FireParticle> particleList = new List<FireParticle>();
		public override bool PreAI()
		{
			cataloguePos();
			Vector2 rotational = new Vector2(0, -1.8f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
			rotational.X *= 0.25f;
			rotational.Y *= 0.75f;
			rotational += Projectile.velocity;
			rotational = rotational.SafeNormalize(Vector2.Zero) * 2f;
			particleList.Add(new FireParticle(Projectile.Center - rotational * 2, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.1f)));
			return base.PreAI();
		}
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if(!particle.active)
                {
					particleList.RemoveAt(i);
					i--;
                }
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = ColorHelpers.Inferno1;
				color.A = 0;
				Vector2 drawPos = particleList[i].position - Main.screenPosition ;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 3; i++)
			{
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = new Color(255, 75, 0, 0);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
				dust.velocity *= 0.9f;
				dust.alpha = 125;
			}
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			target.AddBuff(BuffID.OnFire, 360, false);
		}
		public override bool ShouldUpdatePosition()
		{
			return true;
		}
		public override void AI()
		{
			Projectile.velocity *= 0.998f;
			if (Projectile.timeLeft <= 51)
				Projectile.alpha += 5;
			if (Projectile.timeLeft <= 15)
				Projectile.hostile = false;
			Lighting.AddLight(Projectile.Center, 0.6f, 0.2f, 0.0f);
			Projectile.ai[0]++;
		}
	}
}