using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class MythrilShotStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mythril Shot Staff");
			Tooltip.SetDefault("Syphons four homing bolts out of thin air");
		}
		public override void SetDefaults()
		{
            item.damage = 31;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 44;     //gun image width
            item.height = 44;   //gun image  height
            item.useTime = 34;  //how fast 
            item.useAnimation = 34;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 5;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("Mypulse"); 
            item.shootSpeed = 12;
			item.mana = 9;
			item.reuseDelay = 2;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MythrilBar, 12);
			recipe.AddIngredient(null, "SteelBar", 3);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 4;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(14)); // This defines the projectiles random spread . 30 degree spread.
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				

              }
              return false; 
	}
	}
}
