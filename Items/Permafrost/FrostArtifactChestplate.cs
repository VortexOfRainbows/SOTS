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
			item.width = 38;
			item.height = 22;
            item.value = Item.sellPrice(0, 6, 50, 0);
			item.rare = ItemRarityID.Lime;
			item.defense = 24;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Breastplate");
			Tooltip.SetDefault("16% increased melee and ranged critical strike chance");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("FrostArtifactHelmet") && legs.type == mod.ItemType("FrostArtifactTrousers");
        }
		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 16;
			player.rangedCrit += 16;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostBreastplate, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 24);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}