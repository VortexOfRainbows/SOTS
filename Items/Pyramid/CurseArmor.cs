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
			DisplayName.SetDefault("Cursed Robe");
			Tooltip.SetDefault("Increased maximum mana and void by 80\nReduces mana and void usage by 15%\nSummons a Ruby Monolith to your side\nThe Ruby Monolith increases your void regeneration speed by 10%");
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<CursedHood>();
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = EquipLoader.GetEquipSlot(Mod, "CursedRobe_Legs", EquipType.Legs);
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
			DisplayName.SetDefault("Cursed Hood");
			Tooltip.SetDefault("Increases magic damage and void damage by 8%\nAlso increases magic crit chance and void crit chance by 5%\nThe closest enemy to you is afflicted with a curse\nUpon taking damage, cursed enemies will Flare, dealing 140% additional damage to it and other nearby enemies\nThis effect has a 2 second cooldown");
			this.SetResearchCost(1);
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			modPlayer.CurseVision = true;
			vPlayer.voidDamage += 0.08f;
			player.GetDamage(DamageClass.Magic) += 0.08f;
			vPlayer.voidCrit += 5;
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
			string theKey = "Unbound";
			foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys())
			{
				theKey = key;
			}
			player.setBonus = "Press the '" + theKey + "' key to change the Ruby Monolith into an offensive stance\nWhile in offensive stance, decreases the cooldown of Curse Flaring by 80%\nHowever, increases void drain by 6 instead of increasing void regeneration speed by 10%";
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CursedMatter>(), 8).AddIngredient(ModContent.ItemType<RoyalRubyShard>(), 20).AddTile(TileID.Anvils).Register();
		}
	}
}