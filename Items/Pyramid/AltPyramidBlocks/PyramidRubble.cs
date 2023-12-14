using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.AltPyramidBlocks
{
	public class PyramidRubble : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<PyramidRubbleTile>();
		}
	}
	public class PyramidRubbleTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileMerge[ModContent.TileType<OvergrownPyramidTile>()][Type] = true;
			Main.tileMerge[ModContent.TileType<OvergrownPyramidTileSafe>()][Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PyramidRubble>();
			AddMapEntry(Color.Lerp(new Color(181, 164, 88), Color.Black, 0.15f));
			MineResist = 1.5f;
			MinPick = 0;
			HitSound = SoundID.Tink;
			DustType = 32;
		}
        public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			SOTSUtils.PlaySound(SoundID.Dig, (int)pos.X, (int)pos.Y, 1.05f, -0.6f);
			return false;
        }
	}

	public class RuinedPyramidBrick : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<RuinedPyramidBrickTile>();
		}
	}
	public class RuinedPyramidBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileMerge[ModContent.TileType<OvergrownPyramidTile>()][Type] = true;
			Main.tileMerge[ModContent.TileType<OvergrownPyramidTileSafe>()][Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<RuinedPyramidBrick>();
			AddMapEntry(Color.Lerp(new Color(181, 164, 88), Color.Black, 0.08f));
			MineResist = 2.0f;
			MinPick = 110;
			HitSound = SoundID.Tink;
			DustType = 32;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			if (SOTSWorld.downedCurse)
				return true;
			return false;
		}
	}
}