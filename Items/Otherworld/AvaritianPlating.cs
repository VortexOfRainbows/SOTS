using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class AvaritianPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avaritia Plating");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.rare = ItemRarityID.Cyan;
			item.consumable = true;
			item.createTile = mod.TileType("AvaritianPlatingTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DullPlatingWall", 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DullPlating", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TwilightGel", 5);
			recipe.AddIngredient(null, "TwilightShard", 1);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class AvaritianPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			drop = mod.ItemType("AvaritianPlating");
			AddMapEntry(new Color(32, 90, 85));
			mineResist = 2f;
			minPick = 80;
			soundType = 21;
			soundStyle = 2;
			dustType = mod.DustType("AvaritianDust");
		}
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.0f;
			g = 0.09f;
			b = 0.13f;
            base.ModifyLight(i, j, ref r, ref g, ref b);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			float uniquenessCounter = Main.GlobalTime * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = mod.GetTexture("Items/Otherworld/AvaritianPlatingTileGlow");
			Rectangle frame = new Rectangle(tile.frameX, tile.frameY, 16, 16);
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
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
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				Main.spriteBatch.Draw(texture, pos + offset, frame, color * alphaMult * 0.8f, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			}
		}
        public override bool CanExplode(int i, int j)
		{
			if (Main.tile[i, j].type == mod.TileType("AvaritianPlatingTile"))
			{
				return false;
			}
			return false;
		}
		public override bool Slope(int i, int j)
		{
			return true;
		}
	}
}