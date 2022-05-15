using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class BrittlePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Potion");
			Tooltip.SetDefault("Getting hit surrounds you with ice shards");
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 28;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = Mod.Find<ModBuff>("Brittle").Type;   
            Item.buffTime = 3600 * 4 + 300;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatFood;      
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.BottledWater, 1).AddIngredient(null, "FragmentOfPermafrost", 1).AddIngredient(ItemID.Shiverthorn, 1).AddTile(13).Register();
		}
	}
}