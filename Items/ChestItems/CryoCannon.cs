using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.ChestItems
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
            item.damage = 10;
            item.ranged = true;
            item.width = 50;
            item.height = 32;
            item.useTime = 38; 
            item.useAnimation = 38;
            item.useStyle = 5;    
            item.noMelee = true;
			item.knockBack = 2f;  
            item.value = Item.sellPrice(0, 2, 0, 0);
            item.rare = 2;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("IceCluster"); 
            item.shootSpeed = 9.5f;
			item.useAmmo = ItemID.Snowball;
			item.crit = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, -2);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FrigidBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(3));
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("IceCluster"), damage, knockBack, player.whoAmI);
              }
              return false; 
		}
	}
}
