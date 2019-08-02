using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SOTS.NPCs.Planetarium
{
	public class PlanetariumGaurdian: ModNPC
	{	int crack = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Planetarium Spirit");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 14;  //5 is the flying AI
            npc.lifeMax = 600;   //boss life
            npc.damage = 21;  //boss damage
            npc.defense = 7;    //boss defense
            npc.knockBackResist = 0.5f;
            npc.width = 84;
            npc.height = 88;
            animationType = NPCID.CaveBat;   //this boss will behavior like the DemonEye
            Main.npcFrameCount[npc.type] = 8;    //boss frame/animation
            npc.value = 7500;
            npc.npcSlots = 0f;
            npc.boss = false;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit3;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.buffImmune[24] = true;
            npc.netAlways = false;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/JourneyFromJar");
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{ 
			
			
         //   {
            //return spawnInfo.spawnTileY < Main.rockLayer && Main.player[Main.myPlayer].GetModPlayer<SOTSPlayer>().PlanetariumBiome ? 100f : 0f;
			
            
			
			
				return SpawnCondition.Sky.Chance * 0.1f;
			
		//	}
		//	else
		//	{
		//	return spawnInfo.spawnTileY < Main.rockLayer ? 0f : 0f;
		//	}
			//if(Main.tile[(spawnInfo.spawnTileX), (spawnInfo.spawnTileY)].type == mod.TileType("CustomTileBlock") ) 
           // {
            //return spawnInfo.spawnTileY < Main.rockLayer ? 500f : 0f;
			//}
			//else
		//	{
			//return spawnInfo.spawnTileY < Main.rockLayer ? 0f : 0f;
		//	}

			
		}
		public override void AI()
		{
			
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 84, 88, 160);
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 84, 88, 160);
			npc.ai[0]++;
			if (npc.ai[0] > 180)
			 {
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 0, 5, 682, 33, 0, Main.myPlayer, 0.0f, 1);
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 0, -5, 682, 33, 0, Main.myPlayer, 0.0f, 1);
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 5, 0, 682, 33, 0, Main.myPlayer, 0.0f, 1);
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, -5, 0, 682, 33, 0, Main.myPlayer, 0.0f, 1);
	npc.ai[0] = 0;
		}
		}
		public override void NPCLoot()
		{
			   Player player  = Main.player[npc.target];
			
                Projectile.NewProjectile(npc.Center.X,  npc.Center.Y, 0, 0, mod.ProjectileType("PlanetariumGrassCrack"), 27, 0, Main.myPlayer, 0.0f, 1);
					
									 	
			
	}

	}
	}
	
