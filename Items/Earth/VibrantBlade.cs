using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Earth
{
	public class VibrantBlade : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Blade");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 15;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 0, 80, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;  
			item.shoot = ModContent.ProjectileType<Projectiles.Laser.VibrantBlade>(); 
            item.shootSpeed = 1.9f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 7;
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