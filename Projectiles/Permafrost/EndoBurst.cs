using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Celestial;
using SOTS.Dusts;
using SOTS.Buffs;

namespace SOTS.Projectiles.Permafrost
{    
    public class EndoBurst : ModProjectile 
    {
		public Color blue = new Color(180, 210, 250, 0);
		public Color blue2 = new Color(134, 109, 231, 0);
		List<FireParticle> particleList = new List<FireParticle>();
		int removedCounter = 0;
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--; 
					removedCounter++;
				}
			}
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Endo Burst");
		}
		public override void SetDefaults()
		{
			Projectile.height = 48;
			Projectile.width = 48;
			Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.hide = false;
			Projectile.melee = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			player.AddBuff(ModContent.BuffType<Overheat>(), 630);
			target.AddBuff(BuffID.Frostburn, 300);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = Color.Lerp(blue, blue2, ((removedCounter + i) % 10) / 10f);
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(2, 2), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
			return false;
        }
		public Color randColor()
        {
			return Color.Lerp(blue, blue2, Main.rand.NextFloat(1f));
        }
		int counter = 0;
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
			}
			if (counter > 10)
				Projectile.friendly = false;
			else
			{
				if(counter == 0)
				{
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 20, 0.6f, -0.4f);
					for(int i = 0; i < 30; i++)
					{
						Vector2 circular = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 12f));
						circular.X *= 0.5f;
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.1f;
						dust.velocity += circular * 0.3f + Projectile.velocity * 0.35f;
						dust.scale *= 1.5f;
						dust.fadeIn = 0.1f;
						dust.color = randColor();
					}
				}
				Projectile.friendly = true;
				int baseRate = 13;
				if (SOTS.Config.lowFidelityMode)
					baseRate = 18;
				baseRate -= counter;
				if (baseRate < 3)
					baseRate = 3;
				float sphereRadius = 30;
				float mult = counter / 10f;
				if (mult > 1)
					mult = 1;
				float radius = counter / 6f * sphereRadius;
				if (radius > sphereRadius)
					radius = sphereRadius;
				for (int i = -10; i <= 10; i++)
				{
					Vector2 circular = new Vector2((radius + 12f) * Projectile.direction, 0).RotatedBy(MathHelper.ToRadians(i * 1.8f));
					circular.X *= 0.5f;
					if (Main.rand.NextBool(baseRate))
					{
						particleList.Add(new FireParticle(Projectile.Center + circular, circular * 0.04f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 0.9f)));
					}
					if(Main.rand.NextBool(baseRate))
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.7f;
						dust.velocity += circular * 0.2f;
						dust.scale *= 1.4f + 0.3f * mult;
						dust.fadeIn = 0.1f;
						dust.color = randColor();
					}
				}
			}
			counter++;
			cataloguePos();
        }
	}
}