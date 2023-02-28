using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SOTS.Items.Gems
{
	public class SOTSGemLockTiles : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
			TileID.Sets.FramesOnKillWall[Type] = true; // Necessary since Style3x3Wall uses AnchorWall
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(34, 25, 48), name);
			DustType = DustID.Obsidian;
			MinPick = 250;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 4;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			switch (frameX / 54)
			{
				case 0:
					item = ItemType<SOTSRubyGemLock>();
					break;
				case 1:
					item = ItemType<SOTSSapphireGemLock>();
					break;
				case 2:
					item = ItemType<SOTSEmeraldGemLock>();
					break;
				case 3:
					item = ItemType<SOTSTopazGemLock>();
					break;
				case 4:
					item = ItemType<SOTSAmethystGemLock>();
					break;
				case 5:
					item = ItemType<SOTSDiamondGemLock>();
					break;
				case 6:
					item = ItemType<SOTSAmberGemLock>();
					break;
			}
			if (item > 0)
			{
				Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, item);
				if(frameY >= 54)
					Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 48, KeyType(i, j, frameX));
			}
		}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameX % 54 == 18 && tile.TileFrameY % 54 == 18)
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				Vector2 origin = new Vector2(24, 24);
				Texture2D texture = TextureAssets.Tile[Type].Value;
				Vector2 location = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
				Color color = Lighting.GetColor(i, j, WorldGen.paintColor(Main.tile[i, j].TileColor));
				int xFrame = tile.TileFrameX / 54;
				for(int a = 0; a < 2; a++) //this draws the first layer and then the gem border
				{
					int yFrame = a;
					Rectangle frame = new Rectangle(0 + 50 * xFrame, 50 * yFrame, 48, 48);
					spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
					if (a == 0 && tile.TileFrameY > 54)
					{
						frame = new Rectangle(2 + 50 * xFrame, 52, 44, 44);
						for (int b = 0; b < 6; b++)
						{
							Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(b * 60 + SOTSWorld.GlobalCounter * 2));
							spriteBatch.Draw(texture, location + zero + circular - Main.screenPosition, frame, new Color(120 - b * 7, 110 - b * 2, 100 + b * 4, 0), 0f, new Vector2(22, 22), 1f, SpriteEffects.None, 0f);
						}
						color = Color.Lerp(color, Color.White, 0.5f);
					}
				}
				if(tile.TileFrameY > 54) //This draws the gem inside
				{
					Rectangle frame = new Rectangle(0 + 50 * xFrame, 100, 48, 48);
					for (int b = 0; b < 6; b++)
					{
						Rectangle frame2 = new Rectangle(2 + 50 * xFrame, 102, 44, 44);
						Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(b * 60 + SOTSWorld.GlobalCounter * 2));
						spriteBatch.Draw(texture, location + zero + circular - Main.screenPosition, frame2, new Color(120 - b * 7, 110 - b * 2, 100 + b * 4, 0), 0f, new Vector2(22, 22), 1f, SpriteEffects.None, 0f);
					}
					spriteBatch.Draw(texture, location + zero - Main.screenPosition, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public int KeyType(int i, int j, int frameX = 0)
		{
			Tile tile = Main.tile[i, j];
			int item = ItemID.LargeRuby;
			if (frameX == 0)
				frameX = tile.TileFrameX;
			switch (frameX / 54)
			{
				case 1:
					item = ItemID.LargeSapphire;
					break;
				case 2:
					item = ItemID.LargeEmerald;
					break;
				case 3:
					item = ItemID.LargeTopaz;
					break;
				case 4:
					item = ItemID.LargeAmethyst;
					break;
				case 5:
					item = ItemID.LargeDiamond;
					break;
				case 6:
					item = ItemID.LargeAmber;
					break;
			}
			return item;
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			if (tile.TileFrameY < 54)
			{
				player.cursorItemIconID = KeyType(i, j);
				player.noThrow = 2;
				player.cursorItemIconEnabled = true;
			}
			else
			{
				player.cursorItemIconID = 0;
				player.cursorItemIconEnabled = false;
			}
		}
		public override void MouseOverFar(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			MouseOver(i, j);
			if (tile.TileFrameY < 54)
			{
				if (player.cursorItemIconText == "")
				{
					player.cursorItemIconEnabled = false;
					player.cursorItemIconID = 0;
				}
			}
			else
			{
				player.cursorItemIconID = 0;
				player.cursorItemIconEnabled = false;
			}
		}
		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int key = KeyType(i, j);
			bool able = false;
			if (!able && tile.TileFrameY < 54)
				able = player.ConsumeItem(key);
			int left = i - (tile.TileFrameX / 18) % 3;
			int top = j - (tile.TileFrameY / 18) % 3;
			if (able)
			{
				SOTSUtils.PlaySound(SoundID.Item4, (int)player.Center.X, (int)player.Center.Y, 1.0f, 0.3f);
				for (int x = left; x < left + 3; x++)
				{
					for (int y = top; y < top + 3; y++)
					{
						if (Main.tile[x, y].TileFrameY < 54)
						{
							Main.tile[x, y].TileFrameY += 54;
							NetMessage.SendTileSquare(-1, x, y, 2);
						}
					}
				}
				if (key == ItemID.LargeRuby)
					SOTSWorld.RubyKeySlotted = true;
				if (key == ItemID.LargeSapphire)
					SOTSWorld.SapphireKeySlotted = true;
				if (key == ItemID.LargeEmerald)
					SOTSWorld.EmeraldKeySlotted = true;
				if (key == ItemID.LargeTopaz)
					SOTSWorld.TopazKeySlotted = true;
				if (key == ItemID.LargeAmethyst)
					SOTSWorld.AmethystKeySlotted = true;
				if (key == ItemID.LargeDiamond)
					SOTSWorld.DiamondKeySlotted	 = true;
				if (key == ItemID.LargeAmber)
					SOTSWorld.AmberKeySlotted = true;
				if(Main.netMode != NetmodeID.SinglePlayer)
					SOTSWorld.SyncGemLocks(Main.LocalPlayer); //This should sync the world variables
			}
			return true;
		}
	}
	public abstract class SOTSGemLock : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.GemLockAmber);
			Item.width = 26;
			Item.height = 26;
			Item.rare = ItemRarityID.LightPurple;
			SafeSetDefaults();
		}
		public virtual void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 0;
		}
	}
	public class SOTSRubyGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 0;
		}
	}
	public class SOTSSapphireGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 1;
		}
	}
	public class SOTSEmeraldGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 2;
		}
	}
	public class SOTSTopazGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 3;
		}
	}
	public class SOTSAmethystGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 4;
		}
	}
	public class SOTSDiamondGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 5;
		}
	}
	public class SOTSAmberGemLock : SOTSGemLock
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
			Item.createTile = TileType<SOTSGemLockTiles>();
			Item.placeStyle = 6;
		}
	}
}