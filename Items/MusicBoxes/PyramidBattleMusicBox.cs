using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using SOTS.Items.Otherworld;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;

namespace SOTS.Items.MusicBoxes
{
	public class PyramidBattleMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Music Box (Pyramid Battle)");
		}
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.consumable = true;
			item.createTile = ModContent.TileType<PyramidBattleMusicBoxTile>();
			item.width = 30;
			item.height = 32;
			item.rare = ItemRarityID.LightRed;
			item.value = 100000;
			item.accessory = true;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<RubyKeystone>(), 1);
			recipe.AddIngredient(ModContent.ItemType<RoyalRubyShard>(), 10);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 5);
			recipe.AddIngredient(ModContent.ItemType<CursedTumor>(), 10);
			recipe.AddIngredient(ItemID.MusicBox);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class PyramidBattleMusicBoxTile : ModTile
	{
		public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(191, 142, 111), name);
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 32, ModContent.ItemType<PyramidBattleMusicBox>());
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<PyramidBattleMusicBox>();
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawBack(i, j, spriteBatch);
			DrawGems(i, j, spriteBatch);
			return true;
		}
		public void DrawBack(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			Texture2D texture = mod.GetTexture("Items/MusicBoxes/PyramidBattleMusicBoxTileBack");
			if (tile.frameY % 36 == 0 && tile.frameX % 36 == 0) //check for it being the top left tile
			{
				int currentFrame = tile.frameX / 36;
				Rectangle frame = new Rectangle(currentFrame * 32, 0, 32, 32);
				spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 2) + zero,
					frame, Lighting.GetColor(i, j), 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
			}
		}
		public void DrawGems(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			float counter = Main.GlobalTime * 120;
			float mult = new Vector2(-1f, 0).RotatedBy(MathHelper.ToRadians(counter / 2f)).X;
			Texture2D texture = mod.GetTexture("Items/MusicBoxes/PyramidBattleMusicBoxTileGlow");
			if (tile.frameY % 36 == 0 && tile.frameX % 36 == 0) //check for it being the top left tile
			{
				int currentFrame = tile.frameX / 36;
				for (int k = 0; k < 6; k++)
				{
					Color color = new Color(255, 0, 0, 0);
					switch (k)
					{
						case 0:
							color = new Color(255, 0, 0, 0);
							break;
						case 1:
							color = new Color(255, 50, 0, 0);
							break;
						case 2:
							color = new Color(255, 100, 0, 0);
							break;
						case 3:
							color = new Color(255, 150, 0, 0);
							break;
						case 4:
							color = new Color(255, 200, 0, 0);
							break;
						case 5:
							color = new Color(255, 250, 0, 0);
							break;
					}
					Rectangle frame = new Rectangle(currentFrame * 32, 0, 32, 32);
					Vector2 rotationAround = new Vector2((2 + mult), 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
					spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y + 1) + zero + rotationAround,
						frame, color, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}