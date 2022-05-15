using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using SOTS.Items.Fragments;

namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingChandelier : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(26, 28);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingChandelierTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NaturePlating>(), 4).AddIngredient(ItemID.Torch, 4).AddIngredient(ItemID.Chain, 1).AddTile(TileID.Anvils).Register();
		}
	}
	public class NaturePlatingChandelierTile : Chandelier<NaturePlatingChandelier>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override Vector3 LightClr => SOTSTile.NaturePlatingLight * 3f;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            for (int k = 0; k < 5; k++)
            {
                SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
            }
        }
    }
}