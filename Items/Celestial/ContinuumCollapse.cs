using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class ContinuumCollapse : VoidItem
	{	int coolDown = 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Continuum Collapse");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 62;
			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("CollapseLaser"); 
			item.shootSpeed = 1;
			item.knockBack *= 3;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 0;
		}
	}
}