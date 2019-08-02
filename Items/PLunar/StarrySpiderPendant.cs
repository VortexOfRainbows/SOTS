using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.PLunar
{
	public class StarrySpiderPendant : ModItem
	{int Probe = -1;
	int Probe2 = -1;
	int Probe3 = -1;
	int Probe4 = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stardust Spider Pendant");
			Tooltip.SetDefault("Let the power of creation create horror\n25% increased summon damage\nSummons a buffed spider at no cost");
		}
		public override void SetDefaults()
		{
	
      
            item.width = 46;     
            item.height = 46;   
            item.value = 125000;
            item.rare = 10;

			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			
			player.minionDamage += 0.25f;
			
			if (Probe2 == -1)
			{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 390, 223, 0, player.whoAmI);
					}
				if (!Main.projectile[Probe2].active || Main.projectile[Probe2].type != 390)
				{
					Probe2 = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, 390, 223, 0, player.whoAmI);
				}
				Main.projectile[Probe2].timeLeft = 6;
			
		}
	}
}