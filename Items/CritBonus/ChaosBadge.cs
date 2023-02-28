using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class ChaosBadge : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 34;     
            Item.height = 38;     
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Generic) += 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CobaltBar, 10).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 4).AddTile(TileID.Anvils).Register();
			CreateRecipe(1).AddIngredient(ItemID.PalladiumBar, 10).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}
