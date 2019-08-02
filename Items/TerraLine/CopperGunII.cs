using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class CopperGunII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Metal Muncher");
			Tooltip.SetDefault("Infused with ores of all kinds");
		}
		public override void SetDefaults()
		{
            item.damage = 11;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height = 38;   //gun image  height
            item.useTime = 8;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 50000;
            item.rare = 4;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = 207; 
            item.shootSpeed = 12;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CopperGunI", 1);
			recipe.AddIngredient(ItemID.IronBar, 12);
			recipe.AddIngredient(ItemID.LeadBar, 12);
			recipe.AddIngredient(ItemID.SilverBar, 4);
			recipe.AddIngredient(ItemID.TungstenBar, 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
