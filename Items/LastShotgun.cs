using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class LastShotgun : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Last Prism Shotgun");
			Tooltip.SetDefault("Dev Item");
		}
		public override void SetDefaults()
		{
            item.damage = 125;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 56;     //gun image width
            item.height = 40;   //gun image  height
            item.useTime = 8;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
            item.value = 1000000;
            item.rare = 11;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("LastPrism"); 
            item.shootSpeed = 20;
			item.expert = true;
			item.mana = 24;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "TheHardCore", 5);
			recipe.AddIngredient(ItemID.LastPrism, 1);
			recipe.AddIngredient(ItemID.MoonlordTurretStaff, 1);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddIngredient(null, "SteelBar", 40);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 5;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
