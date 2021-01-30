using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Tide
{    
    public class CoconutShrapnel : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Shrapnel");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616;
			projectile.alpha = 255;
			projectile.timeLeft = 240;
			projectile.width = 4;
			projectile.height = 4;
			projectile.tileCollide = true;
			projectile.extraUpdates = 3;
			projectile.penetrate = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 60;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.OnFire, 300); //fire for 5 seconds
			}
		}
		public override void AI()
		{
			if (projectile.penetrate != 5)
				aiType = 0;
			projectile.velocity.Y += 0.09f;
			projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 212);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			if(Main.rand.NextBool(3))
			{
				num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, DustID.Fire);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.8f;
				Main.dust[num1].velocity += projectile.velocity * -0.1f;
			}
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 212);
				Main.dust[num1].noGravity = false;
				Main.dust[num1].velocity += -2f * projectile.velocity.SafeNormalize(Vector2.Zero);
			}
		}
    }
}
		
			