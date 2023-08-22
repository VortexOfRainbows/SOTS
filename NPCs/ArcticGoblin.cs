using Microsoft.Xna.Framework;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs
{
	public class ArcticGoblin : ModNPC
	{
		public override void SetDefaults()
		{
			NPC.CloneDefaults(NPCID.GoblinPeon);
			AIType = NPCID.GoblinScout;
			NPC.lifeMax = 100;
			NPC.damage = 24;
			AnimationType = NPCID.GoblinPeon;
			Main.npcFrameCount[NPC.type] = 16;
			Banner = NPC.type;
			BannerItem = ItemType<ArcticGoblinBanner>();
		}
		public override void AI()
		{
			NPC.TargetClosest(true);
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (var i = 0; i < 30; ++i)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silt, 2.5f * (float)hit.HitDirection, -2.5f, 0, new Color(), 1f);
				}
				NPC.DeathGore("Gores/GoblinEskimoGore1");
				NPC.DeathGore("Gores/GoblinEskimoGore2");
				NPC.DeathGore("Gores/GoblinEskimoGore2");
				NPC.DeathGore("Gores/GoblinEskimoGore3");
				NPC.DeathGore("Gores/GoblinEskimoGore3");
			}
			else
			{
				for (int i = 0; i < hit.Damage / (float)NPC.lifeMax * 50.0; i++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silt, (float)hit.HitDirection, -1f, 0, new Color(), 0.8f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.TatteredCloth, 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfPermafrost>(), 1, 1, 2));
			npcLoot.Add(ItemDropRule.Common(ItemID.SpikyBall, 2, 1, 15));
			npcLoot.Add(ItemDropRule.Common(ItemType<AncientSteelBar>(), 1, 1, 1));
			npcLoot.Add(ItemDropRule.NormalvsExpert(ItemType<StrawberryIcecream>(), 2, 1));
		}
	}
}