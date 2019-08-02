using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class OrichalcumFlower : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orichalcum Flower");
			Tooltip.SetDefault("Creates a huge spread of petals");
		}
		public override void SetDefaults()
		{
            item.damage = 28;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 44;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 50;  //how fast 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = 221; 
            item.shootSpeed = 9;
			item.mana = 25;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.OrichalcumBar, 14);
			recipe.AddIngredient(null, "BrassBar", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 16;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(90)); // This defines the projectiles random spread . 30 degree spread.
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				

              }
              return false; 
	}
	}
}
