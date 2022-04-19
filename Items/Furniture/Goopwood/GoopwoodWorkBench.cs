using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Slime;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodWorkBench : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.Size = new Vector2(32, 18);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<GoopwoodWorkBenchTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
    public class GoopwoodWorkBenchTile : Workbench<GoopwoodWorkBench>
    {
		protected override void SetDefaults(TileObjectData t)
		{
			Main.tileLighted[Type] = true;
			base.SetDefaults(t);
		}
    }
}