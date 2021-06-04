using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class PortalPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritia Portal Plating");
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
			item.createTile = mod.TileType("PortalPlatingTile");
		}
	}
	public class PortalPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			//Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileMerge[Type][mod.TileType("AvaritianPlating")] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("PortalPlating");
			AddMapEntry(new Color(64, 180, 170));
			mineResist = 2f;
			minPick = 210;
			soundType = 21;
			soundStyle = 2;
			dustType = mod.DustType("AvaritianDust");
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