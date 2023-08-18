using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Furniture.Permafrost
{
	public class PermafrostPlatingDresser : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(38, 26);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<PermafrostPlatingDresserTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PermafrostPlating>(), 16).AddTile(TileID.Anvils).Register();
		}
	}
	public class PermafrostPlatingDresserTile : Dresser
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override int DresserDrop => ModContent.ItemType<PermafrostPlatingDresser>();
        protected override string DresserName => Language.GetTextValue("Mods.SOTS.ContainerName.PermafrostPlatingDresserTile");
		public override LocalizedText DefaultContainerName(int frameX, int frameY)
		{
			return Language.GetText("Mods.SOTS.ContainerName.PermafrostPlatingDresserTile");
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
}