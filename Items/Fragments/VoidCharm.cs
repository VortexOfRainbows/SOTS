using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items.Fragments
{
	public class VoidCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Charm");
			Tooltip.SetDefault("Critical strikes have a 50% chance to regenerate void\n2% increased crit chance");
		}
		public override void SetDefaults()
		{
            item.width = 30;     
            item.height = 32;  
            item.value = Item.sellPrice(0, 0, 75, 0);
            item.rare = 5;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			
			player.meleeCrit += 2;
			player.rangedCrit += 2;
			player.magicCrit += 2;
			player.thrownCrit += 2;
			
			if(Main.rand.Next(2) == 0)
				modPlayer.CritVoidsteal += 2.5f + Main.rand.Next(2);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Chocolate", 4);
			recipe.AddIngredient(null, "FragmentOfTide", 4);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
