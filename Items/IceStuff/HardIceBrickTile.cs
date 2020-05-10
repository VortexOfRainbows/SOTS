
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class HardIceBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 210; 
			dustType = 67;
			drop = mod.ItemType("HardIceBrick");
			AddMapEntry(new Color(180, 250, 255));
			soundType = 21;
			soundStyle = 2;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.5f;
			g = 12.5f;
			b = 13.5f;
			r *= 0.4f;
			g *= 0.4f;
			b *= 0.4f;

		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("HardIceBrickTile"))
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