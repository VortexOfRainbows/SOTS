using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Body)]
	public class CursedRobe : ModItem
	{
        public override void Load()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this, "CursedRobe_Legs");
		}
        public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 6;
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
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesHands[equipSlotBody] = true;
			ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[equipSlotBody] = false;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = EquipLoader.GetEquipSlot(Mod, "CursedRobe_Legs", EquipType.Legs);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<CursedHood>();
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidMeterMax2 += 80;
			player.statManaMax2 += 80;
			vPlayer.voidCost -= 0.15f;
			player.manaCost -= 0.15f;
			modPlayer.RubyMonolith = true;
			modPlayer.RubyMonolithIsNOTVanity = true;
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 12).AddRecipeGroup("SOTS:GemRobes", 1).AddIngredient(ModContent.ItemType<RubyKeystone>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class CursedHood : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CurseVision = true;
			player.GetDamage<VoidGeneric>() += 0.08f;
			player.GetDamage(DamageClass.Magic) += 0.08f;
			player.GetCritChance<VoidGeneric>() += 5;
			player.GetCritChance(DamageClass.Magic) += 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<CursedRobe>();
		}
		public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			modPlayer.CanCurseSwap = true;
			string theKey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
			if(Main.netMode != NetmodeID.Server)
			{
				foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys())
				{
					theKey = key;
				}
				player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.Cursed", theKey);
			}				
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 8).AddIngredient(ModContent.ItemType<RoyalRubyShard>(), 20).AddTile(TileID.Anvils).Register();
		}
	}
}