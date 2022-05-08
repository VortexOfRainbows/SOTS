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
			Tooltip.SetDefault("Prevents other buffs from decaying while active\nDoesn't work on most buffs with below 30 seconds remaining");
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Harmony>();   
            Item.buffTime = 21900;  
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
			recipe.AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1);
			recipe.AddIngredient(ItemID.PrincessFish, 1);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.UnicornHorn, 1);
			recipe.AddIngredient(ItemID.Daybloom, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}