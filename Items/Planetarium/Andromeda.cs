using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;


namespace SOTS.Items.Planetarium
{
	public class Andromeda : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Andromeda");
			Tooltip.SetDefault("50% decreased melee, ranged, magic, and thrown damage\nRemoves knockback from your items\nQuantity over Quality, triples most projectiles fired\nReminds you of a certain mod that you're currently playing :D\n... also happens to duplicate explosives into infinity");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 50;     
            item.height = 22;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.Andromeda = true;
				
			player.meleeDamage -= 0.5f;
			player.magicDamage -= 0.5f;
			player.thrownDamage -= 0.5f;
			player.rangedDamage -= 0.5f;
			
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j]; 
					if(projectile.friendly == true && projectile.hostile == false && player == Main.player[projectile.owner] && projectile.minion == false && projectile.knockBack >= 1 && projectile.active)
					{
						projectile.knockBack -= 10;
						Projectile.NewProjectile(projectile.Center.X + (projectile.velocity.Y * 4), projectile.Center.Y - (projectile.velocity.X * 4), projectile.velocity.X, projectile.velocity.Y, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI);
						Projectile.NewProjectile(projectile.Center.X - (projectile.velocity.Y * 4), projectile.Center.Y + (projectile.velocity.X * 4), projectile.velocity.X, projectile.velocity.Y, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI);

						
						
					}
								
				
			}
			
			
		
			
			
		}
	}
}
