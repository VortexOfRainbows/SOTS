using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class FrostForger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Forger");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 68;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 48;   //gun image  height
            item.useTime = 14;  //how fast 
            item.useAnimation = 14;
            item.useStyle = 1;    
            item.knockBack = 4;
            item.value = 62500;
            item.rare = 6;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BrassBar", 8);
			recipe.AddIngredient(null, "SteelBar", 12);
			recipe.AddIngredient(2161, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
