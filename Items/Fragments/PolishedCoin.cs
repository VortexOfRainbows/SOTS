using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class PolishedCoin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polished Coin");
			Tooltip.SetDefault("Critical strikes have a 50% chance to deal 40 more damage\n3% increased crit chance\nImmunity to bleeding and poisoned");
		}
		public override void SetDefaults()
		{
            item.width = 28;     
            item.height = 28;  
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 5;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			player.meleeCrit += 3;
			player.rangedCrit += 3;
			player.magicCrit += 3;
			player.thrownCrit += 3;
			if(Main.rand.Next(2) == 0)
			{
				modPlayer.CritBonusDamage += 20;
			}
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PutridCoin", 1);
			recipe.AddIngredient(ItemID.AdhesiveBandage, 1);
			recipe.AddIngredient(ItemID.Bezoar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodstainedCoin", 1);
			recipe.AddIngredient(ItemID.AdhesiveBandage, 1);
			recipe.AddIngredient(ItemID.Bezoar, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PutridCoin", 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
			
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BloodstainedCoin", 1);
			recipe.AddIngredient(ItemID.MedicatedBandage, 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
