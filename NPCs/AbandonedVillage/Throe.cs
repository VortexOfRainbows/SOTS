using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent; 

namespace SOTS.NPCs.AbandonedVillage
{
	public class Throe : ModNPC
    {
        public class ThroeFace //These will be used purely for visual purposes, so it doesnt need to be synced on the server
        {
            public int AI;
            public int FrameCounter;
            public List<Vector2> oldPos;
            public Vector2 position;
            public Vector2 velocity;
            public Vector2 target;
            public void Update()
            {
                FrameCounter++;
                AI--;
                if (AI > 0)
                {
                    Vector2 toTarget = target - position;
                    //NPC.Center = Vector2.Lerp(NPC.Center, target, 0.01f);
                    velocity *= 0.965f;
                    velocity += toTarget.SNormalize() * 0.04f;

                    position = Vector2.Lerp(position, Vector2.Zero, 0.01f);
                }
                else
                {
                    target = Main.rand.NextVector2Circular(80, 80) - velocity * 10;
                    AI = 90;
                }
                if(AI % 3 == 0)
                {
                    oldPos.Insert(0, position);
                    if (oldPos.Count > 10)
                        oldPos.RemoveAt(10);
                }
                position += velocity;
            }
            public ThroeFace()
            {
                FrameCounter = Main.rand.Next(25);
                AI = Main.rand.Next(90);
                position = Vector2.Zero;
                velocity = Vector2.Zero;
                target = Vector2.Zero;
                oldPos = new List<Vector2>();
            }
        }
        public ThroeFace[] faces;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(MyHorizontalFrame);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            MyHorizontalFrame = reader.ReadInt32();
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1;
            NPC.lifeMax = 25;
            NPC.damage = 25; 
            NPC.defense = 0;	
            NPC.knockBackResist = 0.25f;
            NPC.width = 34;
            NPC.height = 56;
            NPC.value = 250;
            NPC.boss = NPC.lavaImmune = false;
            NPC.noGravity = NPC.noTileCollide = NPC.netAlways = NPC.dontTakeDamage = true;
            NPC.HitSound = SoundID.NPCHit37;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.alpha = 150;
            NPC.scale = 0.9f;
			Banner = NPC.type;
			BannerItem = ItemType<ThroeBanner>();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return Aggressive && !NPC.dontTakeDamage;
        }
        public bool Aggressive => NPC.life < NPC.lifeMax || NPC.ai[3] < 0;
        public int MyHorizontalFrame = -1;
        public override void AI()
        {
            if (MyHorizontalFrame == -1)
                MyHorizontalFrame = NPC.whoAmI % 3;
            if (NPC.ai[3] >= 0)
            {
                if (faces == null)
                {
                    if (Main.netMode != NetmodeID.Server)
                    {
                        faces = [new ThroeFace(), new ThroeFace(), new ThroeFace(), new ThroeFace()];
                    }
                }
                else
                {
                    foreach (ThroeFace tF in faces)
                        tF.Update();
                }
            }
            NPC.TargetClosest(false);
            Player player = Main.player[NPC.target];

            if(Main.rand.NextBool(2))
            {
                Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2Circular(1, 1), ColorHelper.AVDustColor, -5);
                dust.velocity += (dust.position - new Vector2(5, 5) - NPC.Center).SNormalize() * 0.5f;
                dust.alpha = NPC.alpha;
                dust.scale = Main.rand.NextFloat(1, 2);
            }
            if (faces != null && NPC.ai[3] >= 0)
            {
                int i = 0;
                foreach (ThroeFace tF in faces)
                {
                    if (Main.rand.NextBool(2))
                    {
                        float circular = MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.5f + 90 * i);
                        Vector2 cloneLocation = NPC.Center + tF.position.RotatedBy(circular);
                        Dust dust = PixelDust.Spawn(cloneLocation - new Vector2(NPC.width / 2, NPC.height / 2), NPC.width, NPC.height, Main.rand.NextVector2Circular(1, 1), ColorHelper.AVDustColor, -5);
                        dust.velocity += (dust.position - new Vector2(5, 5) - cloneLocation).SNormalize() * 0.5f;
                        dust.alpha = 150 - (int)(MathF.Sin(MathHelper.ToRadians(tF.AI * 4)) * 50);
                        dust.scale = Main.rand.NextFloat(1, 2);
                    }
                    i++;
                }
            }

            NPC.gfxOffY = -4;

            if (NPC.ai[3] <= -1 && NPC.ai[3] >= -10)
            {
                MyHorizontalFrame = Math.Abs((int)NPC.ai[3]) % 3;
                NPC.velocity += Main.rand.NextVector2Circular(5, 5);
                NPC.ai[3] = -20f;
                NPC.life = (int)NPC.ai[2];
                NPC.ai[2] = 0;
                NPC.localAI[1] = -1;
            }

            if (Aggressive)
            {
                int invis = 0;
                NPC.knockBackResist = 0.5f;
                NPC.velocity *= 0.971f;
                NPC.ai[0]++;
                if (NPC.ai[3] >= 0 || NPC.localAI[1] >= 0)
                {
                    NPC.netUpdate = true;
                    NPC.ai[3] = -20;
                    NPC.ai[2] = 0;
                    NPC.ai[1] = 0.9f;
                    NPC.localAI[1] = -1; //This is to help multiplayer clients create a visual effect, since this variable is not synced in multiplayer
                    if (Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            NPC.NewNPCDirect(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.position.Y + NPC.height), Type, 0, Main.rand.Next(90), 0.875f, NPC.life, -(1 + (i + NPC.whoAmI) % 3));
                        }
                        //This one can be spawned server side since the position for the visual should be synced among all clients
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity * 0.75f, ProjectileType<ThroeProj>(), 0, MyHorizontalFrame, Main.myPlayer, (int)NPC.frameCounter, 0, 56);
                    }
                    if(Main.netMode != NetmodeID.Server)
                    {
                        if (faces != null)
                        {
                            int i = 0;
                            foreach (ThroeFace tF in faces)
                            {
                                //Deliberately setting the Main.myPlayer (owner) parameter to 0 in order to make the spawning of the projectile client sided, as this is a projectile spawned for visual effect, and the visual for Throes is not synced.
                                float circular = MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.5f + 90 * i++);
                                Vector2 cloneLocation = NPC.Center + tF.position.RotatedBy(circular);
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), cloneLocation, (NPC.velocity + tF.velocity) * 0.26f, ProjectileType<ThroeProj>(), 0, 1 + (i + NPC.whoAmI) % 3, -1, tF.FrameCounter, 0.5f, 28);
                            }
                        }
                        SOTSUtils.PlaySound(SoundID.DD2_GhastlyGlaiveImpactGhost, NPC.Center, 1.2f, -0.3f);
                    }
                }
                else
                {
                    float percent = MathHelper.Clamp(NPC.ai[2] / 30f, 0, 1);
                    float distToPlayer = player.Distance(NPC.Center);
                    NPC.dontTakeDamage = NPC.ai[2] < 30;
                    if (NPC.ai[2] < 0)
                    {
                        float inverPercent = MathF.Sin(-NPC.ai[2] / 40f * MathF.PI);
                        invis = (int)(155 * inverPercent);
                        if ((int)NPC.ai[2] == -20)
                        {
                            NPC.Center = player.Center + new Vector2(distToPlayer * 0.8f + 96, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 4));
                            NPC.netUpdate = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ProjectileType<ThroeProj>(), 0, MyHorizontalFrame, Main.myPlayer, (int)NPC.frameCounter, 0, -32);
                        }
                        if ((int)NPC.ai[2] == -40)
                        {
                            NPC.netUpdate = true;
                            if (Main.netMode != NetmodeID.MultiplayerClient)
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ProjectileType<ThroeProj>(), 0, MyHorizontalFrame, Main.myPlayer, (int)NPC.frameCounter, 0.75f, 32);
                        }
                    }


                    Vector2 target = player.Center + new Vector2(distToPlayer * 0.5f * percent + 8, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * 4));

                    Vector2 toTarget = target - NPC.Center;
                    NPC.velocity *= MathHelper.Lerp(1, 0.945f, percent);
                    NPC.velocity += toTarget.SNormalize() * 0.275f * percent * percent;
                }
                NPC.scale = NPC.ai[1];
                invis = Math.Clamp(invis, 0, 155);
                NPC.alpha = Math.Min(255, 150 + (int)(MathF.Sin(MathHelper.ToRadians(NPC.ai[0] * 4)) * 50) + invis);
                NPC.ai[2]++;
            }
            else
            {
                NPC.localAI[1] = 1;
                NPC.dontTakeDamage = false;
                //Wander around in a small area
                NPC.ai[0]--;
                if (NPC.ai[0] > 0)
                {
                    Vector2 target = new Vector2(NPC.ai[1], NPC.ai[2]);

                    Vector2 toTarget = target - NPC.Center;
                    Vector2 toPlayer = player.Center - NPC.Center;
                    float dist = toPlayer.Length();
                    NPC.velocity *= 0.971f;
                    NPC.velocity += toTarget.SNormalize() * 0.033f;
                    if (dist < 160)
                    {
                        float moveAwayFromPlayer = 1 - dist / 160f;
                        NPC.velocity -= toPlayer.SNormalize() * moveAwayFromPlayer * 0.1f;
                    }
                    else
                    {
                        NPC.velocity += toPlayer.SNormalize() * 0.0075f;
                    }
                }
                else
                {
                    NPC.ai[0] = 90;

                    int attemptSpawningOutsideOfTiles = 5;
                    while (attemptSpawningOutsideOfTiles-- > 0)
                    {
                        NPC.ai[1] = NPC.Center.X + Main.rand.NextFloat(-100, 100) - NPC.velocity.X * 5;
                        NPC.ai[2] = NPC.Center.Y + Main.rand.NextFloat(-100, 100) - NPC.velocity.Y * 5;
                        if (!SOTSWorldgenHelper.TrueTileSolid((int)NPC.ai[1] / 16, (int)NPC.ai[2] / 16))
                        {
                            break;
                        }
                    }

                    NPC.netUpdate = true;
                }
                NPC.alpha = 150 + (int)(MathF.Sin(MathHelper.ToRadians(NPC.ai[0] * 4)) * 50);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 6, texture.Height / 10);
            Vector2 drawPos = NPC.Center - screenPos;
            Rectangle frame2 = new Rectangle(MyHorizontalFrame * texture.Width / 3, NPC.frame.Y, texture.Width / 3, texture.Height / 5);
            if (faces != null && NPC.ai[3] >= 0)
            {
                int i = 0;
                foreach(ThroeFace tF in faces)
                {
                    int frameNum = (1 + (i + NPC.whoAmI) % 3) % 3;
                    Rectangle frame = new Rectangle(frameNum * texture.Width / 3, tF.FrameCounter / 5 % 5 * texture.Height / 5, texture.Width / 3, texture.Height / 5);
                    float circular = MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.5f + 90 * i++);
                    Vector2 cloneLocation = drawPos + tF.position.RotatedBy(circular);
                    float sinusoid = MathF.Sin(MathHelper.ToRadians(tF.AI * 4));
                    for (int j = 0; j < tF.oldPos.Count; j++)
                    {
                        float perc = 1 - j / (float)NPC.oldPos.Length;
                        Main.spriteBatch.Draw(texture, drawPos + tF.oldPos[j].RotatedBy(circular - MathHelper.ToRadians(1.5f * j + 1)), frame, drawColor * perc * 0.5f * (0.5882f + sinusoid * 0.196f), NPC.rotation, drawOrigin, NPC.scale * perc * (0.875f - sinusoid * 0.05f), SpriteEffects.None, 0f);
                    }
                    spriteBatch.Draw(texture, cloneLocation, frame, drawColor * (0.5882f + sinusoid * 0.196f), NPC.rotation, drawOrigin, NPC.scale * 0.875f - sinusoid * 0.05f, SpriteEffects.None, 0f);
                }
            }
            float sin = MathF.Sin(MathHelper.ToRadians(NPC.ai[0] * 4));
            drawColor = NPC.GetAlpha(drawColor);

            for (int i = 0; i < NPC.oldPos.Length; i++)
            {
                if (NPC.oldPos[i] == Vector2.Zero)
                    break;
                float perc = 1 - i / (float)NPC.oldPos.Length;
                Vector2 center = NPC.oldPos[i] + NPC.Size / 2;
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, frame2, drawColor * perc * 0.5f, NPC.rotation, drawOrigin, perc * (NPC.scale + 0.05f * sin), SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, drawPos, frame2, drawColor, NPC.rotation, drawOrigin, NPC.scale + 0.05f * sin, SpriteEffects.None, 0f);
            //texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/BleedingGhastGlow");
            //spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 48, 56), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public void TeleportToAdjacentArea()
		{

		}
		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
		{
            if (!Aggressive || NPC.ai[3] >= 0)
            {
                modifiers.SourceDamage *= 0.5f;
                modifiers.SetMaxDamage(Math.Max(NPC.life - NPC.lifeMax / 2, 1));
                modifiers.DisableCrit();
                modifiers.DisableKnockback();
            }
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
            if(Aggressive)
            {
                NPC.ai[2] = -40;
                NPC.netUpdate = true;
            }
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life > 0)
            {
                int num = 0;
                while (num < hit.Damage / NPC.lifeMax * 80.0)
                {
                    Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2Circular(1, 1), ColorHelper.AVDustColor, -2);
                    dust.velocity += (dust.position - new Vector2(5, 5) - NPC.Center).SNormalize() * 0.5f;
                    dust.velocity.X += 2 * hit.HitDirection;
                    dust.alpha = NPC.alpha;
                    dust.scale = Main.rand.NextFloat(1, 2);
                }
            }
            else
            {
                for (int k = 0; k < 30; k++)
                {
                    Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2CircularEdge(2, 3), ColorHelper.AVDustColor, -2);
                    dust.velocity += (dust.position - new Vector2(5, 5) - NPC.Center).SNormalize() * 0.1f;
                    dust.velocity.X += 2 * hit.HitDirection;
                    dust.alpha = NPC.alpha;
                    dust.scale = Main.rand.NextFloat(1, 2);
                }
            }
        }
        private int frame = 0;
		public override void FindFrame(int frameHeight) 
		{
			frame = frameHeight;
			NPC.frameCounter++;
			if (NPC.frameCounter >= 5f)
			{
                NPC.frameCounter -= 5f;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 5 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<SootBlock>(), 1, 1, 2));
            npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfEarth>(), 5, 1, 1));
        }
        public override void OnKill()
        {
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, NPC.velocity, ProjectileType<ThroeProj>(), 0, MyHorizontalFrame, Main.myPlayer, (int)NPC.frameCounter, 0, 64);
            }
        }
    }
    public class ThroeProj : ModProjectile
    {
        public override string Texture => "SOTS/NPCs/AbandonedVillage/Throe";
        public override bool PreDraw(ref Color lightColor)
        {
            int dir = SOTSUtils.SignNoZero(Projectile.ai[2]);
            int frame = (int)(Projectile.ai[0] / 5) % 5;
            int horizontalFrame = (int)Projectile.knockBack % 3;
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width / 6, texture.Height / 10);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Rectangle frameRect = new Rectangle(horizontalFrame * texture.Width / 3, frame * texture.Height / 5, texture.Width / 3, texture.Height / 5);
            float perc = Projectile.timeLeft / 60f;
            float reverse = dir == -1 ? perc : (1 - perc);
            float normal = dir == 1 ? perc : perc * (1 - perc);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(Projectile.ai[2] * reverse, 0).RotatedBy(dir * MathHelper.ToRadians(Projectile.ai[0] * 2 + SOTSWorld.GlobalCounter + i * 90));
                Main.spriteBatch.Draw(texture, circular + drawPos, frameRect, lightColor * 0.325f * normal * (1 - Projectile.ai[1]), Projectile.rotation, drawOrigin, Projectile.scale * (0.9f + 0.2f * reverse), SpriteEffects.None, 0f);
            }
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.height = 80;
            Projectile.width = 80;
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
            Projectile.alpha = 255;
            Projectile.timeLeft = 60;
        }
        private bool runOnce = true;
        public override void AI()
        {
            if(runOnce)
            {
                int particleCount = Projectile.ai[2] != 32 ? 30 : 10;
                if (Projectile.ai[2] > 0)
                {
                    for (int k = 0; k < particleCount; k++)
                    {
                        Dust dust = PixelDust.Spawn(Projectile.position, Projectile.width, Projectile.height, Main.rand.NextVector2CircularEdge(2, 3), ColorHelper.AVDustColor, -2);
                        dust.velocity += (dust.position - new Vector2(5, 5) - Projectile.Center).SNormalize() * 0.1f;
                        dust.alpha = 100;
                        dust.scale = Main.rand.NextFloat(1, 2);
                    }
                }
                runOnce = false;
            }
            Projectile.ai[0]++;
            Projectile.velocity *= 0.93f;
        }
    }
}