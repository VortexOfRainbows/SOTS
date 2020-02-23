using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;

namespace SOTS.Items.Void
{
	public class BowSonOfBow : VoidItem
	{	int explosiveShot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bow Son Of Bow");
			Tooltip.SetDefault("'Are you kidding me? Nothing gets past my bow!'");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 20;  
            item.ranged = true; 
            item.width = 28; 
            item.height = 58;
            item.useTime = 18;
            item.useAnimation = 18;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = .5f;
            item.value = 50000;
            item.rare = 10;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("ArrowSonOfArrow");
            item.shootSpeed = 14.5f;
		}
		public override void GetVoid(Player player)
		{
			voidMana = 3;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            explosiveShot++;
			if(explosiveShot % 3 == 0)
			{
				Projectile.NewProjectile(position.X, position.Y, speedX * 1.5f, speedY * 1.5f, type, damage, 1.1f, player.whoAmI);
			}
			Projectile.NewProjectile(position.X + (speedY * 1), position.Y - (speedX * 1), speedX, speedY, type, damage, .5f, player.whoAmI);
			Projectile.NewProjectile(position.X - (speedY * 1), position.Y + (speedX * 1), speedX, speedY, type, damage, .5f, player.whoAmI);
            return false; 
		}
	}
}
