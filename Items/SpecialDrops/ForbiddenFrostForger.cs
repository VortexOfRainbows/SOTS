using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class ForbiddenFrostForger : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forbidden Frost Forger");
			Tooltip.SetDefault("A mix of two elemental powers");
		}
		public override void SetDefaults()
		{
            item.damage = 120;  //gun damage
            item.melee = true;   //its a gun so set this to true
            item.width = 68;     //gun image width
            item.height =68;   //gun image  height
            item.useTime = 12;  //how fast 
            item.useAnimation = 12;
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
			recipe.AddIngredient(null, "ForbiddenForger", 1);
			recipe.AddIngredient(null, "FrostForger", 1);
			recipe.AddIngredient(ItemID.Excalibur, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
