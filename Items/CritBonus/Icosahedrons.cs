using SOTS.Items.Fragments;
using SOTS.Items.Permafrost;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.CritBonus
{
	public class BorealisIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 1, 50, 0);
			Item.rare = ItemRarityID.Lime;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritFrost = true;
			player.GetCritChance(DamageClass.Generic) += 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<DissolvingAurora>(), 1).AddIngredient(ItemID.FrostCore, 1).AddIngredient(ModContent.ItemType<AbsoluteBar>(), 6).AddTile(TileID.MythrilAnvil).Register();
		}
	}
	public class CursedIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 5, 50, 0);
			Item.rare = ItemRarityID.Yellow;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritCurseFire = true;
			player.GetCritChance(DamageClass.Generic) += 2;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<BorealisIcosahedron>(), 1).AddIngredient(ModContent.ItemType<HellfireIcosahedron>(), 1).AddIngredient(ModContent.ItemType<DissolvingUmbra>(), 1).AddIngredient(ItemID.CursedFlame, 10).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
	public class HellfireIcosahedron : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.CritFire = true;
			player.GetCritChance(DamageClass.Generic) += 1;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 6).AddIngredient(ModContent.ItemType<FragmentOfInferno>(), 6).AddTile(TileID.Anvils).Register();
		}
	}
}
