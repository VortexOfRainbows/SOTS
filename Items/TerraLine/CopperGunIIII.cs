using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunIIII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blood Boil");
			Tooltip.SetDefault("The gun of blood");
		}
		public override void SetDefaults()
		{
            item.damage = 21;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 7;  //how fast 
            item.useAnimation = 7;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 250000;
            item.rare = 7;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = item.shoot = mod.ProjectileType("VampireShot"); 
            item.shootSpeed = 15f;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunIII", 1);
			recipe.AddIngredient(ItemID.TendonBow, 1);
			recipe.AddIngredient(null, "Sharanga", 1);
			recipe.AddIngredient(ItemID.BeesKnees, 1);
			recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
