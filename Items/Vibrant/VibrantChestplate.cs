using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Vibrant
{
	[AutoloadEquip(EquipType.Body)]
	public class VibrantChestplate : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 20;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 1;
			item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Chestplate");
			Tooltip.SetDefault("Increases void damage by 10% and ranged damage by 5%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("VibrantHelmet") && legs.type == mod.ItemType("VibrantLeggings");
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
			recipe.AddIngredient(ItemID.IronChainmail, 1);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadChainmail, 1);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("IronBar", 30);
			recipe.AddIngredient(null, "VeryGlowyMushroom", 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}

	}
}