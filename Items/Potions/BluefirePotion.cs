using SOTS.Buffs;
using SOTS.Items.Fragments;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class BluefirePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 26;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Bluefire>();   
            Item.buffTime = 14700;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 1).AddIngredient(ItemID.LivingFireBlock, 1).AddIngredient(ItemID.Fireblossom, 1).AddTile(TileID.Bottles).Register();
		}
	}
}