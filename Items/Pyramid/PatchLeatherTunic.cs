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
			Item.width = 28;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Patch Leather Tunic");
			Tooltip.SetDefault("Increases minion damage by 10%\nGrants immunity to venom and poison debuffs");
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesHands[equipSlotBody] = false;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<PatchLeatherHat>() && legs.type == ModContent.ItemType<PatchLeatherPants>();
        }
		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.10f;
			player.buffImmune[BuffID.Venom] = true;
			player.buffImmune[BuffID.Poisoned] = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Snakeskin>(), 28).AddRecipeGroup("SOTS:EvilMaterial", 10).AddTile(TileID.Anvils).Register();
		}
	}
}