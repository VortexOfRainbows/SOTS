using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Minions
{    
    public class OtherworldLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Otherworld Lightning");
		}
		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.friendly = true;
			//projectile.magic = true;
			projectile.timeLeft = 3600;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.alpha = 125;
			projectile.scale = 1f;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		Vector2[] trailPos = new Vector2[250];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			Color color = VoidPlayer.OtherworldColor * ((255 - projectile.alpha) / 205f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = projectile.scale * 1.3f;
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
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
		int[] randStorage = new int[250];
		int dist = 250;
		public override void AI()
		{
			if (runOnce)
			{
				projectile.position += projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.6f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-65, 66);
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
					dust4.color = VoidPlayer.OtherworldColor;
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.75f;
				}
			}

			Vector2 temp = originalPos;
			addPos = projectile.Center;
			for (int i = 0; i < dist; i++)
			{
				bool collided = false;
				originalPos += originalVelo;

				for (int reps = 0; reps < 20; reps++)
				{
					Vector2 attemptToPosition = (originalPos + originalVelo * 3f) - addPos;
					addPos += new Vector2(originalVelo.Length(), 0).RotatedBy(attemptToPosition.ToRotation() + MathHelper.ToRadians(randStorage[i]));
					trailPos[i] = addPos;
				}
				for (int n = 0; n < Main.npc.Length; n++)
				{
					NPC npc = Main.npc[n];
					int hitboxWidth = 12;
					if (npc.active && npc.Hitbox.Intersects(new Rectangle((int)addPos.X - hitboxWidth, (int)addPos.Y - hitboxWidth, hitboxWidth * 2, hitboxWidth * 2)) && !npc.friendly && !npc.dontTakeDamage)
					{
						Vector2 velo = projectile.velocity.SafeNormalize(Vector2.Zero);
						if (projectile.owner == Main.myPlayer && projectile.friendly)
						{
							Projectile.NewProjectile(addPos.X + velo.X * 28, addPos.Y + velo.Y * 28, velo.X * 0.1f, velo.Y * 0.1f, ModContent.ProjectileType<ThunderRing>(), projectile.damage, projectile.knockBack, Main.myPlayer);
						}
						if (projectile.friendly)
							collided = true;
						for (int k = i + 1; k < trailPos.Length; k++)
						{
							trailPos[k] = Vector2.Zero;
						}
						break;
					}
				}
				if (collided)
				{
					dist = i + 1;
					break;
				}
			}
			originalPos = temp;
			projectile.alpha += 5;
			if (projectile.alpha >= 255)
				projectile.Kill();

			projectile.scale *= 0.98f;
			projectile.friendly = false;
		}
	}
}
		