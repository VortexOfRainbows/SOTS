using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Legs)]
	public class PatchLeatherPants : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 4;
			item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Boots");
			Tooltip.SetDefault("Increases minion damage by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("PatchLeatherTunic") && head.type == mod.ItemType("PatchLeatherHat");
        }
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Snakeskin", 20);
			recipe.AddRecipeGroup("SOTS:EvilMaterial", 6);
			recipe.AddIngredient(ItemID.Leather, 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}