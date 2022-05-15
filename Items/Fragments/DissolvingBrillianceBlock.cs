using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingBrillianceBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brilliance Block");
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<DissolvingBrillianceTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(20).AddIngredient(ModContent.ItemType<DissolvingBrilliance>(), 1).Register();
		}
	}
	public class DissolvingBrillianceTile : ModTile
	{
		public static Color color = new Color(231, 95, 203);
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			drop = ModContent.ItemType<DissolvingBrillianceBlock>();
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
			r = 2.3f;
			g = 0.95f;
			b = 2.0f;
			r *= 0.3f;
			g *= 0.3f;
			b *= 0.3f;
		}
		public static void DrawEffects(int i, int j, SpriteBatch spriteBatch, Mod mod, bool wall = false)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/BrillianceParticle").Value;
			Texture2D textureBlock = Mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/BrillianceBlockOutline").Value;
			Color color; // = DissolvingBrillianceTile.color;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			float timer = (i + j) * 20 + Main.GlobalTimeWrappedHourly * 100;
			float sinusoid = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(timer * 1.2f));
			for (int side = 0; side < 8; side++)
			{
				int extraI = 0;
				int extraJ = 0;
				if (side == 0)
				{
					extraJ = -1;
				}
				if (side == 1)
				{
					extraJ = -1;
					extraI = 1;
				}
				if (side == 2)
				{
					extraI = 1;
				}
				if (side == 3)
				{
					extraI = 1;
					extraJ = 1;
				}
				if (side == 4)
				{
					extraJ = 1;
				}
				if (side == 5)
				{
					extraJ = 1;
					extraI = -1;
				}
				if (side == 6)
				{
					extraI = -1;
				}
				if (side == 7)
				{
					extraI = -1;
					extraJ = -1;
				}
				Tile next = Framing.GetTileSafely(i + extraI, j + extraJ);
				bool run = true;
				if (next.HasTile && Main.tileSolid[next.TileType])
					run = false;
				if (run)
				{
					int offset = 1;
					if (extraI == 0 || extraJ == 0)
						offset = 0;
					for (int k = 0 + offset; k < 5- offset; k++)
					{
						Vector2 location = new Vector2(i * 16 + 8, j * 16 + 8);
						//color = DissolvingBrillianceTile.color;
						//if(Main.tile[i, j].TileColor != 0)
						color = WorldGen.paintColor((int)Main.tile[i, j].TileColor);
						if (wall && Main.tile[i, j].WallColor != 0)
							color = WorldGen.paintColor((int)Main.tile[i, j].WallColor);
						color = new Color(color.R, color.G, color.B, 0);
						Vector2 circular = new Vector2(0, -20).RotatedBy(MathHelper.ToRadians(side * 45 + (k - 2) * 11.25f));
						circular *= sinusoid;
						Vector2 drawPos = location + circular - Main.screenPosition;
						for (int l = 0; l < 2; l++)
						{
							float x = Main.rand.NextFloat(-1, 1);
							float y = Main.rand.NextFloat(-1, 1);
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y) + zero, null, color * 0.9f, MathHelper.ToRadians(timer), new Vector2(3, 3), 0.8f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			if (Main.tileSolid[Main.tile[i, j ].TileType] && !Main.tileSolidTop[Main.tile[i, j ].TileType])
			{
				//color = DissolvingBrillianceTile.color;
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
			//DrawEffects(i, j, spriteBatch, mod);
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
			Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}