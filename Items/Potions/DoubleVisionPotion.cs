using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Potions
{
	public class DoubleVisionPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Double Vision Potion");
			Tooltip.SetDefault("Adds additional lines to your fishing rod, stacks with itself\nMaxes out at 6 additional lines");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 32;
            item.value = Item.sellPrice(0, 0, 10, 0);
			item.rare = 2;
			item.maxStack = 30;
            item.buffType = mod.BuffType("DoubleVision");   
			int minute = 3600;
            item.buffTime = minute * 6;
            item.UseSound = SoundID.Item3;            
            item.useStyle = 2;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
			
		}
		public override bool UseItem(Player player) 
		{
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(modPlayer.doubledActive == 0)
			{
				modPlayer.doubledAmount = 0;
			}
			modPlayer.doubledAmount++;
			if(modPlayer.doubledAmount > 6)
			{
				modPlayer.doubledAmount = 6;
			}
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "Curgeon", 1);
			recipe.AddIngredient(null, "PhantomFish", 1);
			recipe.AddIngredient(null, "SeaSnake", 1);
			recipe.AddIngredient(null, "FragmentOfTide", 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}