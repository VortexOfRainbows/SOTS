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
			Tooltip.SetDefault("");
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
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfNature", 10).AddTile(TileID.Anvils).ReplaceResult(ItemID.Bezoar);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfEvil", 10).AddIngredient(ItemID.CrimtaneBar, 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.AdhesiveBandage);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfEvil", 10).AddIngredient(ItemID.DemoniteBar, 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.Vitamins);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfTide", 10).AddTile(TileID.Anvils).ReplaceResult(ItemID.Nazar);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfChaos", 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.Megaphone);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfOtherworld", 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.FastClock);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfInferno", 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.ArmorPolish);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfPermafrost", 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.Blindfold);
			CreateRecipe(1).AddIngredient(this, 1).AddIngredient(null, "FragmentOfEarth", 10).AddTile(TileID.MythrilAnvil).ReplaceResult(ItemID.TrifoldMap);
		}
	}
}