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
			Item.rare = 1;
			Item.maxStack = 30;
            Item.buffType = mod.BuffType("Brittle");   
            Item.buffTime = 3600 * 4 + 300;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = 2;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(null, "FragmentOfPermafrost", 1);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}