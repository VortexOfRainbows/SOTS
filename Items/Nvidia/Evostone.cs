using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Nvidia
{
	public class EvostoneTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 37; //obsidian
			drop = ModContent.ItemType<Evostone>();
			AddMapEntry(new Color(57, 46, 76));
		}
	}
	public class Evostone : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evostone");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.LightPurple;
			item.createTile = ModContent.TileType<EvostoneTile>();
		}
	}
	public class EvostoneBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = false;
			Main.tileBlendAll[Type] = true;
			dustType = 37; //obsidian
			drop = ModContent.ItemType<EvostoneBrick>();
			AddMapEntry(new Color(31, 39, 57));
		}
	}
	public class EvostoneBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evostone Brick");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.LightPurple;
			item.createTile = ModContent.TileType<EvostoneBrickTile>();
		}
	}
}