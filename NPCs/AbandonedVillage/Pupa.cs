using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Audio;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using Terraria.GameContent.ItemDropRules;
using SOTS.Items.Banners;

namespace SOTS.NPCs.AbandonedVillage
{
    public class Pupa : ModNPC
    {
		private static Asset<Texture2D> NPCTexture;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
		{
            NPC.lifeMax = 100;
            NPC.damage = 20;
            NPC.defense = 10;
            NPC.width = 38;
			NPC.height = 58;
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.15f;
            NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.Item171;
            NPC.aiStyle = 3;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
            AIType = NPCID.Crab;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PupaBanner>();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			NPCTexture ??= ModContent.Request<Texture2D>(Texture);
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Main.EntitySpriteDraw(NPCTexture.Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY + 2), NPC.frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
        public override void FindFrame(int frameHeight)
        {
            //walking animation
            NPC.frameCounter++;
            if (NPC.frameCounter > 9)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.frame.Y >= frameHeight * 5)
            {
                NPC.frame.Y = 0 * frameHeight;
            }

            //frame when falling/jumping
            if (NPC.velocity.Y > 0 || NPC.velocity.Y < 0 || NPC.localAI[0] > 0)
            {
                NPC.frame.Y = 2 * frameHeight;
            }
        }
        public override void AI()
		{
            NPC.TargetClosest(true);
            NPC.spriteDirection = NPC.direction;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            int DustType = ModContent.DustType<FamishedDustCrimson>();
            if (NPC.life > 0)
            {
                for (int num = 0; num < hit.Damage / (float)NPC.lifeMax * 40f; num++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.0f * hit.HitDirection), -1.4f, 0, default, 1.2f);
            }
            else
            {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.1f * hit.HitDirection), -1.4f, 0, default, 1.55f);
                string dir = "Gores/Pupa/PupaGore";
                Vector2 velo = new(NPC.velocity.X * 0.5f + hit.HitDirection, NPC.velocity.Y * 0.2f - 1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 14), velo, ModGores.GoreType($"{dir}3"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(10, 14), velo, ModGores.GoreType($"{dir}4"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(8, 42), velo, ModGores.GoreType($"{dir}1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(22, 42), velo, ModGores.GoreType($"{dir}2"), 1f);
            }
        }
        public override void OnKill()
        {
            SOTSUtils.PlaySound(SoundID.DD2_ExplosiveTrapExplode, NPC.Center, 1.0f, 0.5f);
            int Fly = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PupaFly>());
            Main.npc[Fly].velocity.X = Main.rand.NextFloat(-1f, 1f);
            Main.npc[Fly].velocity.Y = Main.rand.NextFloat(-9f, -3f);
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 50));
            npcLoot.Add(ItemDropRule.Common(ItemID.BloodySpine, 50));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PintOPunch>(), 200));
        }
    }
}