
using Microsoft.Xna.Framework;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
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
			Main.tileLighted[Type] = false;
			minPick = 100;
			mineResist = 2.0f;
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<HardIceBrick>();
			AddMapEntry(new Color(198, 249, 251));
			soundType = SoundID.Tink;
			soundStyle = 2;
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
	}
	public class HardIceBrickWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(144, 181, 181));
			dustType = ModContent.DustType<ModIceDust>();
			drop = ModContent.ItemType<HardIceBrickWall>();
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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hard Ice Brick Wall");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.createWall = ModContent.WallType<HardIceBrickWallWall>();
		}
	}
}