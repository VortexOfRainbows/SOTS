using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.Projectiles.Nature;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SOTS.Items.Planetarium.FromChests;
using System.Collections.Generic;

namespace SOTS.Items.Chaos
{
	[AutoloadEquip(EquipType.Head)]
	public class ElementalHelmet : ModItem
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
			ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = false;
		}
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 11, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 18;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<ElementalBreastplate>() && legs.type == ModContent.ItemType<ElementalLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Elemental");
			player.SOTSPlayer().ElementalBlinkBuff = true;
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 3;
			voidPlayer.voidMeterMax2 += 100;
            player.GetDamage<VoidGeneric>() += 0.15f;
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.GetDamage(DamageClass.Melee) += 0.15f;
			player.maxMinions += 2;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<PhaseBar>(15).AddIngredient<FragmentOfChaos>(10).AddIngredient<TwilightAssassinsCirclet>(1).AddTile(TileID.MythrilAnvil).Register();
        }
	}
	[AutoloadEquip(EquipType.Legs)]
	public class ElementalLeggings : ModItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
        }
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 14;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 18;
        }
        private void SetupDrawing()
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ElementalBreastplate>() && head.type == ModContent.ItemType<ElementalHelmet>();
		}
		public override void UpdateEquip(Player player)
        {
            VoidPlayer vPlayer = player.VoidPlayer();
            vPlayer.bonusVoidGain += 2;
            vPlayer.voidMeterMax2 += 100;
            player.whipRangeMultiplier += 0.15f;
            player.moveSpeed += 0.15f;
            player.maxTurrets += 2;
            player.GetDamage(DamageClass.Summon) += 0.15f;
            player.GetCritChance(DamageClass.Melee) += 15;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<PhaseBar>(10).AddIngredient<FragmentOfChaos>(10).AddIngredient<TwilightAssassinsLeggings>(1).AddTile(TileID.MythrilAnvil).Register();
        }
	}
	[AutoloadEquip(EquipType.Body)]
	public class ElementalBreastplate : ModItem
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
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = false;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
            {
                foreach (TooltipLine line in tooltips) //goes through each tooltip line
                {
                    if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                    {
                        line.Text = Language.GetTextValue("Mods.SOTS.Items.ElementalBreastplate.TooltipExt", key);
                        return;
                    }
                }
            }
            foreach (TooltipLine line in tooltips) //goes through each tooltip line
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    string Textkey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
                    line.Text = Language.GetTextValue("Mods.SOTS.Items.ElementalBreastplate.TooltipExt", Textkey);
                }
            }
        }
        public override void SetDefaults()
		{
			Item.width = 46;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 12, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 24;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<ElementalHelmet>() && legs.type == ModContent.ItemType<ElementalLeggings>();
		}
		public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Melee) += 0.3f;
            player.GetDamage(DamageClass.Summon) += 0.3f;
			player.lifeRegen += 4;
			player.SOTSPlayer().ElementalBlink = true;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<PhaseBar>(20).AddIngredient<FragmentOfChaos>(10).AddIngredient<TwilightAssassinsChestplate>(1).AddIngredient<BlinkPack>().AddIngredient<ParticleRelocator>().AddTile(TileID.MythrilAnvil).Register();
		}
	}
}