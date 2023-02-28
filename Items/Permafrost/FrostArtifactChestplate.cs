using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Body)]
	public class FrostArtifactChestplate : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 6, 50, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 24;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<FrostArtifactHelmet>() && legs.type == ModContent.ItemType<FrostArtifactTrousers>();
        }
		public override void UpdateEquip(Player player)
		{
			player.GetCritChance(DamageClass.Melee) += 16;
			player.GetCritChance(DamageClass.Ranged) += 16;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FrostBreastplate, 1).AddIngredient<AbsoluteBar>(24).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}