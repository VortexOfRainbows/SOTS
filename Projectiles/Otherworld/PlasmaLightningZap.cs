using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class PlasmaLightningZap : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Lightning");
		}
		public override void SetDefaults()
		{
			projectile.width = 14;
			projectile.height = 14;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 120;
			projectile.scale = 1f;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		Vector2[] trailPos = new Vector2[200];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce || !hit)
				return false;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/HardlightColumn").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(120, 130, 160, 0);
				float scale = projectile.scale * 0.75f;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Width * scale * 0.5f);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
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
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		bool runOnce = true;
		Vector2 addPos = Vector2.Zero;
		Vector2 originalVelo = Vector2.Zero;
		Vector2 originalPos = Vector2.Zero;
		Vector2 nextPos = Vector2.Zero;
		int[] randStorage = new int[200];
		int dist = 200;
		int counter = 0;
		bool hit = false;
		public override void AI()
		{
			NPC target = Main.npc[(int)projectile.ai[0]];
			if (runOnce)
			{
				projectile.velocity = target.Center - projectile.Center;
				SoundEngine.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.5f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-35, 36);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				nextPos = target.Center;
				originalVelo = projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = projectile.Center;
				runOnce = false;
			}

			Vector2 temp = originalPos;
			addPos = projectile.Center;
			for (int i = 0; i < dist; i++)
			{
				bool collided = false;
				originalPos += originalVelo;

				for (int reps = 0; reps < 3; reps++)
				{
					if (counter < 2)
						nextPos = target.Center;
					Vector2 attemptToPosition = nextPos - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
				}
				NPC npc = target;
				if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)addPos.X - 12, (int)addPos.Y - 12, 24, 24)) && !npc.friendly)
				{
					if (projectile.owner == Main.myPlayer && projectile.friendly)
						Projectile.NewProjectile(addPos.X, addPos.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("PlasmaLightningDamage"), projectile.damage, 3f, Main.myPlayer, (int)projectile.knockBack, projectile.ai[1] - 1);
					if(projectile.friendly)
                    {
						hit = true;
						collided = true;
					}
					projectile.friendly = false;
					for (int k = i + 1; k < trailPos.Length; k++)
					{
						trailPos[k] = Vector2.Zero;
					}
				}
				if (collided)
				{
					dist = i + 1;
					break;
				}
			}
			originalPos = temp;
			projectile.alpha += 4;
			if (projectile.alpha >= 255)
				projectile.Kill();
			counter++;
			projectile.scale *= 0.98f;
			if(counter >= 2)
				projectile.friendly = false;
		}
	}
}