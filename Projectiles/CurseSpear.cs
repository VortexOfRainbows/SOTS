using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class CurseSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Impale");
			
		}
		
        public override void SetDefaults()
        {
            projectile.CloneDefaults(64);
            aiType = 64;
            projectile.melee = true;
			projectile.alpha = 0; 
		}
		public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			if(projectile.owner == Main.myPlayer)
			{
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, 496, projectile.damage, projectile.knockBack * 0.3f, player.whoAmI);
				Main.projectile[proj].melee = true;
				Main.projectile[proj].magic = false;
            }
		}
	}
}