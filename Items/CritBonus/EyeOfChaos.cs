using SOTS.Items.Fragments;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class EyeOfChaos : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 38;     
            Item.value = Item.sellPrice(0, 7, 25, 0);
            Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Generic) += 18;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SnakeEyes>(), 1).AddIngredient(ModContent.ItemType<ChaosBadge>(), 1).AddIngredient(ItemID.EyeoftheGolem, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
