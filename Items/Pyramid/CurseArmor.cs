using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Body)]
	public class CursedRobe : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 34;
			item.height = 36;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.defense = 8;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Robe");
			Tooltip.SetDefault("Increased maximum mana and void by 80\nReduces mana and void usage by 15%\nSummons a Ruby Monolith to your side\nThe Ruby Monolith increases your void regen by 4");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<CursedHood>();
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = mod.GetEquipSlot("CursedRobe_Legs", EquipType.Legs);
		}

		public override void UpdateEquip(Player player)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidMeterMax2 += 80;
			player.statManaMax2 += 80;
			vPlayer.voidCost -= 0.15f;
			player.manaCost -= 0.15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 12);
			recipe.AddRecipeGroup("SOTS:GemRobes", 1);
			recipe.AddIngredient(ModContent.ItemType<RubyKeystone>(), 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class CursedHood : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Pink;
			item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Hood");
			Tooltip.SetDefault("Increases magic damage and void damage by 8%\nAlso increases magic crit chance and void crit chance by 5%\nThe closest enemy to you is afflicted with a curse\nUpon taking damage, cursed enemies will flare, doing 5 plus 10% additional damage to it and other nearby enemies");
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidDamage += 0.08f;
			player.magicDamage += 0.08f;
			vPlayer.voidCrit += 5;
			player.magicCrit += 5;
		}
		/*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.mod == "Terraria" && line.Name == "Tooltip0")
					{
						line.text = "Increases your max number of minions and sentries by 1" +
							"\nIncreases minion and melee damage by 7%" +
							"\nIncreased max void by 50" +
							"\nProvides a Holo Eye minion to assist in combat" +
							"\nPress the " + "'" + key + "' key to make it launch a destabilizing beam that applies 4 levels of destabilized, but only once per enemy" +
							"\nDestabilized enemies gain a 5% flat chance to be critically striked" +
							"\nThis is calculated after normal crits, allowing some attacks to double crit" +
							"\nCosts 6 void to launch" +
							"\nHolo Eye remains active in the inventory when favorited or while worn in vanity slots";
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
				{
					string key = "Unbound";
					line.text = "Increases your max number of minions and sentries by 1" +
						"\nIncreases minion and melee damage by 7%" +
						"\nIncreased max void by 50" +
						"\nProvides a Holo Eye minion to assist in combat" +
						"\nPress the " + "'" + key + "' key to make it launch a destabilizing beam that applies 4 levels of destabilized, but only once per enemy" +
						"\nDestabilized enemies gain a 5% flat chance to be critically striked" +
						"\nThis is calculated after normal crits, allowing some attacks to double crit" +
						"\nCosts 6 void to launch" +
						"\nHolo Eye remains active in the inventory when favorited or while worn in vanity slots";
				}
			}
			base.ModifyTooltips(tooltips);
		}*/
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<CursedRobe>();
		}
		public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			player.setBonus = "Use armor set key 2 launch ruby monolith at enemy. This will do the big epic damage. takes 1 minute to repair, where no longer give void regen boost\nUses all void available up to 100 void to launch, dealing that much damage (more because of proj effects)";
			modPlayer.HoloEyeAutoAttack = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 8);
			recipe.AddIngredient(ModContent.ItemType<RoyalRubyShard>(), 20);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}