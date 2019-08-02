using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Chess
{
	public class SightEssence: ModNPC
	{	int restrictor = 0;
		public override void SetStaticDefaults()
		{
			
			DisplayName.SetDefault("Sight Spirit");
		}
		public override void SetDefaults()
		{
			
            npc.aiStyle = 86;  //5 is the flying AI
            npc.lifeMax = 25;   //boss life
            npc.damage = 45;  //boss damage
            npc.defense = 0;    //boss defense
            npc.knockBackResist = 0f;
            npc.width = 16;
            npc.height = 16;
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
				Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), 16, 16, 75);
			
		npc.rotation += 0.3f;
		}
	
	}
}





















