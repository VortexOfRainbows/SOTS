using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class StarEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Relic");
			Tooltip.SetDefault("Creates a star explosion where your cursor is located\n5% decreased mana usage");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 32;   
            item.value = 50000;
            item.rare = 8;
			item.expert = true;
			item.accessory = true;
			item.defense = 3;
			item.shootSpeed = 0;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(ItemID.JungleHat,1);
			recipe.AddIngredient(ItemID.JungleShirt,1);
			recipe.AddIngredient(ItemID.JunglePants,1);
			recipe.AddIngredient(ItemID.Starfury,1);
			recipe.AddIngredient(ItemID.FallenStar,12);
			recipe.AddIngredient(ItemID.PurificationPowder,50);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
			player.manaCost -= 5;
			Vector2 vector14;
					if(timer == 12)
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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, 612, 1, 1, Main.myPlayer, 0.0f, 1);
            
					timer = 0;
					}
					
		}
		
	}
}
