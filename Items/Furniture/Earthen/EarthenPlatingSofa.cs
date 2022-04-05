using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using SOTS.Items.Fragments;

namespace SOTS.Items.Furniture.Earthen
{
	public class EarthenPlatingSofa : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.Size = new Vector2(38, 24);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<EarthenPlatingSofaTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 5);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingSofaTile : Sofa<EarthenPlatingSofa>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
    }
}