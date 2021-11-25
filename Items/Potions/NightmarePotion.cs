using SOTS.Buffs;
using SOTS.Items.Fishing;
using SOTS.Items.Fragments;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Potions
{
	public class NightmarePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nightmare Potion");
			Tooltip.SetDefault("Critical strikes unleash Nightmare Arms that do 10% damage and pull enemies together\nHas a 6 second cooldown");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 32;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = ItemRarityID.Blue;
			item.maxStack = 30;
            item.buffType = ModContent.BuffType<Nightmare>();   
            item.buffTime = 21900;  
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
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 1);
			recipe.AddIngredient(ModContent.ItemType<TinyPlanetFish>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddIngredient(ItemID.Deathweed, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}