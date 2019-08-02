using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.OreItems
{
	public class OnyxBlast : ModItem
	{ int accuracy = 28;
              int numberProjectiles = 5;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Shock Zapper");
			Tooltip.SetDefault("Fires a barrage of onyx shards\n33% chance not to consume ammo");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.OnyxBlaster);
            item.damage = 44;  //gun damage
            item.ranged = true;   //its a gun so set this to true
            item.width = 76;     //gun image width
            item.height = 30;   //gun image  height
            item.useTime = 39;  //how fast 
            item.useAnimation = 39;
			item.rare = 6;
			item.shoot = 10;
            item.value = 450000;
			item.useAmmo = AmmoID.Bullet;
		
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null,"SteelBar", 12);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {	
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(6)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X + Main.rand.Next(-1, 2), perturbedSpeed.Y + Main.rand.Next(-1, 2), type, damage, knockBack, player.whoAmI);
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X + Main.rand.Next(-1, 2), perturbedSpeed.Y + Main.rand.Next(-1, 2), 661, damage, knockBack, player.whoAmI);
              }
              return false; 
	}
		public override bool ConsumeAmmo(Player p)
		{
			if(Main.rand.Next(3) == 0)
			{
				return false;
			}
			else
			{
				return true;
			}
}
	}
}
