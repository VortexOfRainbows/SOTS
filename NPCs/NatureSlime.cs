using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Void;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class NatureSlime : ModNPC
	{	int initiateSize = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flowering Slime");
		}
		public override void SetDefaults()
		{
			//npc.CloneDefaults(NPCID.BlackSlime);
			npc.aiStyle = 1;
            npc.lifeMax = 30;  
            npc.damage = 12; 
            npc.defense = 3;  
            npc.knockBackResist = 1f;
            npc.width = 38;
            npc.height = 38;
            animationType = NPCID.BlueSlime;
			Main.npcFrameCount[npc.type] = 2;  
            npc.value = 100;
            npc.npcSlots = .4f;
			npc.alpha = 75;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
			banner = npc.type;
			bannerItem = ItemType<NatureSlimeBanner>();
		}
		public override bool PreAI()
		{
			npc.TargetClosest(true);
			if(initiateSize == 1)
			{
				initiateSize = -1;
				npc.scale = 1.1f; //why??
				npc.width = (int)(npc.width * npc.scale);
				npc.height = (int)(npc.height * npc.scale);
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
				if (pet.type == NPCType<BloomingHook>() && (int)pet.ai[0] == npc.whoAmI && pet.active)
				{
					total++;
				}
			}
			if (Main.netMode != 1 && counter >= 150 * (1 + total)) 
			{
				counter = 0;
				if (total < 3)
				{
					int npc1 = NPC.NewNPC((int)npc.position.X + npc.width / 2, (int)npc.position.Y + npc.height, NPCType<BloomingHook>());
					Main.npc[npc1].netUpdate = true;
					Main.npc[npc1].ai[0] = npc.whoAmI;
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 100.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, new Color(102, 202, 71, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 50; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(102, 202, 71, 100), 1f);
				}
			}
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FoulConcoction>(), Main.rand.Next(2) + 1);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemType<FragmentOfNature>(), 1);
		}
	}
}