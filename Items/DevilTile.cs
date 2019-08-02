
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class DevilTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 1000000; 
			dustType = 235;
			drop = mod.ItemType("DevilTileBlock");
			AddMapEntry(new Color(200, 200, 200));
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.5f;
			g = 2.5f;
			b = 2.5f;
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("DevilTileBlock"))
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