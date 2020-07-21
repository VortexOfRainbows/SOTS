using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	[AutoloadEquip(EquipType.Body)]
	public class PossessedChainmail : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;

			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 6;
			item.defense = 11;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Chainmail");
			Tooltip.SetDefault("Increases void damage by 15% and ranged damage by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("PossessedHelmet") && legs.type == mod.ItemType("PossessedGreaves");
        }

		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.15f;
			player.rangedDamage += 0.1f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VibrantChestplate", 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(null, "FragmentOfEvil", 3);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}

	}
}