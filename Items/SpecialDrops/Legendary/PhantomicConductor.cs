using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System;

namespace SOTS.Items.SpecialDrops.Legendary
{
	public class PhantomicConductor : ModItem
	{	int coolDown = 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantomic Conductor");
			Tooltip.SetDefault("Legendary drop\nLevels up as you progress\nCharge to summon a phantom apparition that assists in combat for a limited amount of time and doesn't use minion slots\nCharging for longer will increase the minion's lifespan\nCharging greater than the minion's lifespan will change its association\nThis will lower its lifespan and change its attack\nYou can recharge the lifespan after switching associations");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
		}
		public override void SetDefaults()
		{
			item.damage = 8;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.width = 38;
			item.height = 38;
			item.useStyle = 1;
			item.knockBack = 0;
            item.value = Item.sellPrice(1, 25, 0, 0);
			item.rare = 10;
			item.channel = true;
			item.UseSound = SoundID.Item43;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("PhantomApparition"); 
            item.shootSpeed = 10;
			item.noUseGraphic = true;
            item.noMelee = true;
		}
        public override bool CanUseItem(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
         {	
			  Vector2 vector14;
					
					if (player.gravDir == 1f)
					{
						vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
						vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
					bool summon = true;
					for (int l = 0; l < Main.projectile.Length; l++)
					{
						Projectile proj = Main.projectile[l];
						if(proj.active && proj.type == item.shoot && Main.player[proj.owner] == player && proj.friendly)
						{
							summon = false;
						}
					}
					if(summon)
						Projectile.NewProjectile(vector14.X, vector14.Y, 0, 0, type, damage, 0, player.whoAmI);
						
				return false;
		}
		public override void UpdateInventory(Player player)
		{
			item.damage = 8;
			
			if(SOTSWorld.legendLevel == 1)
			{
				item.damage = 11;
			}
			if(SOTSWorld.legendLevel == 2)
			{
				item.damage = 13;
			}
			if(SOTSWorld.legendLevel == 3)
			{
				item.damage = 15;
			}
			if(SOTSWorld.legendLevel == 4)
			{
				item.damage = 17;
			}
			if(SOTSWorld.legendLevel == 5)
			{
				item.damage = 19;
			}
			if(SOTSWorld.legendLevel == 6)
			{
				item.damage = 21;
			}
			if(SOTSWorld.legendLevel == 7)
			{
				item.damage = 22;
			}
			if(SOTSWorld.legendLevel == 8)
			{
				item.damage = 24;
			}
			if(SOTSWorld.legendLevel == 9)
			{
				item.damage = 25;
			}
			if(SOTSWorld.legendLevel == 10)
			{
				item.damage = 29;
			}
			if(SOTSWorld.legendLevel == 11)
			{
				item.damage = 33;
			}
			if(SOTSWorld.legendLevel == 12)
			{
				item.damage = 37;
			}
			if(SOTSWorld.legendLevel == 13)
			{
				item.damage = 42;
			}
			if(SOTSWorld.legendLevel == 14)
			{
				item.damage = 48;
			}
			if(SOTSWorld.legendLevel == 15)
			{
				item.damage = 54;
			}
			if(SOTSWorld.legendLevel == 16)
			{
				item.damage = 60;
			}
			if(SOTSWorld.legendLevel == 17)
			{
				item.damage = 64;
			}
			if(SOTSWorld.legendLevel == 18)
			{
				item.damage = 70;
			}
			if(SOTSWorld.legendLevel == 19)
			{
				item.damage = 74;
			}
			if(SOTSWorld.legendLevel == 20)
			{
				item.damage = 80;
			}
			if(SOTSWorld.legendLevel == 21)
			{
				item.damage = 85;
			}
			if(SOTSWorld.legendLevel == 22)
			{
				item.damage = 90;
			}
			if(SOTSWorld.legendLevel == 23)
			{
				item.damage = 94;
			}
			if(SOTSWorld.legendLevel == 24)
			{
				item.damage = 100;
			}
			if(SOTSWorld.legendLevel == 25)
			{
				item.damage = 110;
			}
		
		}
	}
}