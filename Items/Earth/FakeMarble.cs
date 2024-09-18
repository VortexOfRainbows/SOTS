using Microsoft.Xna.Framework;
using SOTS.Items.Invidia;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SOTS.Items.Earth
{
	public class FakeMarble : ModTile
	{
        public override string Texture => $"Terraria/Images/Tiles_{TileID.Marble}";
        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileMergeDirt[Type] = true;

            Main.tileMerge[ModContent.TileType<FakeMarble>()][ModContent.TileType<EvostoneTile>()] = true;
            Main.tileMerge[ModContent.TileType<FakeMarble>()][ModContent.TileType<VibrantOreTile>()] = true;
            Main.tileMerge[Type][TileID.Marble] = true;
			Main.tileMerge[TileID.Marble][Type] = true;

            AddMapEntry(new Color(168, 178, 204));
			MineResist = 1.0f;
			HitSound = SoundID.Tink;
			DustType = DustID.Marble;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			return true;
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			return true;
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			yield return new Item(ItemID.Marble);
        }
    }
}