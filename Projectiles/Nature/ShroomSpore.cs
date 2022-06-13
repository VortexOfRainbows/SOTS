using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Nature
{    
    public class ShroomSpore : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spore");
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(512);
            AIType = 512;
			Projectile.height = 16;
			Projectile.width = 16;
			Projectile.penetrate = 4;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = true;
			Projectile.alpha = 100;
			Projectile.timeLeft = 100;
		}
		public override void AI()
		{
			Projectile.alpha = 260 - Projectile.timeLeft;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.NextBool(7))
				target.AddBuff(BuffID.Confused, 90, false);
			Projectile.damage = (int)(Projectile.damage * 0.75f);
			Projectile.friendly = Projectile.penetrate > 2;
		}
	}
}
		