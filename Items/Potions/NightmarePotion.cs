using SOTS.Buffs;
using SOTS.Items.Fishing;
using SOTS.Items.Fragments;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class NightmarePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<Nightmare>();   
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
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 1).AddIngredient(ModContent.ItemType<TinyPlanetFish>(), 1).AddIngredient(ItemID.SoulofNight, 1).AddIngredient(ItemID.Deathweed, 1).AddTile(TileID.Bottles).Register();
		}
	}
}