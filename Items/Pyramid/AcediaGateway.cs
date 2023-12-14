using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Pyramid
{
	public class AcediaGateway : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 74;
			Item.height = 66;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Purple;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AcediaGatewayTile>();
		}
	}	
	public class AcediaGatewayTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Height = 9;
			TileObjectData.newTile.Width = 9;
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 16, 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 5, 2);
			TileObjectData.newTile.Origin = new Point16(4, 8);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(44, 12, 62), name);
			//TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = ModContent.DustType<Dusts.AcedianDust>();
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			//float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/AcediaGatewayTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.2f; // + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero;
			spriteBatch.Draw(texture, pos, frame, color * alphaMult * 1f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		}
		public static void DrawGlowmask(int x, int y, SpriteBatch spriteBatch, float percent = 1, int side = -1)
        {
			Tile tile = Main.tile[x, y];
			x = x - tile.TileFrameX / 18;
			y = y - tile.TileFrameY / 18;
			string variant = side == -1 ? "Left" : side == 0 ? "Middle" : "Right";
			int maximumGlow = 8;
			maximumGlow = (int)(maximumGlow * percent + 0.5f);
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/AcediaGatewayTileGlow" + variant).Value;
			Texture2D textureMask = ModContent.Request<Texture2D>("SOTS/Items/Conduit/AcediaGatewayTileGlowMask" + variant).Value;
			for(int twice = 0; twice < 2; twice++)
			{
				for (int i = x; i < x + 9; i++)
				{
					for (int j = y; j < y + 9; j++)
					{
						tile = Main.tile[i, j];
						Color defaultColor = new Color(120, 100, 130, 0);
						Color alternatingColor = ColorHelpers.AcediaColor * 0.65f;
						float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 6;
						float lerpMult = 0.5f + 0.5f * (float)Math.Sin(MathHelper.ToRadians(-uniquenessCounter));
						Color color = Color.Lerp(defaultColor, alternatingColor, lerpMult);
						color.A = 0;
						Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
						Vector2 pos = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y);
						if (twice == 1)
						{ 
							for (int k = 0; k <= maximumGlow; k++)
							{
								float offset = 2f * percent;
								Vector2 circular = new Vector2(offset, 0).RotatedBy(MathHelper.TwoPi * k / maximumGlow + MathHelper.ToRadians(SOTSWorld.GlobalCounter * side * 0.75f));
								spriteBatch.Draw(texture, pos + circular + new Vector2(0, 2), frame, color * percent * 0.45f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
							}
						}
						else
						{
							color = Color.White;
							spriteBatch.Draw(textureMask, pos + new Vector2(0, 2), frame, color * 0.75f * percent, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
		}

		public override bool CanExplode(int i, int j)
        {
            return false;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			int type = Main.tile[i, j].TileFrameX / 18 + (Main.tile[i, j].TileFrameY / 18 * 9);
			if (type != 67)
				return;

			r = 1.1f;
			g = 0.5f;
			b = 1.1f;

			r *= 0.25f;
			b *= 0.25f;
			g *= 0.25f;
		}
	}
}