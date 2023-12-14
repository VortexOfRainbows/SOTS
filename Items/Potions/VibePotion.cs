using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Buffs;
using SOTS.Items.Fragments;

namespace SOTS.Items.Potions
{
	public class VibePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<GoodVibes>();   
            Item.buffTime = 21630; //around 6 minutes
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 1).AddIngredient(ItemID.Daybloom, 1).AddTile(TileID.Bottles).Register();
		}
	}
}