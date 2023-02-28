using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
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
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 14;
            Item.DamageType = DamageClass.Melee;  
            Item.width = 52;   
            Item.height = 48;   
            Item.useStyle = ItemUseStyleID.Swing;
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<AncientSteelBar>(), 20).AddRecipeGroup(RecipeGroupID.Wood, 20).AddTile(TileID.Anvils).Register();
		}
	}
}
