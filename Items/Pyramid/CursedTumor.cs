using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Items.Pyramid
{
	public class CursedTumor : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CursedTumorTile>();
		}
	}
	public class CursedTumorTile : ModTile
	{
        public override void SetStaticDefaults()
		{
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTile>()] = true;
			Main.tileMerge[Type][ModContent.TileType<OvergrownPyramidTileSafe>()] = true;
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<CursedTumor>();
			AddMapEntry(new Color(105, 75, 146));
			MineResist = 1.5f;
			HitSound = SoundID.NPCHit1;
			DustType = ModContent.DustType<CurseDust3>();
		}
        public override void WalkDust(ref int DustType, ref bool makeDust, ref Color color)
        {
			DustType = this.DustType;
			makeDust = true;
            base.WalkDust(ref DustType, ref makeDust, ref color);
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