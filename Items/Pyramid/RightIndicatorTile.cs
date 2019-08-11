
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class RightIndicatorTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("PyramidSlab");
			AddMapEntry(new Color(0, 0, 255));
			mineResist = 3.5f;
			minPick = 100;
            soundType = 21;
            soundStyle = 2;
			dustType = 32;
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("RightIndicatorTile"))
			{
				return false;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
	}
}