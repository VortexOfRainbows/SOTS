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
using Terraria.ModLoader.IO;
using System.IO;
using Steamworks;
using System;
using SOTS.Void;

namespace SOTS.NPCs.AbandonedVillage
{
    public class RotWalker : ModNPC
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(NPC.localAI[3]);
            writer.Write(Buried);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            NPC.localAI[3] = reader.ReadSingle();
            Buried = reader.ReadBoolean();
        }
        public const int AnimSpeed = 4;
		private static Asset<Texture2D> NPCTexture;
        public bool Buried = true;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 15;
        }
        public override void SetDefaults()
		{
            NPC.lifeMax = 60;
            NPC.damage = 35;
            NPC.defense = 8;
            //58 by 60 is the frame size, but the hitbox should be closer to normal fighter
            NPC.width = 28; //Has to be smaller than the sprite size to allow jumping over blocks properly
            NPC.height = 44; //Has to be shorter than the sprite height to prevent falling through platforms erroneously 
            NPC.npcSlots = 1f;
			NPC.knockBackResist = 0.35f;
            NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;
            NPC.aiStyle = NPCAIStyleID.Fighter;
            NPC.value = Item.buyPrice(0, 0, 3, 0);
            NPC.dontTakeDamage = true;
            NPC.behindTiles = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<RotWalkerBanner>();
        }
        public override void ModifyHoverBoundingBox(ref Rectangle boundingBox)
        {
            if(Buried)
            {
                boundingBox.X = boundingBox.Y = 0; //place it out of bounds so it is basically unhoverable when buried
            }
        }
        public bool InAttackFrames => NPC.localAI[3] < -3 * AnimSpeed;
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return InAttackFrames;
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            if(InAttackFrames)
            {
                npcHitbox.X -= 24;
                npcHitbox.Width += 48;
                npcHitbox.Y -= 24;
                npcHitbox.Height += 24;
            }
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPCTexture ??= ModContent.Request<Texture2D>(Texture);
            var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            int textureHeight = NPCTexture.Height() / Main.npcFrameCount[NPC.type];
            Vector2 origin = new Vector2(NPCTexture.Value.Width / 2, textureHeight / 2 + 8);
            bool isBestiary = Main.screenPosition != screenPos;
            if (isBestiary)
                drawColor = Color.White;
            if (Buried && !isBestiary)
            {
                Rectangle frame = NPC.frame;
                float buryPercent = MathHelper.Clamp(1 - (NPC.localAI[3] - 105) / 15f, 0, 1);
                int amt = (int)(36 * buryPercent * buryPercent);
                frame.Height -= amt;
			    Main.EntitySpriteDraw(NPCTexture.Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY + 2 + amt), frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, origin, NPC.scale, effects, 0);
                return false;
            }
            if(NPC.frame.Y >= 10 * textureHeight) //Attack animation will have different origin
            {
                origin.X -= 8 * NPC.spriteDirection;
            }
			Main.EntitySpriteDraw(NPCTexture.Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY + 2), NPC.frame, NPC.GetNPCColorTintedByBuffs(drawColor), NPC.rotation, origin, NPC.scale, effects, 0);
			return false;
		}
        public override void FindFrame(int frameHeight)
        {
            //walking animation
            if (Buried)
                return;
            NPC.frameCounter++;
            if (NPC.frameCounter > AnimSpeed)
            {
                NPC.frame.Y = NPC.frame.Y + frameHeight;
                NPC.frameCounter = 0;
            }
            if (NPC.localAI[3] < 0)
            {
                if (NPC.frame.Y >= frameHeight * 15)
                {
                    NPC.frame.Y = 0;
                }
                else if(NPC.frame.Y < frameHeight * 10)
                {
                    NPC.frameCounter = 0;
                    NPC.frame.Y = frameHeight * 10;
                }
            }
            else if (NPC.velocity.Y > 0 || NPC.velocity.Y < 0 || NPC.localAI[0] > 0)
            {
                NPC.frame.Y = 12 * frameHeight;
            }
            else if (NPC.frame.Y >= frameHeight * 10) // Walk animation goes up to 10
            {
                NPC.frame.Y = 0;
            }

            //frame when falling/jumping
        }
        private bool runOnce = true;
        public override bool PreAI()
        {
            NPC.TargetClosest(runOnce || !Buried);
            runOnce = false;
            NPC.spriteDirection = NPC.direction;
            Vector2 toPlayer = Main.player[NPC.target].Center - NPC.Center;
            if (Buried)
            {
                NPC.behindTiles = true;
                bool growAnim = NPC.localAI[3] >= 105;
                if (toPlayer.Length() < 160 || growAnim)
                {
                    NPC.localAI[3]++;
                    if (growAnim)
                    {
                        if((int)NPC.localAI[3] == 106)
                            SOTSUtils.PlaySound(SoundID.DoubleJump, NPC.Center, 1.5f, -0.5f);
                        for(int i = 0; i < 3; ++i)
                        {
                            Dust d = Dust.NewDustDirect(NPC.position + new Vector2(-4, NPC.height - 4), NPC.width, 0, ModContent.DustType<SootDust>(), Main.rand.NextFloat(-2, 2), -3.5f, 0, default, 1.2f);
                            d.velocity.Y -= 2.5f;
                        }
                    }
                    if (NPC.localAI[3] > 120)
                    {
                        NPC.netUpdate = true;
                        NPC.localAI[3] = 0;
                        NPC.velocity.Y -= 5.5f;
                        Buried = false;
                    }
                }
                else if (!growAnim)
                {
                    NPC.localAI[3] = 0;
                }
                return false;
            }
            return base.PreAI();
        }
        public override void AI()
        {
            NPC.behindTiles = false;
            NPC.dontTakeDamage = false;
            if (NPC.velocity.Y == 0)
                NPC.velocity.X *= 1.05f; //speed up when grounded

            Vector2 toPlayer = Main.player[NPC.target].Center - NPC.Center;
            if (NPC.localAI[3] < 0 || toPlayer.Length() < 48)
            {
                if (NPC.localAI[3] % 5 == 0 && Main.netMode == NetmodeID.Server) 
                    NPC.netUpdate = true;
                if (NPC.localAI[3] == -2 * AnimSpeed)
                {
                    SOTSUtils.PlaySound(SoundID.Item1, NPC.Center, 1.0f, 0.3f);
                }
                NPC.localAI[3]--;
                if (NPC.localAI[3] < 0)
                {
                    NPC.velocity.X *= 0.5f;
                    if (NPC.localAI[3] <= -5 * AnimSpeed)
                    {
                        NPC.localAI[3] = 30;
                    }
                }
            }
            else
                NPC.localAI[3] = 30;
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life > 0)
            {
                for (int num = 0; num < hit.Damage / (float)NPC.lifeMax * 40f; num++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.NextBool(2) ? DustID.Bone : ModContent.DustType<SootDust>(), (float)(2.0f * hit.HitDirection), -1.4f, 0, default, 1.2f);
            }
            else
            {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, Main.rand.NextBool(2) ? DustID.Bone : ModContent.DustType<SootDust>(), (float)(2.1f * hit.HitDirection), -1.4f, 0, default, 1.55f);
                string dir = "Gores/RotWalker/RotWalkerGore";
                Vector2 velo = new(NPC.velocity.X * 0.5f + hit.HitDirection, NPC.velocity.Y * 0.2f - 1);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(22, -14), velo, ModGores.GoreType($"{dir}1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(4, 4), velo, ModGores.GoreType($"{dir}2"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(40, 8), velo, ModGores.GoreType($"{dir}3"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(26, 10), velo, ModGores.GoreType($"{dir}4"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(24, 22), velo, ModGores.GoreType($"{dir}5"), 1f);
            }
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 2));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<OldKey>(), 50));
            npcLoot.Add(ItemDropRule.Common(ItemID.WormFood, 50));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PintOPunch>(), 200));
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            VoidPlayer.VoidBurn(SOTS.Instance, target, 3, 600);
        }
    }
}