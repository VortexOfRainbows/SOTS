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

namespace SOTS.Items.Pyramid
{
	public class BrokenTablet : ModItem
	{	int timer = 0;
		float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Tablet");
			Tooltip.SetDefault("5% reduced mana usage\nSummons Sand Tornadoes every so often");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 22;     
            item.height = 28;   
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
			item.accessory = true;

		}
		
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			timer += 1;
		
		
					if(timer == 600)
					{
					Projectile.NewProjectile(player.Center.X, player.position.Y, 0, 0, 656, 30, 0, player.whoAmI);
					timer = 0;
					}
					player.manaCost -= 0.05f;
				
		}
		
	}
}