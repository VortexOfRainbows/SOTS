using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SOTS.Void;
using System;

namespace SOTS.Items.Celestial
{
	public class ContinuumCollapse : VoidItem
	{	int coolDown = 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Continuum Collapse");
			Tooltip.SetDefault("'Devour all that might be infinite'\nCan hit up to 15 enemies at a time");
		}
		public override void SafeSetDefaults()
		{
			item.damage = 200;
			item.magic = true;
			item.width = 26;
			item.height = 32;
            item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = 12;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.autoReuse = true;            
			item.shoot = mod.ProjectileType("ContinuumSphere"); 
			item.shootSpeed = 1;
			item.knockBack *= 3;
			item.channel = true;
			item.UseSound = SoundID.Item92;
			item.noUseGraphic = true;
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
				//item.UseSound = SoundID.Item22;
				if(summon)
				{
					return true; 
				}
			}
              return false; 
		}
		public override void GetVoid(Player player)
		{
			voidMana = 6;
		}
	}
}