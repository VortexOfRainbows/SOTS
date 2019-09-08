using System;
using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Challenges
{
	public class SpiritsReleased : ModItem
	{
			float oldPosition = 0;
			int activeState = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirits Released");
			Tooltip.SetDefault("Initiates hardmode\nChallenge: S");
		}
		public override void SetDefaults()
		{
            item.width = 32;     
            item.height = 30;   
            item.useTime = 1;  
            item.useAnimation = 30;
            item.useStyle = 5;    
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 0;
            item.rare = -12;
            item.UseSound = SoundID.Item8;
            item.autoReuse = false;
			item.consumable = false;
		}
		public override bool UseItem(Player player)
		{
			player.AddBuff(mod.BuffType("Immune"), 240);
			activeState++;
			if(activeState == 2)
			{
			player.position.Y -= 30;
			}
			if(activeState == 4)
			{
			oldPosition = player.position.Y;
			item.damage = 100;
			player.position.Y = (Main.maxTilesY - 150) * 16;
			}
			if(activeState == 6)
			{
			int npc = NPC.NewNPC((int)player.Center.X, (int)player.Center.Y, NPCID.WallofFlesh);
			Main.npc[npc].life = 1;
			Main.npc[npc].damage = -1;
			}
			if(activeState == 9)
			{
			int index = Projectile.NewProjectile((int)player.Center.X, (Main.maxTilesY - 150) * 16, 0, 0, 14, 100000, 0, Main.myPlayer);
			Main.projectile[index].width = 1000;
			Main.projectile[index].height = 1000;
			Main.projectile[index].timeLeft = 4;
			Main.projectile[index].penetrate = 100;
			Main.projectile[index].tileCollide = false;
			
			
			}
			if(activeState >= 12)
			{
			item.consumable = true;
			player.position.Y = oldPosition;
			}
			return true;
		}
	}
}
