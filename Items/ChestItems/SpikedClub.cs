using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SOTS.Items.ChestItems
{
	public class SpikedClub : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiked Club");
			Tooltip.SetDefault("Lays down spike traps");

		}
		public override void SetDefaults()
		{

			item.damage = 12;
			item.melee = true;
			item.width = 42;
			item.height = 42;
			item.useTime = 38;
			item.useAnimation = 38;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 52500;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("SpikeTrap"); 
            item.shootSpeed = 3.4f;

		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
          {
			  
                  Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				  if(player.spikedBoots >= 1)
				  {
                  Projectile.NewProjectile(position.X, position.Y, speedX * 1.25f, speedY * 1.25f, type, damage, knockBack, player.whoAmI);
				  
				  }
				  if(player.spikedBoots >= 2)
				  {
                  Projectile.NewProjectile(position.X, position.Y, speedX * 1.5f, speedY * 1.5f, type, damage, knockBack, player.whoAmI);
				  
				  }
              return false; 
		}
	}
}