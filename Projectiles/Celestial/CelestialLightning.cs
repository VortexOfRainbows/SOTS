using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;
using SOTS.Buffs;

namespace SOTS.Projectiles.Celestial
{    
    public class CelestialLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Subspace Thunder");
		}
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 55;
			projectile.scale = 1f;
		}
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
        public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		Color color = new Color(100, 255, 100, 0);
		Vector2[] trailPos = new Vector2[120];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			Color color = this.color * ((255 - projectile.alpha) / 205f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = projectile.scale * 1.5f;
				if(k > trailPos.Length - 15)
                {
					int scaleDown = k - (trailPos.Length - 15);
					scale -= 0.1f * scaleDown;
                }
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float distanceBetweenDrawMult = 0.65f;
				if (SOTS.Config.lowFidelityMode)
					distanceBetweenDrawMult = 1f;
				float max = betweenPositions.Length() / (texture.Width * scale * distanceBetweenDrawMult);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					int amt = 2;
					if (!SOTS.Config.lowFidelityMode)
						amt += Main.rand.Next(2);
					for (int j = 0; j < amt; j++)
					{
						float x = Main.rand.NextFloat(-2f, 2f) * scale;
						float y = Main.rand.NextFloat(-2f, 2f) * scale;
						if (j < 1)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (!projectile.hostile)
				return false;
			for(int i = 0; i < trailPos.Length; i ++)
            {
				Rectangle hitbox = new Rectangle((int)trailPos[i].X - 6, (int)trailPos[i].Y - 6, 12, 12);
				if (hitbox.Intersects(targetHitbox))
                {
					return true;
                }
            }
            return false;
        }
        public override bool ShouldUpdatePosition()
		{
			return false;
		}
		bool runOnce = true;
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		int[] randStorage = new int[120];
		int dist = 120;
		public override void AI()
		{
			int type = (int)projectile.ai[0];
			if (runOnce)
			{
				if (type == 0 || type == 3)
					color = new Color(100, 255, 100, 0);
				if(type == 1)
					color = new Color(255, 100, 100, 0);
				if (type == 2)
					color = new Color(255, 100, 255, 0);
				projectile.position += projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				SoundEngine.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.6f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-45, 46);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				originalVelo = projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = projectile.Center;
				runOnce = false;
				for(int i = 0; i < 20; i++)
				{
					int dust3 = Dust.NewDust(projectile.Center - new Vector2(12, 12) - new Vector2(5), 24, 24, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.55f;
					dust4.velocity += projectile.velocity.SafeNormalize(Vector2.Zero) * -2f;
					dust4.color = color;
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.75f;
				}
			}
			Vector2 temp = originalPos;
			addPos = projectile.Center;
			for (int i = 0; i < dist; i++)
			{
				originalPos += originalVelo;
				for (int reps = 0; reps < 20; reps++)
				{
					Vector2 attemptToPosition = (originalPos + originalVelo * 3.5f) - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
				}
			}
			originalPos = temp;
			projectile.alpha += 5;
			if (type == 3)
				projectile.alpha += 10;
			if (projectile.alpha >= 255)
			{
				projectile.Kill();
			}
			else if(projectile.alpha > 205)
            {
				projectile.hostile = false;
            }

			projectile.scale *= 0.98f;
		}
	}
}
		