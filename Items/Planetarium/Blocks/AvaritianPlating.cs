using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium.Blocks
{
	public class AvaritianPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.rare = ItemRarityID.LightRed;
			Item.createTile = ModContent.TileType<AvaritianPlatingTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DullPlatingWall>(), 4).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DullPlating>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(10).AddIngredient(ModContent.ItemType<TwilightGel>(), 5).AddIngredient(ModContent.ItemType<TwilightShard>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
	public class AvaritianPlatingTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = false;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			//ItemDrop/* tModPorter Note: Removed. Tiles and walls will drop the item which places them automatically. Use RegisterItemDrop to alter the automatic drop if necessary. */ = ModContent.ItemType<AvaritianPlating>();
			AddMapEntry(new Color(0, 75, 140));
			MineResist = 2f;
			MinPick = 60;
			HitSound = SoundID.Tink;
			DustType = ModContent.DustType<AvaritianDust>();
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
			float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
			Tile tile = Main.tile[i, j];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/Blocks/AvaritianPlatingTileGlow").Value;
			Color color;
			color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
			color.A = 0;
			float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
			for (int k = 0; k < 5; k++)
			{
				Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
				SOTSTile.DrawSlopedGlowMask(i, j, tile.TileType, texture, color * alphaMult * 0.8f, offset);
			}
		}
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
}