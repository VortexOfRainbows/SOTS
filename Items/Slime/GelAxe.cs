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
			Item.CloneDefaults(ItemID.Shuriken);
			Item.damage = 11;
			Item.alpha = 25;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 32;
			Item.height = 20;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 1.75f;
			Item.value = 100;
			Item.rare = ItemRarityID.Blue;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;            
			Item.shoot = ModContent.ProjectileType<Projectiles.GelAxe>(); 
            Item.shootSpeed *= 1.2f;
			Item.consumable = true;
			Item.maxStack = 999;
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