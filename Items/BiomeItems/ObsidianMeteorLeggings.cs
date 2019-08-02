using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems
{
	[AutoloadEquip(EquipType.Legs)]
	public class ObsidianMeteorLeggings : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 12200;
			item.rare = 5;
			item.defense = 5;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Obsidian Meteor Leggings");
			Tooltip.SetDefault("Gives the obsidian skull effect\nIncreases jump height");
		}
public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("ObsidianMeteorChest") && head.type == mod.ItemType("ObsidianMeteorLeggings");
        }

		public override void UpdateEquip(Player player)
		{
			player.jumpBoost = true;
			player.burned = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			
			recipe.AddIngredient(null, "ObsidianScale", 18);
			recipe.AddIngredient(ItemID.MeteoriteBar, 24);
			recipe.AddIngredient(ItemID.Obsidian, 78);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}