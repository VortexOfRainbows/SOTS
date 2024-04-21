using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs
{[AutoloadBossHead]
	public class knuckles : ModNPC
	{
		public override void SetStaticDefaults()
		{
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =14;  
            NPC.lifeMax = 69696969;
            NPC.damage = 420;
            NPC.defense = 420;
            NPC.knockBackResist = 0f;
            NPC.width = 156;
            NPC.height = 102;
            NPC.value = 420;
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/KnucklesTheme");
			//musicPriority = MusicPriority.BossHigh;
		}
        public override void AI()
		{	
			NPC.position.Y += Main.rand.Next(-5, 6);
			NPC.position.X += Main.rand.Next(-5, 6);
			NPC.velocity += Main.rand.NextVector2Circular(0.1f, 0.1f);

			NPC.ai[0]++;
			if (NPC.ai[0] == 60)
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("WHY ARE YOU RUNNING????", 0, 255, 0);
			if(NPC.ai[0] == 120)
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("DO YOU KNOW DA WAE???", 0, 255, 0);
			if(NPC.ai[0] == 180)
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("YOU DO NOT KNOW DA WAE!", 0, 255, 0);
			if(NPC.ai[0] >= 240)
			{
				if (Main.netMode != NetmodeID.Server)
					Main.NewText("LET US SHOW YOU DA WAE!!!!!!!!!!!!!!!!!!!!!!", 0, 255, 0);
				NPC.SpawnOnPlayer(0, NPC.type);
				NPC.ai[0] = 0;
			}
			NPC.rotation += Main.rand.NextFloat(-100, 100);
			if (Main.player[NPC.target].dead)
			{
			   NPC.timeLeft = 0;
			}
			else
			   NPC.timeLeft = 10000;
		}
	
	}
}





















