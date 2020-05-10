using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class AlmondMilk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Almond Milk");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 20 void");
		}
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 38;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = 1;
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
			voidPlayer.voidMeter += 20;
			VoidPlayer.VoidEffect(player, 20);
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