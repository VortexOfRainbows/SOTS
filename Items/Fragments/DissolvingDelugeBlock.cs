using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingDelugeBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deluge Block");
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
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<DissolvingDelugeTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(20).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).Register();
		}
	}
	public class DissolvingDelugeTile : ModTile
	{
		public static Color color = new Color(64, 72, 178);
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<DissolvingDelugeBlock>();
			AddMapEntry(color);
			MineResist = 0.2f;
			TileID.Sets.GemsparkFramingTypes[Type] = Type;
		}
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
			num = 5;
        }
        public override bool CreateDust(int i, int j, ref int type)
		{
			Dust dust = Dust.NewDustDirect(new Vector2(i * 16, j * 16) - new Vector2(5), 16, 16, DustID.RainbowMk2);
			dust.color = color;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 0.8f;
			g = 1.0f;
			b = 2.5f;
			r *= 0.3f;
			g *= 0.3f;
			b *= 0.3f;
		}
		public static void DrawEffects(int i, int j, SpriteBatch spriteBatch, Mod mod, bool wall = false)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/DelugeParticle").Value;
			Texture2D textureBlock = Mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/DelugeBlockOutline").Value;
			Color color; // = DissolvingDelugeTile.color;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int sides = 0; sides < 8; sides++)
			{
				bool straight = true;
				int extraI = 0;
				int extraJ = 0;
				if (sides == 0)
				{
					extraJ = -1;
				}
				if (sides == 1)
				{
					extraJ = -1;
					extraI = 1;
					straight = false;
				}
				if (sides == 2)
				{
					extraI = 1;
				}
				if (sides == 3)
				{
					extraI = 1;
					extraJ = 1;
					straight = false;
				}
				if (sides == 4)
				{
					extraJ = 1;
				}
				if (sides == 5)
				{
					extraJ = 1;
					extraI = -1;
					straight = false;
				}
				if (sides == 6)
				{
					extraI = -1;
				}
				if (sides == 7)
				{
					extraI = -1;
					extraJ = -1;
					straight = false;
				}

				Tile next = Framing.GetTileSafely(i + extraI, j + extraJ);
				Tile next1 = next;
				Tile next2 = next;
				if (extraI != 0)
					next1 = Framing.GetTileSafely(i + extraI, j);
				if (extraJ != 0)
					next2 = Framing.GetTileSafely(i, j + extraJ);
				bool run = true;
				if ((next.HasTile && (Main.tileSolid[next.TileType] || next1.WallType == ModContent.WallType<DelugeWallWall>())) || (next1.HasTile && ((Main.tileSolid[next1.TileType] && next1.TileType == ModContent.TileType<DissolvingDelugeTile>()) || next1.WallType == ModContent.WallType<DelugeWallWall>())) || (next2.HasTile && ((Main.tileSolid[next2.TileType] && next2.TileType == ModContent.TileType<DissolvingDelugeTile>()) || next2.WallType == ModContent.WallType<DelugeWallWall>())))
					run = false;
				if (run)
					for (int k = 0; k < 8; k += 1)
					{
						Vector2 location = new Vector2(i * 16 + 8, j * 16 + 8);
						//color = DissolvingDelugeTile.color;
						//if(Main.tile[i, j].TileColor != 0)
						color = WorldGen.paintColor((int)Main.tile[i, j].TileColor);
						if (wall && Main.tile[i, j].WallColor != 0)
							color = WorldGen.paintColor((int)Main.tile[i, j].WallColor);
						color = new Color(color.R, color.G, color.B, 0);
						float timer = (((i + j) % 2 == 0 ? 8 : 0) + k + sides * 8) * 22.5f + (int)(Main.GlobalTimeWrappedHourly * 50);
						Vector2 rotationalPosition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(timer));
						Vector2 toLocation = Vector2.Zero;
						toLocation.Y -= rotationalPosition.X;
						toLocation.Y -= 17;

						if (straight)
						{
							toLocation.X -= 8;
							toLocation.X += k * 2;
							toLocation = toLocation.RotatedBy(MathHelper.ToRadians(45 * sides));
						}
						else
						{
							toLocation.Y += 5;
							location -= new Vector2(0, 8).RotatedBy(MathHelper.ToRadians(45 * sides));
							float relation = k - 3.5f;
							toLocation = toLocation.RotatedBy(MathHelper.ToRadians(45 * sides + (11.25f * relation)));
						}
						Vector2 drawPos = location + toLocation - Main.screenPosition;

						color *= (float)(14f / 14f);
						for (int l = 0; l < 3; l++)
						{
							float x = Main.rand.Next(-16, 17) * 0.05f;
							float y = Main.rand.Next(-16, 17) * 0.05f;
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y) + zero, null, color * 0.825f, MathHelper.ToRadians(timer), new Vector2(3, 2), 0.75f, SpriteEffects.None, 0f);
						}
					}
			}
			if (Main.tileSolid[Main.tile[i, j ].TileType] && !Main.tileSolidTop[Main.tile[i, j ].TileType])
			{
				//color = DissolvingDelugeTile.color;
				//if (Main.tile[i, j].TileColor != 0)
				color = WorldGen.paintColor((int)Main.tile[i, j].TileColor);
				if (wall && Main.tile[i, j].WallColor != 0)
					color = WorldGen.paintColor((int)Main.tile[i, j].WallColor);
				color = new Color(color.R, color.G, color.B, 0);
				for (int l = 0; l < 7 - (Main.tile[i, j].IsActuated ? 1 : 0); l++)
				{
					float x = Main.rand.Next(-16, 17) * 0.1f;
					float y = Main.rand.Next(-16, 17) * 0.1f;
					if (Main.tile[i, j].IsActuated && l < 4)
					{
						x = 0;
						y = 0;
					}
					bool canUp = true;
					bool canDown = true;
					bool canLeft = true;
					bool canRight = true;
					if (Main.tile[i, j - 1].HasTile && Main.tileSolid[Main.tile[i, j - 1].TileType])
						canUp = false;

					if (Main.tile[i, j + 1].HasTile && Main.tileSolid[Main.tile[i, j + 1].TileType])
						canDown = false;

					if (Main.tile[i + 1, j].HasTile && Main.tileSolid[Main.tile[i + 1, j].TileType])
						canRight = false;

					if (Main.tile[i - 1, j].HasTile && Main.tileSolid[Main.tile[i + 1, j].TileType])
						canLeft = false;

					if (!canUp && !canDown)
					{
						y = 0;
					}
					else if (!canUp || !canDown)
					{
						if (!canUp)
							y = Math.Abs(y);

						if (!canDown)
							y = -Math.Abs(y);
					}
					if (!canRight && !canLeft)
					{
						x = 0;
					}
					else if (!canRight || !canLeft)
					{
						if (!canRight)
							x = -Math.Abs(x);

						if (!canLeft)
							x = Math.Abs(x);
					}
					Main.spriteBatch.Draw(textureBlock, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y - 2) + zero, new Rectangle(0, 20 * (Main.tile[i, j].IsHalfBlock ? 1 : Main.tile[i, j].slope() > 0 ? Main.tile[i, j].slope() + 1 : 0), 16, 20), color, 0f, default, 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawEffects(i, j, spriteBatch, Mod);
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}