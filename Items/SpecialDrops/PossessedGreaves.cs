using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	[AutoloadEquip(EquipType.Legs)]
	public class PossessedGreaves : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = Item.sellPrice(0, 3, 50, 0);
			item.rare = 6;
			item.defense = 9;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Greaves");
			Tooltip.SetDefault("Decreased void usage by 12%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PossessedChainmail") && head.type == mod.ItemType("PossessedHelmet");
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidCost -= 0.12f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronGreaves, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(null, "FragmentOfEvil", 3);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadGreaves, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(null, "FragmentOfEvil", 3);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}