using SOTS.Items.Fragments;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class ChaosBadge : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Badge");
			Tooltip.SetDefault("Increases crit chance by 10%");
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}
		public override void SetDefaults()
		{
            Item.width = 38;     
            Item.height = 38;     
            Item.value = Item.sellPrice(0, 0, 75, 0);
            Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetCritChance(DamageClass.Melee) += 10;
			player.GetCritChance(DamageClass.Ranged) += 10;
			player.GetCritChance(DamageClass.Magic) += 10;
			player.GetCritChance(DamageClass.Throwing) += 10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.CobaltBar, 10).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 4).AddTile(TileID.Anvils).Register();
			CreateRecipe(1).AddIngredient(ItemID.PalladiumBar, 10).AddIngredient(ModContent.ItemType<FragmentOfChaos>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}
