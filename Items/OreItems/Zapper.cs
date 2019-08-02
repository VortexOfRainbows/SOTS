using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Zapper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sapphire Zapper");
			Tooltip.SetDefault("Uses the power of the sapphire to attract electrons from particles in the air to generate a plasmatic form of lighting");
		}
		public override void SetDefaults()
		{
            item.damage = 8;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 30;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 24;  //how fast 
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 10000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("FriendlyLightning2");
            item.shootSpeed = 12f;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"StoneChipper", 1);
			recipe.AddIngredient(ItemID.Sapphire, 3);
			recipe.AddIngredient(ItemID.SilverBow, 1);
			recipe.AddIngredient(ItemID.Cloud, 30);
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
