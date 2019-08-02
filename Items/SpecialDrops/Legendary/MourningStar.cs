using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class MourningStar : ModItem
	{	int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Mourning Star");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress\nA Mourning Star rotates around you\nDouble tap down to send it to your cursor instead\nThe Mourning Star does 50% damage on your cursor\nThe Mourning Star will deflect enemy projectiles\n15% increased melee and magic damaage\n50% decreased movement speed");
		}
		public override void SetDefaults()
		{
      
			item.damage = 10;
            item.width = 44;     
            item.height = 40;   
            item.value = Item.sellPrice(1, 25, 0, 0);
            item.rare = 11;
			item.accessory = true;
			item.defense = 5;
		

		}
        public override void UpdateInventory(Player player)
		{
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 12;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 15;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 19;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 24;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 30;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 32;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 35;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 38;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 40;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 42;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 45;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 50;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 55;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 60;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 63;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 67;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 71;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 76;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 80;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 85;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 90;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 100;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 112;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 125;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 150;
			}
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MourningStar"), 17, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != mod.ProjectileType("MourningStar"))
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("MourningStar"), 17, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
			player.moveSpeed -= 0.5f;
			player.magicDamage += .15f;
			player.meleeDamage += .15f;
			item.damage = 10;
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 12;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 15;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 19;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 24;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 30;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 32;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 35;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 38;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 40;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 42;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 45;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 50;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 55;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 60;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 63;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 67;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 71;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 76;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 80;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 85;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 90;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 100;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 112;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 125;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 150;
			}
		}
		
	}
}










