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

			item.value = 2800;
			item.rare = 3;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelled Leggings");
			Tooltip.SetDefault("5% increased ranged and summon damage");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("GelledLeatherJacket") && head.type == mod.ItemType("GelledLeatherHelmet");
        }

		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.05f;
			player.rangedDamage += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 16);
			recipe.AddIngredient(ItemID.Leather, 12);
			recipe.AddIngredient(null, "SlimeyFeather", 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}