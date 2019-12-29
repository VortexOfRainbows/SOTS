using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.SpecialDrops
{
	public class BlueJellyfishStaff : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Jellyfish Staff");
			Tooltip.SetDefault("Fires an energy ball that detonates into blue lighting after traveling forward");
		}
		public override void SetDefaults()
		{
            item.damage = 21;
            item.magic = true; 
            item.width = 32;    
            item.height = 32; 
            item.useTime = 30; 
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.knockBack = 3;
			item.value = Item.sellPrice(0, 1, 25, 0);
            item.rare = 3;
			item.UseSound = SoundID.Item92;
            item.noMelee = true; 
            item.autoReuse = false;
            item.shootSpeed = 14.5f; 
			item.shoot = mod.ProjectileType("BlueThunderCluster");
			Item.staff[item.type] = true; 
			item.mana = 12;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
                  Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
            return false;
	}
	}
}
