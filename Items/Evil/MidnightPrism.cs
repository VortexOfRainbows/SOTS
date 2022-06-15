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
			Tooltip.SetDefault("Increases armor penetration by 8, critical strike chance by 5%, and max life by 40\nRelease waves of damage periodically that ignore up to 16 defense total\nRelease more waves at lower health\nCritical strikes unleash Nightmare Arms that do 10% damage and pull enemies together\nHas a 6 second cooldown\nWaves disabled when hidden");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 36;   
            Item.value = Item.sellPrice(0, 7, 50, 0);
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.rippleTimer++;
			modPlayer.rippleBonusDamage += 10;
			if (!hideVisual)
				modPlayer.rippleEffect = true;
			player.statLifeMax2 += 40;
			player.GetArmorPenetration(DamageClass.Generic) += 8;
			modPlayer.CritNightmare = true;
			player.GetCritChance(DamageClass.Magic) += 5;
			player.GetCritChance(DamageClass.Melee) += 5;
			player.GetCritChance(DamageClass.Ranged) += 5;
			player.GetCritChance(DamageClass.Throwing) += 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PrismarineNecklace>(), 1).AddIngredient(ModContent.ItemType<WitchHeart>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}