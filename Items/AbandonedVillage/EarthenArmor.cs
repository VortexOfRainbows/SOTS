using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
    [AutoloadEquip(EquipType.Head)]
    public class EarthenHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
            SetupDrawing();
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            ArmorIDs.Head.Sets.DrawHatHair[equipSlotHead] = true;
            ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = false;
        }
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 3;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EarthenChestplate>() && legs.type == ModContent.ItemType<EarthenLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Earthen");
            SOTSPlayer.ModPlayer(player).attackSpeedMod += 0.20f;
        }
        public override void UpdateEquip(Player player)
        {
            player.nightVision = true;
            player.pickSpeed -= 0.1f;
            player.moveSpeed += 0.1f;
        }
    }
    [AutoloadEquip(EquipType.Body)]
	public class EarthenChestplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
            SetupDrawing();
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[equipSlotBody] = false;
            ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = false;
        }
        public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Blue;
			Item.defense = 6;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<EarthenHelmet>() && legs.type == ModContent.ItemType<EarthenLeggings>();
        }
		public override void UpdateEquip(Player player)
        {
            player.pickSpeed -= 0.1f;
            player.moveSpeed += 0.1f;
        }
	}
    [AutoloadEquip(EquipType.Legs)]
    public class EarthenLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 20;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }
        public override void SetStaticDefaults() => this.SetResearchCost(1);
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<EarthenChestplate>() && head.type == ModContent.ItemType<EarthenHelmet>();
        }
        public override void UpdateEquip(Player player)
        {
            player.pickSpeed -= 0.1f;
            player.moveSpeed += 0.1f;
        }
    }
}