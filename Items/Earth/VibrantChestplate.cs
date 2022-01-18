using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Earth
{
	[AutoloadEquip(EquipType.Body)]
	public class VibrantChestplate : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.defense = 5;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Chestplate");
			Tooltip.SetDefault("Increases void damage by 10% and ranged damage by 5%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<VibrantHelmet>() && legs.type == ModContent.ItemType<VibrantLeggings>();
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.10f;
			player.rangedDamage += 0.05f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}