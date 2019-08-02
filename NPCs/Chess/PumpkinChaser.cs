using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Chess
{
	public class PumpkinChaser: ModNPC
	{	int restrictor = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Pumpkin Chaser");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 86;  //5 is the flying AI
            npc.lifeMax = 1000;   //boss life
            npc.damage = 1;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 26;
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
			npc.rotation++;
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
		player.AddBuff(mod.BuffType("BloodTapped"), 300);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, -5, 0, 326, 42, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 5, 0, 326, 42, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, 5, 326, 42, 1, 0);
				Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0, -5, 326, 42, 1, 0);
		
		}
	}
}





















