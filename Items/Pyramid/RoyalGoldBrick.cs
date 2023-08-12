using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid
{
	public class RoyalGoldBrick : ModItem
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
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<RoyalGoldBrickTile>();
		}
	}
	public class RoyalGoldBrickTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<RoyalGoldBrick>();
			AddMapEntry(new Color(180, 150, 20));
			MineResist = 1.0f;
			MinPick = 0;
			HitSound = SoundID.Tink;
			DustType = DustID.GoldCoin;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}