using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Legs)]
	public class FrigidGreaves : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 18;
            item.value = Item.sellPrice(0, 1, 10, 0);
			item.rare = ItemRarityID.Green;
			item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Greaves");
			Tooltip.SetDefault("Grants Ice Skates effect\n10% increased movement and void attack speed");
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer modPlayer = VoidPlayer.ModPlayer(player);
			modPlayer.voidSpeed += 0.1f;
			player.iceSkate = true;
			player.moveSpeed += 0.1f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 12);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}