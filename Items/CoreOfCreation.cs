using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;


namespace SOTS.Items
{
	public class CoreOfCreation: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creation Core");
			Tooltip.SetDefault("*Cannot be placed");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 17));
		}
		public override void SetDefaults()
		{

			item.width = 50;
			item.height = 50;
			item.value = 0;
			item.rare = 8;
			item.expert = true;
			item.maxStack = 99;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperBar, 3);
			recipe.AddIngredient(ItemID.TinBar, 3);
			recipe.AddIngredient(ItemID.IronBar, 3);
			recipe.AddIngredient(ItemID.LeadBar, 3);
			recipe.AddIngredient(ItemID.SilverBar, 3);
			recipe.AddIngredient(ItemID.TungstenBar, 3);
			recipe.AddIngredient(ItemID.GoldBar, 3);
			recipe.AddIngredient(ItemID.PlatinumBar, 3);
			recipe.AddIngredient(ItemID.CrimtaneBar, 3);
			recipe.AddIngredient(ItemID.DemoniteBar, 3);
			recipe.AddIngredient(ItemID.HellstoneBar, 3);
			recipe.AddIngredient(null, "CoreOfExpertise", 1);
			recipe.AddIngredient(null, "SteelBar", 3);
			recipe.AddIngredient(null, "BrassBar", 3);
			recipe.SetResult(this, 4);
			recipe.AddRecipe();
		}
	}
}