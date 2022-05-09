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
			Item.width = 20;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 10, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 30;
            Item.buffType = mod.BuffType("DoubleVision");   
			int minute = 3600;
            Item.buffTime = minute * 6;
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;      
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
			
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