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
using SOTS.Common.GlobalNPCs;

namespace SOTS.Projectiles.Pyramid
{    
    public class RubyBurst : ModProjectile 
    {	float distance = 14f;  
		float rotation = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ruby Burst");
			
		}
        public override void SetDefaults()
        {
			Projectile.height = 2;
			Projectile.width = 2;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 90;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 70;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
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
			Player player = Main.player[Projectile.owner];
			Vector2 circularLocation = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation));
			cataloguePos(circularLocation + Projectile.Center, trailPos);
			circularLocation = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90));
			cataloguePos(circularLocation + Projectile.Center, trailPos2);
			circularLocation = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180));
			cataloguePos(circularLocation + Projectile.Center, trailPos3);
			circularLocation = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270));
			cataloguePos(circularLocation + Projectile.Center, trailPos4);
			rotation += Projectile.direction * 12.5f;
			Projectile.width += 1;
			Projectile.height += 1;
			Projectile.Center -= new Vector2(0.5f, 0.5f);
			Projectile.velocity *= 0.98f;
			if(Projectile.timeLeft <= 30)
            {
				Projectile.friendly = false;
            }
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Vector2 circularLocation = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation)) + Projectile.Center;
			if (runOnce)
				return false;
			Vector2 current = circularLocation;
			Draw(trailPos, current);
			current = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 90)) + Projectile.Center;
			Draw(trailPos2, current);
			current = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 180)) + Projectile.Center;
			Draw(trailPos3, current);
			current = new Vector2(distance + Projectile.width / 2, 0).RotatedBy(MathHelper.ToRadians(rotation + 270)) + Projectile.Center;
			Draw(trailPos4, current);
			return false;
		}
		public void Draw(Vector2[] trailArray, Vector2 current)
		{
			Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin2 = new Vector2(texture2.Width * 0.5f, texture2.Height * 0.5f);
			Vector2 previousPosition = current;
			Color color = new Color(255, 100, 100, 0);
			color *= (Projectile.timeLeft - 30) / 90f;
			for (int k = 0; k < trailArray.Length; k++)
			{
				if (k >= Projectile.timeLeft / 3 - 10)
					return;
				float scale = Projectile.scale * (trailArray.Length - k) / (float)trailArray.Length;
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
						Main.spriteBatch.Draw(texture2, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin2, scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
	}
}
		