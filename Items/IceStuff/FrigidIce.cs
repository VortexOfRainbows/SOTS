using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	public class FrigidIceTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			minPick = 210;
			dustType = 34;
			drop = mod.ItemType("FrigidIceItem");
			AddMapEntry(new Color(198, 249, 251));
			soundType = 21;
			soundStyle = 2;
		}
		public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("FrigidIce"))
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
	public class FrigidIce : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.createTile = ModContent.TileType<FrigidIceTile>();
		}
	}
}