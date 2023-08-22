using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	//[AutoloadEquip(EquipType.Shield)]
	public class FortressGenerator : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 32;     
            Item.height = 40;
            Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Cyan;
			Item.accessory = true;
			Item.defense = 6;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.lifeRegen += 2;
			player.noKnockback = true;
			player.hasPaladinShield = true;
			player.maxMinions += 1;
			player.maxTurrets += 1;
			player.GetDamage(DamageClass.Generic) += 0.1f;
			PlatformPlayer modPlayer = player.GetModPlayer<PlatformPlayer>();
			modPlayer.platformPairs += 2;
			modPlayer.fortress = true;
			if (hideVisual)
				modPlayer.hideChains = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PlatformGenerator>(), 1).AddIngredient(ItemID.PaladinsShield, 1).AddIngredient(ItemID.PygmyNecklace, 1).AddRecipeGroup("SOTS:T2DD2Armor", 1).AddRecipeGroup("SOTS:T2DD2Accessory", 1).AddIngredient(ItemID.SpectreBar, 10).AddIngredient(ModContent.ItemType<DissolvingDeluge>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}