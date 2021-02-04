using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class GoldLight : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Light");
		}
        public override void SetDefaults()
        {
            projectile.alpha = 255;
            projectile.width = 160;
            projectile.height = 160; 
            projectile.timeLeft = 90;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.ranged = true;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 120;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			float percent = projectile.ai[1] / 90;
			float lightIntesity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			Texture2D texture = Main.projectileTexture[projectile.type];
			float counter = projectile.ai[0] * rotationSpeed;
			float mult = new Vector2(-11.8f, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			for (int i = 0; i < 3; i++)
			{
				Color color = new Color(255, 165, 0, 0);
				Vector2 rotationAround = new Vector2((12 + mult) * projectile.scale, 0).RotatedBy(MathHelper.ToRadians(120 * i + counter));
				float rotation2 = rotationAround.ToRotation() - MathHelper.ToRadians(90);
				float dist = 0;
				int maxDist = 5 + (int)(lightIntesity * 51);
				float scale2 = 1f + 1.5f * lightIntesity;
				for (int k = 0; k < maxDist; k++)
				{
					scale2 += 0.1f + 0.15f * lightIntesity;
					Vector2 fromCenter = projectile.Center + new Vector2(0, dist * projectile.scale).RotatedBy(rotation2);
					int width = (int)(2 * scale2);
					int height = 2;
					Vector2 drawOrigin = new Vector2(width * 0.5f, 0);
					Main.spriteBatch.Draw(texture, fromCenter - Main.screenPosition, new Rectangle(0, 0, width, height), color * lightIntesity * ((maxDist - k) / (float)maxDist), projectile.rotation + rotation2, drawOrigin, 1, SpriteEffects.None, 0f);
					dist += 2;
				}
			}
			return false;
        }
        float rotationSpeed = 0;
        bool runOnce = true;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
		{
			projectile.ai[0]++;
			projectile.ai[1]++;
			if (runOnce)
            {
                runOnce = false;
                rotationSpeed = Main.rand.NextFloat(2, 5) * (Main.rand.Next(2) * 2 - 1);
            }
        }
	}
}
		
			