using SOTS.Buffs;
using SOTS.Items.Fragments;
using SOTS.Items.SpecialDrops;
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
			item.width = 20;
			item.height = 26;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
            item.buffType = ModContent.BuffType<Bluefire>();   
            item.buffTime = 14700;  
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
			recipe.AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 1);
			recipe.AddIngredient(ItemID.LivingFireBlock, 1);
			recipe.AddIngredient(ItemID.Fireblossom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}