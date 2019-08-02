using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;

using Terraria.ModLoader;

namespace SOTS.Items.SpecialDrops
{
	public class MartianCore : ModItem
	{	int mr = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Martian Core");
			Tooltip.SetDefault("Summons a martian saucer to assist you\nGives two magic abilities including mana regeneration");
		}
		public override void SetDefaults()
		{
      
            item.width = 32;     
            item.height = 32;   
            item.value = 100000000;
            item.rare = 9;
			item.accessory = true;


		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{		
					player.ghostHurt = true;
					
					player.AddBuff(BuffID.ManaRegeneration, 300);
					
					mr += 1;
					if(mr == 2)
					{
		Vector2 vector14;
					
						if (player.gravDir == 1f)
					{
					vector14.Y = (float)Main.mouseY + Main.screenPosition.Y;
					}
					else
					{
					vector14.Y = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY;
					}
						vector14.X = (float)Main.mouseX + Main.screenPosition.X;
                Projectile.NewProjectile(vector14.X,  vector14.Y, 0, 0,  mod.ProjectileType("MartianShooter"), 19, 1, Main.myPlayer, 0.0f, 1);
				mr = 0;
	}
		}
}
}
