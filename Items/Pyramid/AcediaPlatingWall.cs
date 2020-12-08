using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class AcediaPlatingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acedia Plating Wall");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.rare = 9;
			item.consumable = true;
			item.createWall = mod.WallType("AcediaPlatingWallWall");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AcediaPlating", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
	public class AcediaPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = true;
			dustType = mod.DustType("AcedianDust");
			drop = mod.ItemType("AcediaPlatingWall");
			AddMapEntry(new Color(180, 64, 170));
		}
	}
	public class UnsafeAcediaWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallLargeFrames[Type] = (byte)2;
			Main.wallHouse[Type] = false;
			dustType = mod.DustType("AcedianDust");
			drop = mod.ItemType("AcediaPlatingWall");
			AddMapEntry(new Color(180, 64, 170));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
		}
	}
}