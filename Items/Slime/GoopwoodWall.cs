using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class GoopwoodWall : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<GoopwoodWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<Wormwood>(), 1);
			recipe.AddRecipe();
		}
	}
	public class GoopwoodWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 7;
			drop = ModContent.ItemType<GoopwoodWall>();
			AddMapEntry(new Color(120, 54, 16));
		}
	}
}