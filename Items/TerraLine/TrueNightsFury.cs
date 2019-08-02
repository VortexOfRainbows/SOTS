using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class TrueNightsFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Night's Fury");
			Tooltip.SetDefault("Bow of the sky");
		}
		public override void SetDefaults()
		{
            item.damage = 55;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 16;     //gun image width
            item.height = 42;   //gun image  height
            item.useTime = 24;  //how fast 
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 1111110;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("NightsArrow2"); 
            item.shootSpeed = 100;
			item.reuseDelay = 8;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(null, "NightsFury", 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
