using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SandyWarblade : VoidItem
	{
		int currentIndex = -1;
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandstone Warhammer");
			Tooltip.SetDefault("Launches homing blood hammer projectiles");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 25;
			item.melee = true;
			item.width = 56;
			item.height = 56;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 1;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("Bloodaxe");  
            item.shootSpeed = 21.5f;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(6, 0);
		}
	}
}