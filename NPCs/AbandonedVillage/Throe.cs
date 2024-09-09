using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Fragments;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
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
            public List<Vector2> oldPos;
            public Vector2 position;
            public Vector2 velocity;
            public Vector2 target;
            public void Update()
            {
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
                AI = Main.rand.Next(90);
                position = Vector2.Zero;
                velocity = Vector2.Zero;
                target = Vector2.Zero;
                oldPos = new List<Vector2>();
            }
        }
        public ThroeFace[] faces;
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5;
            NPCID.Sets.TrailCacheLength[Type] = 10;
            NPCID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            NPCID.Sets.TrailingMode[Type] = 0;
            NPC.aiStyle = -1;
            NPC.lifeMax = 25;
            NPC.damage = 30; 
            NPC.defense = 6;	
            NPC.knockBackResist = 0.5f;
            NPC.width = 32;
            NPC.height = 54;
            NPC.value = 250;
            NPC.boss = NPC.lavaImmune = false;
            NPC.noGravity = NPC.noTileCollide = NPC.netAlways = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
			NPC.DeathSound = null;
            NPC.alpha = 150;
			//Banner = NPC.type;
			//BannerItem = ItemType<LostSoulBanner>();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.life < NPC.lifeMax;
        }
        public override void AI()
        {
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
			Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2Circular(1, 1), ColorHelpers.AVDustColor, -5);
			dust.velocity += (dust.position - new Vector2(5, 5) - NPC.Center).SNormalize() * 0.5f;
            dust.alpha = NPC.alpha;
			dust.scale = Main.rand.NextFloat(1, 2);
            NPC.gfxOffY = -4;

            if (NPC.ai[3] == -1)
            {
                NPC.velocity += Main.rand.NextVector2Circular(4, 4);
                NPC.ai[3] = -2;
                NPC.life = NPC.lifeMax / 2;
            }
            bool aggressive = NPC.life < NPC.lifeMax;
            if (aggressive)
            {
                //Split into multiple aggressive throes
                //TODO!
                NPC.velocity *= 0.971f;
                NPC.ai[0]++;
                if (NPC.ai[3] >= 0)
                {
                    NPC.netUpdate = true;
                    NPC.ai[3] = -2;
                    NPC.ai[1] = 1;
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            NPC.NewNPCDirect(NPC.GetSource_FromAI(), new Vector2(NPC.Center.X, NPC.position.Y + NPC.height), Type, 0, Main.rand.Next(90), 0.875f, 0, -1);
                        }
                    }
                    if(Main.netMode != NetmodeID.Server)
                    {
                        if (faces == null)
                        {
                            foreach (ThroeFace tF in faces)
                            {
                                //Spawn dust where each face used to be
                                //TODO!
                            }
                        }
                        //Spawn dust in an explosion. Make sound effects!
                        //TODO!
                    }
                }
                NPC.scale = NPC.ai[1];
            }
            else
            {
                //Wander around in a small area
                NPC.ai[0]--;
                if (NPC.ai[0] > 0)
                {
                    Vector2 target = new Vector2(NPC.ai[1], NPC.ai[2]);

                    Vector2 toTarget = target - NPC.Center;
                    Vector2 toPlayer = player.Center - NPC.Center;
                    float dist = toPlayer.Length();
                    //NPC.Center = Vector2.Lerp(NPC.Center, target, 0.01f);
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
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 10);
            Vector2 drawPos = NPC.Center - screenPos;
            Rectangle frame = new Rectangle(0, NPC.frame.Y, texture.Width, texture.Height / 5);
            if (faces != null && NPC.ai[3] >= 0)
            {
                int i = 0;
                foreach(ThroeFace tF in faces)
                {
                    float circular = MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.5f + 90 * i++);
                    Vector2 cloneLocation = drawPos + tF.position.RotatedBy(circular);
                    float sinusoid = MathF.Sin(MathHelper.ToRadians(tF.AI * 4));
                    for (int j = 0; j < tF.oldPos.Count; j++)
                    {
                        float perc = 1 - j / (float)NPC.oldPos.Length;
                        Main.spriteBatch.Draw(texture, drawPos + tF.oldPos[j].RotatedBy(circular - MathHelper.ToRadians(1.5f * j + 1)), frame, drawColor * perc * 0.5f * (0.5882f + sinusoid * 0.196f), NPC.rotation, drawOrigin, perc * (0.875f - sinusoid * 0.05f), SpriteEffects.None, 0f);
                    }
                    spriteBatch.Draw(texture, cloneLocation, frame, drawColor * (0.5882f + sinusoid * 0.196f), NPC.rotation, drawOrigin, 0.875f - sinusoid * 0.05f, SpriteEffects.None, 0f);
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
                Main.spriteBatch.Draw(texture, center - Main.screenPosition, frame, drawColor * perc * 0.5f, NPC.rotation, drawOrigin, perc * (NPC.scale + 0.05f * sin), SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(texture, drawPos, frame, drawColor, NPC.rotation, drawOrigin, NPC.scale + 0.05f * sin, SpriteEffects.None, 0f);
            //texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/BleedingGhastGlow");
            //spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, 48, 56), Color.White, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public void TeleportToAdjacentArea()
		{

		}
		public override void ModifyIncomingHit(ref NPC.HitModifiers modifiers)
		{
            if (NPC.life >= NPC.lifeMax)
            {
                modifiers.SourceDamage *= 0.5f;
                modifiers.SetMaxDamage(NPC.lifeMax / 2);
            }
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (Main.netMode == NetmodeID.Server)
                return;
            if (NPC.life > 0)
            {
                int num = 0;
                while (num < hit.Damage / NPC.lifeMax * 50.0)
                {
                    Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2Circular(1, 1), ColorHelpers.AVDustColor, -2);
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
                    Dust dust = PixelDust.Spawn(NPC.position, NPC.width, NPC.height, Main.rand.NextVector2CircularEdge(2, 3), ColorHelpers.AVDustColor, -2);
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
	}
}