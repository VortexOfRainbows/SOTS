using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Furniture.Nature
{
	public class NaturePlatingDresser : ModItem
	{
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(38, 24);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<NaturePlatingDresserTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<NaturePlating>(), 16).AddTile(TileID.Anvils).Register();
		}
	}
	public class NaturePlatingDresserTile : Dresser
	{
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		protected override int DresserDrop => ModContent.ItemType<NaturePlatingDresser>();
        protected override string DresserName => Language.GetTextValue("Mods.SOTS.ContainerName.NaturePlatingDresserTile");
		public override LocalizedText DefaultContainerName(int frameX, int frameY)
		{
			return Language.GetText("Mods.SOTS.ContainerName.NaturePlatingDresserTile");
		}
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D glowmask = (Texture2D)ModContent.Request<Texture2D>(this.GetPath("Glow"));
            SOTSTile.DrawSlopedGlowMask(i, j, -1, glowmask, Color.White, Vector2.Zero);
        }
    }
}