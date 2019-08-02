using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.Items.Planetarium;


namespace SOTS.NPCs.Planetarium
{
	public class PlanetariumSpirit : ModNPC
	{	int crack = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Planetarium Spirit");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 300;   //boss life
            npc.damage = 22;  //boss damage
            npc.defense = 7;    //boss defense
            npc.knockBackResist = 0.5f;
            npc.width = 34;
            npc.height = 54;
            animationType = NPCID.CaveBat;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 8;    //boss frame/animation
            npc.value = 7500;
            npc.npcSlots = 0.025f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{ 
			
				return SpawnCondition.Sky.Chance * 0.1f;
				
			
		}
		public override void AI()
		{
			
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 34, 54, 160);
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 34, 54, 160);
			npc.ai[0]++;
			if (npc.ai[0] > 60)
			 {
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 0, 0, mod.ProjectileType("PlanetariumBolt"), 27, 0, Main.myPlayer, 0.0f, 1);
	npc.ai[0] = 0;
	
		}
		}
		public override void NPCLoot()
		{
               Player player  = Main.player[npc.target];
			
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 0, 0, mod.ProjectileType("PlanetariumCrack"), 27, 0, Main.myPlayer, 0.0f, 1);
					
								 	
			
		
		
		}
	}
}