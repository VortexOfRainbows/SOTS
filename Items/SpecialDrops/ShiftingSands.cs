using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class ShiftingSands : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shifting Sands");
			Tooltip.SetDefault("Fire a gust of sand");
		}
		public override void SetDefaults()
		{
            item.damage = 14;
            item.magic = true; 
            item.width = 28;    
            item.height = 30; 
            item.useTime = 14; 
            item.useAnimation = 24;
            item.useStyle = 5;    
            item.knockBack = 3;
            item.value = 100000;
            item.rare = 2;
			item.UseSound = SoundID.Item34;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 7.5f; 
			item.shoot = mod.ProjectileType("SandPuff");
			item.mana = 8;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                  Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
	}
	}
}
