using SOTS.Items.Pyramid;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth
{
	public class VibrantBullet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Bullet");
			Tooltip.SetDefault("Splits into 3 shards\nOne shard flies straight for 100% damage\nThe other shards deal 25% damage");
		}
		public override void SetDefaults()
		{
			item.damage = 6;
			item.ranged = true;
			item.width = 14;
			item.height = 36;
			item.maxStack = 999;
			item.consumable = true;           
			item.knockBack = 1f;
			item.value = Item.sellPrice(copper: 5);
			item.rare = ItemRarityID.Blue;
			item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantBullet>(); 
			item.shootSpeed = 4f;             
			item.ammo = AmmoID.Bullet;   
            item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}
	public class VibrantArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Arrow");
			Tooltip.SetDefault("Scatters 3 shards below the area of impact for 120% damage");
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 26;
			item.height = 56;
			item.maxStack = 999;
			item.consumable = true;
			item.knockBack = 1f;
			item.value = Item.sellPrice(copper: 5);
			item.rare = ItemRarityID.Blue;
			item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantArrow>();
			item.shootSpeed = 5f;
			item.ammo = AmmoID.Arrow;
			item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<VibrantBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}
}