using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace SOTS.Items.Otherworld
{
	public class TEst : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("test");
			Tooltip.SetDefault("'Quite a deadly combination'");
		}
		public override void SetDefaults()
		{
			
			item.CloneDefaults(279);
			item.damage = 17;
			item.thrown = true;
			item.rare = 2;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ThunderColumnFast"); 
            item.shootSpeed = 7f;
			item.consumable = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(0);
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);
              }
              return false; 
		}
	}
}