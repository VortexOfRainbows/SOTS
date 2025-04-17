using Microsoft.Xna.Framework;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.AbandonedVillage
{
    public class PupaFly : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 6;
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 25;
            NPC.damage = 20;
            NPC.defense = 6;
            NPC.width = 46;
            NPC.height = 46;
            NPC.npcSlots = 1f;
            NPC.knockBackResist = 0.5f;
            NPC.value = Item.buyPrice(0, 0, 1, 0);
            NPC.noGravity = true;
            NPC.noTileCollide = false;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
            NPC.aiStyle = 14;
            AIType = NPCID.Raven;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PupaFlyBanner>();
        }
        public override void FindFrame(int frameHeight)
        {
            //flying animation
            NPC.frameCounter++;
            if (NPC.frameCounter > 3)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 6)
            {
                NPC.frame.Y = 0 * frameHeight;
            }
        }

        public override void AI()
		{
            NPC.spriteDirection = NPC.velocity.X < 0 ? -1 : 1;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            int DustType = ModContent.DustType<FamishedDustCrimson>();
            if (NPC.life > 0)
            {
                for (int num = 0; num < hit.Damage / (float)NPC.lifeMax * 20f; num++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.0f * hit.HitDirection), -1.4f, 0, default, 1.2f);
            }
            else
            {
                for (int k = 0; k < 12; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.1f * hit.HitDirection), -1.4f, 0, default, 1.55f);
                string dir = "Gores/Pupa/PupaFlyGore";
                Vector2 velo = new(NPC.velocity.X * 0.5f + hit.HitDirection, NPC.velocity.Y * 0.2f - 1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(12, 12), velo, ModGores.GoreType($"{dir}1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(6, 0), velo, ModGores.GoreType($"{dir}2"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(6, 14), velo, ModGores.GoreType($"{dir}3"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(26, 0), velo, ModGores.GoreType($"{dir}4"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(26, 14), velo, ModGores.GoreType($"{dir}5"), 1f);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 200));
        }
    }
}