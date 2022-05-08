using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class NaturePlatingWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<NaturePlatingWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<NaturePlating>(), 1);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.Tungsten;
			drop = ModContent.ItemType<NaturePlatingWall>();
			AddMapEntry(Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.2f));
		}
	}
	public class NaturePlatingPanelWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<NaturePlatingPanelWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<NaturePlating>(), 1);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingPanelWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.Tungsten;
			drop = ModContent.ItemType<NaturePlatingPanelWall>();
			AddMapEntry(Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.3f));
		}
	}
	public class EarthenPlatingWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<EarthenPlatingWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.Iron;
			drop = ModContent.ItemType<EarthenPlatingWall>();
			AddMapEntry(Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.2f));
		}
	}
	public class EarthenPlatingPanelWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<EarthenPlatingPanelWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingPanelWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.Iron;
			drop = ModContent.ItemType<EarthenPlatingPanelWall>();
			AddMapEntry(Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.3f));
		}
	}
	public class EarthenPlatingBeam : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 16;
			Item.height = 18;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<EarthenPlatingBeamWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(ModContent.ItemType<EarthenPlating>(), 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingBeamWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = DustID.Iron;
			drop = ModContent.ItemType<EarthenPlatingBeam>();
			AddMapEntry(SOTSTile.EarthenPlatingColor);
		}
	}
}