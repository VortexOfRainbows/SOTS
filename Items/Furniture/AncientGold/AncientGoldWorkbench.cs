using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Furniture.AncientGold
{
	public class AncientGoldWorkbench : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Gold Work Bench");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 16;
			Item.maxStack = 999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.rare = ItemRarityID.LightRed;
			Item.value = 0;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<AncientGoldWorkbenchTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<RoyalGoldBrick>(), 10).Register();
		}
	}
	public class AncientGoldWorkbenchTile : Workbench<AncientGoldWorkbench>
	{
		protected override void SetStaticDefaults(TileObjectData t)
		{
			Main.tileLighted[Type] = true;
			base.SetStaticDefaults(t);
			AdjTiles = new int[] { TileID.WorkBenches };
		}
	}
}