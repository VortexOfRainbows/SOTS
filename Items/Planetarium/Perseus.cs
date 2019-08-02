using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Perseus : ModItem
	{	float firerate = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Perseus");
			Tooltip.SetDefault("Rains projectiles from the weapon in your last inventory slot down on your cursor when enemies are near\nDecreases all damage by 35%");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 42;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.meleeDamage -= 0.35f;
			player.rangedDamage -= 0.35f;
			player.magicDamage -= 0.35f;
			player.minionDamage -= 0.35f;
			player.thrownDamage -= 0.35f;
			
				firerate += 1.5f;
			Item item1 = player.inventory[49];
			int projectileType = item1.shoot;
			int damage = (int)(item1.damage * 0.6f);
			float shootSpeed = 1.25f * item1.shootSpeed;
			float knockBack = item1.knockBack;
			int reload = item1.useTime;
			int useAmmo = item1.useAmmo;
			
			if(useAmmo != 0)
			{
				projectileType = mod.ProjectileType("FireProj");
				for(int i = 0; i < 58; i++)
				{
			Item item2 = player.inventory[i];
				if(useAmmo == item2.ammo)
					{
					int projectileAmmo = item2.shoot;
					projectileType = projectileAmmo;
					damage += (int)(item2.damage * 0.6f);
					break;
					}
				}
				
			}
			
			if(item1.summon == false && item1.type != mod.ItemType("Obliterator") && item1.type != mod.ItemType("TrinityScatter") && item1.type != mod.ItemType("TrinityCrossbow") && item1.type != mod.ItemType("HallowedCrossbow") && item1.type != mod.ItemType("HallowedScatter") && item1.type != mod.ItemType("WormWoodScatter") && item1.type != mod.ItemType("WormWoodCrossbow") &&  item1.type != mod.ItemType("MargritBlaster") && item1.type != 71  && item1.type != 72 && item1.type != 73 && item1.type != 74)
					{	
						
					for(int i = 0; i < 200; i++)
					{
							   NPC target = Main.npc[i];
								if(firerate >= reload && item1.channel == false && item1.damage > 0)
								{
									
					Vector2 playerCursor;
					
					if (player.gravDir == 1f)
					{
					playerCursor.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					playerCursor.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						playerCursor.X = (float)Main.mouseX + Main.screenPosition.X;
						
							   float distanceFromX = target.Center.X - playerCursor.X;
							   float distanceFromY = target.Center.Y - playerCursor.Y;
							   float distance2 = (float)System.Math.Sqrt((double)(distanceFromX * distanceFromX + distanceFromY * distanceFromY));

								   if(distance2 < 160f && !target.friendly && target.active)
								   {
												int proj = Projectile.NewProjectile(target.Center.X + (float)(Main.rand.Next(-100,101) * 0.25f), target.Center.Y - 625f, (Main.rand.Next(-100,101) * 0.02f), shootSpeed + (float)(Main.rand.Next(-100,101) * 0.006f * shootSpeed), projectileType, damage, knockBack, Main.myPlayer, 0f, 0f); 
												Main.projectile[proj].tileCollide = false;
												firerate = 0;
									}
								}
						
					
						}
					}
			
		}
	}
}
