using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class TrueSandstoneBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Sandstone Block");
			Tooltip.SetDefault("The ultimate sandstone");
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = 1;
			Item.rare = ItemRarityID.Yellow;
			Item.consumable = true;
			Item.createTile = mod.TileType("TrueSandstoneTile");
			Item.expert = true;
		}
	}
	public class TrueSandstoneTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<TrueSandstoneBlock>();
			AddMapEntry(new Color(210, 160, 95));
			mineResist = 5.0f;
			minPick = 250;
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
			return false;
		}
	}
}