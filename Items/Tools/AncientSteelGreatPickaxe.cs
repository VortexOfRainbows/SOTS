using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 12;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 44;   
            Item.height = 44;   
            Item.useStyle = ItemUseStyleID.Swing;
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 15).AddRecipeGroup(RecipeGroupID.Wood, 20).AddTile(TileID.Anvils).Register();
		}
	}
}
