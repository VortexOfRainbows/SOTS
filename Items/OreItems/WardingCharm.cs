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

			item.width = 18;
			item.height = 34;
            item.value = Item.sellPrice(0, 0, 35, 0);
			item.rare = 1;
			item.maxStack = 1;
			item.accessory = true;
			item.defense = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TinBar, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1); 
			recipe.AddIngredient(null, "FragmentOfNature", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Bezoar); //obtainable prehardmode
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfEvil", 10);
			recipe.AddIngredient(ItemID.CrimtaneBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.AdhesiveBandage);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfEvil", 10);
			recipe.AddIngredient(ItemID.DemoniteBar, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.Vitamins);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfTide", 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(ItemID.Nazar); //obtainable prehardmode
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfChaos", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.Megaphone);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfOtherworld", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.FastClock);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfInferno", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.ArmorPolish);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.Blindfold);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(null, "FragmentOfEarth", 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(ItemID.TrifoldMap);
			recipe.AddRecipe();
		}
	}
}