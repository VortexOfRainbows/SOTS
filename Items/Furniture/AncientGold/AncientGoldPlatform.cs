using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldPlatform : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(200);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 14;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 0;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AncientGoldPlatformTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 1).Register();
			Recipe.Create(ModContent.ItemType<RoyalGoldBrick>()).AddIngredient(this, 2).Register();
		}
	}
	public class AncientGoldPlatformTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileSolidTop[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CoordinateHeights = new[]{ 16 };
			TileObjectData.newTile.CoordinateWidth = 16;
			TileObjectData.newTile.CoordinatePadding = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleMultiplier = 27;
			TileObjectData.newTile.StyleWrapLimit = 27;
			TileObjectData.newTile.UsesCustomCanPlace = false;
			TileObjectData.addTile(19);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsDoor);
			AddMapEntry(new Color(220, 180, 25));
			DustType = DustID.GoldCoin;
			ItemDrop = ModContent.ItemType<AncientGoldPlatform>();
			AdjTiles = new int[]{ TileID.Platforms };
			TileID.Sets.Platforms[Type] = true;
		}
		public override void PostSetDefaults()
		{
			Main.tileNoSunLight[Type] = false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}