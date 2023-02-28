using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Legs)]
	public class PatchLeatherPants : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PatchLeatherTunic>() && head.type == ModContent.ItemType<PatchLeatherHat>();
        }
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.10f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snakeskin>(), 20).AddRecipeGroup("SOTS:EvilMaterial", 6).AddTile(TileID.Anvils).Register();
		}
	}
}