using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Invidia;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Earth
{
	public class VibrantOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 20;
			Item.height = 18;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(0, 0, 1, 0);
			Item.createTile = ModContent.TileType<VibrantOreTile>();
		}
	}
	public class VibrantOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileOreFinderPriority[Type] = 420; //above gold
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<EvostoneTile>()] = true;
			Main.tileMerge[Type][TileID.Marble] = true;
			Main.tileMerge[TileID.Marble][Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(123, 166, 36), name);
			MineResist = 1.0f;
			MinPick = 40; //no copper/tin pickaxe!
			HitSound = SoundID.Tink;
			//SoundStyle = 2;
			DustType = ModContent.DustType<VibrantDust>();
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.Marble, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
		public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(3) + 1;
			SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Items/VibrantOre" + type), (int)pos.X, (int)pos.Y, 1.8f, Main.rand.NextFloat(0.3f, 0.4f));
			return false;
		}
	}
	public class VibrantCrystalTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(156, 209, 46), name);
			HitSound = SoundID.Item27;
			DustType = ModContent.DustType<VibrantDust>();
		}
        public override bool CanDrop(int i, int j)
        {
			return false;
        }
        public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			bool canLive = ModifyFrames(i, j);
			if (!canLive)
			{
				WorldGen.KillTile(i, j);
				if (Main.netMode != NetmodeID.SinglePlayer)
				{
					NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				}
			}
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5 + tile.TileFrameX + (i % 7 * 3) + (j % 7 * -2);
			float alphaMult = 0.35f + 0.25f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			r = 0.27f * alphaMult;
			g = 0.33f * alphaMult;
			b = 0.15f * alphaMult;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Terraria.GameContent.TextureAssets.Tile[tile.TileType].Value;
			Vector2 drawOffSet = Vector2.Zero;
			if (tile.TileFrameY == 0) //below is active
				drawOffSet.Y += 2;
			if (tile.TileFrameY == 18) //above is active
				drawOffSet.Y -= 2;
			if (tile.TileFrameY == 36) //right is active
				drawOffSet.X += 2;
			if (tile.TileFrameY == 54) //left is active
				drawOffSet.X -= 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.TileColor));
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5 + tile.TileFrameX + (i % 7 * 3) + (j % 7 * -2);
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 2; k++)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, tile.TileType, texture, new Color(100, 100, 100, 0) * alphaMult * 0.6f, drawOffSet);
			}
			return false;
		}
		public override bool CanPlace(int i, int j)
		{
			return RoyalRubyShardTile.TileIsCapable(i, j + 1) || RoyalRubyShardTile.TileIsCapable(i, j - 1) || RoyalRubyShardTile.TileIsCapable(i + 1, j) || RoyalRubyShardTile.TileIsCapable(i - 1, j);
		}
		public bool ModifyFrames(int i, int j, bool randomize = false)
		{
			bool flag = true;
			if (RoyalRubyShardTile.TileIsCapable(i, j + 1)) //checks if below tile is active
			{
				Main.tile[i, j].TileFrameY = 0;
			}
			else if (RoyalRubyShardTile.TileIsCapable(i - 1, j)) //checks if left tile is active
			{
				Main.tile[i, j].TileFrameY = 54;
			}
			else if (RoyalRubyShardTile.TileIsCapable(i + 1, j)) //checks if right tile is active
			{
				Main.tile[i, j].TileFrameY = 36;
			}
			else if (RoyalRubyShardTile.TileIsCapable(i, j - 1)) //checks if above tile is active
			{
				Main.tile[i, j].TileFrameY = 18;
			}
			else
			{
				flag = false;
			}
			if (flag && randomize)
			{
				Main.tile[i, j].TileFrameX = (short)(WorldGen.genRand.Next(18) * 18);
				WorldGen.SquareTileFrame(i, j, true);
				NetMessage.SendTileSquare(-1, i, j, 2, TileChangeType.None);
				//NetMessage.SendData(17, -1, -1, null, 1, i, j, Type);
			}
			return flag;
		}
		public override void PlaceInWorld(int i, int j, Item item)
		{
			ModifyFrames(i, j, true);
		}
	}
	public class VibrantWall : ModItem
	{
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.Blue;
			Item.createWall = ModContent.WallType<VibrantWallWall>();
		}
	}
	public class VibrantWallWall : ModWall
	{
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<VibrantDust>();
			AddMapEntry(new Color(67, 91, 19));
		}
	}
}