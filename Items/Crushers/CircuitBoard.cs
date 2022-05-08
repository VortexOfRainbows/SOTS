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
			Tooltip.SetDefault("The second charge of a Crusher has a 33% chance to not consume void\nThe fourth charge of Crushers no longer consumes void\nExtends the range of Crushers by 1\nIncreases melee speed and melee damage by 5%\nIncreases Crusher charge speed by 10%");
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
			player.meleeSpeed += 0.05f;
			player.meleeDamage += 0.05f;
			vPlayer.CrushTransformer += 0.1f;
			vPlayer.CrushResistor = true;
			vPlayer.CrushCapacitor = true;
			vPlayer.BonusCrushRangeMax++;
			vPlayer.BonusCrushRangeMin++;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CrushingAmplifier>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CrushingCapacitor>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CrushingResistor>(), 1);
			recipe.AddIngredient(ModContent.ItemType<CrushingTransformer>(), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}