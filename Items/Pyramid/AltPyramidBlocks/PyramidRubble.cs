using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
// If you are using c# 6, you can use: "using static Terraria.Localization.GameCulture;" which would mean you could just write "DisplayName.AddTranslation(German, "");"
using Terraria.Localization;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.AltPyramidBlocks
{
	public class PyramidRubble : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pyramid Rubble");
			Tooltip.SetDefault("'It's much less a brick than just a collection of rocks'");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Blue;
			item.consumable = true;
			item.createTile = ModContent.TileType<PyramidRubbleTile>();
		}
	}
	public class PyramidRubbleTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PyramidRubble>();
			AddMapEntry(Color.Lerp(new Color(144, 127, 72), Color.Black, 0.15f));
			mineResist = 1.5f;
			minPick = 0;
			soundType = SoundID.Tink;
			dustType = 32;
		}
        public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			Main.PlaySound(SoundID.Dig, (int)pos.X, (int)pos.Y, 0, 1.05f, -0.6f);
			return false;
        }
	}

	public class RuinedPyramidBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ruined Pyramid Brick");
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
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.LightRed;
			item.consumable = true;
			item.createTile = ModContent.TileType<RuinedPyramidBrickTile>();
		}
	}
	public class RuinedPyramidBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<RuinedPyramidBrick>();
			AddMapEntry(Color.Lerp(new Color(156, 137, 78), Color.Black, 0.08f));
			mineResist = 2.0f;
			minPick = 110;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = 32;
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