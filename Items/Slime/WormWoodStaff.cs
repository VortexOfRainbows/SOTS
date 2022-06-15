using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	public class WormWoodStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Goopwood Staff");
			Tooltip.SetDefault("Fires 3 bouncy, erratic, lingering pink projectiles");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.damage = 20;  
            Item.DamageType = DamageClass.Magic;  
            Item.width = 34;    
            Item.height = 34;   
            Item.useTime = 8;  
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;    
			Item.mana = 20;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<Projectiles.Slime.WormBullet>(); 
            Item.shootSpeed = 2.75f;
			Item.noMelee = true;
		}
		int projectileNum = 0;
        public override bool CanUseItem(Player player)
        {
			projectileNum = 0;
			return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
			    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.ToRadians(-5 + ((projectileNum % 3) * 5)));
			    Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
			}
			projectileNum++;
			return false; 
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32).AddIngredient<Wormwood>(16).AddTile(TileID.Anvils).Register();
		}
		
	}
}
