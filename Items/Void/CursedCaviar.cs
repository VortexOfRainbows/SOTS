using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class CursedCaviar : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Caviar");
			Tooltip.SetDefault("Automatically consumed when void is low\n40% chance to refill 20 void and recieve Well Fed for 30 seconds\n35% chance to refill 15 void and recieve Mana Regeneration for 30 seconds\n25% chance to refill 10 void and recieve Battle for 30 seconds");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 16;
            item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 2;
			item.maxStack = 99;
			item.ammo = item.type;   
			ItemID.Sets.ItemNoGravity[item.type] = false; 

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Curgeon", 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			while(voidPlayer.voidMeter < voidPlayer.voidMeterMax2 / 10)
			{
				int rand = Main.rand.Next(20);
				if(rand <= 7) //40%, 0, 1, 2, 3, 4, 5, 6, 7
				{
					player.AddBuff(BuffID.WellFed, 1800, true);
					voidPlayer.voidMeter += 20;
				}
				else if(rand <= 14) //35%, 8, 9, 10, 11, 12, 13, 14
				{
					player.AddBuff(BuffID.ManaRegeneration, 1800, true);
					voidPlayer.voidMeter += 15;
				}
				else //25%, 15, 16, 17, 18, 19
				{
					player.AddBuff(BuffID.Battle, 1800, true);
					voidPlayer.voidMeter += 10;
				}
				item.stack--;
			}
		}
	}
}