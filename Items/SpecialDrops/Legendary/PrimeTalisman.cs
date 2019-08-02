using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class PrimeTalisman : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloody Prime Talisman");
			Tooltip.SetDefault("-20 defense\nLegendary drop\nLevels up as you progress\nIncreases damage almost exponentially as health lowers\n10% increase to damage");
		}
		public override void SetDefaults()
		{
      
            item.width = 54;     
            item.height = 66;   
            item.value = Item.sellPrice(1, 25, 0, 0);
            item.rare = 11;
			item.accessory = true;
			item.defense = -20;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{ 
				
			player.meleeDamage += 0.1f;
			player.thrownDamage +=  0.1f;
			player.rangedDamage +=  0.1f;
			player.magicDamage +=  0.1f;
			player.minionDamage +=  0.1f;
		
			float ExponentAmount = 0.01f;
			int ProjectileAmount = 30;
			float healthdown = (float)(player.statLifeMax2 - player.statLife); //if max life is 100, and your current health is 33, int healthdown = 67
			float expogrow = (float)(healthdown * ExponentAmount * 0.01); //if healthdown is 67, expogrow = 0.0067 if you haven't beat a boss yet
			
						for(int repeatLevel = SOTSWorld.legendLevel; 0 < repeatLevel; repeatLevel--)
						{
							ExponentAmount += 0.01f;
						}
						if(SOTSWorld.legendLevel == 24)
						{
							ExponentAmount += 0.01f;
						}
						if(SOTSWorld.legendLevel == 25)
						{
							ExponentAmount += 0.02f;
						}
				
				if(expogrow >= 0)
				{
					player.meleeDamage += expogrow;
					player.thrownDamage += expogrow;
					player.rangedDamage += expogrow;
					player.magicDamage +=  expogrow;
				}
				
				if(player.immune == true && (Main.rand.Next(0, ProjectileAmount)) == 0)
				{
					Projectile.NewProjectile(player.Center.X, player.Center.Y,  (Main.rand.Next(-7,8)),(Main.rand.Next(-2,8)), mod.ProjectileType("BloodSpike"), (ProjectileAmount * 7), 0, Main.myPlayer, 0.0f, 0);
				}
			}
		}
	}

