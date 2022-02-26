using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chaos
{
	public class PhaseBar : ModItem
	{
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Bar");
			Tooltip.SetDefault("'It borders on the edge of reality'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.IronBar);
			item.width = 30;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 999;
			item.placeStyle = 9;
			item.createTile = ModContent.TileType<TheBars>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PhaseOre>(), 12);
			recipe.AddIngredient(ItemID.CrystalShard, 4);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 1);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}