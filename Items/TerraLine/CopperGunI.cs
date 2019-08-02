using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunI : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Gun");
			Tooltip.SetDefault("Infused with true Coopers");
		}
		public override void SetDefaults()
		{
            item.damage = 8;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 10;  //how fast 
            item.useAnimation = 10;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 10000;
            item.rare = 2;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 242; 
            item.shootSpeed = 8;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CopperShortsword, 2);
			recipe.AddIngredient(ItemID.CopperBar, 20);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(null, "RedPowerChamber", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
