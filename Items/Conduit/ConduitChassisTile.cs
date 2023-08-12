using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class ConduitChassis : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Blue;
			Item.Size = new Vector2(36, 36);
			Item.value = Item.buyPrice(0, 0, 5, 0);
			Item.createTile = ModContent.TileType<ConduitChassisTile>();
		}
	}
	public class ConduitChassisTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false; //sunlight passes through these pipes
			Main.tileLighted[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<ConduitChassis>();
			AddMapEntry(new Color(66, 77, 93));
			MineResist = 15f;
			MinPick = 300;
			HitSound = SoundID.Tink;
			DustType = DustID.Lead;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
			TileID.Sets.DrawsWalls[Type] = true;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Main.tileNoSunLight[Type] = false;
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
			return false;
		}
	}
}