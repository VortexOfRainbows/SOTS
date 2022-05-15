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
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(32, 18);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodWorkBenchTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 10).Register();
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