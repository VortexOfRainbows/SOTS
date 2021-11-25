using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class GelAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gel Throwing Axe");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Shuriken);
			item.damage = 11;
			item.alpha = 25;
			item.thrown = true;
			item.width = 32;
			item.height = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 1.75f;
			item.value = 100;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;            
			item.shoot = ModContent.ProjectileType<Projectiles.GelAxe>(); 
            item.shootSpeed *= 1.2f;
			item.consumable = true;
			item.maxStack = 999;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Gel, 20);
			recipe.AddIngredient(ItemID.IronBar, 1);
			recipe.AddTile(TileID.Solidifier);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}