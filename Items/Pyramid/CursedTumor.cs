using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class CursedTumor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Tumor Brick");
			Tooltip.SetDefault("'A living clump of matter residing in a broken down brick\n'It has the consistency of a tumor'");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Pink;
			item.consumable = true;
			item.createTile = mod.TileType("CursedTumorTile");
		}
	}
	public class CursedTumorTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("CursedTumor");
			AddMapEntry(new Color(50, 45, 90));
			mineResist = 1.5f;
			soundType = SoundID.NPCHit;
			soundStyle = 1;
			dustType = mod.DustType("CurseDust3");
		}
        public override void WalkDust(ref int dustType, ref bool makeDust, ref Color color)
        {
			dustType = this.dustType;
			makeDust = true;
            base.WalkDust(ref dustType, ref makeDust, ref color);
        }
        public override bool HasWalkDust()
        {
            return true;
        }
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
}