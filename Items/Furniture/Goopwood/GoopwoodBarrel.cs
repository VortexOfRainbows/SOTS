using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Slime;
using Terraria.Localization;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodBarrel : ModItem
	{
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(24, 26);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodBarrelTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Wormwood>(), 9).AddRecipeGroup(RecipeGroupID.IronBar, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class GoopwoodBarrelTile : ContainerType
    {
        protected override string GetChestName()
        {
            return Language.GetTextValue("Mods.SOTS.ContainerName.GoopwoodBarrelTile");
        }
        protected override int ChestDrop => ModContent.ItemType<GoopwoodBarrel>();
        protected override int DustType => 7; //wood
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(new Color(191, 142, 111, 255), Color.Black, 0.17f);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(GetChestName());
            AddMapEntry(color, name, MapChestName);
        }
    }
}