
using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	public class HardIceBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			MinPick = 100;
			MineResist = 2.0f;
			DustType = ModContent.DustType<ModIceDust>();
			ItemDrop = ModContent.ItemType<HardIceBrick>();
			AddMapEntry(new Color(67, 139, 228));
			HitSound = SoundID.Tink;
		}
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return SOTSWorld.downedAmalgamation;
        }
        public override bool CanExplode(int i, int j)
		{
			return SOTSWorld.downedAmalgamation;
		}
		public override bool Slope(int i, int j)
		{
			return SOTSWorld.downedAmalgamation;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.SnowBlock, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
	public class HardIceBrickWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(80, 35, 180));
			DustType = ModContent.DustType<ModIceDust>();
			ItemDrop = ModContent.ItemType<HardIceBrickWall>();
		}
        public override void KillWall(int i, int j, ref bool fail)
        {
			if(!SOTSWorld.downedAmalgamation)
            {
				fail = true;
            }
        }
    }
	public class HardIceBrickWall : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(400);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<HardIceBrickWallWall>();
		}
	}
}