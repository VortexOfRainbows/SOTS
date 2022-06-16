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
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 6;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 14;
			Item.height = 36;
			Item.maxStack = 999;
			Item.consumable = true;           
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantBullet>(); 
			Item.shootSpeed = 4f;             
			Item.ammo = AmmoID.Bullet;   
            Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<VibrantBar>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
	public class VibrantArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Arrow");
			Tooltip.SetDefault("Scatters 3 shards below the area of impact for 120% damage");
			this.SetResearchCost(99);
		}
		public override void SetDefaults()
		{
			Item.damage = 12;
			Item.DamageType = DamageClass.Ranged;
			Item.width = 26;
			Item.height = 56;
			Item.maxStack = 999;
			Item.consumable = true;
			Item.knockBack = 1f;
			Item.value = Item.sellPrice(copper: 5);
			Item.rare = ItemRarityID.Blue;
			Item.shoot = ModContent.ProjectileType<Projectiles.Earth.VibrantArrow>();
			Item.shootSpeed = 5f;
			Item.ammo = AmmoID.Arrow;
			Item.UseSound = SoundID.Item23;
		}
		public override void AddRecipes()
		{
			CreateRecipe(200).AddIngredient(ModContent.ItemType<VibrantBar>(), 1).AddTile(TileID.Anvils).Register();
		}
	}
}