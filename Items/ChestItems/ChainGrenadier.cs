using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace SOTS.Items.ChestItems
{
	public class ChainGrenadier : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chain Grenadier");
			Tooltip.SetDefault("'This is not a bad idea!'");
		}
		public override void SetDefaults()
		{
            item.damage = 36; 
            item.ranged = true;   
            item.width = 48;    
            item.height = 28; 
            item.useTime = 12; 
            item.useAnimation = 12;
            item.useStyle = 5;    
            item.noMelee = false;
			item.knockBack = 1f;  
            item.value = 152500;
            item.rare = 8;
            item.UseSound = SoundID.Item61;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GrenadierBolt"); 
            item.shootSpeed = 13;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(8)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.VortexBeater, 1);
			recipe.AddIngredient(null, "Grenadier", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
