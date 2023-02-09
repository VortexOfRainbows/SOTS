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
			player.GetCritChance(DamageClass.Generic) += 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PrismarineNecklace>(), 1).AddIngredient(ModContent.ItemType<WitchHeart>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}