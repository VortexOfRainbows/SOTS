using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class CeremonialKnife : VoidItem
	{	int coolDown = 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ceremonial Knife");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.CloneDefaults(3368); //arhkalis
			item.damage = 62;
			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 8;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("CeremonialSlash"); 
			item.shootSpeed = 10;
			item.knockBack *= 3;
			item.expert = true;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
		}
	}
}