
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
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			MinPick = 100;
			MineResist = 2.0f;
			DustType = ModContent.DustType<ModIceDust>();
			ItemDrop = ModContent.ItemType<HardIceBrick>();
			AddMapEntry(new Color(148, 179, 240));
			SoundType = SoundID.Tink;
			SoundStyle = 2;
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
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hard Ice Brick Wall");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.createWall = ModContent.WallType<HardIceBrickWallWall>();
		}
	}
}