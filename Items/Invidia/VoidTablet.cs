using SOTS.Items.Fragments;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Invidia
{
	public class VoidTablet : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 34;   
            Item.value = Item.sellPrice(0, 0, 20, 0);
            Item.rare = ItemRarityID.Blue;
			Item.accessory = true;
			Item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 1;
			voidPlayer.voidMeterMax2 += 50;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.additionalHeal -= 20;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<Evostone>(), 50).AddIngredient(ModContent.ItemType<FragmentOfEarth>(), 4).AddTile(TileID.Anvils).Register();
		}
	}
}