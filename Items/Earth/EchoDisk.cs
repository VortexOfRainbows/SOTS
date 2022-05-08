using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class EchoDisk : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Echo Disc");
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.useTime = 27;
			Item.useAnimation = 27;
			Item.ranged = true;
			Item.value = Item.sellPrice(0, 0, 80, 0);
			Item.rare = ItemRarityID.Blue;
			Item.width = 26;
			Item.height = 26;
			Item.maxStack = 1;
			Item.autoReuse = true;            
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.EchoDisk>(); 
            Item.shootSpeed = 9.5f;
			Item.knockBack = 3f;
			Item.consumable = false;
			Item.noUseGraphic = true;
			Item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 6);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
}