using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class NaturePlatingWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<NaturePlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<NaturePlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class NaturePlatingWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Tungsten;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<NaturePlatingWall>();
			AddMapEntry(Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.2f));
		}
	}
	public class NaturePlatingPanelWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<NaturePlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<NaturePlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class NaturePlatingPanelWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Tungsten;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<NaturePlatingPanelWall>();
			AddMapEntry(Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.3f));
		}
	}
	public class EarthenPlatingWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<EarthenPlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<EarthenPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class EarthenPlatingWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Iron;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EarthenPlatingWall>();
			AddMapEntry(Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.2f));
		}
	}
	public class EarthenPlatingPanelWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<EarthenPlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<EarthenPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class EarthenPlatingPanelWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Iron;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EarthenPlatingPanelWall>();
			AddMapEntry(Color.Lerp(SOTSTile.EarthenPlatingColor, Color.Black, 0.3f));
		}
	}
	public class EarthenPlatingBeam : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
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
			CreateRecipe(4).AddIngredient(ModContent.ItemType<EarthenPlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<EarthenPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class EarthenPlatingBeamWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Iron;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<EarthenPlatingBeam>();
			AddMapEntry(SOTSTile.EarthenPlatingColor);
		}
	}
	public class PermafrostPlatingWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<PermafrostPlatingWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<PermafrostPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class PermafrostPlatingWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.Silver;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PermafrostPlatingWall>();
			AddMapEntry(Color.Lerp(SOTSTile.PermafrostPlatingColor, Color.Black, 0.2f));
		}
    }
    public class TidalPlatingWall : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.createWall = ModContent.WallType<TidalPlatingWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<TidePlating>(1).AddTile(TileID.WorkBenches).Register();
            Recipe.Create(ModContent.ItemType<TidePlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class TidalPlatingWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Lead;
            AddMapEntry(Color.Lerp(SOTSTile.TidePlatingColor, Color.Black, 0.2f));
        }
    }
    public class OtherworldPlatingWall : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.createWall = ModContent.WallType<OtherworldPlatingWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<OtherworldPlating>(1).AddTile(TileID.WorkBenches).Register();
            Recipe.Create(ModContent.ItemType<OtherworldPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class OtherworldPlatingWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Lead;
            AddMapEntry(Color.Lerp(SOTSTile.OtherworldPlatingColor, Color.Black, 0.2f));
        }
    }
    public class EvilPlatingWall : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.createWall = ModContent.WallType<EvilPlatingWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<EvilPlating>(1).AddTile(TileID.WorkBenches).Register();
            Recipe.Create(ModContent.ItemType<EvilPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class EvilPlatingWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Demonite;
            AddMapEntry(Color.Lerp(SOTSTile.EvilPlatingColor, Color.Black, 0.2f));
        }
    }
    public class InfernoPlatingWall : ModItem
    {
        public override void SetStaticDefaults() => this.SetResearchCost(400);
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneWall);
            Item.width = 28;
            Item.height = 28;
            Item.rare = ItemRarityID.Blue;
            Item.createWall = ModContent.WallType<InfernoPlatingWallWall>();
        }
        public override void AddRecipes()
        {
            CreateRecipe(4).AddIngredient<InfernoPlating>(1).AddTile(TileID.WorkBenches).Register();
            Recipe.Create(ModContent.ItemType<InfernoPlating>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
        }
    }
    public class InfernoPlatingWallWall : ModWall
    {
        public override void SetStaticDefaults()
        {
            Main.wallHouse[Type] = true;
            DustType = DustID.Iron;
            AddMapEntry(Color.Lerp(SOTSTile.InfernoPlatingColor, Color.Black, 0.2f));
        }
    }
}