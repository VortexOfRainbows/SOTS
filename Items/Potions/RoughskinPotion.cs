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
			this.SetResearchCost(20);
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
            Item.useStyle = ItemUseStyleID.EatFood;      
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<Fragments.FragmentOfEarth>(), 1).AddIngredient(null, "Snakeskin", 8).AddIngredient(ItemID.Blinkroot, 1).AddTile(TileID.Bottles).Register();
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<Fragments.FragmentOfEarth>(), 1).AddIngredient(null, "SeaSnake", 1).AddIngredient(ItemID.Blinkroot, 1).AddTile(TileID.Bottles).Register();
		}
	}
}