using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;
using Terraria.ID;
using SOTS.Helpers;

namespace SOTS.Projectiles.Minions
{    
    public class OtherworldLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Otherworld Lightning");
		}
		public override void SetDefaults()
		{
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.friendly = true;
			//Projectile.magic = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.alpha = 125;
			Projectile.scale = 1f;
		}
		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}
		Vector2[] trailPos = new Vector2[250];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			Color color = ColorHelper.OtherworldColor * ((255 - Projectile.alpha) / 205f);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				float scale = Projectile.scale * 1.3f;
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
						if (trailPos[k] != Projectile.Center)
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
				Projectile.position += Projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
				for (int i = 0; i < randStorage.Length; i++)
				{
					randStorage[i] = Main.rand.Next(-65, 66);
				}
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				originalVelo = Projectile.velocity.SafeNormalize(Vector2.Zero) * 8f;
				originalPos = Projectile.Center;
				runOnce = false;
				for(int i = 0; i < 20; i++)
				{
					int dust3 = Dust.NewDust(Projectile.Center - new Vector2(12, 12) - new Vector2(5), 24, 24, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.55f;
					dust4.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * -2f;
					dust4.color = ColorHelper.OtherworldColor;
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale *= 2.75f;
				}
			}

			Vector2 temp = originalPos;
			addPos = Projectile.Center;
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
						Vector2 velo = Projectile.velocity.SafeNormalize(Vector2.Zero);
						if (Projectile.owner == Main.myPlayer && Projectile.friendly)
						{
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), addPos.X + velo.X * 28, addPos.Y + velo.Y * 28, velo.X * 0.1f, velo.Y * 0.1f, ModContent.ProjectileType<ThunderRing>(), Projectile.damage, Projectile.knockBack, Main.myPlayer);
						}
						if (Projectile.friendly)
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
			Projectile.alpha += 5;
			if (Projectile.alpha >= 255)
				Projectile.Kill();

			Projectile.scale *= 0.98f;
			Projectile.friendly = false;
		}
	}
}
		