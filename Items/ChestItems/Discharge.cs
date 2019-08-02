using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.ChestItems
{
	public class Discharge : ModItem
	{ 	float charge = 0;
		int checkDelay = 50;
		public override void SetStaticDefaults()
		{ 
			DisplayName.SetDefault("Discharge");
			Tooltip.SetDefault("Increases in charge as you get hit, right click to display charge\nClick to discharge\nNaturally loses charge when above 50 charge");
		}
		public override void SetDefaults()
		{
            item.damage = 11;  //gun damage
            item.magic = true;   //its a gun so set this to true
            item.width = 48;     //gun image width
            item.height = 48;   //gun image  height
            item.useTime = 50;  //how fast 
            item.useAnimation = 50;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0.1f;
            item.value = 175000;
            item.rare = 6;
            item.UseSound = SoundID.Item9;
            item.autoReuse = false;
            item.shoot = mod.ProjectileType("GoldThunder"); 
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
				Main.NewText("Charge of " + text, 195, 145, 0);
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
			   if(numberProjectiles > 30)
			  {
				  numberProjectiles = 30;
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
			
			if(charge > 51)
			{
				if(Main.rand.Next(40) == 0)
				{
				charge -= 1;
				}
			}
			item.damage = 11;
			item.useTime = 50;
			item.damage += (int)(0.4f * charge);
			item.useTime = (int)(50 - (0.33f * charge));
			if(item.useTime < 10)
			{
				item.useTime = 10;
			}
			if(item.damage > 35)
			{
				item.damage = 35;
			}
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if(modPlayer.onhit == 1)
			{
				charge += modPlayer.onhitdamage * 0.15f;
				modPlayer.onhitdamage = 0;	
				if(player.name == "Straffex")
				{
				charge += 4;
				}
				int chargeInt = (int)charge;
				string text = chargeInt.ToString();;
				Main.NewText("Charge of " + text, 195, 145, 0);

			}
			
				
		}
	}
}
