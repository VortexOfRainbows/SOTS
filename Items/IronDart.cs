using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.Items
{
	public class IronDart : ModItem
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Iron Dart");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.width = 14;
			item.height = 24;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 2f;
			item.value = 4;
			item.rare = 1;
			item.shoot = mod.ProjectileType("IronDartProj");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 2f;                  //The speed of the projectile
			item.ammo = AmmoID.Dart;   
            item.UseSound = SoundID.Item23;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}