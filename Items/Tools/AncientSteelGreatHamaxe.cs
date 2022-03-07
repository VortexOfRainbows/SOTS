using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.GhostTown;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class AncientSteelGreatHamaxe : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Steel Great Hamaxe");
			Tooltip.SetDefault("Critical strikes deal 50% more damage and may also apply a stacking, permanent bleed for 5 damage per second");
		}
		public override void SetDefaults()
		{
            item.damage = 14;
            item.melee = true;  
            item.width = 52;   
            item.height = 48;   
            item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useTime = 18;
			item.useAnimation = 24;
			item.hammer = 45;
			item.axe = 12;
			item.knockBack = 4f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.tileBoost = 4;
			item.autoReuse = true;
			item.crit = 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientSteelBar>(), 20);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
