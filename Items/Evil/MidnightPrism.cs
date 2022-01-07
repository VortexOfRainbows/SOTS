using SOTS.Items.Fragments;
using SOTS.Items.Potions;
using SOTS.Items.Tide;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Evil
{	
	public class MidnightPrism : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midnight Prism");
			Tooltip.SetDefault("Increases armor penetration by 8, critical strike chance by 5%, and max life by 40\nRelease waves of damage periodically that ignore up to 16 defense\nRelease more waves at lower health\nCritical strikes unleash Nightmare Arms that do 10% damage and pull enemies together\nHas a 6 second cooldown\nWaves disabled when hidden");
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 7, 50, 0);
			item.rare = ItemRarityID.Lime;
			item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.rippleTimer++;
			modPlayer.rippleBonusDamage += 10;
			if (!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 40;
			player.armorPenetration += 8;
			modPlayer.CritNightmare = true;
			player.magicCrit += 5;
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.thrownCrit += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<PrismarineNecklace>(), 1);
			recipe.AddIngredient(ModContent.ItemType<WitchHeart>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}