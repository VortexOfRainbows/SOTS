using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class DumbHomingLifestealBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dracula");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616;
			projectile.alpha = 255;
			projectile.timeLeft = 220;
			projectile.width = 4;
			projectile.height = 4;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			int healdata = (damage - target.defense) / 40 + 1;
			if(healdata >= 1)
			{
				player.statLife += healdata;
				player.HealEffect(healdata);
			}
		}
		int particleCounter = 0;
		public override void AI()
		{
			projectile.alpha = 255;
			particleCounter++;
			if(particleCounter > 4)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
			}
		}
	}
}
		
			