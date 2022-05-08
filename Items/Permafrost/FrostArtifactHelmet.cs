using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Head)]
	public class FrostArtifactHelmet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 6, 25, 0);
			Item.rare = ItemRarityID.Lime;
			Item.defense = 16;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Artifact Helmet");
			Tooltip.SetDefault("14% increased melee and ranged damage");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<FrostArtifactChestplate>() && legs.type == ModContent.ItemType<FrostArtifactTrousers>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Most melee and ranged attacks summon a handful of Polar Cannons that each deal 25% damage";
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.polarCannons += 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeDamage += 0.14f;
			player.rangedDamage += 0.14f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FrostHelmet, 1);
			recipe.AddIngredient(ModContent.ItemType<AbsoluteBar>(), 16);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}