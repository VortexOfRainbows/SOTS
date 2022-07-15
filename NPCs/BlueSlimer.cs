using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class BlueSlimer : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blue Slimer");
		}
		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.Slimer);
            NPC.aiStyle = 44;
            NPC.lifeMax = 20;
            NPC.damage = 8; 
            NPC.defense = 0; 
            NPC.knockBackResist = 0.45f;
            NPC.width = 92;
            NPC.height = 48;
            AnimationType = NPCID.Slimer;  
			Main.npcFrameCount[NPC.type] = 4;  
            NPC.value = 15;
            NPC.npcSlots = 0.85f;
            NPC.noGravity = true;
			NPC.alpha = 90;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ItemType<BlueSlimerBanner>();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 110.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)hitDirection, -1f, NPC.alpha, new Color(0, 80, 255, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 60; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)(2 * hitDirection), -2f, NPC.alpha, new Color(0, 80, 255, 100), 1f);
				}
				/*
				int num1 = Main.rand.Next(4);
				if(num1 != 0)
					Gore.NewGore(npc.position, npc.velocity, ModGores.GoreType("Gores/BlueSlimerGore1"), 1f);
				if(num1 != 1)
					Gore.NewGore(npc.position, npc.velocity, ModGores.GoreType("Gores/BlueSlimerGore2"), 1f);
				if(num1 != 2)
					Gore.NewGore(npc.position, npc.velocity, ModGores.GoreType("Gores/BlueSlimerGore3"), 1f);
				if(num1 != 3)
					Gore.NewGore(npc.position, npc.velocity, ModGores.GoreType("Gores/BlueSlimerGore4"), 1f); */
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
			npcLoot.Add(ItemDropRule.Common(ItemID.Feather, 5, 1, 2));
		}
        public override void OnKill()
		{
			NPC npc = NPC.NewNPCDirect(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, NPCID.BlueSlime); //this should sync it in multiplayer
			npc.netUpdate = true;
		}
    }
}