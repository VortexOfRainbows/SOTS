using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid.PyramidWalls
{
	public class AncientGoldBeam : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<AncientGoldBeamWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<PyramidBrick>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<PyramidBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AncientGoldBeamWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.GoldCoin;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AncientGoldBeam>();
			AddMapEntry(new Color(170, 144, 18));
		}
	}
	public class AncientGoldBrickWall : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 7;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Pink;
			Item.consumable = true;
			Item.createWall = ModContent.WallType<AncientGoldBrickWallTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<PyramidBrick>(), 1).AddTile(TileID.WorkBenches).Register();
			Recipe.Create(ModContent.ItemType<PyramidBrick>()).AddIngredient(this, 4).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AncientGoldBrickWallTile : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = DustID.GoldCoin;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AncientGoldBrickWall>();
			AddMapEntry(new Color(150, 130, 15));
		}
	}
}