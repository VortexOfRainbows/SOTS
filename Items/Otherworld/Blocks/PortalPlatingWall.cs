using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Dusts;

namespace SOTS.Items.Otherworld.Blocks
{
	public class PortalPlatingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritia Plating Wall");
			Tooltip.SetDefault("");
		}

		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.rare = ItemRarityID.LightRed;
			Item.createWall = ModContent.WallType<AvaritianPlatingWallWall>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AvaritianPlating>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
	public class AvaritianPlatingWallWall : ModWall
	{
		public static void DrawWallGlow(int wallType, int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = ModContent.GetTexture("SOTS/Items/Otherworld/Blocks/AvaritianPlatingWallWallGlow");
			int xLength = 32;
			int xOff = 0;
			/*if (Main.tile[i - 1, j].wall != 0)// && Main.tile[i + 1, j].wall == 0)
			{
				xOff += 8;
				xLength -= 8;
			}
			if (Main.tile[i + 1, j].wall != 0)// && Main.tile[i - 1, j].wall == 0)
			{
				xLength -= 8;
			}*/
			Rectangle frame = new Rectangle(tile.wallFrameX() + xOff, tile.wallFrameY(), xLength, 32);
			Color color;
			color = WorldGen.paintColor((int)tile.wallColor()) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			Main.spriteBatch.Draw(Main.wallTexture[wallType], pos + new Vector2(-8 + xOff, -8), frame, Lighting.GetColor(i, j, Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			for (int k = 0; k < 3; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.2f * k;
				Main.spriteBatch.Draw(texture, pos + offset + new Vector2(-8 + xOff, -8), frame, color * alphaMult * 0.4f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawWallGlow(Type, i, j, spriteBatch);
			return false;
		}
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			dustType = ModContent.DustType<AvaritianDust>();
			drop = ModContent.ItemType<PortalPlatingWall>();
			AddMapEntry(new Color(0, 130, 215));
		}
	}
	public class PortalPlatingWallWall : ModWall //unsafe avaritian plating wall, prevents houses from forming in planetarium
	{
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			AvaritianPlatingWallWall.DrawWallGlow(Type, i, j, spriteBatch);
			return false;
		}
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			dustType = ModContent.DustType<AvaritianDust>();
			drop = ModContent.ItemType<PortalPlatingWall>();
			AddMapEntry(new Color(0, 130, 215));
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}