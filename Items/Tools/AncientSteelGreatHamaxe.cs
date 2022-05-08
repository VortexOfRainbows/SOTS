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
            Item.damage = 14;
            Item.melee = true;  
            Item.width = 52;   
            Item.height = 48;   
            Item.useStyle = ItemUseStyleID.SwingThrow;
			Item.useTurn = true;
			Item.useTime = 18;
			Item.useAnimation = 24;
			Item.hammer = 45;
			Item.axe = 12;
			Item.knockBack = 4f;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 4;
			Item.autoReuse = true;
			Item.crit = 4;
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
