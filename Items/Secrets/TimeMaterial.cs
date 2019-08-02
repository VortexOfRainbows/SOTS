using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	public class TimeMaterial : ModItem
	{	int timer = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Time Material");
			Tooltip.SetDefault("Secret Material");
		}
		public override void SetDefaults()
		{
      
            item.width = 26;     
            item.height = 26;   
            item.value = 0;
            item.rare = -11;
			item.shootSpeed = 0;
			item.maxStack = 999;

		}
		
	}
}
