using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace SOTS.Items.Potions
{
	public class VibePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibe Potion");
			Tooltip.SetDefault("Increases attack speed by 20% while not moving and by 5% while moving\nIncreases void regeneration and life regen while not moving");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
            item.buffType = mod.BuffType("GoodVibes");   
            item.buffTime = 21630; //around 6 minutes
            item.UseSound = SoundID.Item3;            
            item.useStyle = ItemUseStyleID.EatingUsing;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "FragmentOfNature", 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}