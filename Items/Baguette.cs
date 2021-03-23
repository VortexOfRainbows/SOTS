using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class Baguette : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crusty Baguette");
			Tooltip.SetDefault("Killing enemies will drop baguette crumbs\nPickup baguette crumbs to increase the range and damage of your baguette, and heal lost life\n'Surrender is not an option'");
		}
		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 14;
			item.useAnimation = 14;
			item.useStyle = 5;
			item.knockBack = 7.5f;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			//item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<ExtendoBaguette>(); 
            item.shootSpeed = 9f;
			item.noUseGraphic = true;
			item.channel = true;
			item.noMelee = true;
		}
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			flat += modPlayer.baguetteLength;
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }
        public override void HoldItem(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.baguetteDrops = true;
		}
		public override bool CanUseItem(Player player)
		{
			int num = 0;
			for(int i = 0; i < Main.maxProjectiles; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.owner == player.whoAmI && proj.type == ModContent.ProjectileType<ExtendoBaguette>() && proj.active && proj.alpha != 255)
                {
					num++;
					break;
                }
            }
			return num < 1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			return false; 
		}
    }
}
	
