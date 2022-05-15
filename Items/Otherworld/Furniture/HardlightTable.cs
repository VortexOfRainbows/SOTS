using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.Blocks;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Otherworld.Furniture
{
	public class HardlightTable : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Table");
			Tooltip.SetDefault("");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableBase").Value;
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableBase").Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableMiniOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableMiniFill").Value;
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture,new Vector2(position.X + x, position.Y + y),null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableMiniOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableMiniFill").Value;
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X) + x, (float)(Item.Center.Y - (int)Main.screenPosition.Y) + y + 2),null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 36;
			Item.height = 30;
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = ModContent.TileType<HardlightTableTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AvaritianPlating>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}	
	public class HardlightTableTile : ModTile
	{
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18) //make it only draw if correct frame to prevent extra iterations
				return;
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableTileGlow").Value;
			Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2((i * 16 - (int)Main.screenPosition.X), (j * 16 - (int)Main.screenPosition.Y)) + zero;
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.10f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.75f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileTable[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 1, 1);
			TileObjectData.addTile(Type);
			AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hardlight Table");
			AddMapEntry(new Color(55, 55, 55));
			disableSmartCursor = true;
			DustType = ModContent.DustType<AvaritianDust>();
			AdjTiles = new int[] { TileID.Tables };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = ModContent.ItemType<HardlightTable>();
			Item.NewItem(i * 16, j * 16, 32, 16, drop);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18)
				return;

			r = 0.52f;
			g = 0.52f;
			b = 0.52f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18)
				return true;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableOutline").Value;
			Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Otherworld/Furniture/HardlightTableFill").Value;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(3f, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 40));
			dynamicAddition.Y += 2;
			if (Main.tile[i - 1, j - 2].HasTile || Main.tile[i, j - 2].HasTile || Main.tile[i + 1, j - 2].HasTile)
				dynamicAddition.Y = 0;
			for (int k = 0; k < 5; k++)
			{
				Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
				pos.Y -= 13 + dynamicAddition.Y;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, 0f, new Vector2(26, 13), 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, pos, null, color, 0f, new Vector2(26, 13), 1f, SpriteEffects.None, 0f);
			}
			return true;
		}
	}
}