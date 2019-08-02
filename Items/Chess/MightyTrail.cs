using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Chess
{
	public class MightyTrail : ModItem
	{int Probe = -1;
	int Probe2 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Trail");
			Tooltip.SetDefault("Bubble farts\nFarts aren't always visible");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 26;     
            item.height = 36;   
            item.value = 125000;
            item.rare = 6;

			item.accessory = true;
			item.defense = 1;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if(Main.rand.Next(10) == 0)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, Main.rand.Next(-4,5), Main.rand.Next(-4,5), 405, 40, 1, 0);
			}
		}
	}
}