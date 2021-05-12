using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Fragments
{
	public class FocusCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Focus Crystal");
			Tooltip.SetDefault("Critical strikes deal 60 more damage\n5% increased crit chance\nImmunity to bleeding and poisoned debuffs");
		}
		public override void SetDefaults()
		{
            item.width = 36;     
            item.height = 34;  
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
			item.defense = 1;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
			modPlayer.CritBonusDamage += 30;
            player.buffImmune[BuffID.Bleeding] = true; 
            player.buffImmune[BuffID.Poisoned] = true; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PolishedCoin", 1);
			recipe.AddIngredient(null, "OtherworldlyAmplifier", 1);
			recipe.AddIngredient(null, "SanguiteBar", 5); //To be replaced later (dissolving inferno)
			recipe.AddIngredient(null, "FragmentOfInferno", 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
