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
			item.damage = 12;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = 27;
			item.useAnimation = 27;
			item.ranged = true;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.width = 26;
			item.height = 26;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<Projectiles.Earth.EchoDisk>(); 
            item.shootSpeed = 9.5f;
			item.knockBack = 3f;
			item.consumable = false;
			item.noUseGraphic = true;
			item.noMelee = true;
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