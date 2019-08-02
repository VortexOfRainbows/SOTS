using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunIIIII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Blood Boil");
			Tooltip.SetDefault("The gun of blood... but better?");
		}
		public override void SetDefaults()
		{
            item.damage = 33;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 5;  //how fast 
            item.useAnimation = 5;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 1000000;
            item.rare = 7;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = item.shoot = mod.ProjectileType("TrueBlood"); 
            item.shootSpeed = 15.5f;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunIIII", 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(ItemID.VampireKnives, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
