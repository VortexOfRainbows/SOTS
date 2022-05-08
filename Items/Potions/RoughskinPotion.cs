using SOTS.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class RoughskinPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Roughskin Potion");
			Tooltip.SetDefault("Increases defense by 4 and damage by 4%");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Roughskin>();   
            Item.buffTime = 19000;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = 2;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfEarth>(), 1);
			recipe.AddIngredient(null, "Snakeskin", 8);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ModContent.ItemType<Fragments.FragmentOfEarth>(), 1);
			recipe.AddIngredient(null, "SeaSnake", 1);
			recipe.AddIngredient(ItemID.Blinkroot, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}