using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using SOTS.Void;

namespace SOTS.Items.Challenges
{
	public class ChallengePlayer : ModPlayer
	{
		int timer = 5400;
		int timer2 = 3600;
		int timer3 = 1800;
		int typeRestriction = 0;
		int randEnemy = 0;
		public override void ResetEffects() 
		{
			if(SOTSWorld.challengeIcarus)
			{
				if(player.velocity.Y < -1f)
				{
				player.velocity.Y *= 0.961f;
				}
				player.maxFallSpeed *= 2f;
			}
		}
		public override void PostUpdateBuffs()
		{
			
			if(SOTSWorld.challengePermanence)
			{
				timer3++;
				if(randEnemy != -1)
				{
					NPC npc = Main.npc[randEnemy];
					npc.dontTakeDamage = false;
				}
				
				if(timer3 >= 1800)
				{
					for(int i = Main.rand.Next(211); i != -1; i = Main.rand.Next(211))
					{
						if(i >= 200)
						{
							Main.NewText("No Enemy Selected", 110, 5, 200);
							randEnemy = -1;
							break;
						}
						NPC npc1 = Main.npc[i];
						if(npc1.boss && Main.rand.Next(33) == 0 && !npc1.friendly && npc1.active)
						{
							Main.NewText("Boss Selected", 110, 5, 200);
							randEnemy = i;
							break;
						}
						else if(!npc1.boss && Main.rand.Next(npc1.lifeMax + 600) >= npc1.lifeMax && !npc1.friendly && npc1.active)
						{
							Main.NewText("Enemy Selected", 110, 5, 200);
							randEnemy = i;
							break;
						}
					}
					timer3 = Main.rand.Next(-90,91);
				}
				
				if(randEnemy != -1)
				{
					NPC npc = Main.npc[randEnemy];
					npc.dontTakeDamage = true;
					npc.life = npc.lifeMax;
				}
			}
			if(SOTSWorld.challengeLock)
			{
				timer2++;
				if(timer2 >= 3600)
				{
					typeRestriction = Main.rand.Next(6);
					if(typeRestriction == 0) //melee
					{
						Main.NewText("Melee unlocked", 110, 5, 200);
						
					}
					if(typeRestriction == 1) //magic, void
					{
						Main.NewText("Magic and Void unlocked", 110, 5, 200);
						
					}
					if(typeRestriction == 2) //ranged
					{
						Main.NewText("Ranged unlocked", 110, 5, 200);
						
					}
					if(typeRestriction == 3) //summon, thrown
					{
						Main.NewText("Summon and Thrown unlocked", 110, 5, 200);
					}
					if(typeRestriction == 4) //locked
					{
						Main.NewText("Everything locked", 110, 5, 200);
						timer += 3300;
					}
					if(typeRestriction == 5) //non locked
					{
						Main.NewText("Everything unlocked", 110, 5, 200);
						timer += 2500;
					}
					timer2 = Main.rand.Next(-180,181);
				}
				
				player.meleeDamage -= 1.5f;
				player.rangedDamage -= 1.5f;
				player.magicDamage -= 1.5f;
				player.thrownDamage -= 1.5f;
				player.minionDamage -= 1.5f;
				
					if(typeRestriction == 0) //melee
					{
						player.meleeDamage += 1.5f;
					}
					if(typeRestriction == 1) //magic, void
					{
						player.magicDamage += 1.5f;
						VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);	
						voidPlayer.voidDamage += 1.5f;
					}
					if(typeRestriction == 2) //ranged
					{
						player.rangedDamage += 1.5f;
					}
					if(typeRestriction == 3) //summon, thrown
					{
						player.minionDamage += 1.5f;
						player.thrownDamage += 1.5f;
					}
					if(typeRestriction == 4) //locked
					{
						//screw you
					}
					if(typeRestriction == 5) //none locked
					{
						player.magicDamage += 1.5f;
						player.meleeDamage += 1.5f;
						player.thrownDamage += 1.5f;
						player.minionDamage += 1.5f;
						player.rangedDamage += 1.5f;
					}
			}
			if(SOTSWorld.challengeDecay)
			{
				timer++;
				if(timer >= 5400)
				{
					for(int i = Main.rand.Next(59); i != -1; i = Main.rand.Next(59))
					{
						if(i == 58)
						{
							Main.NewText("Purge failed", 110, 5, 200);
							break;
						}
						Item item = player.inventory[i];
						if(item.createTile == -1 && item.pick < 1 && item.axe < 1 && item.hammer < 1 && item.damage > 0)
						{
							string slot = (i + 1).ToString();
							if(i < 50)
							{
							slot = (i + 1).ToString();
							Main.NewText("Purged item in inventory slot " + slot, 110, 5, 200);
							}
							else
							{
							slot = (i - 49).ToString();
							Main.NewText("Purged item in ammo/coin slot " + slot, 110, 5, 200);
							}
						
							//item.type = mod.ItemType("PurgeResidue");
							item.stack = 0;
							player.QuickSpawnItem(mod.ItemType("PurgeResidue"), 1);	
							break;
						}
						else if(item.maxStack > 1 && item.pick < 1 && item.axe < 1 && item.hammer < 1 && item.createTile == -1)
						{
							string slot = (i + 1).ToString();
							if(i < 50)
							{
							slot = (i + 1).ToString();
							Main.NewText("Purged item in inventory slot " + slot, 110, 5, 200);
							}
							else
							{
							slot = (i - 49).ToString();
							Main.NewText("Purged item in ammo/coin slot " + slot, 110, 5, 200);
							}
						
							//item.type = mod.ItemType("PurgeResidue");
							item.stack = 0;
							player.QuickSpawnItem(mod.ItemType("PurgeResidue"), 1);	
							break;
						}
					}
					timer = Main.rand.Next(-300,301);
				}
			}
			if(SOTSWorld.challengeIce)
			{
				player.statLifeMax2 -= 80;
			}
			if(SOTSWorld.challengeGlass)
			{
				int statLife = (int)(player.statLifeMax2 * .5f);
				player.statLifeMax2 = statLife;
				if(player.statLifeMax2 < 2)
				{
					player.statLifeMax = 2;
				}
				player.meleeDamage += .3f;
				player.magicDamage += .3f;
				player.rangedDamage += .3f;
				player.thrownDamage += .3f;
				player.minionDamage += .3f;
			}
		}
		public override void PostHurt(bool pvp, bool quiet, double damage, int hitDirection, bool crit) 
		{
			
			if(SOTSWorld.challengeIce)
			{
				player.statLifeMax -= (int)(.5f * damage);
				if(player.statLifeMax < 1)
				{
					player.statLifeMax = 1;
				}
			}
			
		}
	}
}