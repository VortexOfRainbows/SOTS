using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class ManaSapper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amethyst Sapper");
			Tooltip.SetDefault("Uses the power of the amethyst to syphon mana from your enemies");
		}
		public override void SetDefaults()
		{
            item.damage = 11;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 30;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 22;  //how fast 
            item.useAnimation = 22;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 10000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ManaSyphon"); 
            item.shootSpeed = 12;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneChipper", 1);
			recipe.AddIngredient(ItemID.Amethyst, 3);
			recipe.AddIngredient(ItemID.CopperBow, 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
