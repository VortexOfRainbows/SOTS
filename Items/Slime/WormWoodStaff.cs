using Microsoft.Xna.Framework;
using Terraria;
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
		}
		public override void SetDefaults()
		{
            Item.damage = 20;  
            Item.magic = true;  
            Item.width = 34;    
            Item.height = 34;   
            Item.useTime = 8;  
            Item.useAnimation = 24;
            Item.useStyle = 1;    
			Item.mana = 20;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 2, 0, 0);
            Item.rare = ItemRarityID.LightRed;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
			Item.shoot = mod.ProjectileType("WormBullet"); 
            Item.shootSpeed = 2.75f;
			Item.noMelee = true;
		}
		int projectileNum = 0;
        public override bool CanUseItem(Player player)
        {
			projectileNum = 0;
			return base.CanUseItem(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			for (int i = 0; i < numberProjectiles; i++)
			{
			    Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(-5 + ((projectileNum % 3) * 5)));
			    Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			projectileNum++;
			return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 32);
			recipe.AddIngredient(null, "Wormwood", 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
	}
}
