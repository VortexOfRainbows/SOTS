using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Pyramid.AncientGold
{
	public class RoyalGoldBrick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Brick");
			Tooltip.SetDefault("'You can feel the regalness'");
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
			Item.createTile = mod.TileType("RoyalGoldBrickTile");
		}
	}
	public class RoyalGoldBrickTile : ModTile
	{
		public override void SetDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<RoyalGoldBrick>();
			AddMapEntry(new Color(180, 150, 20));
			mineResist = 1.0f;
			minPick = 0;
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = DustID.GoldCoin;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}