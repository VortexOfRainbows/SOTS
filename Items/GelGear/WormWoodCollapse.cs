using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;


namespace SOTS.Items.GelGear
{
	public class WormWoodCollapse : VoidItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Worm Wood Collapse");
			Tooltip.SetDefault("Charge to increase damage by up to 500%");
		}
		public override void SafeSetDefaults()
		{
            item.damage = 30;
            item.melee = true;  
            item.width = 42;
            item.height = 42;  
            item.useTime = 40; 
            item.useAnimation = 40;
            item.useStyle = 5;    
            item.knockBack = 0f;
            item.value = Item.sellPrice(0, 1, 80, 0);
            item.rare = 4;
            item.UseSound = SoundID.Item22;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("WormWoodArm"); 
            item.shootSpeed = 0f;
			item.channel = true;
            item.noUseGraphic = true; 
            item.noMelee = true;
			Item.staff[item.type] = true; 
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
				VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			
				bool summon = true;
				for (int l = 0; l < Main.projectile.Length; l++)
				{
					Projectile proj = Main.projectile[l];
					if(proj.active && proj.type == item.shoot && Main.player[proj.owner] == player)
					{
						summon = false;
					}
				}
			if(player.altFunctionUse != 2)
			{
				item.UseSound = SoundID.Item22;
				if(summon)
				{
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 0, player.whoAmI);
					Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, 1, player.whoAmI);
				}
			}
              return false; 
		}
		public override void GetVoid(Player player)
		{
				voidMana = 4;
		}
	}
}
