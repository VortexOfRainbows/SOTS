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


namespace SOTS.Items
{
	public class RollofDuctTape : ModItem
	{ 	int firerate = 0;
		bool overheated = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roll of Duct Tape");
			Tooltip.SetDefault("Fires the weapon from your last inventory slot at your cursor when an enemy is near\nThe weapon will deal half damage");
		}
		public override void SetDefaults()
		{
            
            item.width = 24;     
            item.height = 24;     
            item.value = 250000;
            item.rare = 7;
			item.accessory = true;
			
		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{ 
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
			firerate++;
			Item item1 = player.inventory[49];
			int projectileType = item1.shoot;
			int damage = (int)(0.5f * item1.damage);
			int reload = item1.useTime;
			int useAmmo = item1.useAmmo;
			float shootSpeed = 0.8f * item1.shootSpeed;
			float knockBack = item1.knockBack;
			
			if(useAmmo != 0)
			{
				projectileType = mod.ProjectileType("FireProj");
				for(int i = 0; i < 58; i++)
				{
			Item item2 = player.inventory[i];
				if(useAmmo == item2.ammo)
					{
					int projectileAmmo = item2.shoot;
					projectileType = projectileAmmo;
					damage += item2.damage;
					break;
					}
				}
				
			}
			
			if(item1.summon == false && item1.type != mod.ItemType("Obliterator") && item1.type != mod.ItemType("TrinityScatter") && item1.type != mod.ItemType("TrinityCrossbow") && item1.type != mod.ItemType("HallowedCrossbow") && item1.type != mod.ItemType("HallowedScatter") && item1.type != mod.ItemType("WormWoodScatter") && item1.type != mod.ItemType("WormWoodCrossbow") &&  item1.type != mod.ItemType("MargritBlaster") && item1.type != 71  && item1.type != 72 && item1.type != 73 && item1.type != 74)
			{	
				
			for(int i = 0; i < 200; i++)
			{
						if(firerate >= reload && item1.channel == false)
						{
					   //Enemy NPC variable being set
					   NPC target = Main.npc[i];

					   //Getting the shooting trajectory
					   float distanceFromX = target.Center.X - vector14.X;
					   float distanceFromY = target.Center.Y - vector14.Y;
					   float shootToX = target.Center.X - player.Center.X;
					   float shootToY = target.Center.Y - player.Center.Y;
					   float distance = (float)System.Math.Sqrt((double)(shootToX * shootToX + shootToY * shootToY));
					   float distance2 = (float)System.Math.Sqrt((double)(distanceFromX * distanceFromX + distanceFromY * distanceFromY));

					   //If the distance between the projectile and the live target is active
						   if(distance2 < 90f && !target.friendly && target.active)
						   {
								  
									   //Dividing the factor of 3f which is the desired velocity by distance
									   distance = shootSpeed / distance;
						   
									   //Multiplying the shoot trajectory with distance times a multiplier if you so choose to
									   shootToX *= distance * 5;
									   shootToY *= distance * 5;
						   
									   //Shoot projectile and set ai back to 0
									   Projectile.NewProjectile(player.Center.X, player.Center.Y, shootToX, shootToY, projectileType, damage, knockBack, Main.myPlayer, 0f, 0f); //Spawning a projectile
									  
										firerate = 0;
										
								   
							}
						}
				
			
				}
			}
			
			
			
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MargritRing", 1);
			recipe.AddIngredient(null, "ShadowflameBracer", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
