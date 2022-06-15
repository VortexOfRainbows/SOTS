using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using SOTS.Items.Otherworld.FromChests;

namespace SOTS.Items.CritBonus
{
	public class CloverCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clover Charm");
			Tooltip.SetDefault("Critical strikes have a 50% chance to steal life\n3% increased crit chance");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 30;     
            Item.height = 32;  
            Item.value = Item.sellPrice(0, 0, 25, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Melee) += 3;
			player.GetCritChance(DamageClass.Ranged) += 3;
			player.GetCritChance(DamageClass.Magic) += 3;
			player.GetCritChance(DamageClass.Throwing) += 3;
			if(Main.rand.NextBool(2))
				modPlayer.CritLifesteal += Main.rand.Next(3) + 3;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.Wood, 20).AddIngredient(ModContent.ItemType<FragmentOfNature>(), 4).AddIngredient(ItemID.SkyBlueFlower, 1).AddTile(TileID.WorkBenches).Register();
		}
	}
	public class VoidCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Charm");
			Tooltip.SetDefault("Critical strikes have a 50% chance to regenerate void\n2% increased crit chance");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Melee) += 2;
			player.GetCritChance(DamageClass.Ranged) += 2;
			player.GetCritChance(DamageClass.Magic) += 2;
			player.GetCritChance(DamageClass.Throwing) += 2;
			if (Main.rand.NextBool(2))
				modPlayer.CritVoidsteal += 2.5f + Main.rand.Next(2);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Chocolate>(), 4).AddIngredient(ModContent.ItemType<FragmentOfTide>(), 4).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class SoulCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul Charm");
			Tooltip.SetDefault("Critical strikes steal life, regenerate void, and recover mana\n3% increased crit chance");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 34;
			Item.value = Item.sellPrice(0, 5, 0, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Melee) += 3;
			player.GetCritChance(DamageClass.Ranged) += 3;
			player.GetCritChance(DamageClass.Magic) += 3;
			player.GetCritChance(DamageClass.Throwing) += 3;
			modPlayer.CritManasteal += 7 + Main.rand.Next(4);
			modPlayer.CritLifesteal += 2 + Main.rand.Next(2);
			modPlayer.CritVoidsteal += 2.25f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CloverCharm>(), 1).AddIngredient(ModContent.ItemType<VoidCharm>(), 1).AddIngredient(ModContent.ItemType<Starbelt>(), 1).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}
