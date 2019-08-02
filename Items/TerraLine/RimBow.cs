using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.TerraLine
{
	public class RimBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rainbow's Rim");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{

			item.damage = 150;
			item.melee = true;
			item.width = 62;
			item.height = 62;
			item.useTime = 22;
			item.useAnimation = 22;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 100000;
			item.rare = 7;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;          
			item.shoot = mod.ProjectileType("RimBeam");
			item.expert = true;
			item.shootSpeed = 5;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TerraBlade, 1);
			recipe.AddIngredient(ItemID.RainbowGun, 1);
			recipe.AddIngredient(null, "TheHardCore", 1);
			recipe.AddIngredient(null, "ManicBane", 1);
			recipe.AddIngredient(null, "StirringSands", 1);
			recipe.AddIngredient(null, "StormShock", 1);
			recipe.AddIngredient(null, "Duallex", 1);
			recipe.AddIngredient(null, "Stabber", 1);
			recipe.AddIngredient(null, "CarbonCrusher", 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}