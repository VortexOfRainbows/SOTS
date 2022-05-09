using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Pyramid;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Earth
{
	public class VibrantCrystalLargeTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLighted[Type] = true;
			// Allow attaching sign to the ground
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.Origin = new Point16(0, 0);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };

			// Allow attaching to a solid object that is to the right of the sign
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(1);

			// Allow hanging from ceilings
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorRight = AnchorData.Empty;
			TileObjectData.newAlternate.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(2);

			// Allow attaching to a solid object that is to the left of the sign
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.Origin = new Point16(0, 0);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newAlternate.AnchorBottom = AnchorData.Empty;
			TileObjectData.addAlternate(3);
			TileObjectData.addTile(Type);


			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Large Vibrant Shard");
			AddMapEntry(new Color(156, 209, 46), name);
			dustType = ModContent.DustType<VibrantDust>();
			disableSmartCursor = true;
			soundType = SoundID.Item;
			soundStyle = 27;
			minPick = 40;
			mineResist = 0.1f;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<VibrantOre>(), 1 + Main.rand.Next(3));
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY % 36 != 0)
			{
				top--;
			}
			float uniquenessCounter = Main.GlobalTime * -100 + (left + top) * 5 + tile.TileFrameX + (left % 7 * 3) + (top % 7 * -2);
			float alphaMult = 0.45f + 0.35f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			r = 0.27f * alphaMult;
			g = 0.33f * alphaMult;
			b = 0.15f * alphaMult;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY % 36 != 0)
			{
				top--;
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Main.tileTexture[tile.TileType];
			Vector2 drawOffSet = Vector2.Zero;
			if (tile.TileFrameY < 36) //pointing up
				drawOffSet.Y += 2;
			else if (tile.TileFrameY < 72) //pointing left
				drawOffSet.X += 2;
			else if (tile.TileFrameY < 108) //pointing down
				drawOffSet.Y -= 2;
			else   //pointing right
				drawOffSet.X -= 2;
			Vector2 location = new Vector2(i * 16, j * 16);
			Color color2 = Lighting.GetColor(i, j, WorldGen.paintColor(tile.color()));
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			spriteBatch.Draw(texture, location + drawOffSet + zero - Main.screenPosition, frame, color2, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			if(i != left && j != top)
			{
				float uniquenessCounter = Main.GlobalTime * -100 + (left + top) * 5 + tile.TileFrameX + (left % 7 * 3) + (top % 7 * -2);
				float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
				for(int x = 0; x < 4; x++)
				{
					Vector2 offset = new Vector2(16, 16); 
					frame = new Rectangle(tile.TileFrameX - 18, tile.TileFrameY - 18, 16, 16);
					if (x == 1)
					{
						offset = new Vector2(16, 0);
						frame = new Rectangle(tile.TileFrameX - 18, tile.TileFrameY, 16, 16);
					}
					if (x == 2)
                    {
						offset = new Vector2(0, 16);
						frame = new Rectangle(tile.TileFrameX, tile.TileFrameY - 18, 16, 16);
					}
					if (x == 3)
					{
						offset = new Vector2(0, 0);
						frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
					}
					for (int k = 0; k < 2; k++)
					{
 						spriteBatch.Draw(texture, location + zero - offset + drawOffSet - Main.screenPosition, frame, new Color(100, 100, 100, 0) * alphaMult * 0.6f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
		{
			ModifyFrames(i, j);
			return true;
		}
		public void ModifyFrames(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY % 36 != 0)
			{
				top--;
			}
			bool flag = true;
			if (RoyalRubyShardTile.TileIsCapable(left, top + 2) && RoyalRubyShardTile.TileIsCapable(left + 1, top + 2)) //checks if below tile is active
			{
				if (Main.tile[left, top].frameY != 0)
				{
					Main.tile[left, top].frameY = (short)(Main.tile[left, top].frameY % 36);
					Main.tile[left + 1, top].frameY = (short)(Main.tile[left + 1, top].frameY % 36);
					Main.tile[left + 1, top + 1].frameY = (short)(Main.tile[left + 1, top + 1].frameY % 36);
					Main.tile[left, top + 1].frameY = (short)(Main.tile[left, top + 1].frameY % 36);
				}
				else
					flag = false;
			}
			else if (RoyalRubyShardTile.TileIsCapable(left - 1, top) && RoyalRubyShardTile.TileIsCapable(left - 1, top + 1)) //checks if left tile is active
			{
				if(Main.tile[left, top].frameY != 108)
				{
					Main.tile[left, top].frameY = (short)(Main.tile[left, top].frameY % 36 + 108);
					Main.tile[left + 1, top].frameY = (short)(Main.tile[left + 1, top].frameY % 36 + 108);
					Main.tile[left + 1, top + 1].frameY = (short)(Main.tile[left + 1, top + 1].frameY % 36 + 108);
					Main.tile[left, top + 1].frameY = (short)(Main.tile[left, top + 1].frameY % 36 + 108);
				}
				else
					flag = false;
			}
			else if (RoyalRubyShardTile.TileIsCapable(left + 2, top) && RoyalRubyShardTile.TileIsCapable(left + 2, top + 1)) //checks if right tile is active
			{
				if(Main.tile[left, top].frameY != 36)
				{
					Main.tile[left, top].frameY = (short)(Main.tile[left, top].frameY % 36 + 36);
					Main.tile[left + 1, top].frameY = (short)(Main.tile[left + 1, top].frameY % 36 + 36);
					Main.tile[left + 1, top + 1].frameY = (short)(Main.tile[left + 1, top + 1].frameY % 36 + 36);
					Main.tile[left, top + 1].frameY = (short)(Main.tile[left, top + 1].frameY % 36 + 36);
				}
				else
					flag = false;
			}
			else if (RoyalRubyShardTile.TileIsCapable(left, top - 1) && RoyalRubyShardTile.TileIsCapable(left + 1, top - 1)) //checks if above tile is active
			{
				if(Main.tile[left, top].frameY != 72)
				{
					Main.tile[left, top].frameY = (short)(Main.tile[left, top].frameY % 36 + 72);
					Main.tile[left + 1, top].frameY = (short)(Main.tile[left + 1, top].frameY % 36 + 72);
					Main.tile[left + 1, top + 1].frameY = (short)(Main.tile[left + 1, top + 1].frameY % 36 + 72);
					Main.tile[left, top + 1].frameY = (short)(Main.tile[left, top + 1].frameY % 36 + 72);
				}
				else
					flag = false;
			}
			else
				flag = false;
			//if (flag)
			//{
				//WorldGen.TileFrame(i, j, true);
				//NetMessage.SendTileSquare(-1, i, j, 4, TileChangeType.None);
				//NetMessage.SendData(17, -1, -1, null, 1, i, j, Type);
			//}
		}
	}
}