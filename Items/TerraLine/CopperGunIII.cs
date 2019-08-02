using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunIII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Head Hunter");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 15;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 7;  //how fast 
            item.useAnimation = 7;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 207; 
            item.shootSpeed = 12;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunII", 1);
			recipe.AddIngredient(ItemID.Bone, 100);
			recipe.AddIngredient(ItemID.Minishark, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
