using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class HeartSwap : ModProjectile 
    {	int Telep = 0;
	float swapX;
	float swapY;
	int delay;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Swapped");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 34; 
            projectile.timeLeft = 13;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 255;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{   
			/*
			if(target.type == mod.NPCType("Libra"))
			{
				if(player.name == "Xypher" || player.name == "X")
				{
					Main.NewText("Trying out Xypher this time huh?", 255, 255, 255);
				}
				else
				{
					Main.NewText("Lucky ducky.", 255, 255, 255);
					
				}
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("ManaMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					*/
			
			
		}
	}
	
}