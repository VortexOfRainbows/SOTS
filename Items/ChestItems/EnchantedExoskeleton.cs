using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.ChestItems
{
	public class EnchantedExoskeleton : ModItem
	{ 	int firerate = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Exoskeleton");
			Tooltip.SetDefault("Grants the ability to wield up to 5 enchanted boomerangs at a time\nEnchanted boomerangs ingnore enemy defense\nRedirects boomerangs towards your cursor");
		}
		public override void SetDefaults()
		{
            
            item.width = 40;     
            item.height = 42;     
            item.value = 52500;
            item.rare = 3;
			item.accessory = true;
			item.defense = 5;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.EnchantedBoomerangs = 0;
					  
			  Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
						
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
					float minDist = 2800;
					int target2 = -1;
					float dX = 0f;
					float dY = 0f;
					float distance = 0;
					float speed = 1.8f;
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner] && projectile.type == ProjectileID.EnchantedBoomerang)
					{
							dX = vector14.X - projectile.Center.X;
							dY = vector14.Y - projectile.Center.Y;
								
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
						   
							projectile.velocity += new Vector2(dX * speed, dY * speed);
							projectile.tileCollide = false;
						
						
					}
								
				
			}
				
				
				for(int i = 0; i < 50; i++)
				{
					Item item1 = player.inventory[i];
					if(item1.type == ItemID.EnchantedBoomerang && modPlayer.EnchantedBoomerangs < 5)
					{
						modPlayer.EnchantedBoomerangs++;
					}
				}
			
		}
	}
}
