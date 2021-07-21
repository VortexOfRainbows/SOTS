using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;

namespace SOTS.Items
{
	public class TomeOfTheReaper : ModItem
	{	int counter = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tome of the Reaper");
			Tooltip.SetDefault("Cast three scythes that move towards your cursor");
		}
		public override void SetDefaults()
		{
            item.damage = 56; 
            item.magic = true; 
            item.width = 30;   
            item.height = 36;   
            item.useTime = 13;   
            item.useAnimation = 13;
            item.useStyle = 5;    
            item.noMelee = true;  
            item.knockBack = 4.5f;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.rare = 6;
            item.UseSound = SoundID.Item8;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<ReaperScythe>(); 
            item.shootSpeed = 9.5f;
			item.mana = 16;
			item.reuseDelay = 16;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.DemonScythe, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, -speedX, -speedY, type, damage, knockBack, player.whoAmI);
			counter = 0;
				Vector2 perturbedSpeed = new Vector2(-speedX, -speedY).RotatedBy(MathHelper.ToRadians(15)) * 0.9f; 
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				perturbedSpeed = new Vector2(-speedX, -speedY).RotatedBy(MathHelper.ToRadians(-15)) * 0.9f;
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			return false; 
		}
	}
}
