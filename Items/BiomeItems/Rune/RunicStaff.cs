using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.BiomeItems.Rune
{
	public class RunicStaff : ModItem
	{ 	int ProjAmount = 1;
		float shootSpeed = 5f;
		float flower = 1f;
		public override void SetStaticDefaults()
		{ 
			DisplayName.SetDefault("Runic Staff");
			Tooltip.SetDefault("Absorbs power from runes surrounding it");
		}
		public override void SetDefaults()
		{
            item.damage = 10;  //gun damage
            item.melee = false;  
            item.magic = false;  
            item.ranged = false;  
            item.thrown = false;  
            item.summon = false;  
            item.width = 44;     //gun image width
            item.height = 50;   //gun image  height
            item.useTime = 30;  //how fast 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 0f;
            item.value = 1000000000;
            item.rare = 11;
            item.UseSound = SoundID.Item9;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("RuneBolt1"); 
            item.shootSpeed = 5f;
			item.reuseDelay = 0;
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			
				int numberProjectiles = ProjAmount;
				
			  for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians((int)(numberProjectiles * 1.5f * flower))); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
              }
			  
              return false; 
		}
		public override bool AltFunctionUse(Player player)
        {
            return true;
        }
		public override void UpdateInventory(Player player)
		{
			item.damage = 10;
			item.useTime = 30;
            item.useAnimation = 30;
            item.shootSpeed = 5f;
			item.reuseDelay = 0;
			ProjAmount = 1;
			flower = 1;
			int maxStack1 = 20;
			int maxStack2 = 12;
			int maxStack3 = 8;
			int maxStack4 = 4;
			int maxStack5 = 2;
			int maxStack6 = 1;
			int maxStack7 = 4;
				
				for(int i = 0; i < 50; i++)
				{
					Item rune = player.inventory[i];
					if(rune.type == mod.ItemType("BaseRune"))//nIncreases damage by 1	Decreases firerate by 1	Decreases projectile speed by 1.5	Stacks up to 20
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack1--;
							if(maxStack1 >= 0)
							{
							item.damage += 1;
							item.useTime += 1;
							item.useAnimation += 1;
							item.shootSpeed -= 1.5f;
							}
						}
					}
					if(rune.type == mod.ItemType("SingleShotRune"))//Increases damage by 3	Increases firerate by 1	Increases projectile speed by 0.5	Stacks up to 12
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack2--;
							if(maxStack2 >= 0)
							{
							item.damage += 3;
							item.useTime -= 1;
							item.useAnimation -= 1;
							item.shootSpeed += 0.5f;
							}
						}
					}
					if(rune.type == mod.ItemType("ShotgunRune"))//Decreases damage by 2	Decreases firerate by 1	Increases projectile speed by 1	Stacks up to 8
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack3--;
							if(maxStack3 >= 0)
							{
							item.damage -= 2;
							item.useTime += 1;
							item.useAnimation += 1;
							item.shootSpeed += 1f;
							ProjAmount += 2;
							}
						}
					}
					if(rune.type == mod.ItemType("FlowerRune"))//Decreases damage by 1	Decreases firerate by 2	Increases projectile speed by 2.5	Stacks up to 4
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack4--;
							if(maxStack4 >= 0)
							{
							item.damage -= 1;
							item.useTime += 2;
							item.useAnimation += 2;
							item.shootSpeed += 2.5f;
							ProjAmount += 1;
							ProjAmount *= 2;
							flower += 4;
							}
						}
					}
					if(rune.type == mod.ItemType("RocketRune"))//Increases damage by 4	Increases firerate by 1	Increases projectile speed by 3	Stacks up to 2
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack5--;
							if(maxStack5 >= 0)
							{
							item.damage += 4;
							item.useTime -= 1;
							item.useAnimation -= 1;
							item.shootSpeed += 3f;
							}
						}
					}
					if(rune.type == mod.ItemType("LaserRune"))//Increases damage by 5	Increases firerate by 5	Increases projectile speed by 5	Decreases shotspread	Stacks up to 2
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack5--;
							if(maxStack5 >= 0)
							{
							item.damage += 5;
							item.useTime -= 5;
							item.useAnimation -= 5;
							item.shootSpeed += 5f;
							flower *= 0.5f;
							}
						}
					}
					if(rune.type == mod.ItemType("CloudRune"))
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack5 -= 2;
							
						}
					}
					if(rune.type == mod.ItemType("OilRune"))//Decreases damage by 16	Increases firerate by 8	Decreases shotspread
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack2 -= 12;
							if(maxStack2 >= 0)
							{
							item.damage -= 16;
							item.useTime -= 8;
							item.useAnimation -= 8;
							flower *= 0.25f;
							}
						}
					}
					if(rune.type == mod.ItemType("RailRune"))//Accelerates projectile damage and speed during travel	Stacks up to 1	C tier rune, conflicts with other C tiers
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack7 -= 4;
							
						}
					}
					if(rune.type == mod.ItemType("ThunderRune"))//Increases damage by 10	Attacks induce paralysis	Stacks up to 1	C tier rune, conflicts with other C tiers
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack7 -= 4;
							if(maxStack7 >= 0)
							{
								item.damage += 10;
							}								
						}
					}
					if(rune.type == mod.ItemType("VampiricRune"))//Decreases damage by 2	Increases firerate by 2	Stacks up to 4	C tier rune, conflicts with other C tiers
					{
						for(int stack = rune.stack; stack > 0; stack--)
						{
							maxStack7 -= 1;
							if(maxStack7 >= 0)
							{
								item.damage -= 2;
								item.useTime -= 2;
								item.useAnimation -= 2;
							}								
						}
					}
					if(rune.type == mod.ItemType("BoomerangRune"))//Decreases firerate by 4	Decreases projectile speed by 2.5	Stacks up to 1	Creates a child opportunity
					{
						
							item.shootSpeed -= 2.5f;
							item.useTime += 4;
							item.useAnimation += 4;
							i += 100;
						
					}
					if(rune.type == mod.ItemType("TurretRune"))//Decreases firerate by 5	Decreases projectile speed by 5	Stacks up to 1	Creates a child opportunity
					{
						
							item.shootSpeed -= 4f;
							item.useTime += 5;
							item.useAnimation += 5;
							i += 100;
						
					}
				}
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
				
		}
	}
}
