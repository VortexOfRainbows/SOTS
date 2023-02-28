using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Crushers
{
	public class CircuitBoard : ModItem
	{	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 38;     
            Item.height = 30;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.LightRed;
			Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
			player.GetDamage(DamageClass.Melee) += 0.05f;
			vPlayer.CrushTransformer += 0.1f;
			vPlayer.CrushResistor = true;
			vPlayer.CrushCapacitor = true;
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CrushingAmplifier>(), 1).AddIngredient(ModContent.ItemType<CrushingCapacitor>(), 1).AddIngredient(ModContent.ItemType<CrushingResistor>(), 1).AddIngredient(ModContent.ItemType<CrushingTransformer>(), 1).AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}