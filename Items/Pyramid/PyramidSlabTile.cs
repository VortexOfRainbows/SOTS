
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class PyramidSlabTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("PyramidSlab");
			AddMapEntry(new Color(170, 150, 85));
			mineResist = 3.5f;
			minPick = 110;
            soundType = SoundID.Tink;
            soundStyle = 2;
			dustType = DustID.Sand;
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