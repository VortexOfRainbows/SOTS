using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class Titanic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Titanic");
			Tooltip.SetDefault("Water and bolts");
		}
		public override void SetDefaults()
		{
            item.damage = 46;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 45;     //gun image width
            item.height = 45;   //gun image  height
            item.useTime = 25;  //how fast 
            item.useAnimation = 25;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 100000;
            item.rare = 6;
            item.UseSound = SoundID.Item28;
            item.autoReuse = true;
            item.shoot = 27; 
            item.shootSpeed = 7;
			item.mana = 12;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TitaniumBar, 14);
			recipe.AddIngredient(ItemID.WaterBolt, 1);
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
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(11)); // This defines the projectiles random spread . 30 degree spread.
				  
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				

              }
              return false; 
	}
	}
}
