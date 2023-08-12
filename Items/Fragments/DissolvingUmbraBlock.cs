using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Fragments
{
	public class DissolvingUmbraBlock : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.Orange;
			Item.createTile = ModContent.TileType<DissolvingUmbraTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(20).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).Register();
		}
	}
	public class DissolvingUmbraTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<DissolvingUmbraBlock>();
			AddMapEntry(new Color(251, 32, 0));
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
			dust.color = new Color(251, 32, 0);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.8f;
			dust.velocity *= 2.4f;
			return false;
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 2.5f;
			g = 0.4f;
			b = 0.1f;
			r *= 0.3f;
			g *= 0.3f;
			b *= 0.3f;
		}
		public static void DrawEffects(int i, int j, Mod mod, bool wall = false)
        {
			Texture2D texture = mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/UmbraParticle").Value;
			Texture2D textureBlock = mod.Assets.Request<Texture2D>("Assets/SpiritBlocks/UmbraBlockOutline").Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float height = 8;
			float timer = Main.GlobalTimeWrappedHourly * -90 + (i + j) * 10;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 offset = new Vector2(0, 0);
			if(Main.tile[i, j].TopSlope || Main.tile[i, j].IsHalfBlock)
			{
				offset += new Vector2(0, 8);
			}
			int maxLength = 9;
			for (int j2 = 1; j2 < maxLength; j2++)
			{
				Tile tile2 = Framing.GetTileSafely(i, j - j2);
				if ((tile2.HasTile && Main.tileSolid[tile2.TileType] && !Main.tileSolidTop[tile2.TileType]) || !WorldGen.InWorld(i, j - j2, 27))
				{
					maxLength = j2;
					break;
				}
			}
			if(maxLength > 1)
            {
				maxLength *= 2;
				Vector2 previous = new Vector2(i * 16 + 8, j * 16 + 16);
				for (int z = 0; z < maxLength; z++)
				{
					float dynamicMult = 0.52f + 0.48f * (float)Math.Cos(MathHelper.ToRadians(180f * z / maxLength));
					Vector2 dynamicAddition = new Vector2(20f / maxLength * z * 0.2f + 0.3f, 0).RotatedBy(MathHelper.ToRadians(z * 24 + timer)) * dynamicMult;
					Vector2 pos = new Vector2(i * 16 + 8, j * 16);
					pos.Y -= z * 8;
					pos += dynamicAddition;
					Vector2 rotateTo = pos - previous;
					float lengthTo = rotateTo.Length();
					float stretch = (lengthTo / height) * 1.00275f;
					if (z == 0)
						stretch = 1f;
					float compress = 0.9f - (z / (float)maxLength) * 0.6f;
					Vector2 scaleVector2 = new Vector2(stretch, compress);
					if (z != 0)
					{
						float alphaScale = (32f - z * 2f) / 20f;
						for (int k = 0; k < 3; k++)
						{
							Main.spriteBatch.Draw(texture, previous + zero - Main.screenPosition + Main.rand.NextVector2Circular(0.5f, 0.5f) + offset, null, color * alphaScale, rotateTo.ToRotation(), origin, scaleVector2, SpriteEffects.None, 0f);
						}
					}
					previous = pos;
				}
			}
			if (Main.tileSolid[Main.tile[i, j ].TileType] && !Main.tileSolidTop[Main.tile[i, j ].TileType])
			{
				color = WorldGen.paintColor((int)Main.tile[i, j].TileColor);
				if (wall)
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
					Main.spriteBatch.Draw(textureBlock, new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + x, (float)(j * 16 - (int)Main.screenPosition.Y) + y - 2) + zero,
					new Rectangle(0, 20 * (Main.tile[i, j].IsHalfBlock ? 1 : Main.tile[i, j].Slope > 0 ? (int)Main.tile[i, j].Slope + 1 : 0), 16, 20), color, 0f, default, 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawEffects(i, j, Mod);
			return true;
		}
		public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
        {
            Framing.SelfFrame8Way(i, j, Main.tile[i, j], resetFrame);
            return false;
        }
	}
}