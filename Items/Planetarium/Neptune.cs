using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Planetarium
{
	public class Neptune : ModItem
	{	float boost = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Neptune");
			Tooltip.SetDefault("Lowers thrown damage by 20%\nAdds three additional projectiles to most thrown weapons");
		}
		public override void SetDefaults()
		{
      
			item.maxStack = 1;
            item.width = 30;     
            item.height = 42;   
            item.value = 0;
            item.rare = 9;
			item.accessory = true;

		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.thrownDamage -= 0.2f;
					player.AddBuff(mod.BuffType("Neptune"), 300);
			
		}
	}
}
