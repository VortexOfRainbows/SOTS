using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldCampfire : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 18;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.value = 0;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AncientGoldCampfireTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<RoyalGoldBrick>(10).AddIngredient<AncientGoldTorch>(5).Register();
		}
	}	
	public class AncientGoldCampfireTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);
			ModTranslation name = CreateMapEntryName();
			AddMapEntry(new Color(255, 220, 100), name);
			TileID.Sets.DisableSmartCursor[Type] = true;
			DustType = DustID.GoldCoin;
			AdjTiles = new int[] { TileID.Furnaces };
			AnimationFrameHeight = 36;
		}
        public override void NearbyEffects(int i, int j, bool closer)
		{
			Main.SceneMetrics.HasCampfire = true;
			base.NearbyEffects(i, j, closer);
		}
		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			// Flips the sprite
			SpriteEffects effects = SpriteEffects.None;
			Tile tile = Main.tile[i, j];
			Texture2D texture = SOTSTile.GetTileDrawTexture(i, j);
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			int k = Main.tileFrame[Type] % 8;
			int animate = k * 36;
			Main.spriteBatch.Draw(texture,
				new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero,
				new Rectangle(tile.TileFrameX, tile.TileFrameY + animate, 16, 16),
				Lighting.GetColor(i, j), 0f, default(Vector2), 1f, effects, 0f);
			return false; 
		}
		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if (frameCounter > 2)
			{
				frameCounter = 0;
				frame++;
				if (frame >= 8)
				{
					frame = 0;
				}
			}
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int drop = Mod.Find<ModItem>("AncientGoldCampfire").Type;
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 32, drop);
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (long)((ulong)i));
			Color color = new Color(100, 80, 80, 0);
			int frameX = Main.tile[i, j].TileFrameX / 18;
			int frameY = Main.tile[i, j].TileFrameY / 18;
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Furniture/AncientGold/AncientGoldCampfireTile_Flame").Value;
			int k = Main.tileFrame[Type] % 8;
			int animate = k * 36;
			for (k = 0; k < 7; k++)
			{
				float x = Utils.RandomInt(ref randSeed, -10, 11) * 0.1f;
				float y = Utils.RandomInt(ref randSeed, -10, 11) * 0.1f;
				if (k <= 1)
				{
					x = 0;
					y = 0;
				}
				Main.spriteBatch.Draw(texture, new Vector2(i * 16 - Main.screenPosition.X + x, j * 16 - Main.screenPosition.Y + y) + zero, new Rectangle(frameX * 18, frameY * 18 + animate, 16, 16), color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].TileFrameX < 18 || Main.tile[i, j].TileFrameX > 35 || Main.tile[i, j].TileFrameY % 36 < 18)
				return;

			r = 1.3f;
			g = 1.1f;
			b = 1.1f;
		}
	}
}