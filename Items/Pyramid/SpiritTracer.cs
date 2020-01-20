using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.Pyramid
{
	public class SpiritTracer : VoidItem
	{
		bool inInventory = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit Tracer");
			Tooltip.SetDefault("Fires phantom arrows\nCan hit up to 3 enemies at a time");
		}
		public override void SafeSetDefaults()
		{

			item.damage = 33;
			item.ranged = true;
			item.width = 30;
			item.height = 74;
			item.useTime = 18;
			item.useAnimation = 18;
			item.useStyle = 5;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 7, 25, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;            
			item.shoot = 1; 
            item.shootSpeed = 16.5f;
			item.useAmmo = AmmoID.Arrow;
			item.noMelee = true;
			item.expert = true;
		}
		public override void GetVoid(Player player)
		{
				voidMana = 6;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(6, 0);
		}
        public override bool BeforeUseItem(Player player)
		{
			if(inInventory)
				return true;
			return false;
		}
		public override void UpdateInventory(Player player)
		{
			inInventory = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			inInventory = false;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TracerArrow"), damage, knockBack, player.whoAmI, 0, type);
			return false; 
		}
	}
}