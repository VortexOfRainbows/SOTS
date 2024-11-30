using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.UI.ResourceSets;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.AbandonedVillage
{
	public class Fistfull : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(fistPosition);
            writer.WriteVector2(fistVelo);
            writer.Write(NPC.localAI[3]);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            fistPosition = reader.ReadVector2();
            fistVelo = reader.ReadVector2();
            NPC.localAI[3] = reader.ReadSingle();
        }
        private Vector2 fistPosition;
        private Vector2 fistVelo;
        private Vector2 WormTrailStartPos => NPC.Center;
        private List<Vector2> segments = new List<Vector2>();
        private bool SegmentsNearCenter = false;
        private void UpdateSegments()
        {
            while (segments.Count < 15)
            {
                segments.Add(WormTrailStartPos + new Vector2(1 * NPC.direction, 0.1f) * segments.Count);
            }

            SegmentsNearCenter = true;
            List<Vector2> temp = new List<Vector2>();
            for (int i = 0; i < segments.Count; i++)
                temp.Add(new Vector2(segments[i].X, segments[i].Y));
            Vector2 prev = WormTrailStartPos;
            segments[0] = NPC.Center;
            float Next = NPC.localAI[3] < 280 ? 0.5f : 0.36f;
            float Prev = NPC.localAI[3] < 280 ? 0.5f : 0.64f;
            for (int i = 1; i < segments.Count; i++)
            {
                if (NPC.localAI[3] < 60)
                {
                    segments[i] = NPC.Center;
                    continue;
                }
                Vector2 next = i >= segments.Count - 1 ? fistPosition : temp[i + 1];
                Vector2 toNext = next - temp[i];
                Vector2 toPrev = prev - temp[i];
                segments[i] += toNext * Next + toPrev * Prev;
                prev = temp[i];
                if(SegmentsNearCenter && segments[i].Distance(NPC.Center) > 6)
                {
                    SegmentsNearCenter = false;
                }
            }
        }
        private void DrawSegments(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            Texture2D chaine = Request<Texture2D>(Texture + "Chain").Value;
            Vector2 origin = new Vector2(0, chaine.Height / 2);
            Vector2 prev = fistPosition;
            for (int i = segments.Count - 1; i >= 0; i--)
            {
                float fromCenter = 1 - Math.Min(1, segments[i].Distance(NPC.Center) / 40f);
                float percent = 1f - (float)i / segments.Count;
                Vector2 toPrev = prev - segments[i];
                Vector2 position = segments[i] - screenPos;
                float r = toPrev.ToRotation();
                float Dist = toPrev.Length();
                int x = (int)segments[i].X / 16;
                int y = (int)segments[i].Y / 16;
                spriteBatch.Draw(chaine, position, null, Lighting.GetColor(x, y, Color.Lerp(Color.White, Color.Black, percent * percent * percent + 0.5f * fromCenter)), r, origin, 
                    new Vector2((Dist + 2) / chaine.Width, 1.0f - MathHelper.Clamp(Dist / chaine.Width / 1.5f - 1.5f, 0, .2f) - 0.3f * percent - fromCenter * 0.2f), 
                    toPrev.X < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                prev = segments[i];
            }
        }
        private static int DustType => Main.rand.NextBool() ? DustType<FamishedDustCorruption>() : DustType<FamishedDustCrimson>();
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 5;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = 3;
            NPC.width = 24; //Has to be smaller than the sprite size to allow jumping over blocks properly
            NPC.height = 40; //Has to be shorter than the sprite height to prevent falling through platforms erroneously 
            //Very similar stats to face monster, but with extra defense (2) and life (5), but less damage (-3)
            NPC.damage = 22;
            NPC.lifeMax = 75;
			NPC.defense = 12;
            NPC.knockBackResist = 0.4f;
            NPC.value = Item.buyPrice(0, 0, 2, 50);
			NPC.scale = 1.0f;
			NPC.HitSound = SoundID.NPCHit19;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.noTileCollide = false;
			Banner = NPC.type;
			BannerItem = ItemType<FistfullBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureF = Request<Texture2D>(this.Texture + "Hand").Value;
            Texture2D textureIdle = Request<Texture2D>(this.Texture + "Punch").Value;
			int height = texture.Height / Main.npcFrameCount[NPC.type];
            Vector2 drawOrigin = new Vector2(texture.Width / 2, height / 2 + 8);
			Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY);
			Rectangle frame = new Rectangle(0, NPC.frame.Y, texture.Width, height);
            if (fistPosition != NPC.Center && !runOnce && NPC.localAI[3] > 60)
            {
                float fromCenter = Math.Min(1, fistPosition.Distance(NPC.Center) / 40f);
                spriteBatch.Draw(textureIdle, drawPos, null, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
                Player player = Main.player[NPC.target];
                Vector2 toPlayer = player.Center - fistPosition;
                float scale = 0.5f + 0.4f * fromCenter;
                DrawSegments(spriteBatch, screenPos);
                spriteBatch.Draw(textureF, fistPosition - screenPos, null, Lighting.GetColor((int)fistPosition.X / 16, (int)fistPosition.Y / 16, Color.Lerp(Color.Black, Color.White, 0.5f + 0.5f * fromCenter))
                    , toPlayer.ToRotation() + MathF.PI / 2f, textureF.Size() / 2, NPC.scale * scale, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            else
            {
			    spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale, NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            }
            //texture = GetTexture("SOTS/NPCs/TeratomaGlow");
            //spriteBatch.Draw(texture, drawPos, frame, Color.White, npc.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
		}
        private bool runOnce = true;
        public override bool PreAI()
        {
            UpdateSegments();
            NPC.TargetClosest(true);
            Player player = Main.player[NPC.target];
            if (runOnce)
            {
                fistPosition = NPC.Center;
                NPC.netUpdate = true;
                runOnce = false;
            }
            fistPosition += fistVelo + NPC.velocity * 0.5f;
            if((Collision.CanHitLine(NPC.Center, 0, 0, player.position, player.width, player.height) && NPC.Distance(player.Center) < 256) || NPC.localAI[3] > 60) //Fistfull can only hit the player from about 16 blocks = 256 units away
                NPC.localAI[3]++;
            else if (NPC.localAI[3] > 0)
            {
                NPC.localAI[3]--;
            }
            if (NPC.localAI[3] > 60)
            {
                if ((int)NPC.localAI[3] == 61)
                {
				    SOTSUtils.PlaySound(SoundID.NPCDeath1, NPC.Center, 0.856f, -0.4f);
                }
                float speedM = MathF.Min(1, (NPC.localAI[3] - 60f) / 30f);
                if(NPC.velocity.Y < 0)
                    NPC.velocity.Y *= 0.0f;
                NPC.velocity.X *= 0.01f;
                Vector2 toPlayer = player.Center - fistPosition;
                fistVelo *= 0.9325f;
                fistVelo += toPlayer.SNormalize() * 0.21f * speedM;
                Vector2 toNPC = NPC.Center - fistPosition;
                fistPosition = Vector2.Lerp(fistPosition, NPC.Center, 0.012f);
                fistVelo += toNPC * 0.00002f * NPC.localAI[3] / 150f;
                if (NPC.localAI[3] < 280)
                {
                    if (NPC.localAI[3] % 60 == 0)
                    {
                        SOTSUtils.PlaySound(SoundID.Item175, fistPosition, 0.6f, -0.55f);
                        fistVelo += toPlayer * 0.02f + toPlayer.SNormalize() * 8.75f;
                    }
                    if (NPC.localAI[3] > 90 && NPC.localAI[3] % 60 > 30)
                    {
                        speedM = MathF.Sin(NPC.localAI[3] % 60 / 60f * MathF.PI);
                        speedM *= speedM;
                        fistVelo += toNPC * 0.0035f * speedM + toNPC.SNormalize() * 0.15f * speedM;
                    }
                }
                else
                {
                    fistPosition = Vector2.Lerp(fistPosition, NPC.Center, (NPC.localAI[3] - 280) / 150f);
                    if(fistPosition.Distance(NPC.Center) < 6 && SegmentsNearCenter)
                    {
                        NPC.localAI[3] = -Main.rand.Next(60, 180);
                        NPC.netUpdate = true;
                    }
                }
            }
            else
            {
                fistPosition = NPC.Center;
            }
            return base.PreAI();
        }
        public override void AI()
		{
			if (NPC.velocity.Y == 0 && Math.Abs(NPC.velocity.X) > 0.5f && !Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(NPC.position + new Vector2(0, (NPC.height - 2) * NPC.scale) - new Vector2(5), (int)(NPC.width * NPC.scale), 4, DustType, 0, 0, 0, default, 0.8f);
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				NPC.velocity.X *= 0.97125f;
			}
            NPC.spriteDirection = NPC.direction;
		}
		public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter += 0.5f + MathF.Sqrt(MathF.Abs(NPC.velocity.X * 0.5f));
            if (NPC.frameCounter >= 8f)
            {
                NPC.frameCounter -= 8f;
                NPC.frame.Y += frameHeight;
                if (NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
                {
                    NPC.frame.Y = 1 * frameHeight;
                }
            }
        }
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
            {
                for (int num = 0; num < hit.Damage / NPC.lifeMax * 40f; num++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.0f * hit.HitDirection), -1.4f, 0, default, 1.5f);
            }
			else
            {
                for (int k = 0; k < 20; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustType, (float)(2.1f * hit.HitDirection), -1.4f, 0, default, 1.55f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position - new Vector2(0, 20), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, 4), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore2"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(6, 24), NPC.velocity, ModGores.GoreType("Gores/Fistfull/FistfullGore3"), 1f);
                if (NPC.localAI[3] > 60)
                {
                    for(int i = 0; i < segments.Count; i++)
                    {
                        for (int k = 0; k < 4; k++)
                        {
                            Dust d = Dust.NewDustDirect(segments[i] - new Vector2(4) - new Vector2(5, 5), 10, 10, DustType, (float)(1.0f * hit.HitDirection), -1.0f, 0, default, 1.4f);
                            d.velocity *= 0.4f;
                        }
                    }
                    for(int i = 0; i < Main.rand.Next(1, 5); i++)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), fistPosition - new Vector2(16 * Main.rand.NextFloat(1), 16 * Main.rand.NextFloat(1)), fistVelo, ModGores.GoreType("Gores/Fistfull/FistfullGore4"), Main.rand.NextFloat(0.66f, 1f));
                    }
                    for (int i = 0; i < Main.rand.Next(1, 5); i++)
                    {
                        Gore.NewGore(NPC.GetSource_Death(), fistPosition - new Vector2(16 * Main.rand.NextFloat(1), 16 * Main.rand.NextFloat(1)), fistVelo, ModGores.GoreType("Gores/Fistfull/FistfullGore5"), Main.rand.NextFloat(0.66f, 1f));
                    }
                }
            }
        }
        public override bool ModifyCollisionData(Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox)
        {
            bool colliding = NPC.Hitbox.Intersects(victimHitbox);
            if (colliding)
                return true;
            int width = 32;
            Rectangle hitBox = new Rectangle((int)fistPosition.X - width/2, (int)fistPosition.Y - width/2, width, width);
            if (hitBox.Intersects(victimHitbox))
            {
                npcHitbox = victimHitbox;
            }
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Vertebrae, 5));
            npcLoot.Add(ItemDropRule.Common(ItemID.RottenChunk, 5));
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEvil>(), 5));
            npcLoot.Add(ItemDropRule.Common(ItemType<OldKey>(), 100));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PintOPunch>(), 200));
        }
    }
}