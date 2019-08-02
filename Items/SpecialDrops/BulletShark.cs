using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	public class BulletShark : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bullet Shark");
			Tooltip.SetDefault("");
		}
		public override void SafeSetDefaults()
		{
			item.ranged = true;
			item.damage = 20;
			item.width = 52;
			item.height = 30;
			item.useTime = 50;
			item.useAnimation = 50;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 75000;
			item.rare = 5;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("BulletSharkProj"); 
            item.shootSpeed = 2.5f;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 20;
		}
	}
}