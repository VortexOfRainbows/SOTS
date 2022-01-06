using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Evil
{	
	public class WitchHeart : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Witch's Heart");
			Tooltip.SetDefault("Increases critical strike chance by 5%\nCritical strikes unleash Nightmare Arms that do 10% damage and pull enemies together\nHas a 6 second cooldown\nIncreases max life by 20");
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 36;
			item.value = Item.sellPrice(0, 4, 50, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritNightmare = true;
			player.magicCrit += 5;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.thrownCrit += 5;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1);
			recipe.AddIngredient(ModContent.ItemType<NightmarePotion>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}