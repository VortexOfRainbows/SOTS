using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingToilet : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 26);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingToiletTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingToiletTile : Chair<NaturePlatingToilet>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}