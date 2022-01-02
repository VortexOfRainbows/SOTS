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
			DisplayName.SetDefault("Soul Access Potion");
			
			Tooltip.SetDefault("Increases void regeneration speed by 10%");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 28;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Green;
			item.maxStack = 30;
            item.buffType = ModContent.BuffType<SoulAccess>();   
            item.buffTime = 22000;  
            item.UseSound = SoundID.Item3;            
            item.useStyle = ItemUseStyleID.EatingUsing;        
            item.useTurn = true;
            item.useAnimation = 16;
            item.useTime = 16;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ModContent.ItemType<PhantomFish>(), 1);
			recipe.AddIngredient(ModContent.ItemType<SoulResidue>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Peanut>(), 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddTile(13);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}