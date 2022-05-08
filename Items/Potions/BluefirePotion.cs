using SOTS.Buffs;
using SOTS.Items.Fragments;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class BluefirePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bluefire Potion");
			Tooltip.SetDefault("Killed enemies explode into flames for 40% of the damage dealt to them on the killing blow");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Bluefire>();   
            Item.buffTime = 14700;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatingUsing;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 1);
			recipe.AddIngredient(ItemID.LivingFireBlock, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}