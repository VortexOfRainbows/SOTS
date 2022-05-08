using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Slime;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodChandelier : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(28, 30);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodChandelierTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 12);
			recipe.AddIngredient(ItemID.PinkTorch, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class GoopwoodChandelierTile : Chandelier<GoopwoodChandelier>
	{
		protected override Vector3 LightClr => new Vector3(1.3f, 0.12f, 1.3f);
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = ModContent.GetTexture(this.GetPath("Flame"));
			for (int k = 0; k < 5; k++)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
			}
		}
	}
}