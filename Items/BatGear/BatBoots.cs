using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BatGear
{
	[AutoloadEquip(EquipType.Legs)]
	public class BatBoots : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 75000;
			item.rare = 5;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bat Boots");
			Tooltip.SetDefault("Increases max health by 25");
		}
public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BatChest") && head.type == mod.ItemType("BatHat");
        }

		public override void UpdateEquip(Player player)
		{
			player.statLifeMax2 += 25;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 12);
			recipe.AddIngredient(ItemID.Leather, 10);
			recipe.AddIngredient(null, "GoblinRockBar", 16);
			recipe.AddIngredient(ItemID.Bone, 30);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}