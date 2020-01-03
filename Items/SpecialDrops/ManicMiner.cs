using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using SOTS.Void;

namespace SOTS.Items.SpecialDrops
{
	public class ManicMiner : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manic Miner");
			Tooltip.SetDefault("Converts void into mining lasers");
		}
		public override void SafeSetDefaults()
		{
			item.width = 34;
			item.height = 26;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = 5;
			item.knockBack = 6;
            item.value = Item.sellPrice(0, 1, 5, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item12;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ManaMiner"); 
            item.shootSpeed = 5.75f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
              int numberProjectiles = 1;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(2)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

              }
              return false; 
		}
	}
}