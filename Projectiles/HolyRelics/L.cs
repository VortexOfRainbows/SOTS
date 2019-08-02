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

namespace SOTS.Projectiles.HolyRelics
{    
    public class L : ModProjectile 
    {	int wait = -1;
			int num1 = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Attack Libra");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(440);
            aiType = 440; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.width = 48;
			projectile.height = 48;
			projectile.friendly = true;
			projectile.timeLeft = 6;
		}
		public override void AI()
		{	
			projectile.alpha = 255;
			
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.life += damage;
            target.immune[projectile.owner] = 5;
			if(target.type == mod.NPCType("Libra"))
			{
					Main.NewText("Now you get to try out my favorite!", 255, 255, 255);
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("LMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
			
		}
	}
}
		
			