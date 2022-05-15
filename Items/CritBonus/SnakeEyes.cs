using SOTS.Items.Fragments;
using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class SnakeEyes : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snake Eyes");
			Tooltip.SetDefault("Increases crit chance by 8%");
		}
		public override void SetDefaults()
		{
            Item.width = 32;     
            Item.height = 38;     
            Item.value = Item.sellPrice(0, 0, 40, 0);
            Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Melee) += 8;
			player.GetCritChance(DamageClass.Ranged) += 8;
			player.GetCritChance(DamageClass.Magic) += 8;
			player.GetCritChance(DamageClass.Throwing) += 8;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snakeskin>(), 10).AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}
