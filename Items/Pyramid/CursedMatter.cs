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
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 6));
		}
		public override void SetDefaults()
		{

			Item.width = 26;
			Item.height = 30;
            Item.value = Item.sellPrice(0, 0, 7, 50);
			Item.rare = 3;
			Item.maxStack = 999;
		}
	}
}