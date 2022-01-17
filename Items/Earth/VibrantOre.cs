using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Nvidia;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class VibrantOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Shard");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.width = 20;
			item.height = 18;
			item.rare = ItemRarityID.Blue;
			item.value = Item.sellPrice(0, 0, 1, 0);
			item.createTile = ModContent.TileType<VibrantOreTile>();
		}
	}
	public class VibrantOreTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileValue[Type] = 420; //above gold
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMerge[Type][ModContent.TileType<EvostoneTile>()] = true;
			Main.tileMerge[Type][TileID.Marble] = true;
			Main.tileMerge[TileID.Marble][Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<VibrantOre>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vibrant Ore");
			AddMapEntry(new Color(123, 166, 36), name);
			mineResist = 1.0f;
			minPick = 40; //no copper/tin pickaxe!
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = ModContent.DustType<VibrantDust>();
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			SOTS.MergeWithFrame(i, j, Type, TileID.Marble, forceSameDown: false, forceSameUp: false, forceSameLeft: false, forceSameRight: false, resetFrame);
			return false;
		}
		public override bool KillSound(int i, int j)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			int type = Main.rand.Next(3) + 1;
			Main.PlaySound(SoundLoader.customSoundType, (int)pos.X, (int)pos.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Items/VibrantOre" + type), 2f, Main.rand.NextFloat(0.9f, 1.1f));
			return false;
		}
	}
	public class VibrantCrystalTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileShine[Type] = 200;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileNoFail[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Vibrant Shard");
			AddMapEntry(new Color(156, 209, 46), name);
			soundType = SoundID.Item;
			soundStyle = 27;
			dustType = ModContent.DustType<VibrantDust>();
		}
        public override bool Drop(int i, int j)
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
					NetMessage.SendData(MessageID.TileChange, -1, -1, null, 0, i, j, 0f, 0, 0, 0);
				}
			}
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5 + tile.frameX + (i % 7 * 3) + (j % 7 * -2);
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
			Texture2D texture = Main.tileTexture[tile.type];
			Vector2 drawOffSet = Vector2.Zero;
			if (tile.frameY == 0) //below is active
				drawOffSet.Y += 2;
			if (tile.frameY == 18) //above is active
				drawOffSet.Y -= 2;
			if (tile.frameY == 36) //right is active
				drawOffSet.X += 2;
			if (tile.frameY == 54) //left is active
				drawOffSet.X -= 2;
			Vector2 location = new Vector2(i * 16, j * 16) + drawOffSet;
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.color()));
			Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
			spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5 + tile.frameX + (i % 7 * 3) + (j % 7 * -2);
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 2; k++)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture, new Color(100, 100, 100, 0) * alphaMult * 0.6f, drawOffSet);
			}
			return false;
		}
		public override bool CanPlace(int i, int j)
		{
			return TileIsCapable(i, j + 1) || TileIsCapable(i, j - 1) || TileIsCapable(i + 1, j) || TileIsCapable(i - 1, j);
		}
		private bool TileIsCapable(Tile tile)
		{
			return tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && tile.slope() == 0 && !tile.halfBrick() && !tile.inActive();
		}
		private bool TileIsCapable(int i, int j)
		{
			return TileIsCapable(Main.tile[i, j]);
		}
		public bool ModifyFrames(int i, int j, bool randomize = false)
		{
			bool flag = true;
			if (TileIsCapable(i, j + 1)) //checks if below tile is active
			{
				Main.tile[i, j].frameY = 0;
			}
			else if (TileIsCapable(i - 1, j)) //checks if left tile is active
			{
				Main.tile[i, j].frameY = 54;
			}
			else if (TileIsCapable(i + 1, j)) //checks if right tile is active
			{
				Main.tile[i, j].frameY = 36;
			}
			else if (TileIsCapable(i, j - 1)) //checks if above tile is active
			{
				Main.tile[i, j].frameY = 18;
			}
			else
			{
				flag = false;
			}
			if (flag && randomize)
			{
				Main.tile[i, j].frameX = (short)(WorldGen.genRand.Next(18) * 18);
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
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unsafe Vibrant Ore Wall");
		}
        public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneWall);
			item.width = 28;
			item.height = 28;
			item.rare = ItemRarityID.Blue;
			item.createWall = ModContent.WallType<VibrantWallWall>();
		}
	}
	public class VibrantWallWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = ModContent.DustType<VibrantDust>();
			AddMapEntry(new Color(67, 91, 19));
		}
	}
}