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
			Item.rare = 4;
			Item.defense = 3;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Boots");
			Tooltip.SetDefault("Increases minion damage by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == Mod.Find<ModItem>("PatchLeatherTunic") .Type&& head.type == Mod.Find<ModItem>("PatchLeatherHat").Type;
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