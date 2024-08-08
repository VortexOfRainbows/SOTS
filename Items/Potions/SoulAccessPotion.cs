using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fishing;
using SOTS.Buffs;
using SOTS.Items.Slime;
using SOTS.Items.Pyramid;

namespace SOTS.Items.Potions
{
	public class SoulAccessPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(20);
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 9999;
            Item.buffType = ModContent.BuffType<SoulAccess>();   
            Item.buffTime = 22000;  
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(ModContent.ItemType<PhantomFish>(), 1).AddIngredient(ModContent.ItemType<SoulResidue>(), 1).AddIngredient(ModContent.ItemType<Peanut>(), 1).AddIngredient(ItemID.Deathweed, 1).AddTile(13).Register();
		}
	}
}