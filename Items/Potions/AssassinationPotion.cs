using SOTS.Items.Planetarium;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class AssassinationPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<Buffs.Assassination>();   
            Item.buffTime = 3600 * 12 + 30;  
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient<TwilightGel>(10).AddIngredient<Fragments.FragmentOfOtherworld>(1).AddIngredient(ItemID.Fireblossom, 1).AddTile(TileID.Bottles).Register();
		}
	}
}