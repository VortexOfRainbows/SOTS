using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Enchants
{
	public class MagicalEnchant : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magical Relic");
			Tooltip.SetDefault("Reduces magic damage by 10%\nGrants 20% redudced mana usage\nCreates a star explosion where your cursor is located\nDev Item");
		}
		public override void SetDefaults()
		{
      
            item.width = 30;     
            item.height = 28;   
            item.value = 50000;
            item.rare = 8;
			item.expert = true;
			item.accessory = true;
			item.defense = 4;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Manalet", 1);
			recipe.AddIngredient(null, "AntimaterialMandible", 5);
			recipe.AddIngredient(null, "CoreOfCreation", 1);
			recipe.AddIngredient(null, "CoreOfStatus", 1);
			recipe.AddIngredient(null, "StarEnchant", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			timer += 1;
			player.magicDamage -= 0.10f ;
			player.manaCost -= 20;
			Vector2 vector14;
			if(timer == 10)
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
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0, 612, 9, 1, Main.myPlayer, 0.0f, 1);
            
					timer = 0;
					}
		}
	}
}
