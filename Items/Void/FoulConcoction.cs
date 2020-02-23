using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class FoulConcoction: ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foul Concoction");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 4 void");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 18;
            item.value = Item.sellPrice(0, 0, 0, 20);
			item.rare = 2;
			item.maxStack = 333;
			item.ammo = item.type;   
			ItemID.Sets.ItemNoGravity[item.type] = false; 

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Peanut", 1);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Peanut", 2);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.AddIngredient(ItemID.Acorn, 2);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RottenChunk, 1);
			recipe.AddIngredient(ItemID.Gel, 2);
			recipe.AddIngredient(ItemID.Acorn, 2);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Vertebrae, 1);
			recipe.AddIngredient(ItemID.Gel, 2);
			recipe.AddIngredient(ItemID.Acorn, 2);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			while(voidPlayer.voidMeter < voidPlayer.voidMeterMax2 / 10)
			{
			item.stack--;
			voidPlayer.voidMeter += 4;
			}
			
		}
	}
}