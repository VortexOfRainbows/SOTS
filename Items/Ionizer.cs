using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Ionizer : ModItem
	{ 	float charge = 0;
		int checkDelay = 50;
		public override void SetStaticDefaults()
		{ 
			DisplayName.SetDefault("Ionizer");
			Tooltip.SetDefault("Increases in charge as you get hit, right click to display charge\nClick to discharge\nCharge never deteriorates");
		}
		public override void SetDefaults()
		{
            item.damage = 12;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height = 52;   //gun image  height
            item.useTime = 50;  //how fast 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.1f;
            item.value = 175000;
            item.rare = 8;
            item.UseSound = SoundID.Item9;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("FriendlyLightning2"); 
            item.shootSpeed = 12.25f;
			item.mana = 0;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  if(player.altFunctionUse == 2)
			  {
				  if(checkDelay >= 100)
				  {
				int chargeInt = (int)charge;
				string text = chargeInt.ToString();;
				Main.NewText("Charge of " + text, 195, 145, 225);
				checkDelay = 50;
				  }
			  }
			  else
			  {
				  if(checkDelay > 50)
				  {
				checkDelay = 0;
				  }
              int numberProjectiles = 1 + (int)(0.12f * charge);
			  if(numberProjectiles > 50)
			  {
				  numberProjectiles = 50;
			  }
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians((int)(charge / 3))); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
			  }
              return false; 
		}
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override void UpdateInventory(Player player)
		{
				  if(checkDelay >= 45 && checkDelay < 50)
				  {
					charge = 0;
				  }
			checkDelay++;
			
			item.damage = 12;
			item.useTime = 50;
			item.damage += (int)(0.25f * charge);
			item.useTime = (int)(50 - (0.45f * charge));
			if(item.useTime < 5)
			{
				item.useTime = 5;
			}
			if(item.damage > 120)
			{
				item.damage = 120;
			}
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.onhit == 1)
			{
				charge += modPlayer.onhitdamage * 0.25f;
				charge += 1;
				modPlayer.onhitdamage = 0;	
				if(player.name == "Straffex")
				{
				charge += 1;
				}
				int chargeInt = (int)charge;
				string text = chargeInt.ToString();;
				Main.NewText("Charge of " + text, 195, 145, 225);

			}
			
				
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(3456, 20);
			recipe.AddIngredient(null, "Vacuumizer", 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
