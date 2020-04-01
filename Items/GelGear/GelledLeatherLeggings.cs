using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Legs)]
	public class GelledLeatherLeggings : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 18;

            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 3;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelled Leggings");
			Tooltip.SetDefault("4% increased summon damage");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("GelledLeatherJacket") && head.type == mod.ItemType("GelledLeatherHelmet");
        }

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.04f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddIngredient(ItemID.Leather, 10);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}