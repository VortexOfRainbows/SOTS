using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Slime;

namespace SOTS.Items.Furniture.Goopwood
{
	public class GoopwoodBarrel : ModItem
	{
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.Size = new Vector2(24, 26);
			Item.rare = ItemRarityID.Blue;
			Item.createTile = ModContent.TileType<GoopwoodBarrelTile>();
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Wormwood>(), 9);
			recipe.AddRecipeGroup(RecipeGroupID.IronBar, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class GoopwoodBarrelTile : ContainerType
    {
        protected override string ChestName => "Goopwood Barrel";
        protected override int ChestDrop => ModContent.ItemType<GoopwoodBarrel>();
        protected override int DustType => 7; //wood
        protected override void AddMapEntires()
        {
            Color color = Color.Lerp(SOTSTile.NaturePlatingColor, Color.Black, 0.17f);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(ChestName);
            AddMapEntry(color, name, MapChestName);
        }
        protected override int ShowHoverItem(Player player, int i, int j, int x, int y)
        {
            return ChestDrop;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 32, 32, chestDrop);
            base.KillMultiTile(i, j, frameX, frameY);
        }
    }
}