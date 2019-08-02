using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Chess
{
	public class MightKnight : ModNPC
	{	int velocityDown = 0;
	int boom = 0;
	int hurt = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Might Knight");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 1;  //5 is the flying AI
            npc.lifeMax = 475;   //boss life
            npc.damage = 56;  //boss damage
            npc.defense = 4;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 32;
            npc.height = 48;
			 Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 1000;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = false;
		}
		public override void NPCLoot()
		{
			
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.SoulofMight), 1);
		
			if((Main.rand.Next(19) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("SoulSingularity")), 1);
				
			if((Main.rand.Next(199) == 0))
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (mod.ItemType("MightJump")), 1);
			
		}
		public override void AI()
		{
			if(boom == 1)
			{
				npc.ai[0] -= npc.ai[0];
				npc.ai[0] -= 9;
			}
			else
			{
			npc.ai[0]++;
			npc.ai[0]++;
			npc.ai[0]++;
			
			if(npc.ai[0] < 0)
			{
				
			npc.ai[0] += -(npc.ai[0]);
			}
				
			}
			if(hurt == 0)
			{
				hurt++;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 0, 14, 1, 1, 0);
				
			}
			npc.ai[1]++;
			if(npc.ai[1] >= 210 && npc.ai[1] <= 470)
			{
				npc.rotation++;
				npc.velocity.Y -= 0.26f;
				npc.velocity.X *= 0.99f;
			}
			if(npc.ai[1] >= 471)
			{
				npc.velocity.Y += 8f;
				npc.ai[1] = -120;
				npc.rotation = 0;
				boom = 1;
			}
			if(npc.velocity.Y == 0 && boom == 1)
			{
				boom = 0;
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -9, mod.ProjectileType("MightWater"), 83, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 3, -8, mod.ProjectileType("MightWater"), 83, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -3, -8, mod.ProjectileType("MightWater"), 83, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -4, -2, mod.ProjectileType("MightEssence2"), 83, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 4,  -2, mod.ProjectileType("MightEssence2"), 83, 1, 0);
				
			}
			
			
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			//	player.AddBuff(mod.BuffType("BloodTapped"), 900);
		}
		
			
			
			
}
	}
