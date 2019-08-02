using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class EoCPendant : ModItem
	{int Probe = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eye Pendant");
			Tooltip.SetDefault("Summons a Spazmini to protect you");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 36;     
            item.height = 34;   
            item.value = 125000;
            item.rare = 10;

			item.accessory = true;
			item.defense = 2;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions++;
			if (Probe == -1)
			{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 388, 23, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe].active || Main.projectile[Probe].type != 388)
				{
					Probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 388, 23, 0, player.whoAmI);
				}
				Main.projectile[Probe].timeLeft = 6;
				
		}
	}
}