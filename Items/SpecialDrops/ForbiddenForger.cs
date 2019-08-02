using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class ForbiddenForger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Forger");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
            item.damage = 72;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height =68;   //gun image  height
            item.useTime = 25;  //how fast 
            item.useAnimation = 25;
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
			recipe.AddIngredient(null, "BrassBar", 12);
			recipe.AddIngredient(null, "SteelBar", 8);
			recipe.AddIngredient(3783, 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
