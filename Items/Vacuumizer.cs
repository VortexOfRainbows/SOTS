using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Vacuumizer : ModItem
	{ 	float charge = 0;
		int checkDelay = 50;
		public override void SetStaticDefaults()
		{ 
			DisplayName.SetDefault("Vacuumizer");
			Tooltip.SetDefault("Increases in charge as you get hit, right click to display charge\nClick to discharge\nNaturally loses charge when above 67 charge");
		}
		public override void SetDefaults()
		{
            item.damage = 25;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 52;     //gun image width
            item.height = 52;   //gun image  height
            item.useTime = 50;  //how fast 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.1f;
            item.value = 175000;
            item.rare = 7;
            item.UseSound = SoundID.Item9;
            item.autoReuse = false;
            item.shoot = 709; 
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
				Main.NewText("Charge of " + text, 145, 145, 255);
				checkDelay = 50;
				  }
			  }
			  else
			  {
				  if(checkDelay > 50)
				  {
				checkDelay = 0;
				  }
              int numberProjectiles = 1 + (int)(0.1f * charge);
			   if(numberProjectiles > 40)
			  {
				  numberProjectiles = 40;
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
            item.shootSpeed = 12.25f * 0.05f * Main.rand.Next(20,100);
				  if(checkDelay >= 45 && checkDelay < 50)
				  {
					charge = 0;
				  }
			checkDelay++;
			
			if(charge > 68)
			{
				if(Main.rand.Next(42) == 0)
				{
				charge -= 1;
				}
			}
			item.damage = 12;
			item.useTime = 50;
			item.damage += (int)(0.25f * charge);
			item.useTime = (int)(50 - (0.45f * charge));
			if(item.useTime < 8)
			{
				item.useTime = 8;
			}
			if(item.damage > 56)
			{
				item.damage = 56;
			}
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.onhit == 1)
			{
				charge += modPlayer.onhitdamage * 0.24f;
				charge += 1;
				modPlayer.onhitdamage = 0;	
				if(player.name == "Straffex")
				{
				charge += 2;
				}
				int chargeInt = (int)charge;
				string text = chargeInt.ToString();;
				Main.NewText("Charge of " + text, 145, 145, 255);

			}
			
				
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SteelBar", 22);
			recipe.AddIngredient(ItemID.Bone, 28);
			recipe.AddIngredient(null, "Discharge", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
