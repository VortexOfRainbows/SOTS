using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Items.Earth;
using SOTS.Items.Fragments;

namespace SOTS.Items.Evil
{
	[AutoloadEquip(EquipType.Head)]
	public class PossessedHelmet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 20;
            Item.value = Item.sellPrice(0, 2, 75, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 9;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Helmet");
			Tooltip.SetDefault("Increases max void by 100\n10% increased ranged crit chance");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == ModContent.ItemType<PossessedChainmail>() && legs.type == ModContent.ItemType<PossessedGreaves>();
        }
        public override void UpdateArmorSet(Player player)
        {	
			player.setBonus = "Increases void gain by 5";
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 5f;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.VibrantArmor = true;
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidMeterMax2 += 100;
			player.rangedCrit += 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantHelmet>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Legs)]
	public class PossessedGreaves : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Greaves");
			Tooltip.SetDefault("Decreased void usage by 15%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<PossessedChainmail>() && head.type == ModContent.ItemType<PossessedHelmet>();
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidCost -= 0.15f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantLeggings>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Body)]
	public class PossessedChainmail : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(0, 3, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 11;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Possessed Chainmail");
			Tooltip.SetDefault("Increases void damage by 15% and ranged damage by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<PossessedHelmet>() && legs.type == ModContent.ItemType<PossessedGreaves>();
		}

		public override void UpdateEquip(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidDamage += 0.15f;
			player.rangedDamage += 0.1f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantChestplate>(), 1);
			recipe.AddIngredient(ItemID.SoulofNight, 15);
			recipe.AddIngredient(ModContent.ItemType<FragmentOfEvil>(), 4);
			recipe.SetResult(this);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.AddRecipe();
		}
	}
}