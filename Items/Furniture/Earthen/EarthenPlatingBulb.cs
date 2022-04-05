using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Furniture.Earthen
{
	public class EarthenPlatingBulb : ModItem
	{
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.StoneBlock);
			item.Size = new Vector2(16, 20);
			item.rare = ItemRarityID.Blue;
			item.createTile = ModContent.TileType<EarthenPlatingBulbTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EarthenPlating>(), 4);
			recipe.AddIngredient(ItemID.Torch, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class EarthenPlatingBulbTile : Candle<EarthenPlatingBulb>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override Vector3 LightClr => SOTSTile.EarthenPlatingLight * 3f;
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
			for (int k = 0; k < 5; k++)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0),  Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
			}
		}
    }
}