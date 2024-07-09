using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Dusts;
using Terraria.GameContent;

namespace SOTS.Items.Planetarium.Blocks
{
	public class PortalPlatingWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.rare = ItemRarityID.LightRed;
			Item.createWall = ModContent.WallType<AvaritianPlatingWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<AvaritianPlating>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class AvaritianPlatingWallWall : ModWall
	{
		public static void DrawWallGlow(int wallType, int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Planetarium/Blocks/AvaritianPlatingWallWallGlow");
			int xLength = 32;
			int xOff = 0;
			/*if (Main.tile[i - 1, j].WallType != 0)// && Main.tile[i + 1, j].WallType == 0)
			{
				xOff += 8;
				xLength -= 8;
			}
			if (Main.tile[i + 1, j].WallType != 0)// && Main.tile[i - 1, j].WallType == 0)
			{
				xLength -= 8;
			}*/
			Rectangle frame = new Rectangle(tile.WallFrameX + xOff, tile.WallFrameY, xLength, 32);
			Color color;
			color = WorldGen.paintColor((int)tile.WallColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			Main.spriteBatch.Draw(TextureAssets.Wall[wallType].Value, pos + new Vector2(-8 + xOff, -8), frame, Lighting.GetColor(i, j, Color.White), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
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
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			DustType = ModContent.DustType<AvaritianDust>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PortalPlatingWall>();
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
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = false;
			DustType = ModContent.DustType<AvaritianDust>();
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<PortalPlatingWall>();
			AddMapEntry(new Color(0, 130, 215));
		}
        public override bool Drop(int i, int j, ref int type)
        {
			type = ModContent.ItemType<PortalPlatingWall>();
			return true;
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}