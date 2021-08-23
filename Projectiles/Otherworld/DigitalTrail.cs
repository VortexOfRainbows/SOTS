using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class DigitalTrail : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Trail");
		}
        public override void SetDefaults()
        {
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.timeLeft = 3600;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.melee = true;
			projectile.alpha = 255;
			projectile.ai[1] = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 20;
			projectile.hide = true;
		}
		public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[projectile.owner];
			Vector2 center = player.Center;
			bool hitThroughWall = Collision.CanHitLine(center - new Vector2(5, 5), 10, 10, target.Hitbox.TopLeft(), target.Hitbox.Width, target.Hitbox.Height) && !target.friendly;
			return hitThroughWall || target.behindTiles;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindProjectiles.Add(index);
        }
		public override bool PreAI()
		{
			projectile.hide = projectile.ai[0] < 0;
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			return base.PreAI();
		}
		public override void PostAI()
		{
			checkPos();
			cataloguePos();
		}
		Vector2[] trailPos = new Vector2[22];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture = Main.projectileTexture[projectile.type];
			bool black = projectile.ai[0] < 0;
			if (black)
				texture = ModContent.GetTexture("SOTS/Projectiles/Otherworld/DigitalTrailBlack");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			if (previousPosition == Vector2.Zero)
			{
				return true;
			}
			int availableTrails = 0;
			int availableTrails2 = 0;
			for (int k = 1; k < trailPos.Length; k++)
			{
				if (trailPos[k] != projectile.Center)
					availableTrails2++;
				availableTrails++;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
			}
			for (int k = 1; k < trailPos.Length; k++)
			{
				float mult = 0.1f + (0.25f + 0.65f * availableTrails2 / trailPos.Length) * (float)Math.Sin(MathHelper.ToRadians(180 * ((availableTrails - k) / ((float)availableTrails))));
				float scale = projectile.scale * mult;
				if (black)
					scale *= 2f;
				else
					scale *= 1.35f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float max = betweenPositions.Length() / (5f * scale);
				if (trailPos[k] != projectile.Center)
					for (int i = 0; i < max; i++)
					{
						Color color = Color.White * mult;
						if (!black)
							color = new Color(110, 135, 140, 0) * mult;
						drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
						for (int j = 0; j < (black ? 1 : 2); j++)
						{
							float x = Main.rand.Next(-10, 11) * 0.1f * scale;
							float y = Main.rand.Next(-10, 11) * 0.1f * scale;
							if (j == 0)
							{
								x = 0;
								y = 0;
							}
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
						}
					}
				previousPosition = currentPos;
			}
			return true;
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if(iterator >= trailPos.Length)
            {
				projectile.Kill();
            }
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 1.25f;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Vector2 currentPos = trailPos[k];
				if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), previousPosition, currentPos, 12f * scale, ref point))
				{
					return true;
				}
				previousPosition = currentPos;
			}
			return false;
		}
		bool end = false;
		float counter = 0;
		public override void AI()
		{
			if (projectile.ai[1] != -1 && end == false)
			{
				Projectile parent = null;
				for (short i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == projectile.owner && proj.identity == (int)projectile.ai[1])
					{
						parent = proj;
						break;
					}
				}
				Projectile owner = parent;
				if(owner != null && owner.active && owner.type == ModContent.ProjectileType<DigitalSlash>() && owner.owner == projectile.owner && ((int)owner.localAI[1] == projectile.whoAmI || (int)owner.localAI[0] == projectile.whoAmI))
				{
					Vector2 center = owner.Center;
					projectile.Center = center;
					projectile.velocity = owner.velocity;
					projectile.rotation = owner.rotation;
					//float circular = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(counter * 12.5f * projectile.ai[0])).X;
					projectile.Center += new Vector2(projectile.ai[0] * 6 + 24, 0).RotatedBy(projectile.rotation);
					projectile.timeLeft = 120;
				}
				else
                {
					end = true;
				}
			}
			counter++;
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		