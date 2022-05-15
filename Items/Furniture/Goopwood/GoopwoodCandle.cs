using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Slime;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodCandle : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(16, 20);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodCandleTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 4).AddIngredient(ItemID.PinkTorch, 1).AddTile(TileID.WorkBenches).Register();
			CreateRecipe(5).AddIngredient(ModContent.ItemType<Wormwood>(), 1).AddIngredient(ItemID.PinkGel, 1).ReplaceResult(ItemID.PinkTorch);
		}
	}	
	public class GoopwoodCandleTile : Candle<GoopwoodCandle>
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override Vector3 LightClr => new Vector3(1.2f, 0.1f, 1.2f);
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Flame"));
			for (int k = 0; k < 5; k++)
			{
				SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, new Color(100, 100, 100, 0), Main.rand.NextVector2Circular(1, 1) * (k * 0.25f));
			}
		}
	}
}