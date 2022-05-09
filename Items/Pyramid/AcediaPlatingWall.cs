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
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Cyan;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<UnsafeAcediaWallWall>();
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
	}
}