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
    public class FrostShard : ModProjectile 
    {	int prepareFire = -2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Shard");
			
		}
		
        public override void SetDefaults()
        {
			projectile.aiStyle = 1;
			projectile.height = 32;
			projectile.width = 34;
			projectile.penetrate = 3;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
			projectile.alpha = 200;
		}
		public override void AI()
		{
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
			if(prepareFire == -2)
			{
				prepareFire = Main.rand.Next(61,480);
			}
			projectile.alpha -= 2;
			int IcyAbomInt = -1;
			for(int i = 0; i < 200; i++)
			{
				NPC IcyAbom = Main.npc[i];
				if(IcyAbom.type == mod.NPCType("ShardKing") && IcyAbom.active)
				{
					IcyAbomInt = i;
					break;
				}
			}
			if(IcyAbomInt >= 0)
			{
				NPC IcyAbom = Main.npc[IcyAbomInt];
				Player target = Main.player[IcyAbom.target];
				
				if(prepareFire > 0)
				{
				projectile.velocity.Y = 0;
				prepareFire -= 4;
				projectile.rotation = MathHelper.ToRadians(prepareFire);
				}
				if(prepareFire <= 0 && prepareFire > -5)
				{
					prepareFire = -6;
					projectile.hostile = true;
					projectile.friendly = false;
				   float shootToX = target.position.X + (float)target.width * 0.5f - projectile.Center.X;
				   float shootToY = target.position.Y + (float)target.height * 0.5f  - projectile.Center.Y - 60;
				   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
							distance = 3.25f / distance;
						  
						   shootToX *= distance * 5;
						   shootToY *= distance * 5;
				   
				   projectile.velocity.X = shootToX;
				   projectile.velocity.Y = shootToY;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			 Vector2 position = projectile.Center;
            Main.PlaySound(SoundID.Item14, (int)position.X, (int)position.Y);
            int radius = 3;     //this is the explosion radius, the highter is the value the bigger is the explosion
			bool snowNearby = false;
			for (int x = -radius; x <= radius; x++)
				{
					for (int y = -radius; y <= radius; y++)
					{
						int xPosition = (int)(x + position.X / 16.0f);
						int yPosition = (int)(y + position.Y / 16.0f);
	 
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
						{
							if(Main.tile[xPosition, yPosition].type == 147)
							snowNearby = true;
						}
					}
				}
				
			if(!snowNearby)
			{
				for (int x = -radius; x <= radius; x++)
				{
					for (int y = -radius; y <= radius; y++)
					{
						int xPosition = (int)(x + position.X / 16.0f);
						int yPosition = (int)(y + position.Y / 16.0f);
	 
						if (Math.Sqrt(x * x + y * y) <= radius + 0.5)   //this make so the explosion radius is a circle
						{
							if(Main.tile[xPosition, yPosition].type == 0)
							{
							Main.tile[xPosition, yPosition].type = 147;
							}
							else if(Main.tile[xPosition, yPosition].type == 1)
							{
							Main.tile[xPosition, yPosition].type = 161;
							}
							else if(Main.tile[xPosition, yPosition].type == 2)
							{
							Main.tile[xPosition, yPosition].type = 147;
							}
							else if(!Main.tile[xPosition, yPosition - 1].active())
							{
							Main.tile[xPosition, yPosition - 1].type = 147;
							}
						
						}
					}
				}
			}
		
	}
}
}
	