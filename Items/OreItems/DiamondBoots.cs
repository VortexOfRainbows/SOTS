using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	[AutoloadEquip(EquipType.Legs)]
	public class DiamondBoots : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 525000;
			item.rare = 6;
			item.defense = 9;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Diamond Trousers");
			Tooltip.SetDefault("Increases jump height\nAlso decreases damage taken by 5%");
		}
public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("DiamondCrest") && head.type == mod.ItemType("DiamondSpacer");
        }

		public override void UpdateEquip(Player player)
		{
			player.jumpBoost = true;
			player.endurance += 0.05f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 16);
			recipe.AddIngredient(ItemID.Topaz, 12);
			recipe.AddIngredient(ItemID.Amethyst, 12);
			recipe.AddIngredient(ItemID.HellstoneBar, 8);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}