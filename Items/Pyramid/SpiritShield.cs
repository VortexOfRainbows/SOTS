using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{	[AutoloadEquip(EquipType.Shield)]
	public class SpiritShield : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Shield");
			Tooltip.SetDefault("Increases void gain by 2, life regen by 1, and reduces damage taken by 2%");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 26;     
            Item.height = 40;   
            Item.value = Item.sellPrice(0, 3, 50, 0);
            Item.rare = ItemRarityID.Orange;
			Item.defense = 2;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 2;
			player.lifeRegen += 1;
			player.endurance += 0.02f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<SoulResidue>(), 25).AddIngredient(ModContent.ItemType<EmeraldBracelet>(), 1).AddIngredient(ItemID.BandofRegeneration, 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}