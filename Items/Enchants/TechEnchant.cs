using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class TechEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tech Relic");
			Tooltip.SetDefault("Creates fireworks under your cursor");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 30;   
            item.value = 50000;
            item.rare = 9;
			item.expert = true;
			item.accessory = true;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(ItemID.Wrench,1);
			recipe.AddIngredient(ItemID.Wire,1000);
			recipe.AddIngredient(ItemID.Actuator,100);
			recipe.AddIngredient(ItemID.RedRocket,1000);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
			player.manaCost -= 14;
		Vector2 vector14;
					if(timer == 50)
					{
				
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                Projectile.NewProjectile(vector14.X, vector14.Y, 0, 1000,  mod.ProjectileType("ExplodingShot"), 50, 1, Main.myPlayer, 0.0f, 2);
            
					timer = 0;
					}
					
		}
		
	}
}
