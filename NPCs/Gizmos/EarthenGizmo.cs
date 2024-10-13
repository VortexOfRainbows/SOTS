using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.WorldgenHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Gizmos
{
	public class EarthenGizmo : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(AIMode);
            writer.Write(AI2);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AIMode = reader.ReadInt32();
            AI2 = reader.ReadInt32();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 8);
            Vector2 drawPos = NPC.Center - screenPos + new Vector2(0, -6);
            spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, texture.Height / 4), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, NPC.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 4;
        }
        public override void SetDefaults()
        {
            NPC.aiStyle = -1; 
            NPC.lifeMax = 55;   
            NPC.damage = 20; 
            NPC.defense = 8;  
            NPC.knockBackResist = .6f;
            NPC.width = 32; //Has to be smaller than the sprite size to allow jumping over blocks properly
            NPC.height = 40; //Has to be shorter than the sprite height to prevent falling through platforms erroneously 
            NPC.value = 1200;
            NPC.npcSlots = 0.5f;
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath14;
            NPC.netAlways = true;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<EarthenGizmoBanner>();
		}
        private Vector2 TrackPlayerPositon = Vector2.Zero;
		public int AIMode = 0;
        private int AI2 = 0;
		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
            if (AIMode < 80 || TrackPlayerPositon == Vector2.Zero)
                TrackPlayerPositon = player.Center;
            Vector2 toPlayer = TrackPlayerPositon - NPC.Center;
			float toPlayerLen = toPlayer.Length();
			if(toPlayerLen < 2400) //150 blocks radius
            {
                NPC.DiscourageDespawn(600);
            }
            bool canSeePlayerShort = Collision.CanHitLine(NPC.Center + new Vector2(0, 18), 0, 0, player.Center + new Vector2(0, 18), 0, 0);
            bool canSeePlayerTall = Collision.CanHitLine(NPC.Center - new Vector2(0, 40), 0, 0, player.Center - new Vector2(0, 40), 0, 0);
            bool canSeePlayerLeft = Collision.CanHitLine(NPC.Center - new Vector2(20, 0), 0, 0, player.Center + new Vector2(20, 0), 0, 0);
            bool canSeePlayerRight = Collision.CanHitLine(NPC.Center + new Vector2(20, 0), 0, 0, player.Center - new Vector2(20, 0), 0, 0);
            bool cantSeeAtAll = !canSeePlayerLeft && !canSeePlayerRight && !canSeePlayerShort && !canSeePlayerTall;
            if (cantSeeAtAll)
            {
				if(AIMode < 1)
                {
                    AIMode++;
                    if (AIMode == 1)
                    {
                        AIMode = 10;
                        NPC.netUpdate = true;
                    }
                } 
            }
            if (AIMode >= 1) //Drill mode
            {
                bool insideBlock = Collision.SolidTiles(NPC.Center - new Vector2(4, 8 + NPC.height / 2), 8, 4);
                bool centerInsideBlock = Collision.SolidTiles(NPC.Center - new Vector2(8, 8), 16, 16);
                NPC.aiStyle = -1;
                if (AIMode > 65)
                {
                    if (centerInsideBlock)
                    {
                        if(centerInsideBlock && AIMode > 70 && AIMode < 80)
                        {
                            if (AI2 % 36 == 0)
                            {
                                SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Enemies/EarthenElementalDig"), NPC.Center, 0.7f, -0.2f);
                            }
                            AI2++;
                        }
                        for(float i = 0; i < 1f; i += 0.5f)
                        {
                            if (Main.rand.NextBool(3))
                            {
                                Dust dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 7, NPC.Center.Y - 7) + NPC.velocity.SNormalize() * 16 + NPC.velocity * i, 4, 4, ModContent.DustType<CopyDust4>());
                                dust.color = new Color(255, 191, 0);
                                dust.noGravity = true;
                                dust.fadeIn = 0.1f;
                                dust.scale *= 1.3f;
                                dust.velocity *= 0.3f;
                                dust.alpha = 100;
                                dust.velocity += NPC.velocity * 0.2f;
                            }
                            else
                            {
                                PixelDust.Spawn(NPC.Center - new Vector2(5, 5) + NPC.velocity.SNormalize() * 16 + NPC.velocity * i, 0, 0, NPC.velocity * 0.2f + Main.rand.NextVector2Circular(1, 1), new Color(255, 191, 0) * 0.6f, 8);
                            }
                        }
                        int minTilePosX = (int)(NPC.position.X / 16.0) - 1;
                        int maxTilePosX = (int)((NPC.position.X + NPC.width) / 16.0) + 2;
                        int minTilePosY = (int)(NPC.position.Y / 16.0) - 1;
                        int maxTilePosY = (int)((NPC.position.Y + NPC.height) / 16.0) + 2;
                        if (minTilePosX < 0)
                            minTilePosX = 0;
                        if (maxTilePosX > Main.maxTilesX)
                            maxTilePosX = Main.maxTilesX;
                        if (minTilePosY < 0)
                            minTilePosY = 0;
                        if (maxTilePosY > Main.maxTilesY)
                            maxTilePosY = Main.maxTilesY;
                        for (int i = minTilePosX; i < maxTilePosX; ++i)
                        {
                            for (int j = minTilePosY; j < maxTilePosY; ++j)
                            {
                                if (SOTSWorldgenHelper.TrueTileSolid(i, j))
                                {
                                    Vector2 vector2 = new Vector2(i * 16, j * 16);
                                    if (Main.rand.NextBool(80 - (AI2 % 36)) && NPC.position.X + NPC.width > vector2.X && NPC.position.X < vector2.X + 16.0 && NPC.position.Y + NPC.height > vector2.Y && NPC.position.Y < vector2.Y + 16.0)
                                    {
                                        WorldGen.KillTile(i, j, true, true, false);
                                    }
                                }
                            }
                        }
                    }
                    NPC.behindTiles = true;
                    NPC.noTileCollide = true;
                    NPC.noGravity = true;
                }
                else
                    AI2 = 0;
                if (AIMode < 70)
				{
					AIMode++;
					NPC.velocity.X *= 0.985f;
                    if (AIMode == 70 || insideBlock)
                    {
                        NPC.netUpdate = true;
                        if (!insideBlock)
                            NPC.velocity.Y -= 13.5f;
                        else
                            NPC.velocity.Y -= 2f;
                        if (AIMode == 70)
                        {
                            SOTSUtils.PlaySound(SoundID.Item53, NPC.Center, 1.0f, -0.5f);
                            for (int i = 0; i < 30; i++)
                            {
                                if (Main.rand.NextBool(3))
                                {
                                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 7, NPC.Center.Y - 7) - NPC.velocity.SNormalize() * 16, 4, 4, ModContent.DustType<CopyDust4>());
                                    dust.color = new Color(255, 191, 0);
                                    dust.noGravity = true;
                                    dust.fadeIn = 0.1f;
                                    dust.scale *= 1.5f;
                                    dust.velocity *= -2f * Main.rand.NextFloat();
                                    dust.alpha = 100;
                                    dust.velocity += NPC.velocity * 0.5f * Main.rand.NextFloat();
                                }
                                else
                                {
                                    PixelDust.Spawn(NPC.Center - new Vector2(5, 5) - NPC.velocity.SNormalize() * 16, 0, 0, NPC.velocity * -0.5f * Main.rand.NextFloat() + Main.rand.NextVector2Circular(5, 5), new Color(255, 191, 0) * 0.7f, 7).scale = 1.5f;
                                }
                            }
                        }
                        AIMode = 70;
                    }
				}
				else
                {
                    float bestDistToPlayer = toPlayerLen + 64;
                    Vector2 destination = NPC.Center;
                    float bestExitDist = toPlayerLen + 64;
                    Vector2 bestExit = NPC.Center;

                    Vector2 paddVelocity = Vector2.Zero; //This value will help prevent the drill from drilling too close to being out of tiles
					for(int j = 0; j < 16; j++)
                    {
                        bool lastHadNoTile = false;
                        float currentBest = toPlayerLen;
                        Vector2 myPos = NPC.Center;
                        for (int i = 0; i <= 11; i++)
                        {
                            Vector2 direction = new Vector2(-1, 0).RotatedBy(j * MathHelper.PiOver4 / 2f);
                            myPos += direction * 16;
                            bool newPosHasTile = SOTSWorldgenHelper.TrueTileSolid((int)myPos.X / 16, (int)myPos.Y / 16, false);
                            bool noTilesToDig = !newPosHasTile && lastHadNoTile;
                            float currentDistToPlayer = TrackPlayerPositon.Distance(myPos);
                            if (noTilesToDig)
                            {
                                if(i < 3 && j % 2 == 0)
                                {
                                    paddVelocity -= direction;
                                }
                                currentDistToPlayer += 32;
                            }
                            if (currentDistToPlayer < currentBest)
                            {
                                currentBest = currentDistToPlayer;
                            }
                            if (noTilesToDig || i >= 11)
                            {
                                if(noTilesToDig)
                                {
                                    currentBest += (bestDistToPlayer - currentBest) * 0.5f + 32;
                                }
                                if (currentBest < bestDistToPlayer)
                                {
                                    bestDistToPlayer = currentBest;
                                    destination = myPos;
                                }
                                if (j % 4 == 0)
                                {
                                    float currentExit = TrackPlayerPositon.Distance(myPos);
                                    if(j == 0 || j == 8)
                                    {
                                        currentExit += i * 16;
                                    }
                                    else
                                    {
                                        currentExit += i * 8;
                                        if(MathF.Abs(toPlayer.X) < 160)
                                        {
                                            currentExit -= 32;
                                        }
                                    }
                                    if (currentExit < bestExitDist && noTilesToDig)
                                    {
                                        bestExitDist = currentExit;
                                        bestExit = myPos;
                                    }
                                }
                                break;
                            }
                            lastHadNoTile = !newPosHasTile;
                        }
                    }
                    bool falseExit = false;
                    if(bestExit == NPC.Center)
                    {
                        bestExit = player.Center;
                        falseExit = true;
                    }
					Vector2 toDest = destination - NPC.Center;
                    if(AIMode < 80 && AIMode >= 73)
                    {
					    NPC.velocity += toDest.SNormalize() * (3f + toPlayerLen / 480f) + paddVelocity * 0.65f;
                    }
                    Vector2 horizontalBiasedToPlayer = toPlayer;
                    horizontalBiasedToPlayer.Y *= 2f;
                    float hDist = horizontalBiasedToPlayer.Length();
                    float RemainingTravelableDistance = toPlayerLen - bestDistToPlayer;
                    //Main.NewText(RemainingTravelableDistance);
                    bool readyToExit = (RemainingTravelableDistance < 16 && AIMode >= 73 && (RemainingTravelableDistance != 0 || !centerInsideBlock || AIMode >= 79)) || (hDist < 240 && AIMode >= 79);
                    if (readyToExit && AIMode < 80)
                    {
                        NPC.netUpdate = true;
                        SOTSUtils.PlaySound(new Terraria.Audio.SoundStyle("SOTS/Sounds/Enemies/EarthenElementalDig"), NPC.Center, 0.8f, -0.1f);
                        AIMode = 80;
                    }
                    if(AIMode >= 80)
                    {
                        Vector2 toExit = bestExit - NPC.Center;
                        if (falseExit)
                            toExit.X *= 0.5f;
                        //Main.NewText(toExit);
                        if (centerInsideBlock)
                        {
                            NPC.velocity *= 0.825f;
                            NPC.velocity += toExit.SNormalize() * 1.56f;
                        }
                        else
                        {
                            if (AI2 != 0)
                            {
                                SOTSUtils.PlaySound(SoundID.Item53, NPC.Center, 1.0f, -0.5f); //Play a biong sound after exiting the ground
                                NPC.netUpdate = true;
                                AI2 = 0;
                            }
                            if (cantSeeAtAll)
                            {
                                NPC.velocity *= 0.95f;
                                NPC.velocity += toExit.SNormalize() * 1.56f;
                            }
                        }
                        if (!centerInsideBlock || Main.rand.NextBool(3))
                        {
                            for (int i = 0; i < 2; i++)
                            {
                                if (Main.rand.NextBool(3))
                                {
                                    Dust dust = Dust.NewDustDirect(new Vector2(NPC.Center.X - 7, NPC.Center.Y - 7) + NPC.velocity.SNormalize() * 16, 4, 4, ModContent.DustType<CopyDust4>());
                                    dust.color = new Color(255, 191, 0);
                                    dust.noGravity = true;
                                    dust.fadeIn = 0.1f;
                                    dust.scale *= 1.5f;
                                    dust.velocity *= 2f * Main.rand.NextFloat();
                                    dust.alpha = 100;
                                    dust.velocity += NPC.velocity * 0.5f * Main.rand.NextFloat();
                                }
                                else
                                {
                                    PixelDust.Spawn(NPC.Center - new Vector2(5, 5) + NPC.velocity.SNormalize() * 16, 0, 0, NPC.velocity * 0.5f * Main.rand.NextFloat() + Main.rand.NextVector2Circular(5, 5), new Color(255, 191, 0) * 0.7f, 7).scale = 1.5f;
                                }
                            }
                        }
                        AIMode++;
                        if(AIMode >= 110)
                        {
                            if (cantSeeAtAll)
                                AIMode = -1;
                            else
                                AIMode = -60;
                            NPC.netUpdate = true;
                        }
                    }
                    else
                    {
                        if (!insideBlock && AIMode < 73)
                        {
                            NPC.velocity.Y += 0.7f;
                            NPC.netUpdate = true;
                        }
                        else
                        {
                            if((AIMode < 79 && centerInsideBlock) || (insideBlock && AIMode < 73))
                            {
                                if(AIMode == 70)
                                {
                                    SOTSUtils.PlaySound(SoundID.Item52, NPC.Center, 1.0f, -0.5f);
                                }
                                AIMode++;
                            }
                        }
                    }
                    if(AIMode < 80)
                    {
                        if (AIMode < 71)
                            NPC.velocity *= 0.825f;
                        else
                            NPC.velocity *= 0.5f;
                    }
                    else
                        NPC.velocity *= 0.925f;
                    NPC.rotation = SOTSUtils.AngularLerp(NPC.rotation, NPC.velocity.ToRotation() + MathHelper.PiOver2, 0.175f);
                }
            }
			else //Walking mode (fighter AI)
            {
                NPC.aiStyle = NPCAIStyleID.Fighter;
                NPC.noTileCollide = false;
                NPC.behindTiles = false;
				NPC.noGravity = false;
				NPC.velocity.X += MathF.Sign(toPlayer.X) * 0.07f;
				if (Math.Abs(toPlayer.X) > 32)
					NPC.velocity.X *= 0.94f;
				NPC.velocity.Y += 0.05f; //Add additional gravity
				if (NPC.velocity.Y < 0)
					NPC.velocity.Y *= 0.99f;
            }
            if (NPC.velocity.Y < 3 && AIMode < 70)
                NPC.rotation = SOTSUtils.AngularLerp(NPC.rotation, 0, 0.25f);
            NPC.spriteDirection = NPC.direction;
		}
		private int frame = 0;
		public override void FindFrame(int frameHeight)
        {
            float frameSpeed = 10f;
            frame = frameHeight;
			float baseSpeed = 0.5f;
			if(AIMode >= 70)
			{
				baseSpeed += 2.5f;
			}
			NPC.frameCounter += baseSpeed + MathF.Sqrt(MathF.Abs(NPC.velocity.X));
			if (NPC.frameCounter >= frameSpeed) 
			{
				NPC.frameCounter -= frameSpeed;
				NPC.frame.Y += frame;
				if(NPC.frame.Y >= 4 * frame)
				{
					NPC.frame.Y = 0;
				}
			}
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfEarth>(), 1, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EarthenPlating>(), 1, 5, 10));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EarthDrive>(), 100));
        }
  //      public override void OnKill()
		//{
		//	//for (int k = 0; k < 7; k++)
		//	//{
		//	//	Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
		//	//	int damage2 = SOTSNPCs.GetBaseDamage(NPC) / 2;
		//	//	if (Main.netMode != NetmodeID.MultiplayerClient)
		//	//		Projectile.NewProjectile(NPC.GetSource_Death(), NPC.Center.X, NPC.Center.Y, circularLocation.X, circularLocation.Y, ProjectileType<Projectiles.SporeCloud>(), damage2, 2f, Main.myPlayer);
		//	//}
  //      }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / (double)NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2 * hit.HitDirection, -2f, Scale: 0.8f);
					num++;
				}
			}
            else
            {
            	for (int k = 0; k < 20; k++)
            	{
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2 * hit.HitDirection, -2f, Scale: 0.9f);
                }
                for (int i = 0; i < 3; i++)
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(0, NPC.height / 2), NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore1"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore2"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(NPC.width / 6, NPC.height / 2), NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore3"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(2 * NPC.width / 6, NPC.height / 2), NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore4"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(3 * NPC.width / 6, NPC.height / 2), NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore3"), 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position + new Vector2(4 * NPC.width / 6, NPC.height / 2), NPC.velocity, ModGores.GoreType("Gores/Gizmos/EarthenGizmoGore4"), 1f);
            }
        }
	}
}