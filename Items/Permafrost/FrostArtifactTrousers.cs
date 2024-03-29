using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Legs)]
	public class FrostArtifactTrousers : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 20;
            Item.value = Item.sellPrice(0, 6, 25, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 16;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FrostArtifactChestplate>() && head.type == ModContent.ItemType<FrostArtifactHelmet>();
        }
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
			player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.FrostLeggings, 1).AddIngredient<AbsoluteBar>(20).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}