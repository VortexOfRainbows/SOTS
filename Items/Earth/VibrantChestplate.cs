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
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 5;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Chestplate");
			Tooltip.SetDefault("Increases void damage by 10% and ranged damage by 5%");
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<VibrantHelmet>() && legs.type == ModContent.ItemType<VibrantLeggings>();
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			player.GetDamage<VoidGeneric>() += 0.10f;
			player.GetDamage(DamageClass.Ranged) += 0.05f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<VibrantBar>(), 12).AddTile(TileID.Anvils).Register();
		}

	}
}