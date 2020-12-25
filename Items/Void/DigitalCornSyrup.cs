using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class DigitalCornSyrup: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Digital Corn Syrup");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 15 void\n'Yes, really'");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 40;
            item.value = Item.sellPrice(0, 0, 5, 0);
			item.rare = 1;
			item.maxStack = 999;

			item.useStyle = 2;
			item.useTime = 15;
			item.useAnimation = 15;
			item.UseSound = SoundID.Item3;
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
			while(voidPlayer.voidMeter < (voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls) / 10 && voidPlayer.voidMeterMax2 - voidPlayer.lootingSouls > 40)
			{
				RefillEffect(player);
				item.stack--;
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Peanut", 1);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 2);
			//recipe.AddRecipe();
		}
	}
}