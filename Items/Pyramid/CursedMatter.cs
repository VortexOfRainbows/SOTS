using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	public class CursedMatter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Matter");
			Tooltip.SetDefault("");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 6));
		}
		public override void SetDefaults()
		{

			item.width = 26;
			item.height = 30;
            item.value = Item.sellPrice(0, 0, 7, 50);
			item.rare = 3;
			item.maxStack = 999;
		}
	}
}