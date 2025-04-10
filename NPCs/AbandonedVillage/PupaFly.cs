using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
            NPC.lifeMax = 75;
            NPC.damage = 30;
            NPC.defense = 0;
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
            if (NPC.life <= 0) 
            {
                //gores here
            }
        }
    }
}