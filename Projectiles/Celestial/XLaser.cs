using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class XLaser : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Light");
		}
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 90;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
		}
		bool runOnce = true;
		float bonusRotation = 420f;
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
		private float lerpMath(float point, float point2, float scale)
		{
			return point * scale + point2 * (1f - scale);
		}
		public override void AI()
		{
			if (projectile.timeLeft > 30)
			{
				projectile.scale += 0.0125f;
			}
			else
			{
				projectile.scale -= 0.025f;
			}
			projectile.rotation = MathHelper.ToRadians(bonusRotation);
			if (runOnce)
				Main.PlaySound(SoundID.Item92, (int)projectile.Center.X, (int)projectile.Center.Y);
			runOnce = false;
			bonusRotation = lerpMath(420f, 0, (projectile.timeLeft / 90f) * (projectile.timeLeft / 90f));
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.1f / 255f);
		}
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item93, (int)projectile.Center.X, (int)projectile.Center.Y);
			if(Main.netMode != 1)
			{
				for(int i = 0; i < 4; i++)
                {
					Vector2 velo = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(90 * i + 45));
					Projectile.NewProjectile(projectile.Center, velo, ModContent.ProjectileType<CelestialLightning>(), projectile.damage, 0, Main.myPlayer, 1);
				}
			}
        }
    }
}