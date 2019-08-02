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
    public class Nova : ModProjectile 
    {	int wait = -1;
			int num1 = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
			
		}
		
        public override void SetDefaults()
        {
			projectile.CloneDefaults(440);
            aiType = 440; //18 is the demon scythe style
			projectile.alpha = 255;
			projectile.width = 8;
			projectile.height = 8;
		}
		
		public override void AI()
		{	
			projectile.alpha = 255;
			
			if(wait == -1)
			wait = Main.rand.Next(4);
		
			if(wait == 0)
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 219);
		
			if(wait == 1)
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 220);
		
			if(wait == 2)
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 221);
		
			if(wait == 3)
			num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), 8, 8, 222);
		

			
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
				target.AddBuff(mod.BuffType("CosmicRadiation"), 900, false);
			Player player  = Main.player[target.target];	
			if(target.type == mod.NPCType("Libra"))
			{
				if(player.name == "Omega" || player.name == "O")
				{
					Main.NewText("Trying out Omega this time huh?", 255, 255, 255);
				}
				else
				{
					Main.NewText("Trying to kill me, huh?", 255, 255, 255);
					
				}
					Item.NewItem((int)target.position.X, (int)target.position.Y, target.width, target.height, (mod.ItemType("VortexMaterial")), 1);
					target.timeLeft = 1;
					target.timeLeft--;
					target.life = 0;
					
			}
}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
				target.AddBuff(mod.BuffType("CosmicRadiation"), 900, false);
			
}		
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 1;     //this is the explosion radius, the highter is the value the bigger is the explosion
			int tileType = Main.rand.Next(469);
			if(Main.tileSolid[tileType] == true)
			{
                    int xPosition = (int)(position.X / 16.0f);
                    int yPosition = (int)(position.Y / 16.0f);
				WorldGen.PlaceTile(xPosition, yPosition, tileType);
			}
			return true;
		
	}
	}
}
		
			