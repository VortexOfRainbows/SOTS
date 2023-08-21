using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.FromChests;
using SOTS.Projectiles.Chaos;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Phase
{
    public class PhaseAssaulterHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Ichor,
                    BuffID.Venom,
                    BuffID.OnFire,
                    BuffID.BetsysCurse
                }
            });
        }
        Vector2 directVelo;
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterHeadPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                spriteBatch.Draw(textureP, NPC.Center - screenPos+ new Vector2(0, NPC.gfxOffY) + circular, NPC.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            NPC.knockBackResist = 0f;
            NPC.aiStyle =0;
            NPC.lifeMax = 9500;
            NPC.damage = 100;
            NPC.defense = 30;
            NPC.width = 54;
            NPC.height = 56;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.value = Item.buyPrice(0, 3, 20, 0);
            NPC.npcSlots = 3f;
            NPC.netAlways = true;
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
        }
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = NPC.lifeMax * 15 / 19;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PhaseOre>(), 3, 6, 11));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfChaos>(), 5, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<TwilightShard>(), 8, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfOtherworld>(), 11, 1, 1));
        }
        public override void AI()
        {
            if(Main.rand.NextBool(50))
            {
                Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, NPC.alpha, ColorHelpers.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(NPC.alpha - 20, 0, 255);
            }
            Player player = Main.player[NPC.target];
            float dist2 = (NPC.Center - player.Center).Length();

            if(dist2 < 480 || NPC.ai[1] >= 300)
            {
                int damage = NPC.GetBaseDamage() / 2;
                NPC.ai[1]++;
                if ((int)NPC.ai[1] == 300)
                {
                    Vector2 outward = new Vector2(0, -1).RotatedBy(NPC.rotation);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + outward * 32, outward * 6f, ModContent.ProjectileType<PhaseEraser>(), (int)(damage * 0.7f), 0, Main.myPlayer, NPC.whoAmI);
                    }
                }
                if (NPC.ai[1] > 600)
                    NPC.ai[1] = -120;
            }
        }
        bool runOnce = true;
        public override void PostAI()
        {
            NPC.velocity = directVelo;
            if (NPC.ai[0] >= 0)
            {
                NPC nextBody = Main.npc[(int)NPC.ai[0]];
                if (nextBody.type == ModContent.NPCType<PhaseAssaulterBody>())
                {
                    Vector2 toPrevious = nextBody.Center - NPC.Center;
                    Vector2 combined = toPrevious.RotatedBy(-1.57f).SafeNormalize(Vector2.Zero) + directVelo.RotatedBy(1.57f).SafeNormalize(Vector2.Zero);
                    NPC.rotation = combined.ToRotation();
                }
            }
        }
        int despawn = 0;
        public override bool PreAI()
        {
            if(runOnce)
            {
                NPC.ai[0] = -1;
                runOnce = false;
            }
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            if (player.dead || player.Distance(NPC.Center) > 5400)
            {
                despawn++;
            }
            else if (despawn > 0)
                despawn--;
            if (despawn >= 300)
            {
                NPC.active = false;
            }
            float dist2 = (NPC.Center - player.Center).Length();
            float farDist = 800f;
            NPC.alpha = (int)(255f * (dist2 / farDist)) - 50;
            NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 255);
            if (NPC.alpha > 200)
            {
                NPC.dontTakeDamage = true;
            }
            else
                NPC.dontTakeDamage = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (NPC.ai[0] < 0)
                {
                    NPC.realLife = NPC.whoAmI;
                    int latestNPC = NPC.whoAmI;
                    int randomWormLength = 15;
                    latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PhaseAssaulterBody>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                    NPC.ai[0] = latestNPC;
                    Main.npc[latestNPC].realLife = NPC.whoAmI;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PhaseAssaulterBody>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                        Main.npc[latestNPC].realLife = NPC.whoAmI;
                    }
                    latestNPC = NPC.NewNPC(NPC.GetSource_Misc("SOTS:WormEnemy"), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PhaseAssaulterTail>(), NPC.whoAmI, 0, latestNPC, 0, NPC.whoAmI);
                    Main.npc[latestNPC].realLife = NPC.whoAmI;
                    NPC.netUpdate = true;
                }
            }
            DoWormStuff();
            NPC.rotation = directVelo.ToRotation() + 1.57f;
            return true;
        }
        public void DoWormStuff()
        {
            float speed = 14f;
            float acceleration = 0.09f;
            Vector2 npcCenter = NPC.Center;
            Vector2 targetPos = Main.player[NPC.target].Center;
            float targetRoundedPosX = targetPos.X;
            float targetRoundedPosY = targetPos.Y;
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;
            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            float absDirX = Math.Abs(dirX);
            float absDirY = Math.Abs(dirY);
            float newSpeed = speed / length;
            dirX = dirX * newSpeed;
            dirY = dirY * newSpeed;
            if (directVelo.X > 0.0 && dirX > 0.0 || directVelo.X < 0.0 && dirX < 0.0 || (directVelo.Y > 0.0 && dirY > 0.0 || directVelo.Y < 0.0 && dirY < 0.0))
            {
                if (directVelo.X < dirX)
                    directVelo.X = directVelo.X + acceleration;
                else if (directVelo.X > dirX)
                    directVelo.X = directVelo.X - acceleration;
                if (directVelo.Y < dirY)
                    directVelo.Y = directVelo.Y + acceleration;
                else if (directVelo.Y > dirY)
                    directVelo.Y = directVelo.Y - acceleration;
                if (Math.Abs(dirY) < speed * 0.2 && (directVelo.X > 0.0 && dirX < 0.0 || directVelo.X < 0.0 && dirX > 0.0))
                {
                    if (directVelo.Y > 0.0)
                        directVelo.Y = directVelo.Y + acceleration * 2f;
                    else
                        directVelo.Y = directVelo.Y - acceleration * 2f;
                }
                if (Math.Abs(dirX) < speed * 0.2 && (directVelo.Y > 0.0 && dirY < 0.0 || directVelo.Y < 0.0 && dirY > 0.0))
                {
                    if (directVelo.X > 0.0)
                        directVelo.X = directVelo.X + acceleration * 2f;
                    else
                        directVelo.X = directVelo.X - acceleration * 2f;
                }
            }
            else if (absDirX > absDirY)
            {
                if (directVelo.X < dirX)
                    directVelo.X = directVelo.X + acceleration * 1.1f;
                else if (directVelo.X > dirX)
                    directVelo.X = directVelo.X - acceleration * 1.1f;
                if (Math.Abs(directVelo.X) + Math.Abs(directVelo.Y) < speed * 0.5)
                {
                    if (directVelo.Y > 0.0)
                        directVelo.Y = directVelo.Y + acceleration;
                    else
                        directVelo.Y = directVelo.Y - acceleration;
                }
            }
            else
            {
                if (directVelo.Y < dirY)
                    directVelo.Y = directVelo.Y + acceleration * 1.1f;
                else if (directVelo.Y > dirY)
                    directVelo.Y = directVelo.Y - acceleration * 1.1f;
                if (Math.Abs(directVelo.X) + Math.Abs(directVelo.Y) < speed * 0.5)
                {
                    if (directVelo.X > 0.0)
                        directVelo.X = directVelo.X + acceleration;
                    else
                        directVelo.X = directVelo.X - acceleration;
                }
            }
            if (NPC.localAI[0] != 1)
                NPC.netUpdate = true;
            NPC.localAI[0] = 1f;
            if ((directVelo.X > 0.0 && NPC.oldVelocity.X < 0.0 || directVelo.X < 0.0 && NPC.oldVelocity.X > 0.0 || (directVelo.Y > 0.0 && NPC.oldVelocity.Y < 0.0 || directVelo.Y < 0.0 && NPC.oldVelocity.Y > 0.0)) && !NPC.justHit)
                NPC.netUpdate = true;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1f;
            return null;
        }
    }
    public class PhaseAssaulterBody : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Ichor,
                    BuffID.Venom,
                    BuffID.OnFire,
                    BuffID.BetsysCurse
                }
            });
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterBodyGlow");
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterBodyPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                spriteBatch.Draw(textureP, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) + circular, NPC.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            NPC.knockBackResist = 0.0f;
            NPC.width = 46;
            NPC.height = 42;
            NPC.damage = 80;
            NPC.defense = 200;
            NPC.lifeMax = 6969;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.value = 10000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Banner = ModContent.NPCType<PhaseAssaulterHead>();
            BannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha <= 205;
        }
        public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 0.5f;
        }
        public override bool PreAI()
        {
            if (Main.rand.NextBool(50))
            {
                Dust dust = Dust.NewDustDirect(NPC.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, NPC.alpha, ColorHelpers.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(NPC.alpha - 20, 0, 255);
            }
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            float dist2 = (NPC.Center - player.Center).Length();
            float farDist = 800f;
            NPC.alpha = (int)(255f * (dist2 / farDist)) - 50;
            NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 255);
            if (NPC.alpha > 200)
            {
                NPC.dontTakeDamage = true;
            }
            else
                NPC.dontTakeDamage = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                }
                if (!NPC.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.DamageNPC, -1, -1, null, NPC.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            NPC.alpha = Main.npc[(int)NPC.ai[1]].alpha;

            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                int height = NPC.height;
                if (Main.npc[(int)NPC.ai[1]].type == ModContent.NPCType<PhaseAssaulterHead>())
                    height += 4;
                else
                    height -= 4;
                float dist = (length - height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void PostAI()
        {
            NPC.timeLeft = 100000;
        }
    }
    public class PhaseAssaulterTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[]
                {
                    BuffID.Poisoned,
                    BuffID.Frostburn,
                    BuffID.Ichor,
                    BuffID.Venom,
                    BuffID.OnFire,
                    BuffID.BetsysCurse
                }
            });
        }
        public void TrailPreDraw(SpriteBatch spriteBatch, Vector2 screenPos)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseSpeederTrail");
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            Vector2 previousPosition = NPC.Center + new Vector2(0, 8).RotatedBy(NPC.rotation);
            for (int k = 0; k < trailPos.Length; k++)
            {
                if (trailPos[k] == Vector2.Zero)
                {
                    break;
                }
                Color color = ColorHelpers.ChaosPink * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f));
                color.A = 0;
                color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
                Vector2 drawPos = trailPos[k] - screenPos;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                float lengthTowards = betweenPositions.Length() / texture.Height;
                spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(lengthTowards * 3, 2f), SpriteEffects.None, 0f);
                previousPosition = currentPos;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            TrailPreDraw(spriteBatch, screenPos);
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterTailPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, NPC.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
                spriteBatch.Draw(textureP, NPC.Center - screenPos+ new Vector2(0, NPC.gfxOffY) + circular, NPC.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f)), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0f);
            }
            texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            spriteBatch.Draw(texture, NPC.Center - screenPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            NPC.knockBackResist = 0.0f;
            NPC.width = 46;
            NPC.height = 32;
            NPC.damage = 60;
            NPC.defense = 24;
            NPC.lifeMax = 130000;
            NPC.noTileCollide = true;
            NPC.netAlways = true;
            NPC.noGravity = true;
            NPC.dontCountMe = true;
            NPC.value = 100000;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            Banner = ModContent.NPCType<PhaseAssaulterHead>();
            BannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha <= 205;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public Vector2[] trailPos = new Vector2[120];
        public void cataloguePos()
        {
            Vector2 current = NPC.Center + new Vector2(0, 8).RotatedBy(NPC.rotation);
            for (int i = 0; i < trailPos.Length; i++)
            {
                Vector2 previousPosition = trailPos[i];
                trailPos[i] = current;
                current = previousPosition;
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 from = NPC.Center + new Vector2(0, 12).RotatedBy(NPC.rotation);
                Dust dust = Dust.NewDustDirect(from - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, NPC.alpha, ColorHelpers.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.velocity += new Vector2(-2, 0).RotatedBy(NPC.rotation);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(NPC.alpha - 20, 0, 255);
            }
        }
        public override bool PreAI()
        {
            cataloguePos();
            if (NPC.ai[3] > 0)
                NPC.realLife = (int)NPC.ai[3];
            NPC.TargetClosest();
            Player player = Main.player[NPC.target];
            float dist2 = (NPC.Center - player.Center).Length();
            float farDist = 800f;
            NPC.alpha = (int)(255f * (dist2 / farDist)) - 50;
            NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 255);
            if (NPC.alpha > 200)
            {
                NPC.dontTakeDamage = true;
            }
            else
                NPC.dontTakeDamage = false;
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)NPC.ai[1]].active)
                {
                    NPC.life = 0;
                    NPC.HitEffect(0, 10.0);
                    NPC.active = false;
                    NetMessage.SendData(28, -1, -1, null, NPC.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
            NPC.alpha = Main.npc[(int)NPC.ai[1]].alpha;
            if (NPC.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(NPC.position.X + (float)NPC.width * 0.5f, NPC.position.Y + (float)NPC.height * 0.5f);
                float dirX = Main.npc[(int)NPC.ai[1]].position.X + (float)(Main.npc[(int)NPC.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)NPC.ai[1]].position.Y + (float)(Main.npc[(int)NPC.ai[1]].height / 2) - npcCenter.Y;
                NPC.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - 22) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                NPC.velocity = Vector2.Zero;
                NPC.position.X = NPC.position.X + posX;
                NPC.position.Y = NPC.position.Y + posY;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.netUpdate = true;
            }
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void PostAI()
        {
            NPC.timeLeft = 10000000;
        }
    }
}