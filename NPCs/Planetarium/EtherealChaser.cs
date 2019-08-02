using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Planetarium
{
	public class EtherealChaser : ModNPC
	{	int restrictor = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Ethereal Chaser");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 86;  //5 is the flying AI
            npc.lifeMax = 100;   //boss life
            npc.damage = 35;  //boss damage
            npc.defense = 3;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 28;
            npc.height = 28;
            animationType = NPCID.SkeletronHead;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 1;    //boss frame/animation
            npc.value = 0;
            npc.npcSlots = 1f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath7;
            npc.netAlways = true;
		}
		public override void AI()
		{	
		 Player player  = Main.player[npc.target];
			
			
			if(Main.rand.Next(2) == 0)
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 28, 28, 235);
			
			npc.rotation += 0.3f;
			
			restrictor++;
			if(restrictor > 120)
			{
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -2, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 2, 0, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -2, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 2, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -1, -1, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 1, -1, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -1, 1, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 1, 1, mod.ProjectileType("EtherealTrail2"), (int)(npc.damage * 0.25f), 0, 0);
				restrictor = 0;
			}
		}
	
	}
}





















