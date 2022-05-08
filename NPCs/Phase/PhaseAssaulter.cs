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
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Phase
{
    public class PhaseAssaulterHead : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phase Assaulter");
        }
        Vector2 directVelo;
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterHeadPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(textureP, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + circular, npc.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - npc.alpha) / 255f)), npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
            }
            texture = Main.npcTexture[npc.type];
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.White), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            npc.lifeMax = 9500;
            npc.damage = 100;
            npc.defense = 30;
            npc.width = 54;
            npc.height = 56;
            npc.lavaImmune = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.value = Item.buyPrice(0, 3, 20, 0);
            npc.npcSlots = 3f;
            npc.netAlways = true;
            banner = npc.type;
            bannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
            SetupDebuffImmunities();
        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = 15000;
        }
        public void SetupDebuffImmunities()
        {
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.BetsysCurse] = true;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public override void NPCLoot()
        {
            if (!Main.rand.NextBool(3))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PhaseOre>(), Main.rand.Next(6) + 6); //drops 6 to 11 ore at a time, since you need quite a lot
            if (Main.rand.NextBool(5))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfChaos>(), 1);
            if (Main.rand.NextBool(8))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<TwilightShard>(), 1);
            if (Main.rand.NextBool(11))
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfOtherworld>(), 1);
            //if (Main.rand.NextBool(20))
               // Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PhaseBar>(), 1);
        }
        public override void AI()
        {
            if(Main.rand.NextBool(50))
            {
                Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, npc.alpha, VoidPlayer.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(npc.alpha - 20, 0, 255);
            }
            Player player = Main.player[npc.target];
            float dist2 = (npc.Center - player.Center).Length();

            if(dist2 < 480 || npc.ai[1] >= 300)
            {
                int damage = npc.damage / 2;
                if (Main.expertMode)
                {
                    damage = (int)(damage / Main.expertDamage);
                }
                npc.ai[1]++;
                if ((int)npc.ai[1] == 300)
                {
                    Vector2 outward = new Vector2(0, -1).RotatedBy(npc.rotation);
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(npc.Center + outward * 32, outward * 6f, ModContent.ProjectileType<PhaseEraser>(), (int)(damage * 0.7f), 0, Main.myPlayer, npc.whoAmI);
                    }
                }
                if (npc.ai[1] > 600)
                    npc.ai[1] = -120;
            }
        }
        bool runOnce = true;
        public override void PostAI()
        {
            npc.velocity = directVelo;
            if (npc.ai[0] >= 0)
            {
                NPC nextBody = Main.npc[(int)npc.ai[0]];
                if (nextBody.type == ModContent.NPCType<PhaseAssaulterBody>())
                {
                    Vector2 toPrevious = nextBody.Center - npc.Center;
                    Vector2 combined = toPrevious.RotatedBy(-1.57f).SafeNormalize(Vector2.Zero) + directVelo.RotatedBy(1.57f).SafeNormalize(Vector2.Zero);
                    npc.rotation = combined.ToRotation();
                }
            }
        }
        int despawn = 0;
        public override bool PreAI()
        {
            if(runOnce)
            {
                npc.ai[0] = -1;
                runOnce = false;
            }
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            if (player.dead || player.Distance(npc.Center) > 5400)
            {
                despawn++;
            }
            else if (despawn > 0)
                despawn--;
            if (despawn >= 300)
            {
                npc.active = false;
            }
            float dist2 = (npc.Center - player.Center).Length();
            float farDist = 800f;
            npc.alpha = (int)(255f * (dist2 / farDist)) - 50;
            npc.alpha = (int)MathHelper.Clamp(npc.alpha, 0, 255);
            if (npc.alpha > 200)
            {
                npc.dontTakeDamage = true;
            }
            else
                npc.dontTakeDamage = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (npc.ai[0] < 0)
                {
                    npc.realLife = npc.whoAmI;
                    int latestNPC = npc.whoAmI;
                    int randomWormLength = 15;
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PhaseAssaulterBody>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                    npc.ai[0] = latestNPC;
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PhaseAssaulterBody>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                        Main.npc[latestNPC].realLife = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<PhaseAssaulterTail>(), npc.whoAmI, 0, latestNPC, 0, npc.whoAmI);
                    Main.npc[latestNPC].realLife = npc.whoAmI;
                    npc.netUpdate = true;
                }
            }
            DoWormStuff();
            npc.rotation = directVelo.ToRotation() + 1.57f;
            return true;
        }
        public void DoWormStuff()
        {
            float speed = 14f;
            float acceleration = 0.09f;
            Vector2 npcCenter = npc.Center;
            Vector2 targetPos = Main.player[npc.target].Center;
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
            if (npc.localAI[0] != 1)
                npc.netUpdate = true;
            npc.localAI[0] = 1f;
            if ((directVelo.X > 0.0 && npc.oldVelocity.X < 0.0 || directVelo.X < 0.0 && npc.oldVelocity.X > 0.0 || (directVelo.Y > 0.0 && npc.oldVelocity.Y < 0.0 || directVelo.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;
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
            DisplayName.SetDefault("Phase Assaulter");
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterBodyGlow");
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterBodyPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(textureP, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + circular, npc.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - npc.alpha) / 255f)), npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
            }
            texture = Main.npcTexture[npc.type];
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.White), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            npc.knockBackResist = 0.0f;
            npc.width = 46;
            npc.height = 42;
            npc.damage = 80;
            npc.defense = 200;
            npc.lifeMax = 6969;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 10000;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            banner = ModContent.NPCType<PhaseAssaulterHead>();
            bannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
            SetupDebuffImmunities();
        }
        public void SetupDebuffImmunities()
        {
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.BetsysCurse] = true;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.alpha <= 205;
        }
        public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
        {
            damage *= 0.5f;
            return true;
        }
        public override bool PreAI()
        {
            if (Main.rand.NextBool(50))
            {
                Dust dust = Dust.NewDustDirect(npc.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, npc.alpha, VoidPlayer.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(npc.alpha - 20, 0, 255);
            }
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            float dist2 = (npc.Center - player.Center).Length();
            float farDist = 800f;
            npc.alpha = (int)(255f * (dist2 / farDist)) - 50;
            npc.alpha = (int)MathHelper.Clamp(npc.alpha, 0, 255);
            if (npc.alpha > 200)
            {
                npc.dontTakeDamage = true;
            }
            else
                npc.dontTakeDamage = false;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }
                if (!npc.active && Main.netMode == NetmodeID.Server)
                {
                    NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, npc.whoAmI, -1f, 0f, 0f, 0, 0, 0);
                }
            }
            npc.alpha = Main.npc[(int)npc.ai[1]].alpha;

            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                int height = npc.height;
                if (Main.npc[(int)npc.ai[1]].type == ModContent.NPCType<PhaseAssaulterHead>())
                    height += 4;
                else
                    height -= 4;
                float dist = (length - height) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
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
            npc.timeLeft = 100000;
        }
    }
    public class PhaseAssaulterTail : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phase Assaulter");
        }
        public void TrailPreDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseSpeederTrail");
            Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
            Vector2 previousPosition = npc.Center + new Vector2(0, 8).RotatedBy(npc.rotation);
            for (int k = 0; k < trailPos.Length; k++)
            {
                if (trailPos[k] == Vector2.Zero)
                {
                    break;
                }
                Color color = VoidPlayer.ChaosPink * (0.1f + 0.9f * ((255 - npc.alpha) / 255f));
                color.A = 0;
                color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
                Vector2 drawPos = trailPos[k] - Main.screenPosition;
                Vector2 currentPos = trailPos[k];
                Vector2 betweenPositions = previousPosition - currentPos;
                float lengthTowards = betweenPositions.Length() / texture.Height;
                spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(lengthTowards * 3, 2f), SpriteEffects.None, 0f);
                previousPosition = currentPos;
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            TrailPreDraw(spriteBatch);
            Texture2D texture = Main.npcTexture[npc.type];
            Texture2D textureP = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Phase/PhaseAssaulterTailPink");
            Vector2 origin = new Vector2(texture.Width * 0.5f, npc.height * 0.5f);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2, 0).RotatedBy(npc.rotation + i * MathHelper.PiOver2);
                Main.spriteBatch.Draw(textureP, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + circular, npc.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - npc.alpha) / 255f)), npc.rotation, origin, npc.scale, SpriteEffects.None, 0f);
            }
            texture = Main.npcTexture[npc.type];
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, npc.frame, npc.GetAlpha(Color.White), npc.rotation, origin, npc.scale, SpriteEffects.None, 0);
            return false;
        }
        public override void SetDefaults()
        {
            npc.knockBackResist = 0.0f;
            npc.width = 46;
            npc.height = 32;
            npc.damage = 60;
            npc.defense = 24;
            npc.lifeMax = 130000;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.value = 100000;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath14;
            banner = ModContent.NPCType<PhaseAssaulterHead>();
            bannerItem = ModContent.ItemType<PhaseAssaulterBanner>();
            SetupDebuffImmunities();
        }
        public void SetupDebuffImmunities()
        {
            npc.buffImmune[BuffID.OnFire] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.buffImmune[BuffID.Frostburn] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            npc.buffImmune[BuffID.BetsysCurse] = true;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return npc.alpha <= 205;
        }
        public override bool CheckActive()
        {
            return false;
        }
        public Vector2[] trailPos = new Vector2[120];
        public void cataloguePos()
        {
            Vector2 current = npc.Center + new Vector2(0, 8).RotatedBy(npc.rotation);
            for (int i = 0; i < trailPos.Length; i++)
            {
                Vector2 previousPosition = trailPos[i];
                trailPos[i] = current;
                current = previousPosition;
            }
            if (Main.rand.NextBool(3))
            {
                Vector2 from = npc.Center + new Vector2(0, 12).RotatedBy(npc.rotation);
                Dust dust = Dust.NewDustDirect(from - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, npc.alpha, VoidPlayer.ChaosPink, 1.4f);
                dust.velocity *= 0.3f;
                dust.velocity += new Vector2(-2, 0).RotatedBy(npc.rotation);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.alpha = (int)MathHelper.Clamp(npc.alpha - 20, 0, 255);
            }
        }
        public override bool PreAI()
        {
            cataloguePos();
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            npc.TargetClosest();
            Player player = Main.player[npc.target];
            float dist2 = (npc.Center - player.Center).Length();
            float farDist = 800f;
            npc.alpha = (int)(255f * (dist2 / farDist)) - 50;
            npc.alpha = (int)MathHelper.Clamp(npc.alpha, 0, 255);
            if (npc.alpha > 200)
            {
                npc.dontTakeDamage = true;
            }
            else
                npc.dontTakeDamage = false;
            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                    NetMessage.SendData(28, -1, -1, null, npc.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
                }
            }
            npc.alpha = Main.npc[(int)npc.ai[1]].alpha;
            if (npc.ai[1] < (double)Main.npc.Length)
            {
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - 22) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.netUpdate = true;
            }
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override void PostAI()
        {
            npc.timeLeft = 10000000;
        }
    }
}