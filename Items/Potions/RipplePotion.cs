using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class RipplePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple Potion");
			Tooltip.SetDefault("Release waves of damaging water periodically\nRelease more waves at lower health\nWaves ignore up to 8 defense");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
            item.buffType = mod.BuffType("RippleBuff");   
            item.buffTime = 3600 * 7 + 300;  
            item.UseSound = SoundID.Item3;            
            item.useStyle = 2;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "FragmentOfTide", 1);
			recipe.AddIngredient(ItemID.Waterleaf, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}