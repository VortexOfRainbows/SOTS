using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingToilet : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 30);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingToiletTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 8).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingToiletTile : Chair<PermafrostPlatingToilet>
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