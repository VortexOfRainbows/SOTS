using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Earth
{
	[AutoloadEquip(EquipType.Legs)]
	public class VibrantLeggings : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Leggings");
			Tooltip.SetDefault("Decreased void usage by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("VibrantChestplate") && head.type == mod.ItemType("VibrantHelmet");
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidCost -= 0.10f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}