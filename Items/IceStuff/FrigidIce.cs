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
			mineResist = 0.5f;
			dustType = DustID.Ice;
			drop = ModContent.ItemType<FrigidIce>();
			AddMapEntry(new Color(198, 249, 251));
			soundType = SoundID.Tink;
			soundStyle = 2;
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