using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Items.Otherworld.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.Blocks
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
			item.CloneDefaults(ItemID.StoneBlock);
			item.rare = ItemRarityID.LightRed;
			item.createTile = ModContent.TileType<AvaritianPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DullPlatingWall>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DullPlating>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TwilightGel>(), 5);
			recipe.AddIngredient(ModContent.ItemType<TwilightShard>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this, 10);
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
			drop = ModContent.ItemType<AvaritianPlating>();
			AddMapEntry(new Color(0, 75, 140));
			mineResist = 2f;
			minPick = 60;
			soundType = 21;
			soundStyle = 2;
			dustType = ModContent.DustType<AvaritianDust>();
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
			Texture2D texture = mod.GetTexture("Items/Otherworld/Blocks/AvaritianPlatingTileGlow");
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].color()) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 5; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				SOTSTile.DrawSlopedGlowMask(i, j, tile.type, texture, color * alphaMult * 0.8f, offset);
			}
		}
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}