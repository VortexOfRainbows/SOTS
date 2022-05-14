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
			Item.width = 22;
			Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 2, 0);
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
            Item.buffType = ModContent.BuffType<Nightmare>();   
            Item.buffTime = 21900;  
            Item.UseSound = SoundID.Item3;            
            Item.useStyle = ItemUseStyleID.EatingUsing;        
            Item.useTurn = true;
            Item.useAnimation = 16;
            Item.useTime = 16;
            Item.consumable = true;       
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
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