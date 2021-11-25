using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GhostTown
{
	public class CharredWoodTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 122; //boreal wood
			drop = ModContent.ItemType<CharredWood>();
			AddMapEntry(new Color(105, 82, 61));
		}
	}
	public class CharredWood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charred Wood");
			Tooltip.SetDefault("'Too burnt to be used'");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.createTile = ModContent.TileType<CharredWoodTile>();
		}
	}
}