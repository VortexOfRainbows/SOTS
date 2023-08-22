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
			// DisplayName.SetDefault("Coconut Shrapnel");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(616);
            AIType = 616;
			Projectile.alpha = 255;
			Projectile.timeLeft = 240;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 3;
			Projectile.penetrate = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			if (Main.rand.NextBool(5))
			{
				target.AddBuff(BuffID.OnFire, 300); //fire for 5 seconds
			}
		}
		public override void AI()
		{
			if (Projectile.penetrate != 5)
				AIType = 0;
			Projectile.velocity.Y += 0.09f;
			Projectile.alpha = 255;
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, DustID.BubbleBurst_White);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			if(Main.rand.NextBool(3))
			{
				num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, DustID.Torch);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.8f;
				Main.dust[num1].velocity += Projectile.velocity * -0.1f;
			}
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 10; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, DustID.BubbleBurst_White);
				Main.dust[num1].noGravity = false;
				Main.dust[num1].velocity += -2f * Projectile.velocity.SafeNormalize(Vector2.Zero);
			}
		}
    }
}
		
			