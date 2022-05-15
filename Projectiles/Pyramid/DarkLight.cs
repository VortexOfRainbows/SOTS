using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class DarkLight : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Light");
		}
        public override void SetDefaults()
        {
            Projectile.alpha = 255;
            Projectile.width = 216;
            Projectile.height = 216; 
            Projectile.timeLeft = 90;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
        public override bool PreDraw(ref Color lightColor)
        {
			float percent = Projectile.ai[1] / 90;
			float lightIntesity = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			float counter = Projectile.ai[0] * rotationSpeed;
			float mult = new Vector2(-11.8f, 0).RotatedBy(MathHelper.ToRadians(180f * percent)).Y;
			for (int i = 0; i < 5; i++)
			{
				Color color = new Color(50, 0, 150, 0);
				Vector2 rotationAround = new Vector2((12 + mult) * Projectile.scale, 0).RotatedBy(MathHelper.ToRadians(72 * i + counter));
				float rotation2 = rotationAround.ToRotation() - MathHelper.ToRadians(90);
				float dist = 2;
				int maxDist = 10 + (int)(lightIntesity * 55);
				float scale2 = 1f + 1.5f * lightIntesity;
				for (int k = 0; k < maxDist; k++)
				{
					scale2 += 0.1f + 0.15f * lightIntesity;
					Vector2 fromCenter = Projectile.Center + new Vector2(0, dist * Projectile.scale).RotatedBy(rotation2);
					int width = (int)(2 * scale2);
					int height = 2;
					Vector2 drawOrigin = new Vector2(width * 0.5f, 0);
					Main.spriteBatch.Draw(texture, fromCenter - Main.screenPosition, new Rectangle(0, 0, width, height), color * lightIntesity * ((maxDist - k) / (float)maxDist), Projectile.rotation + rotation2, drawOrigin, 1, SpriteEffects.None, 0f);
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
			Projectile.ai[0]++;
			Projectile.ai[1]++;
			if (runOnce)
            {
                runOnce = false;
                rotationSpeed = Main.rand.NextFloat(2, 5) * (Main.rand.Next(2) * 2 - 1);
            }
        }
	}
}
		
			