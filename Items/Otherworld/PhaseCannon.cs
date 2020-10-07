using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld
{
	public class PhaseCannon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Cannon");
			Tooltip.SetDefault("Fire a supercharged ball of plasma that can travel through walls");
		}
		public override void SetDefaults()
		{
            item.damage = 26; 
            item.ranged = true;  
            item.width = 52;   
            item.height = 28; 
            item.useTime = 90; 
            item.useAnimation = 90;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 4f;
            item.value = Item.sellPrice(0, 2, 25, 0);
            item.rare = ItemRarityID.LightPurple;
            item.UseSound = SoundID.Item92;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("FriendlyOtherworldlyBall");
			item.shootSpeed = 10; //not important
		}
		public override void HoldItem(Player player)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			ref int index = ref modPlayer.phaseCannonIndex;
			if (index >= 0)
			{
				Projectile proj = Main.projectile[index];
				if(proj.alpha > 0)
					proj.alpha -= 6;
			}
			if (index == -1)
			{
				Vector2 mouse = Main.MouseWorld;
				if (player.whoAmI == Main.myPlayer)
				{
					index = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("OtherworldlyTracer"), item.damage, item.knockBack, player.whoAmI, 1000, -1);
				}
			}
			else if (index < -1)
			{
				index++;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			ref int index = ref modPlayer.phaseCannonIndex;
			if (index < 0)
			{
				return false;
			}
			Projectile proj = Main.projectile[index];
			proj.ai[1] = -3;
			index = -31;
			Vector2 mouse = Main.MouseWorld;
			Vector2 distTo = mouse - position;
			distTo /= 30f;
			Projectile.NewProjectile(position.X, position.Y, distTo.X, distTo.Y, type, damage, knockBack, player.whoAmI);
			return false;
		}
	}
}
