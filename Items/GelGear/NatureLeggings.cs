using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Legs)]
	public class NatureLeggings : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 18;
            item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 1;
			item.defense = 2;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Leggings");
			Tooltip.SetDefault("5% increased minion damage and movement speed");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("NatureShirt") && head.type == mod.ItemType("NatureWreath");
        }
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.05f;
			player.moveSpeed += 0.05f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddIngredient(null, "FragmentOfNature", 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}

	}
}