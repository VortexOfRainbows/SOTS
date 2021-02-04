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
using SOTS.Void;
using SOTS.NPCs.ArtificialDebuffs;

namespace SOTS.Projectiles.Pyramid
{    
    public class RubyBurst : ModProjectile 
    {	float distance = 14f;  
		float rotation = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruby Burst");
			
		}
        public override void SetDefaults()
        {
			projectile.height = 2;
			projectile.width = 2;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.melee = true;
			projectile.timeLeft = 90;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 70;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[10];
		Vector2[] trailPos2 = new Vector2[10];
		Vector2[] trailPos3 = new Vector2[10];
		Vector2[] trailPos4 = new Vector2[10];
		public void cataloguePos(Vector2 catalogue, Vector2[] trialArray)
		{
			Vector2 current = catalogue;
			for (int i = 0; i < trialArray.Length; i++)
			{
				Vector2 previousPosition = trialArray[i];
				trialArray[i] = current;
				current = previousPosition;
			}
		}
		public override void AI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
            }
			Player player = Main.player[projectile.owner];
			Vector2 circularLocation = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation));
			cataloguePos(circularLocation + projectile.Center, trailPos);
			circularLocation = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
			cataloguePos(circularLocation + projectile.Center, trailPos2);
			circularLocation = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
			cataloguePos(circularLocation + projectile.Center, trailPos3);
			circularLocation = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
			cataloguePos(circularLocation + projectile.Center, trailPos4);
			rotation += projectile.direction * 12.5f;
			projectile.width += 1;
			projectile.height += 1;
			projectile.Center -= new Vector2(0.5f, 0.5f);
			projectile.velocity *= 0.98f;
			if(projectile.timeLeft <= 30)
            {
				projectile.friendly = false;
            }
		}
        public override void Kill(int timeLeft)
		{
			base.Kill(timeLeft);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 circularLocation = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation)) + projectile.Center;
			if (runOnce)
				return false;
			Vector2 current = circularLocation;
			Draw(spriteBatch, trailPos, current);
			current = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90)) + projectile.Center;
			Draw(spriteBatch, trailPos2, current);
			current = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180)) + projectile.Center;
			Draw(spriteBatch, trailPos3, current);
			current = new Vector2(distance + projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270)) + projectile.Center;
			Draw(spriteBatch, trailPos4, current);
			return false;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2[] trailArray, Vector2 current)
		{
			Texture2D texture2 = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = current;
			Color color = new Color(255, 100, 100, 0);
			color *= (projectile.timeLeft - 30) / 90f;
			for (int k = 0; k < trailArray.Length; k++)
			{
				if (k >= projectile.timeLeft / 3 - 10)
					return;
				float scale = projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
				scale *= 1f;
				if (trailArray[k] == Vector2.Zero)
				{
					return;
				}
				Vector2 drawPos = trailArray[k] - Main.screenPosition;
				Vector2 currentPos = trailArray[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color *= 0.95f;
				float max = betweenPositions.Length() / (4 * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j <= 1)
						{
							x = 0;
							y = 0;
						}
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin2, scale, projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
	}
}
		