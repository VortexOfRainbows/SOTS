using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using Terraria;
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
			npc.CloneDefaults(NPCID.Slimer);
            NPC.aiStyle =44;
            NPC.lifeMax = 20;
            NPC.damage = 8; 
            NPC.defense = 0; 
            NPC.knockBackResist = 0.45f;
            NPC.width = 92;
            NPC.height = 48;
            animationType = NPCID.Slimer;  
			Main.npcFrameCount[NPC.type] = 4;  
            npc.value = 15;
            npc.npcSlots = 0.85f;
            npc.noGravity = true;
			npc.alpha = 90;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ItemType<BlueSlimerBanner>();
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)npc.lifeMax * 110.0)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)hitDirection, -1f, npc.alpha, new Color(0, 80, 255, 100), 1f);
					num++;
				}
			}
			else
			{
				for (int k = 0; k < 60; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, 4, (float)(2 * hitDirection), -2f, npc.alpha, new Color(0, 80, 255, 100), 1f);
				}
				/*
				int num1 = Main.rand.Next(4);
				if(num1 != 0)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore1"), 1f);
				if(num1 != 1)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore2"), 1f);
				if(num1 != 2)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore3"), 1f);
				if(num1 != 3)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/BlueSlimerGore4"), 1f); */
			}
		}
		public override void NPCLoot()
		{
			if (Main.expertMode)
			{
				NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.BlueSlime);
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Gel), Main.rand.Next(3) + 1);
			if(Main.rand.Next(5) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (ItemID.Feather), Main.rand.Next(2) + 1);	
			}
		}	
	}
}