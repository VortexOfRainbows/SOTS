using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingChair : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 32);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingChairTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NaturePlating>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class NaturePlatingChairTile : Chair<NaturePlatingChair>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override void SetDefaults(TileObjectData t)
        {
            Main.tileLighted[Type] = true;
            base.SetDefaults(t);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Glow"));
			SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
		}
    }
}