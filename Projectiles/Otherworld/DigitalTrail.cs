using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using SOTS.Utilities;

namespace SOTS.Projectiles.Otherworld
{    
    public class DigitalTrail : ModProjectile //, IPixellated
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Trail");
		}
        public override void SetDefaults()
        {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.alpha = 255;
			Projectile.ai[1] = -1;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.hide = true;
			Projectile.ownerHitCheck = true;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindProjectiles.Add(index);
        }
		public override bool PreAI()
		{
			Projectile.hide = Projectile.ai[0] < 0;
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
		Vector2 toOwner = Vector2.Zero;
		public override bool PreDraw(ref Color lightColor)
		{
			Draw(spriteBatch, lightColor);
			return true;
		}

		public void Draw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if (runOnce)
				return;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			bool black = Projectile.ai[0] < 0;
			if (black)
			{
				texture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/DigitalTrailBlack").Value;
			}
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			if (previousPosition == Vector2.Zero)
			{
				return;
			}
			int availableTrails = 0;
			int availableTrails2 = 0;
			for (int k = 1; k < trailPos.Length; k++)
			{
				if (trailPos[k] != Projectile.Center)
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
				float scale = Projectile.scale * mult;
				if (black)
					scale *= 3f;
				else
					scale *= 1.75f;
				if (trailPos[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float length = 5f;
				if (black)
					length = 2.5f;
				float max = betweenPositions.Length() / (length * scale);
				if (trailPos[k] != Projectile.Center)
					for (int i = 0; i < max; i++)
					{
						Color color = Color.Black * mult;
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
							float rotation = betweenPositions.ToRotation();
							Main.spriteBatch.Draw(texture, (drawPos + new Vector2(x, y)), null, color, rotation, drawOrigin, new Vector2(black ? 0.5f : 1f, 1f) * scale, SpriteEffects.None, 0f);
						}
					}
				previousPosition = currentPos;
			}
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
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
			Vector2 current = Projectile.Center;
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
				Projectile.Kill();
            }
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
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
			if (Projectile.ai[1] != -1 && end == false)
			{
				Projectile parent = null;
				for (short i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];
					if (proj.active && proj.owner == Projectile.owner && proj.identity == (int)Projectile.ai[1])
					{
						parent = proj;
						break;
					}
				}
				Projectile owner = parent;
				if(owner != null && owner.active && owner.type == ModContent.ProjectileType<DigitalSlash>() && owner.owner == Projectile.owner && ((int)owner.localAI[1] == Projectile.whoAmI || (int)owner.localAI[0] == Projectile.whoAmI))
				{
					Vector2 center = owner.Center;
					Projectile.Center = center;
					Projectile.velocity = owner.velocity;
					Projectile.rotation = owner.rotation;
					//float circular = new Vector2(0, 5).RotatedBy(MathHelper.ToRadians(counter * 12.5f * Projectile.ai[0])).X;
					Projectile.Center += new Vector2(Projectile.ai[0] * 6 + 24, 0).RotatedBy(Projectile.rotation);
					Projectile.timeLeft = 120;
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
		