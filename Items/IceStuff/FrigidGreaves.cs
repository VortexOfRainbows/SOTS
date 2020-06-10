using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Legs)]
	public class FrigidGreaves : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 22;
			item.height = 18;
			
            item.value = Item.sellPrice(0, 2, 10, 0);
			item.rare = 2;
			item.defense = 3;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Greaves");
			Tooltip.SetDefault("Grants Ice Skates effect\n10% increased movement speed");
		}
		public override void UpdateEquip(Player player)
		{
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