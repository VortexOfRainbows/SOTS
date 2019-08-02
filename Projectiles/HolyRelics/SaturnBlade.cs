using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.HolyRelics
{    
    public class SaturnBlade : ModProjectile 
    {	int wait = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Saturnus Blade");
			
		}
		
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 360;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true; 
            projectile.melee = true; 
            projectile.aiStyle = 0; //18 is the demon scythe style
			projectile.alpha = 0;
		}
		public override void AI()
		{
			
			Player player  = Main.player[projectile.owner];

			double deg = (double) projectile.ai[1]; 
			double rad = deg * (Math.PI / 180); 
			double dist = 96; 


			projectile.position.X = player.Center.X - (int)(Math.Cos(rad) * dist) - projectile.width/2;
			projectile.position.Y = player.Center.Y - (int)(Math.Sin(rad) * dist) - projectile.height/2;
		 
		 
			projectile.ai[1] += 2f;
			projectile.rotation += 0.3f;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)((target.lifeMax/50));
			crit = false;
			if(target.lifeMax < 200)
			{
				damage += 4;
				damage += (int)(target.life/10);
			}
			if(target.boss == true)
			{
				damage += 3;
			}
			damage += (int)(target.damage/10);
			damage += (int)(target.defense/10);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
			Player player  = Main.player[target.target];	
			if(target.type == mod.NPCType("Libra"))
			{
				if(player.name == "Saturn" || player.name == "S")
				{
					Main.NewText("Trying out Saturn this time huh?", 255, 255, 255);
				}
				else
				{
					Main.NewText("Yeah... The sprites do look pretty similar...", 255, 255, 255);
					
				}
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("BladeMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
		}
	}
}
		
			