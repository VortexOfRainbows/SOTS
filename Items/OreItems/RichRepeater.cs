using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class RichRepeater : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rich Repeater");
		}
		public override void SetDefaults()
		{
            item.damage = 11;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 38;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 23;  //how fast 
            item.useAnimation = 23;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000;
            item.rare = 2;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 206; 
            item.shootSpeed = 8;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RichMahoganyBow, 1);
			recipe.AddIngredient(ItemID.LeadBar, 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 2;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
