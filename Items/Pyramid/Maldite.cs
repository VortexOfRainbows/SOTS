using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Pyramid
{
	public class Maldite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maldite");
			Tooltip.SetDefault("");
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
			item.createTile = mod.TileType("MalditeTile");
		}
	}
	public class MalditeTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileBrick[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("Maldite");
			AddMapEntry(new Color(255, 255, 255));
			soundType = SoundID.NPCHit;
			soundStyle = 1;
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, mod.TileType("CursedTumorTile"), forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
	}
}