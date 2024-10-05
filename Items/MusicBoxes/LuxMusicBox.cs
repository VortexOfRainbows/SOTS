using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Projectiles.Chaos;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using SOTS.Helpers;
//using SOTS.Items.Trophies;

namespace SOTS.Items.MusicBoxes
{
	public class LuxMusicBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.createTile = ModContent.TileType<LuxMusicBoxTile>();
			Item.width = 32;
			Item.height = 28;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.accessory = true;
		}
		/*public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(null, "AdvisorTrophy", 1);
			recipe.AddIngredient(ItemID.MusicBox);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}
	public class LuxMusicBoxTile : ModTile
    {
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<LuxMusicBox>());
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
			player.cursorItemIconID = ModContent.ItemType<LuxMusicBox>();
		}
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			int frameX = Main.tile[i, j].TileFrameX / 18;
			int frameY = Main.tile[i, j].TileFrameY / 18;
			if (frameX % 2 == 0 && frameY % 2 == 0) //top left tile
			{
				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				if (Main.drawToScreen)
				{
					zero = Vector2.Zero;
				}
				int total = 6 + 3 * frameX;
				for (int k = 0; k < total; k++)
				{
					float radians = MathHelper.ToRadians(360f * k / total);
					Vector2 circularOffset = new Vector2(1.0f + frameX * 0.5f, 0).RotatedBy(radians + MathHelper.ToRadians(SOTSWorld.GlobalCounter * (2 + frameX)));
					Color color = ColorHelper.Pastel(radians, false);
					color.A = 0;
					color *= 0.5f + 0.1f * frameX;
					spriteBatch.Draw(Mod.Assets.Request<Texture2D>("Items/MusicBoxes/LuxMusicBoxTileGlow").Value, new Vector2(i * 16 - Main.screenPosition.X, j * 16 - Main.screenPosition.Y + 2) + zero + circularOffset, new Rectangle(frameX * 16, 0, 32, 32), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
				}
				if(frameX == 2)
					DrawRings(new Vector2(i * 16 + 16, j * 16 + 10) + zero, spriteBatch, true);
			}
		}
		private void DrawRings(Vector2 position, SpriteBatch spriteBatch, bool front = false)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<ChaosSphere>()].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int startEnd = 0;
			if (front)
				startEnd = 180;
			for(int j = 0; j < 2; j ++)
			{
				for (int i = startEnd; i < startEnd + 180; i += 6)
				{
					float radians = MathHelper.ToRadians(i);
					Color color = ColorHelper.Pastel(radians, false);
					color.A = 0;
					color *= 0.5f;
					float sinusoid = (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * (3 - j)));
					Vector2 circular = new Vector2(11 + 7 * j, 0).RotatedBy(radians + MathHelper.ToRadians(SOTSWorld.GlobalCounter * (2 + j * 0.5f)));
					circular.X *= sinusoid;
					circular = circular.RotatedBy(MathHelper.ToRadians(45 - 90 * j + (SOTSWorld.GlobalCounter * (j * 2 - 1))));
					spriteBatch.Draw(texture, position - Main.screenPosition + circular, null, color, 0f, drawOrigin, 0.35f, SpriteEffects.None, 0f);
				}
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
				DrawRings(new Vector2(i * 16 + 16, j * 16 + 10) + zero, spriteBatch, false);
			return true;
		}
        public override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
			Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }
    }
}