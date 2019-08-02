using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Eclipse : ModItem
	{	int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eclipse");
			Tooltip.SetDefault("Fire an additional projectile towards you from your cursor\nIf the projectile collides with a tile or despawns, it will send a planetary flame towards your cursor");
			///\nIf the projectile goes through you, you will heal 1 health, 3 void, and 5 mana (maybe later)
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 42;     
            item.height = 42;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
                modPlayer.Eclipse = true;
			timer++;
			if(timer >= 180)
			{
				timer = 1;
				
				Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("Eclipse"), 0, 0, 0);
			}
			
		}
	}
}
