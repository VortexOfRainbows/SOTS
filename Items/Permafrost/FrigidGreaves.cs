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
			Item.width = 22;
			Item.height = 18;
            Item.value = Item.sellPrice(0, 1, 10, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Greaves");
			Tooltip.SetDefault("Grants Ice Skates effect\n10% increased movement and void attack speed");
			this.SetResearchCost(1);
		}
		public override void UpdateEquip(Player player)
		{
			player.GetAttackSpeed<VoidGeneric>()+= 0.1f;
			player.iceSkate = true;
			player.moveSpeed += 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(12).AddTile(TileID.Anvils).Register();
		}
	}
}