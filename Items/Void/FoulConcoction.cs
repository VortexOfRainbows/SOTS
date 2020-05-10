using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;


namespace SOTS.Items.Void
{
	public class FoulConcoction: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Foul Concoction");
			Tooltip.SetDefault("Automatically consumed when void is low\nRefills 4 void");
		}
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 18;
            item.value = Item.sellPrice(0, 0, 0, 50);
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
			voidPlayer.voidMeter += 4;
			VoidPlayer.VoidEffect(player, 4);
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
			recipe.AddIngredient(null, "Peanut", 1);
			recipe.AddIngredient(ItemID.PinkGel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "Peanut", 5);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.AddIngredient(ItemID.Acorn, 5);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RottenChunk, 1);
			recipe.AddIngredient(ItemID.Gel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Vertebrae, 1);
			recipe.AddIngredient(ItemID.Gel, 1);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}