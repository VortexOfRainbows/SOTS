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

namespace SOTS.Projectiles 
{    
    public class PatronusShot : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spectre");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.timeLeft = 15000;
			projectile.magic = true;
		}
		
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			owner.statLife += 1;
			owner.HealEffect(1);
			if(target.type == mod.NPCType("Libra"))
			{
					Main.NewText("Now you get to try out my favorite!", 255, 255, 255);
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("LMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
            
		}
		public override void AI()
		{projectile.alpha = 255;
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 10, 1, 63);
}
	}
}
		
			