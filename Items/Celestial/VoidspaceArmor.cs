using SOTS.Items.Fragments;
using SOTS.Items.Slime;
using SOTS.Projectiles.Nature;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Celestial
{
	[AutoloadEquip(EquipType.Head)]
	public class VoidspaceMask : ModItem
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
			Item.width = 26;
			Item.height = 26;
			Item.value = Item.sellPrice(0, 9, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 16;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<VoidspaceBreastplate>() && legs.type == ModContent.ItemType<VoidspaceLeggings>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Voidspace");
            player.sotsPlayer().VoidspaceFlames = true;
        }
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 4;
			voidPlayer.voidMeterMax2 += 50;
			player.statManaMax2 += 100;
            player.GetCritChance<VoidGeneric>() += 15;
            player.GetCritChance(DamageClass.Magic) += 25;
            player.GetCritChance(DamageClass.Ranged) += 25;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SanguiteBar>(15).AddIngredient<FragmentOfInferno>(6).AddTile(TileID.MythrilAnvil).Register();
        }
	}
	[AutoloadEquip(EquipType.Legs)]
	public class VoidspaceLeggings : ModItem
	{
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();

        }
        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 14;
            Item.value = Item.sellPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Yellow;
            Item.defense = 20;
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
			return body.type == ModContent.ItemType<VoidspaceBreastplate>() && head.type == ModContent.ItemType<VoidspaceMask>();
		}
		public override void UpdateEquip(Player player)
        {
            VoidPlayer vPlayer = player.VoidPlayer();
            player.GetDamage<VoidGeneric>() += 0.15f;
            player.GetDamage(DamageClass.Magic) += 0.25f;
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.moveSpeed += 0.2f;
            vPlayer.bonusVoidGain += 3;
            vPlayer.voidMeterMax2 += 50;
        }
		public override void AddRecipes()
        {
            CreateRecipe(1).AddIngredient<SanguiteBar>(10).AddIngredient<FragmentOfInferno>(6).AddTile(TileID.MythrilAnvil).Register();
        }
	}
	[AutoloadEquip(EquipType.Body)]
	public class VoidspaceBreastplate : ModItem
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
		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 15, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.defense = 24;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<VoidspaceMask>() && legs.type == ModContent.ItemType<VoidspaceLeggings>();
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer vPlayer = player.VoidPlayer();
			vPlayer.GainVoidOnHurt = vPlayer.GainHealthOnVoidUse = 0.15f;
			player.manaCost -= 0.15f;
            vPlayer.bonusVoidGain += 3;
            vPlayer.voidMeterMax2 += 100;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SanguiteBar>(20).AddIngredient<FragmentOfInferno>(6).AddTile(TileID.MythrilAnvil).Register();
		}
	}
}