using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using SOTS.Items.Banners;
using SOTS.Projectiles.AbandonedVillage;

namespace SOTS.Items.MusicBoxes
{
	public class ExcavatorMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<ExcavatorMusicBoxTile>();
			Item.width = 32;
			Item.height = 42;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.accessory = true;
		}
		public override void AddRecipes()
		{
            CreateRecipe(1).AddIngredient<ExcavatorTrophy>(1).AddIngredient(ItemID.MusicBox, 1).AddTile(TileID.HeavyWorkBench).Register();
        }
	}
	public class ExcavatorMusicBoxTile : ModTile
    {
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<ExcavatorMusicBox>());
        }
        public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = [16, 18]; 
			TileObjectData.newTile.LavaDeath = false;
			//TileObjectData.newTile.DrawYOffset = 2;
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
			player.cursorItemIconID = ModContent.ItemType<ExcavatorMusicBox>();
		}
		private void DrawLightning(Vector2 start, Vector2 end, SpriteBatch spriteBatch)
		{
			Texture2D texture = SOTSUtils.WhitePixel;
			Vector2 drawOrigin = new Vector2(0, 1);
			Vector2 prev = start;
			int max = 18;
			float rad = MathHelper.ToRadians(SOTSWorld.GlobalCounter * 3f + start.X * 12);
			Color c1 = new Color(ExcavatorOrb.Color.R + 20, ExcavatorOrb.Color.G + 10, ExcavatorOrb.Color.B, 0) * 0.67f;
            for (int i = 0; i <= max; ++i)
			{
				float percent = (i + 0.5f) / max;
				float sin = MathF.Sin(percent * MathF.PI);
				float fakeRand = 4 * MathF.Sin(percent * MathF.PI * 5 + rad * 4);
                fakeRand += 2 * MathF.Cos(percent * MathF.PI * 7 + rad * 3);
                fakeRand += 2 * MathF.Sin(-percent * MathF.PI * 12 - rad * 5);
                fakeRand += 2 * MathF.Cos(percent * MathF.PI * 6 - rad * 2);

                Vector2 p = Vector2.Lerp(start, end, percent);
				p.Y += fakeRand * sin * (0.5f + 0.5f * sin) - sin * 3;
				Vector2 toPrev = prev - p;
				float r = toPrev.ToRotation();
				float len = toPrev.Length();
                Main.spriteBatch.Draw(texture, p - Main.screenPosition, null, c1, r, drawOrigin, new Vector2((len + 2) / 2f, 2.25f), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, p - Main.screenPosition, null, Color.White * 0.9f, r, drawOrigin, new Vector2(len / 2f, 0.75f), SpriteEffects.None, 0f);
				prev = p;
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int frameX = Main.tile[i, j].TileFrameX / 18;
			int frameY = Main.tile[i, j].TileFrameY / 18;
			if (frameX == 2 && frameY == 0)
                DrawLightning(new Vector2(i * 16 + 4, j * 16 + 3.5f) + zero, new Vector2(i * 16 + 28, j * 16 + 3.5f) + zero, spriteBatch);
			return true;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            int frameX = Main.tile[i, j].TileFrameX / 18;
            int frameY = Main.tile[i, j].TileFrameY / 18;
            if (frameX >= 2)
            {
                Texture2D glowmask = ModContent.Request<Texture2D>(this.GetPath("Glow")).Value;
                Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
                if (Main.drawToScreen)
                {
                    zero = Vector2.Zero;
                }
                Main.spriteBatch.Draw(glowmask, new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y) + zero, new Rectangle(frameX * 18, frameY * 18, 16, 16), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}