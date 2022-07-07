using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class WardingCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warding Charm");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 35, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 1;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CopperBar, 20).AddTile(TileID.Anvils).Register();
			CreateRecipe(1).AddIngredient(ItemID.TinBar, 20).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.Bezoar).AddIngredient(this, 1).AddIngredient<FragmentOfNature>(10).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.AdhesiveBandage).AddIngredient(this, 1).AddIngredient<FragmentOfEvil>(10).AddIngredient(ItemID.CrimtaneBar, 10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.Vitamins).AddIngredient(this, 1).AddIngredient<FragmentOfEvil>(10).AddIngredient(ItemID.DemoniteBar, 10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.Nazar).AddIngredient(this, 1).AddIngredient<FragmentOfTide>(10).AddTile(TileID.Anvils).Register();
			Recipe.Create(ItemID.Megaphone).AddIngredient(this, 1).AddIngredient<FragmentOfChaos>(10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.FastClock).AddIngredient(this, 1).AddIngredient<FragmentOfOtherworld>(10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.ArmorPolish).AddIngredient(this, 1).AddIngredient<FragmentOfInferno>(10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.Blindfold).AddIngredient(this, 1).AddIngredient<FragmentOfPermafrost>(10).AddTile(TileID.MythrilAnvil).Register();
			Recipe.Create(ItemID.TrifoldMap).AddIngredient(this, 1).AddIngredient<FragmentOfEarth>(10).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}