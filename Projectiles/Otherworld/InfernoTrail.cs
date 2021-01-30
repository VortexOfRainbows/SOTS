using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Otherworld
{    
    public class InfernoTrail : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Trail");
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
			projectile.idStaticNPCHitCooldown = 14;
		}
		Color color = Color.White;
		public override bool PreAI()
		{
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
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 1.3f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.6f / 255f);
			checkPos();
			cataloguePos();
		}
		Vector2[] trailPos = new Vector2[24];
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return true;
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/InfernoTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = trailPos[0];
			if (previousPosition == Vector2.Zero)
			{
				return true;
			}
			for (int k = 1; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.75f;
				if (trailPos[k] == Vector2.Zero)
				{
					return true;
				}
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				Color color = this.color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.25f;
				color = color.MultiplyRGBA(new Color(140, 110, 100, 0));
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 4; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.4f * scale;
						float y = Main.rand.Next(-10, 11) * 0.4f * scale;
						if (j <= 1)
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
			return true;
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			Player player = Main.player[projectile.owner];
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
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		int endHow = 0;
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			float point = 0f;
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length/2; k++)
			{
				float scale = projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				scale *= 0.7f;
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
		public override void AI()
		{
			if(projectile.ai[1] != -1 && end == false)
			{
				Projectile proj = Main.projectile[(int)projectile.ai[1]];
				if(proj.active && proj.type == mod.ProjectileType("InfernoHook") && proj.owner == projectile.owner && (((InfernoHook)proj.modProjectile).storeData1 == projectile.whoAmI || ((InfernoHook)proj.modProjectile).storeData2 == projectile.whoAmI))
				{
					Vector2 center = proj.Center - new Vector2(13, 0).RotatedBy(proj.velocity.ToRotation() + MathHelper.ToRadians(90 * projectile.ai[0]));
					projectile.position.X = center.X - projectile.width/2;
					projectile.position.Y = center.Y - projectile.height/2;
					projectile.velocity = proj.velocity;
					projectile.timeLeft = 60;
				}
				else
                {
					end = true;
				}
			}
			else
            {
				//projectile.position += projectile.velocity * 0.18f;
            }
		}
        public override bool ShouldUpdatePosition()
        {
			return false;
        }
    }
}
		