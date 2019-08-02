using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.ChestItems
{
	public class WingedKnife : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winged Knife");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			
			item.CloneDefaults(279);
			item.damage = 12;
			item.thrown = true;
			item.rare = 3;
			item.width = 34;
			item.height = 26;
			item.maxStack = 1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("WingedKnife"); 
            item.shootSpeed = 9.3f;
			item.consumable = false;
			
		}
	}
}