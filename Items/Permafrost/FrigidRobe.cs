using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Permafrost
{
	[AutoloadEquip(EquipType.Body)]
	public class FrigidRobe : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 20, 0);
			Item.rare = ItemRarityID.Green;
			Item.defense = 1;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Robe");
			Tooltip.SetDefault("Frigid Javelin gains better bouncing capabilities");
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
			return head.type == ModContent.ItemType<FrigidCrown>() && legs.type == ModContent.ItemType<FrigidGreaves>();
		}
		public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.setBonus = "Frigid Javelin no longer costs void\nDecreases void damage by 15%";
			player.GetDamage<VoidGeneric>() -= 0.15f;
			modPlayer.frigidJavelinNoCost = true;
		}
		/*public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
		}*/
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.frigidJavelinBoost += 3;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<FrigidBar>(16).AddTile(TileID.Anvils).Register();
		}

	}
}