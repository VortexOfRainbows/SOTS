using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class CursedCaviar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Caviar");
			Tooltip.SetDefault("Automatically consumed when void is low\n40% chance to refill 20 void and recieve Well Fed for 90 seconds\n35% chance to refill 15 void and recieve Mana Regeneration for 90 seconds\n25% chance to refill 10 void and recieve Battle for 90 seconds");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 16;
            item.value = Item.sellPrice(0, 0, 12, 50);
			item.rare = 2;
			item.maxStack = 999;

			item.useStyle = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override bool UseItem(Player player)
		{
			return true;
		}
		public void RefillEffect(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			int rand = Main.rand.Next(20);
			if (rand <= 7) //40%, 0, 1, 2, 3, 4, 5, 6, 7
			{
				player.AddBuff(BuffID.WellFed, 5400, true);
				voidPlayer.voidMeter += 20;
				VoidPlayer.VoidEffect(player, 20);
			}
			else if (rand <= 14) //35%, 8, 9, 10, 11, 12, 13, 14
			{
				player.AddBuff(BuffID.ManaRegeneration, 5400, true);
				voidPlayer.voidMeter += 15;
				VoidPlayer.VoidEffect(player, 15);
			}
			else //25%, 15, 16, 17, 18, 19
			{
				player.AddBuff(BuffID.Battle, 5400, true);
				voidPlayer.voidMeter += 10;
				VoidPlayer.VoidEffect(player, 10);
			}
		}
		public override bool ConsumeItem(Player player)
		{
			return true;
		}
		public override void OnConsumeItem(Player player)
		{
			RefillEffect(player);
			base.OnConsumeItem(player);
		}
		public override void UpdateInventory(Player player)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			while(voidPlayer.voidMeter < voidPlayer.voidMeterMax2 / 10)
			{
				RefillEffect(player);
				item.stack--;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Curgeon", 1);
			recipe.AddTile(TileID.CookingPots);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
}