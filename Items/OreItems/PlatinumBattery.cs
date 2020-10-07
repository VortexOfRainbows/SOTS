using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.OreItems
{
	public class PlatinumBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Battery");
			Tooltip.SetDefault("Increases void regen by 0.75 and max void by 20\nRegenerate void when hit, but also have a 10% chance to recieve broken armor when near max void");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 18;     
            item.height = 36;   
            item.value = Item.sellPrice(0, 0, 45, 0);
            item.rare = 2;
			item.accessory = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.AddIngredient(null, "FragmentOfEvil", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.075f;
			voidPlayer.voidMeterMax2 += 20;
			
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			if(modPlayer.onhit == 1)
			{
				voidPlayer.voidMeter += 2 + (modPlayer.onhitdamage / 11);
				VoidPlayer.VoidEffect(player, 2 + (modPlayer.onhitdamage / 11));
				if(voidPlayer.voidMeter >= voidPlayer.voidMeterMax2 - 5 && Main.rand.Next(10) == 0)
				{
					int minute = 3600;
					player.AddBuff(BuffID.BrokenArmor, (int)(minute * 2.5f), false);
				}
			}
		}
	}
}