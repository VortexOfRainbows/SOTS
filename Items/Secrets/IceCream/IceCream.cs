using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;



namespace SOTS.Items.Secrets.IceCream
{
	public class IceCream : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Alpha Polymer");
			
			Tooltip.SetDefault("A strenchy but strong rubber\nIts creation was an accident, created by melting a broken down version of the Alpha Virus in extreme heat");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 24;
			item.value = 50;
			item.rare = 9;
			item.maxStack = 9999;
            item.UseSound = SoundID.Item3;                //this is the sound that plays when you use the item
            item.useStyle = 2;                 //this is how the item is holded when used
            item.useTurn = true;
            item.useAnimation = 4;
            item.useTime = 4;
			item.autoReuse = true;
            item.consumable = true;       
			
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "IceCreamOre", 3);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
			recipe.needLava = true;
		}
		public override void CaughtFishStack(ref int stack)
		{
			stack = Main.rand.Next(5,13);
		}
	}
}