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

			item.value = 2800;
			item.rare = 3;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gelled Jacket");
			Tooltip.SetDefault("5% increased ranged crit");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("GelledLeatherHelmet") && legs.type == mod.ItemType("GelledLeatherLeggings");
        }

		public override void UpdateEquip(Player player)
		{
		
			player.rangedCrit += 5;
			
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 20);
			recipe.AddIngredient(ItemID.Leather, 16);
			recipe.AddIngredient(null, "SlimeyFeather", 16);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}