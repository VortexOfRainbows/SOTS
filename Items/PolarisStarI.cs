using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class PolarisStarI : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Polaris Star I");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 19;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 20;   //gun image  height
            item.useTime = 18;  //how fast 
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item12;
            item.autoReuse = true;
            item.shoot = 440; 
            item.shootSpeed = 35;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RobotHat, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
