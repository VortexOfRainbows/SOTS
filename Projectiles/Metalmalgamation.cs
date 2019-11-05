using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles
{
    public class Metalmalgamation : ModProjectile 
    {	int Epic = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metalmalgamation");
			
		}
 
        public override void SetDefaults()
        {
            projectile.extraUpdates = 0;
            projectile.width = 16;
            projectile.height = 16;	       
            projectile.aiStyle = 99; 
            projectile.friendly = true;	
            projectile.penetrate = -1;	
			projectile.melee = true;	        
            ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 11f;
            ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 225f;
            ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 11f;
        }
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Epic += 4;
			Vector2 rotateArea = new Vector2(7, 0).RotatedBy(MathHelper.ToRadians(Epic));
			if(Epic % 9 == 0)
			{
				int shard = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, rotateArea.X, rotateArea.Y, mod.ProjectileType("MargritBoltFriendly"), (int)(projectile.damage * .65f), projectile.knockBack, player.whoAmI);
				Main.projectile[shard].penetrate = 1;
				Main.projectile[shard].timeLeft = 27;
				Main.projectile[shard].alpha = 125;
				
				rotateArea = rotateArea.RotatedBy(MathHelper.ToRadians(180));
				shard = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, rotateArea.X, rotateArea.Y, mod.ProjectileType("MargritBoltFriendly"), (int)(projectile.damage * .65f), projectile.knockBack, player.whoAmI);
				Main.projectile[shard].penetrate = 1;
				Main.projectile[shard].timeLeft = 27;
				Main.projectile[shard].alpha = 125;
				
				Main.PlaySound(SoundID.Item11, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			}
		}
    }
}