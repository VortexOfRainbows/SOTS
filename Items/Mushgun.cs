using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Mushgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mush Shot");
			Tooltip.SetDefault("Shrooooooms!");
		}
		public override void SetDefaults()
		{
            item.damage = 14;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 24;   //gun image  height
            item.useTime = 12;  //how fast 
            item.useAnimation = 48;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0;
            item.value = 7500;
            item.rare = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.shoot = 131; 
            item.shootSpeed = 86;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LeadBar, 10);
			recipe.AddIngredient(ItemID.GlowingMushroom, 50);
			recipe.AddIngredient(ItemID.FallenStar, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
