
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class PyramidSlabTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<PyramidSlab>();
			AddMapEntry(new Color(181, 164, 88));
			MineResist = 3.5f;
			MinPick = 110;
            SoundType = SoundID.Tink;
            SoundStyle = 2;
			DustType = 32;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			if(SOTSWorld.downedCurse)
				return true;
			return false;
		}
	}
}