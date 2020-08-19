using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.Otherworld
{
	public class HardlightFabricator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hardlight Fabricator");
			Tooltip.SetDefault("Used to craft otherworldly objects and gear");
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightFabricatorBase");
			Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightFabricatorBase");
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, lightColor * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/HardlightGearMiniBorder");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightGearMiniFill");
			Color color = new Color(110, 110, 110, 0);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if(k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2(position.X, position.Y), null, color * 0.5f, 0f, origin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture,new Vector2(position.X + x, position.Y + y),null, color * (1f - (item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/HardlightGearMiniBorder");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightGearMiniFill");
			Color color = new Color(110, 110, 110, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.03f;
				float y = Main.rand.Next(-10, 11) * 0.03f;
				if (k == 0)
					Main.spriteBatch.Draw(texture2, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color * 0.5f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X) + x, (float)(item.Center.Y - (int)Main.screenPosition.Y) + y + 2),null, color * (1f - (item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = 9;
			item.value = Item.sellPrice(0, 0, 0, 0);
			item.consumable = true;
			item.createTile = mod.TileType("HardlightFabricatorTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "AvaritianPlating", 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}	
	public class HardlightFabricatorTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hardlight Fabricator");
			AddMapEntry(new Color(55, 55, 55), name);
			disableSmartCursor = true;
			dustType = mod.DustType("AvaritianDust");
			adjTiles = new int[] { TileID.WorkBenches };
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = mod.ItemType("HardlightFabricator");
			Item.NewItem(i * 16, j * 16, 32, 16, drop);
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].frameX < 18 || Main.tile[i, j].frameX > 35 || Main.tile[i, j].frameY % 36 < 18)
				return;

			r = 0.9f;
			g = 0.9f;
			b = 1.1f;
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int style = Main.tile[i, j].frameY / 36;
			style++;
			int type = 0;
			if (Main.tile[i, j].frameX >= 18 && Main.tile[i, j].frameX < 36 && Main.tile[i, j].frameY % 36 >= 18)
				type = 1;
			if (Main.tile[i, j].frameX < 18 && Main.tile[i, j].frameY % 36 >= 18)
				type = 2;
			if(Main.tile[i, j].frameX >= 36 && Main.tile[i, j].frameY % 36 >= 18)
				type = 3;


			Texture2D texture = mod.GetTexture("Items/Otherworld/HardlightGearBorder");
			Texture2D texture2 = mod.GetTexture("Items/Otherworld/HardlightGearFill");
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 40));
			if(type == 1)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					pos.Y -= 20 + dynamicAddition.Y;
					if(k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, Main.GlobalTime * (i % 2 == 0 ? 1 : -1), new Vector2(13, 13), 0.8f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, Main.GlobalTime * (i % 2 == 0 ? 1 : -1), new Vector2(13, 13), 0.8f, SpriteEffects.None, 0f);
				}
			if(type == 2)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					if (k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);
				}
			if(type == 3)
				for (int k = 0; k < 5; k++)
				{
					Vector2 pos = new Vector2((float)(i * 16 - (int)Main.screenPosition.X) + 8, (float)(j * 16 - (int)Main.screenPosition.Y) + 8) + zero;
					if (k == 0)
						Main.spriteBatch.Draw(texture2, pos, null, color * 0.5f, -Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);

					Main.spriteBatch.Draw(texture, pos, null, color, -Main.GlobalTime, new Vector2(13, 13), 0.5f, SpriteEffects.None, 0f);
				}
			return true;
		}
	}
}