using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles.Permafrost;

namespace SOTS.Items.Permafrost
{
	public class CryoCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryo Cannon");
			Tooltip.SetDefault("Uses snowballs as ammo");
		}
		public override void SetDefaults()
		{
            Item.damage = 10;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 50;
            Item.height = 32;
            Item.useTime = 38; 
            Item.useAnimation = 38;
            Item.useStyle = ItemUseStyleID.Shoot;    
            Item.noMelee = true;
			Item.knockBack = 2f;  
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item61;
            Item.autoReuse = true;
            Item.shoot = ModContent.ProjectileType<IceCluster>(); 
            Item.shootSpeed = 9.5f;
			Item.useAmmo = ItemID.Snowball;
			Item.crit = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, -2);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<FrigidBar>(), 8).AddTile(TileID.Anvils).Register();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<IceCluster>(), damage, knockBack, player.whoAmI, -1);
			return false; 
		}
	}
}
