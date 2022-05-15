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
			DisplayName.SetDefault("Cursed Tumor Block");
			Tooltip.SetDefault("'Baked beans'");
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
			Item.rare = ItemRarityID.Pink;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("CursedTumorTile").Type;
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
			drop = ModContent.ItemType<CursedTumor>();
			AddMapEntry(new Color(105, 75, 146));
			MineResist = 1.5f;
			soundType = SoundID.NPCHit;
			soundStyle = 1;
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