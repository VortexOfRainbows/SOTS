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
	public class BigRockInSky : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Rock In Sky");
			Tooltip.SetDefault("13 solar mass");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 440;     
            item.height = 440;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for(int i = 0; i < 1000; i++)
			{
				Projectile projectile = Main.projectile[i];
				if(projectile.friendly)
				{
					projectile.scale *= 1.01f;
					projectile.width = (int)(projectile.width * 1.02f);
					projectile.height = (int)(projectile.height * 1.02f);
				}
			}
		}
	}
}
