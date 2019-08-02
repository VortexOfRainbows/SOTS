using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class AncientFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Fury");
			Tooltip.SetDefault("This is what the Starfury used to look like");
		}
		public override void SetDefaults()
		{

			item.damage = 7;
			item.magic = true;
			item.width = 16;
			item.height = 16;
			item.useTime = 1;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = 9; 
            item.shootSpeed = 38;
			item.mana = 5;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.GoldChest, 1);
			recipe.AddIngredient(ItemID.GoldenKey, 1);
			recipe.AddIngredient(ItemID.FallenStar, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}