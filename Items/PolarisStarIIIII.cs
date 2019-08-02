using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items
{
	public class PolarisStarIIIII : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spur II");
			Tooltip.SetDefault("Surprise! Not the final form");
		}
		public override void SetDefaults()
		{
            item.damage = 26;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 40;     //gun image width
            item.height = 20;   //gun image  height
            item.useTime = 8;  //how fast 
            item.useAnimation = 8;
            item.useStyle = 5;    
            item.noMelee = false; //so the item's animation doesn't do damage
            item.knockBack = 4;
            item.value = 455000;
            item.rare = 8;
            item.UseSound = SoundID.Item12;
            item.autoReuse = true;
            item.shoot = 440; 
            item.shootSpeed = 35;
			item.expert = true;
			item.mana = 2;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddIngredient(null, "PolarisStarIIII", 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 3;
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
	}
}
