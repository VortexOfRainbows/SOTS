using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.AbandonedVillage
{
    [AutoloadEquip(EquipType.Head)]
    public class ExcavatorHelmet : ModItem
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
        }
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 24;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 7;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ExcavatorBreastplate>() && legs.type == ModContent.ItemType<ExcavatorLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Excavator");
            player.VoidPlayer().voidRegenSpeed += 0.3f;
            player.SOTSPlayer().MissileTailAttackRate = 60;
        }
        public override void UpdateEquip(Player player)
        {
            var v = player.VoidPlayer();
            v.voidMeterMax2 += 25;
            v.voidGainMultiplier += 0.25f;
            v.voidRegenSpeed += 0.1f;
        }
    }
    [AutoloadEquip(EquipType.Body)]
	public class ExcavatorBreastplate : ModItem
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
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
        }
        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 6, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.defense = 10;
		}
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return head.type == ModContent.ItemType<ExcavatorHelmet>() && legs.type == ModContent.ItemType<ExcavatorLeggings>();
        }
		public override void UpdateEquip(Player player)
        {
            player.endurance += 0.1f;
        }
	}
    [AutoloadEquip(EquipType.Legs)]
    public class ExcavatorLeggings : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 16;
            Item.value = Item.sellPrice(0, 6, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 8;
        }
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
            SetupDrawing();
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotBody] = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ExcavatorBreastplate>() && head.type == ModContent.ItemType<ExcavatorHelmet>();
        }
        public override void UpdateEquip(Player player)
        {
            var v = player.VoidPlayer();
            v.voidMeterMax2 += 25;
            v.voidGainMultiplier += 0.25f;
            v.voidRegenSpeed += 0.1f;
        }
    }
}