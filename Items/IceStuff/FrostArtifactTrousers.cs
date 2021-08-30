using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.IceStuff
{
	[AutoloadEquip(EquipType.Legs)]
	public class FrostArtifactTrousers : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 20;
            item.value = Item.sellPrice(0, 6, 25, 0);
			item.rare = ItemRarityID.Lime;
			item.defense = 16;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Trousers");
			Tooltip.SetDefault("10% increased melee and movement speed");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("FrostArtifactChestplate") && head.type == mod.ItemType("FrostArtifactHelmet");
        }
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 0.1f;
			player.meleeSpeed += 0.1f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostLeggings, 1);
			recipe.AddIngredient(null, "AbsoluteBar", 20);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}