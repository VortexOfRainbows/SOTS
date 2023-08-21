using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using SOTS.Items.Otherworld;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using System.Collections.Generic;

namespace SOTS.Items.MusicBoxes
{
	public class CurseMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.autoReuse = true;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<CurseMusicBoxTile>();
			Item.width = 32;
			Item.height = 26;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 100000;
			Item.accessory = true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PyramidSlab>(10).AddIngredient<PyramidBrick>(10).AddIngredient(ModContent.ItemType<CursedMatter>(), 10).AddIngredient(ModContent.ItemType<CursedTumor>(), 10).AddIngredient(ItemID.MusicBox).AddTile(TileID.HeavyWorkBench).Register();
		}
	}
	public class CurseMusicBoxTile : ModTile
    {
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<CurseMusicBox>());
        }
        public override bool CreateDust(int i, int j, ref int type)
		{
			return false;
		}
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(191, 142, 111), name);
		}
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<CurseMusicBox>();
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			return true;
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			DrawGems(i, j, spriteBatch);
		}
        public void DrawGems(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Tile tile = Main.tile[i, j];
			float counter = Main.GlobalTimeWrappedHourly * 120;
			float mult = new Vector2(-4f, 0).RotatedBy(MathHelper.ToRadians(counter / 2f)).X;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/MusicBoxes/CurseMusicBoxTileGlow").Value;
			if (tile.TileFrameY % 36 != 0 && tile.TileFrameX % 36 != 0) //check for it being the top left tile
			{
				i--;
				j--;
				int currentFrame = tile.TileFrameX / 36;
				Rectangle frame = new Rectangle(currentFrame * 32, 0, 32, 32);
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
					Vector2 rotationAround = new Vector2(8 + mult, 0).RotatedBy(MathHelper.ToRadians(60 * k + counter));
					spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X - 2, j * 16 - (int)Main.screenPosition.Y + 2) + zero + rotationAround,
						frame, color * 0.2f, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
				}
				spriteBatch.Draw(texture, new Vector2(i * 16 - (int)Main.screenPosition.X - 2, j * 16 - (int)Main.screenPosition.Y + 2) + zero,
					frame, Color.White, 0f, default(Vector2), 1.0f, SpriteEffects.None, 0f);
			}
		}
	}
}