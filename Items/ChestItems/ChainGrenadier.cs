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
			Tooltip.SetDefault("Born in bigger trashfire; made of even more crap; developed by 3 braincells\nA very good weapon");
		}
		public override void SetDefaults()
		{
            item.damage = 38;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 28;   //gun image  height
            item.useTime = 30;  //how fast 
            item.useAnimation = 30;
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
              int numberProjectiles = 4;
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
			recipe.AddIngredient(null, "Grenadier", 1);
			recipe.AddIngredient(null, "CrushPistol", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
