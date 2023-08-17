using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class NatureSlime : ModNPC
	{	int initiateSize = 1;
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			NPC.aiStyle =1;
            NPC.lifeMax = 30;  
            NPC.damage = 12; 
            NPC.defense = 3;  
            NPC.knockBackResist = 1f;
            NPC.width = 38;
            NPC.height = 38;
            AnimationType = NPCID.BlueSlime;
			Main.npcFrameCount[NPC.type] = 2;  
            NPC.value = 100;
            NPC.npcSlots = .4f;
			NPC.alpha = 75;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ItemType<NatureSlimeBanner>();
		}
		public override bool PreAI()
		{
			NPC.TargetClosest(true);
			if(initiateSize == 1)
			{
				initiateSize = -1;
				NPC.scale = 1.1f; //why??
				NPC.width = (int)(NPC.width * NPC.scale);
				NPC.height = (int)(NPC.height * NPC.scale);
			}
			return true;
		}
		int counter = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(counter);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			counter = reader.ReadInt32();
		}
		public override void AI()
		{
			counter++;
			int total = 0;
			for(int i = 0; i < Main.maxNPCs; i++)
			{
				NPC pet = Main.npc[i];
				if (pet.type == NPCType<BloomingHook>() && (int)pet.ai[0] == NPC.whoAmI && pet.active)
				{
					total++;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient && counter >= 150 * (1 + total)) 
			{
				counter = 0;
				if (total < 3)
				{
					int npc1 = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X + NPC.width / 2, (int)NPC.position.Y + NPC.height, NPCType<BloomingHook>());
					Main.npc[npc1].netUpdate = true;
					Main.npc[npc1].ai[0] = NPC.whoAmI;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < hit.Damage / (double)NPC.lifeMax * 100.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)hit.HitDirection, -1f, NPC.alpha, new Color(102, 202, 71, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, (float)(2 * hit.HitDirection), -2f, NPC.alpha, new Color(102, 202, 71, 100), 1f);
				}
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
			npcLoot.Add(ItemDropRule.Common(ItemID.Gel, 1, 1, 3));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfNature>(), 1, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemType<FoulConcoction>(), 5, 1, 1));
		}
	}
}