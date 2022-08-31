using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using SOTS.Items.Fragments;

namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingBench : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(36, 24);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingBenchTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 5).AddIngredient(ItemID.Silk, 2).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingBenchTile : Sofa<PermafrostPlatingBench>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
    }
}