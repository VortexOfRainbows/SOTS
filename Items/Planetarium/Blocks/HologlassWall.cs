using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Items.Planetarium.Blocks;

namespace SOTS.Items.Planetarium.Blocks
{
	public class HologlassWall : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/Blocks/HologlassWallOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Planetarium/Blocks/HologlassWallFill").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/Blocks/HologlassWallOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Planetarium/Blocks/HologlassWallFill").Value;
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
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(400);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneWall);
			Item.width = 28;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed;
			Item.createWall = ModContent.WallType<HologlassWallWall>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(4).AddIngredient(ModContent.ItemType<HardlightBlock>(), 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class HologlassWallWall : ModWall
	{
		public void DrawWallGlow(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * 6f;
			float offsetY = 2 * ((int)uniquenessCounter % 4);
			offsetY -= 6;
			Tile tile = Main.tile[i, j];
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Planetarium/Blocks/HologlassWallWallOutline");
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Planetarium/Blocks/HologlassWallWallFill");
			Texture2D textureScan = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Planetarium/Blocks/HologlassWallWallScan");
			Texture2D textureFix = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Planetarium/Blocks/HologlassWallWallInline");
			int xLength = 32;
			int xOff = 0;
			Rectangle frame = new Rectangle(tile.WallFrameX + xOff, tile.WallFrameY, xLength, 32);
			Color color = WorldGen.paintColor(tile.WallColor) * (100f / 255f);
			Color forBorder = color;
			forBorder.A = 0;
			color.A = 0;
			color = Color.Lerp(color, Color.Black, 0.08f);
			forBorder = Color.Lerp(forBorder, Color.Black, 0.04f);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
				zero = Vector2.Zero;
			Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.1f;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, pos + new Vector2(-8 + xOff, -8), frame, color * 0.5f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, pos + offset + new Vector2(-8 + xOff, -8), frame, forBorder, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				if(k <= 1)
				{
					if (Main.tile[i, j - 1].WallType != 0 && Main.tile[i, j - 1].WallType != Type)
					{
						Rectangle frameF = new Rectangle(0, 0, 20, 20);
						Main.spriteBatch.Draw(textureFix, pos + offset + new Vector2(-2 + xOff, -2), frameF, forBorder, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					if (Main.tile[i, j + 1].WallType != 0 && Main.tile[i, j + 1].WallType != Type)
					{
						Rectangle frameF = new Rectangle(0, 40, 20, 20);
						Main.spriteBatch.Draw(textureFix, pos + offset + new Vector2(-2 + xOff, -2), frameF, forBorder, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					if (Main.tile[i - 1, j].WallType != 0 && Main.tile[i - 1, j].WallType != Type)
					{
						Rectangle frameF = new Rectangle(0, 20, 20, 20);
						Main.spriteBatch.Draw(textureFix, pos + offset + new Vector2(-2 + xOff, -2), frameF, forBorder, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
					if (Main.tile[i + 1, j].WallType != 0 && Main.tile[i + 1, j].WallType != Type)
					{
						Rectangle frameF = new Rectangle(0, 60, 20, 20);
						Main.spriteBatch.Draw(textureFix, pos + offset + new Vector2(-2 + xOff, -2), frameF, forBorder, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
					}
				}
				if (k <= 1)
					Main.spriteBatch.Draw(textureScan, pos + new Vector2(-8 + xOff, -8 + offsetY), frame, color * 0.9f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawWallGlow(i, j, spriteBatch);
			return false;
		}
		public override void SetStaticDefaults()
		{
			Main.wallHouse[Type] = true;
			Main.wallLight[Type] = true;
			DustType = DustID.Electric;
			////ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<HologlassWall>();
			AddMapEntry(new Color(25, 120, 170));
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
	}
}