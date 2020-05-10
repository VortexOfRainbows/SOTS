using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class Chocolate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chocolate");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 15 void\n'The number one thing to bring on pirating adventures'");
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 0, 2, 0);
			item.rare = 3;
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
			voidPlayer.voidMeter += 15;
			VoidPlayer.VoidEffect(player, 15);
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
	}
}