using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Items.Secrets
{
	public class LostSoul : ModItem
	{	bool lost = false;
		int probe = -1;
		int down = 0;
		int up = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lost Soul");
			Tooltip.SetDefault("Secret Item\nSummon Locked");
		}
		public override void SetDefaults()
		{
      
            item.width = 56;     
            item.height = 50;   
            item.value = 0;
            item.rare = -12;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
				item.defense = 0;
			if(lost)
			{
				item.defense = 10;
			}
				if (probe == -1)
				{
					probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("LostSoul"), 12, 0, player.whoAmI);
					}
				if (!Main.projectile[probe].active || Main.projectile[probe].type != mod.ProjectileType("LostSoul"))
				{
					probe = Projectile.NewProjectile(player.position.X, player.position.Y, 0, 0, mod.ProjectileType("LostSoul"), 12, 0, player.whoAmI);
				}
				Main.projectile[probe].timeLeft = 6;
			player.thrownDamage -= player.thrownDamage;
			player.minionDamage += 0.15f;
			player.meleeDamage -= player.meleeDamage;
			player.magicDamage -= player.magicDamage;
			player.rangedDamage -= player.rangedDamage;
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			modPlayer.lostSoul = true;
			if(modPlayer.Catalyst)
			{
			player.thrownDamage += 1.15f;
			player.minionDamage += 0.15f;
			player.meleeDamage += 1.15f;
			player.magicDamage += 1.15f;
			player.rangedDamage += 1.15f;
				
			}
			if(player.controlDown && down <= 9 && down >= 1)
			  {
				  player.position.X = Main.projectile[probe].Center.X - (player.width * 0.5f);
				  player.position.Y = Main.projectile[probe].Center.Y - (player.height * 0.5f);
				  down = 0;
				  
					  lost = true;
				  
			  }
			  if(down > 0)
			  {
			  down--;
			  }
			if(player.controlDown) 
			  {
				  down = 10;
			  }
			  if(player.controlUp && up <= 9 && up >= 1)
			  {
				  Main.projectile[probe].position.X = player.Center.X - (Main.projectile[probe].width * 0.5f);
				  Main.projectile[probe].position.Y = player.Center.Y - (Main.projectile[probe].height * 0.5f);
				  up = 0;
				  
					  lost = false;
				  
			  }
			  if(up > 0)
			  {
			  up--;
			  }
				if(player.controlUp) 
			  {
				  up = 10;
			  }
			  if(lost == true)
			  {
				  player.velocity.X = 0;
				  player.velocity.Y = 0;
				  if(player.controlDown)
				  {
					  Main.projectile[probe].position.Y += 11.5f;
				  }
				  if(player.controlUp)
				  {
					  Main.projectile[probe].position.Y += -11.5f;
				  }
				  if(player.controlRight)
				  {
					  Main.projectile[probe].position.X += 11.5f;
				  }
				  if(player.controlLeft)
				  {
					  Main.projectile[probe].position.X += -11.5f;
				  }
			  }
			}
		}
		
	}

