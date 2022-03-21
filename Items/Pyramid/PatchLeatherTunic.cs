using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Body)]
	public class PatchLeatherTunic : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = 4;
			item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Tunic");
			Tooltip.SetDefault("Increases minion damage by 10%\nGrants immunity to venom and poison debuffs");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == mod.ItemType("PatchLeatherHat") && legs.type == mod.ItemType("PatchLeatherPants");
        }
		public override void DrawHands(ref bool drawHands, ref bool drawArms) {
			drawHands = true;
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.10f;
			player.buffImmune[BuffID.Venom] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Snakeskin>(), 28);
			recipe.AddRecipeGroup("SOTS:EvilMaterial", 10);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}