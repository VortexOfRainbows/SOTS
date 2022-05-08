using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nvidia
{
	public class EvostoneTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 37; //obsidian
			drop = ModContent.ItemType<Evostone>();
			AddMapEntry(new Color(31, 39, 57));
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.Marble, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class Evostone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evostone");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvostoneTile>();
		}
	}
	public class EvostoneBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 37; //obsidian
			drop = ModContent.ItemType<EvostoneBrick>();
			AddMapEntry(new Color(46, 63, 77));
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
	}
	public class EvostoneBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evostone Brick");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<EvostoneBrickTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Evostone>(), 2);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class EvostoneBrickWallTile : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = 37;
			drop = ModContent.ItemType<EvostoneBrickWall>();
			AddMapEntry(new Color(25, 38, 49));
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
	}
	public class EvostoneBrickWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evostone Brick Wall");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<EvostoneBrickWallTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EvostoneBrick>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<EvostoneBrick>(), 1);
			recipe.AddRecipe();
		}
	}
	public class DarkShinglesTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 37; //obsidian
			drop = ModContent.ItemType<DarkShingles>();
			AddMapEntry(new Color(82, 56, 103));
			soundType = SoundID.Tink;
			soundStyle = 2;
		}
	}
	public class DarkShingles : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dark Shingles");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<DarkShinglesTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EvostoneBrick>(), 2);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}