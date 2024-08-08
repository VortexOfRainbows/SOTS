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
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<Harmony>();   
            Item.buffTime = 21900;  
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1).AddIngredient(ItemID.PrincessFish, 1).AddIngredient(ItemID.SoulofLight, 1).AddIngredient(ItemID.UnicornHorn, 1).AddIngredient(ItemID.Daybloom, 1).AddTile(TileID.Bottles).Register();
		}
	}
}