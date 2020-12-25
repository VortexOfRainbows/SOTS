using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class AcediaPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acedia Portal Plating");
			Tooltip.SetDefault("'It bares striking resemblance to luminite'");
		}

		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Cyan;
			item.consumable = true;
			item.createTile = mod.TileType("AcediaPlatingTile");
		}
	}
	public class AcediaPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			//Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("AcediaPlating");
			AddMapEntry(new Color(44, 12, 62));
			mineResist = 2f;
			minPick = 210;
			soundType = 21;
			soundStyle = 2;
			dustType = mod.DustType("AcedianDust");
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
	}
}