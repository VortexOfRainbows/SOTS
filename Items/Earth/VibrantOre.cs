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
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<VibrantOre>();
			AddMapEntry(new Color(166, 214, 67));
			mineResist = 1.6f;
			minPick = 40; //no copper/tin pickaxe!
			soundType = SoundID.Tink;
			soundStyle = 2;
			dustType = ModContent.DustType<VibrantDust>();
		}
		/*public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			SOTSTile.DrawSlopedGlowMask(i, j, Main.tile[i, j].type, ModContent.GetTexture("SOTS/Items/Earth/VibrantOreTileGlow"), new Color(70, 80, 70, 0));
		}*/
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
			AddMapEntry(new Color(135, 182, 39), name);
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
			r = 0.27f;
			g = 0.33f;
			b = 0.15f;
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