using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.GhostTown;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class AncientSteelGreatPickaxe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Great Pickaxe");
			Tooltip.SetDefault("Able to mine Meteorite\nCritical strikes deal 50% more damage and may also apply a stacking, permanent bleed for 5 damage per second");
		}
		public override void SetDefaults()
		{
            Item.damage = 12;
            Item.melee = true;  
            Item.width = 44;   
            Item.height = 44;   
            Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTurn = true;
            Item.useTime = 20;
            Item.useAnimation = 30;
			Item.pick = 50;
			Item.knockBack = 3.25f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 4;
			Item.autoReuse = true;
			Item.consumable = false;
			Item.crit = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 15);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
