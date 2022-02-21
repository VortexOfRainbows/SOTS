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
            item.damage = 12;
            item.melee = true;  
            item.width = 44;   
            item.height = 44;   
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
            item.useTime = 20;
            item.useAnimation = 30;
			item.pick = 50;
			item.knockBack = 3.25f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.tileBoost = 4;
			item.autoReuse = true;
			item.consumable = false;
			item.crit = 4;
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
