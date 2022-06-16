using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Slime
{
	public class Wormwood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood");
			this.SetResearchCost(100);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 22;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.Blue;
			Item.consumable = true;
			Item.createTile = Mod.Find<ModTile>("WormwoodTile").Type;
		}
		public override void AddRecipes()
		{
			CreateRecipe(2).AddRecipeGroup(RecipeGroupID.Wood, 2).AddIngredient(ItemID.Gel, 5).AddTile(TileID.Solidifier).Register();
		}
	}
	public class WormwoodTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			ItemDrop = Mod.Find<ModItem>("Wormwood").Type;
			AddMapEntry(new Color(140, 70, 20));
			DustType = 7; //dynasty wood dust
		}
	}
}