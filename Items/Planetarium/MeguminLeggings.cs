using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	[AutoloadEquip(EquipType.Legs)]
	public class MeguminLeggings : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;

			item.value = 125000;
			item.rare = 10;
			item.defense = 1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Archmage's Boots");
			Tooltip.SetDefault("Max mana increased by 150\nMana cost increased by 33%\n33% decrease to all other damage types");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("MeguminShirt") && head.type == mod.ItemType("MeguminHat");
        }
		public override void UpdateEquip(Player player)
		{
			player.statManaMax2 += 150;
			player.manaCost += 0.33f;
				player.meleeDamage -= 0.33f;
				player.rangedDamage -= 0.33f;
				player.minionDamage -= 0.33f;
				player.thrownDamage -= 0.33f;
				
		}
		public override void AddRecipes()
		{
			/*
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GelBar", 12);
			recipe.AddIngredient(ItemID.Leather, 10);
			recipe.AddIngredient(null, "GoblinRockBar", 16);
			recipe.AddIngredient(ItemID.Bone, 30);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
			*/
		}

	}
}