using SOTS.Buffs;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class HarmonyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Harmony Potion");
			Tooltip.SetDefault("Prevents other buffs from decaying while active\nDoesn't work on certain shorter buffs");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
            item.buffType = ModContent.BuffType<Harmony>();   
            item.buffTime = 21900;  
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
			recipe.AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1);
			recipe.AddIngredient(ItemID.PrincessFish, 1);
			recipe.AddIngredient(ItemID.LightningBug, 1);
			recipe.AddIngredient(ItemID.UnicornHorn, 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}