using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.Blocks
{
	public class HardlightBlock : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Blocks/HardlightBlockOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Blocks/HardlightBlockFill").Value;
			Color color = new Color(100, 100, 100, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2(position.X + x, position.Y + y), null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Blocks/HardlightBlockOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Blocks/HardlightBlockFill").Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y), null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetStaticDefaults() => this.SetResearchCost(100);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 20;
			Item.height = 20;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = ModContent.TileType<HardlightBlockTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<TwilightGel>(), 2).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
	public class HardlightBlockTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileNoSunLight[Type] = false;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<HardlightBlock>();
			//AddMapEntry(new Color(0, 0, 0, 0));
			MineResist = 1.0f;
			MinPick = 0;
			HitSound = SoundID.NPCHit53;
			DustType = DustID.Electric;
		}
        public override bool KillSound(int i, int j, bool fail)
		{
			Vector2 pos = new Vector2(i * 16, j * 16) + new Vector2(8, 8);
			SOTSUtils.PlaySound(SoundID.NPCHit53, (int)pos.X, (int)pos.Y, 0.5f, 0.5f);
			return false;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 7;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Main.tileNoSunLight[Type] = false;
			if (!Main.tile[i + 1, j].HasTile || Main.tile[i + 1, j].TileType == Type || !Main.tileSolid[Main.tile[i + 1, j].TileType])
				Draw(i, j, spriteBatch);
			return false;
		}
		public static int closestPlayer(int i, int j, ref float minDist)
		{
			int p = -1;
			for (int k = 0; k < Main.player.Length; k++)
			{
				Player player = Main.player[k];
				if(player.active && !player.dead)
				{
					Vector2 pos = new Vector2(i * 16 + 8, j * 16 + 8);
					float length = (player.Center - pos).Length();
					if (length < minDist)
					{
						minDist = length;	
						p = player.whoAmI;
						if (Main.netMode == NetmodeID.SinglePlayer)
							break;
					}
				}
			}
			return p;
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			float currentDistanceAway = 128;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
				return;
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 128f, 0.725f);
			if (alphaScale <= 0.00001f)
				return;
			r = 0.8f * alphaScale;
			g = 0.9f * alphaScale;
			b = 1.8f * alphaScale;
		}
        public static void Draw(int i, int j, SpriteBatch spriteBatch)
        {
			float currentDistanceAway = 128;
			int playerN = closestPlayer(i, j, ref currentDistanceAway);
			if (playerN == -1)
				return;
			float alphaScale = (float)Math.Pow(1.0f - currentDistanceAway / 128f, 0.725f);
			if (alphaScale <= 0.00001f)
				return;
			if(!Main.tile[i, j].HasUnactuatedTile)
            {
				alphaScale *= 0.45f;
            }
			Texture2D omega = Terraria.GameContent.TextureAssets.Tile[ModContent.TileType<HardlightBlockTile>()].Value;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 origin = new Vector2(2, 2);
			Vector2 pos = new Vector2(i * 16, j * 16) + zero - Main.screenPosition;
			bool blockBelow = isHardlightBlock(i, j + 1);
			for (int k = 0; k < 3 + 3 * alphaScale; k++)
			{
				Color color = WorldGen.paintColor(Main.tile[i, j].TileColor) * (160f * alphaScale / 255f);
				color.A = 0;
				if (k == 0) //sets up the middle part
				{
					color *= 0.3125f;
					Rectangle midFrame = getTileFrame(2, 3);
					int left = 0;
					int right = 0;
					int up = 0;
					int down = 0;
					if (blockBelow)
                    {
						midFrame = getTileFrame(2, 2);
						if(!isHardlightBlock(i - 1, j + 1) && !isHardlightBlock(i + 1, j + 1))
						{
							midFrame = getTileFrame(5, 0);
						}
						else if (!isHardlightBlock(i - 1, j + 1))
						{
							midFrame = getTileFrame(6, 0);
						}
						else if (!isHardlightBlock(i + 1, j + 1))
						{
							midFrame = getTileFrame(5, 1);
						}
						down += 2;
					}
					if (isHardlightBlock(i - 1, j))
						left += 2;
					if (isHardlightBlock(i + 1, j))
						right += 2;
					if (isHardlightBlock(i, j - 1))
						up += 2;
					midFrame.X += 4 - left;
					midFrame.Y += 4 - up;
					midFrame.Width = 12 + left + right;
					midFrame.Height = 12 + down + up;
					Main.spriteBatch.Draw(omega, pos + new Vector2(2 - left, 2 - up), midFrame, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					if(isHardlightBlock(i - 1, j - 1) || (isHardlightBlock(i - 1, j) && isHardlightBlock(i, j - 1)))
					{
						Rectangle frame = getTileFrame(1, 0);
						if (isHardlightBlock(i - 1, j))
						{
							frame = getTileFrame(0, 0);
						}
						Main.spriteBatch.Draw(omega, pos, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i + 1, j - 1) || (isHardlightBlock(i + 1, j) && isHardlightBlock(i, j - 1)))
					{
						Rectangle frame = getTileFrame(3, 0);
						if (isHardlightBlock(i + 1, j))
						{
							frame = getTileFrame(4, 0);
						}
						Main.spriteBatch.Draw(omega, pos, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i - 1, j + 1) || (isHardlightBlock(i - 1, j) && isHardlightBlock(i, j + 1)))
					{
						Rectangle frame = getTileFrame(1, 5);
						if (blockBelow && isHardlightBlock(i - 1, j + 1))
                        {
							frame = getTileFrame(0, 5);
						}
						Main.spriteBatch.Draw(omega, pos, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i + 1, j + 1) || (isHardlightBlock(i + 1, j) && isHardlightBlock(i, j + 1)))
					{
						Rectangle frame = getTileFrame(3, 5);
						if (blockBelow && isHardlightBlock(i + 1, j + 1))
						{
							frame = getTileFrame(4, 5);
						}
						Main.spriteBatch.Draw(omega, pos, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
				}
				else
                {
					Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.2f * (k - 1);
					for(int xMod = -1; xMod <= 1; xMod += 2) //does both left and right borders
					{
						if (!isHardlightBlock(i + xMod, j))
						{
							int frameY = 2;
							int frameX = 2;
							if (!blockBelow)
							{
								frameY++;
							}
							if (!isHardlightBlock(i + xMod, j - 1) && isHardlightBlock(i, j - 1))
							{
								if (xMod == -1)
								{
									frameY += 4;
								}
								else
								{
									frameX += 2;
								}
							}
							if (!isHardlightBlock(i + xMod, j + 1) && isHardlightBlock(i, j + 1))
							{
								if (xMod == -1)
								{
									frameX--;
								}
								else
								{
									frameX++;
								}
							}
							Rectangle frame = getTileFrame(frameX + xMod, frameY);
							Main.spriteBatch.Draw(omega, pos + offset, frame, color, 0f, origin, 1f, SpriteEffects.None, 0f);
						}
					}
					for (int yMod = -1; yMod <= 1; yMod += 2) //does both up and down borders
					{
						if (!isHardlightBlock(i, j + yMod))
						{
							int frameY = 2;
							int frameX = 2;
							if (yMod == -1)
                            {
								frameY -= 1;
                            }
							if(yMod == 1)
                            {
								frameY += 2;
							}
							if (!isHardlightBlock(i - 1, j + yMod) && isHardlightBlock(i - 1, j))
							{
								if (yMod == -1)
								{
									frameY += 6;
									frameX++;
								}
								else
								{
									frameY++;
								}
							}
							if (!isHardlightBlock(i + 1, j + yMod) && isHardlightBlock(i + 1, j))
							{
								if (yMod == -1)
								{
									frameY--;
								}
								else
								{
									frameY += 2;
								}
							}
							Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(frameX, frameY), color, 0f, origin, 1f, SpriteEffects.None, 0f);
						}
					}

					if(isHardlightBlock(i - 1, j - 1) && !isHardlightBlock(i - 1, j) && !isHardlightBlock(i, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(5, 6), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					else if(!isHardlightBlock(i - 1, j) && !isHardlightBlock(i, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(1, 1), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}

					if (isHardlightBlock(i + 1, j - 1) && !isHardlightBlock(i + 1, j) && !isHardlightBlock(i, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(6, 6), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					else if(!isHardlightBlock(i + 1, j) && !isHardlightBlock(i, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(3, 1), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}

					if (isHardlightBlock(i - 1, j + 1) && !isHardlightBlock(i - 1, j) && !isHardlightBlock(i, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(5, 7), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					else if(!isHardlightBlock(i - 1, j) && !isHardlightBlock(i, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(1, 4), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}

					if (isHardlightBlock(i + 1, j + 1) && !isHardlightBlock(i + 1, j) && !isHardlightBlock(i, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(6, 7), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					else if(!isHardlightBlock(i + 1, j) && !isHardlightBlock(i, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(3, 4), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if(isHardlightBlock(i - 1, j) && isHardlightBlock(i, j - 1) && !isHardlightBlock(i - 1, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(5, 4), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i + 1, j) && isHardlightBlock(i, j - 1) && !isHardlightBlock(i + 1, j - 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(6, 4), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i - 1, j) && isHardlightBlock(i, j + 1) && !isHardlightBlock(i - 1, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(5, 5), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
					if (isHardlightBlock(i + 1, j) && isHardlightBlock(i, j + 1) && !isHardlightBlock(i + 1, j + 1))
					{
						Main.spriteBatch.Draw(omega, pos + offset, getTileFrame(6, 5), color, 0f, origin, 1f, SpriteEffects.None, 0f);
					}
				}
			}
        }
		public static bool isHardlightBlock(Tile tile)
        {
			return tile.HasTile && tile.TileType == ModContent.TileType<HardlightBlockTile>();
		}
		public static bool isHardlightBlock(int i, int j)
		{
			return isHardlightBlock(Main.tile[i, j]);
		}
		public static Rectangle getTileFrame(int cellX, int cellY)
        {
			return new Rectangle(2 + 22 * cellX, 2 + 22 * cellY, 20, 20);
		}
        public override bool CanExplode(int i, int j)
		{
			return true;
		}
		public override bool Slope(int i, int j)
		{
			return false;
		}
	}
}