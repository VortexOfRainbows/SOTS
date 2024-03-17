using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Tools
{
	public class EarthGrinder : ModItem
	{	
		public override void SetStaticDefaults() => this.SetResearchCost(1);
		public override void SetDefaults()
		{
            Item.damage = 20;
            Item.DamageType = DamageClass.Melee;
			Item.width = 56;   
            Item.height = 58;   
            Item.useStyle = ItemUseStyleID.Swing;
			Item.useTurn = true;
			Item.useTime = 16;
			Item.useAnimation = 24;
			Item.hammer = 55;
			Item.axe = 16;
			Item.knockBack = 5f;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.tileBoost = 3;
			Item.autoReuse = true;
		}
		public override void AddRecipes()
		{
			//CreateRecipe(1).AddIngredient(ModContent.ItemType<FrigidBar>(), 16).AddTile(TileID.Anvils).Register();
		}
	}
}
