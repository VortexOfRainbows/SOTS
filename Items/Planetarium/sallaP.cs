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
	public class sallaP : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("sallaP");
			Tooltip.SetDefault("deeps elitecjrop setalecca yldipaR\nraepS");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 32;   
            item.value = 1000000;
            item.rare = 9;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			for(int j = 0; j < 1000; j++)
			{
				Projectile projectile = Main.projectile[j];
					
						projectile.velocity.X *= 1.1345f;
						projectile.velocity.Y *= 1.1345f;
					
								
				
			}
			
			
		
			
			
		}
	}
}
