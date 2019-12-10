using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Body)]
	public class GelledLeatherJacket : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 20;

            item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 3;
			item.defense = 4;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelled Jacket");
			Tooltip.SetDefault("Increases max minions");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("GelledLeatherHelmet") && legs.type == mod.ItemType("GelledLeatherLeggings");
        }

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 22);
			recipe.AddIngredient(ItemID.Leather, 18);
			recipe.AddIngredient(null, "SlimeyFeather", 18);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}