using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Conduit;
using SOTS.Items.Pyramid;
using SOTS.Projectiles;
using SOTS.Projectiles.Anomaly;
using SOTS.Projectiles.Pyramid;
using SOTS.Projectiles.Tide;
using SOTS.WorldgenHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Tide
{
	public class PhantarayBig : ModNPC
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(wanderDirection);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            wanderDirection = reader.ReadVector2();
        }
        public const float AttackWindup = 75;
        Vector2 wanderDirection = Vector2.Zero;
        bool previousWet;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 11;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 450;   
            NPC.damage = 75; 
            NPC.defense = 20;  
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.SplashWeak;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<UltracapBanner>();
		}
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            NPC.damage = (int)(NPC.damage * 5 / 6);
            NPC.lifeMax = (int)(NPC.lifeMax * 3 / 5);
        }
        public override void UpdateLifeRegen(ref int damage)
        {
			if(!NPC.wet && NPC.ai[1] <= -60)
			{
				int lossPerSecond = (int)(NPC.ai[1] / 12f);
                NPC.lifeRegen += lossPerSecond;
				damage -= (int)(lossPerSecond / 8f);
            }
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
            Texture2D textureGlow = ModContent.Request<Texture2D>("SOTS/NPCs/Tide/PhantarayBigGlow").Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / (2 * Main.npcFrameCount[NPC.type]));
			Vector2 drawPos = NPC.Center - screenPos;
            spriteBatch.Draw(textureGlow, drawPos, NPC.frame, Color.White * (1f - (NPC.alpha / 255f) * 0.5f), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			float percentCharged = NPC.ai[3] / 120f;
			if (percentCharged > 1)
				percentCharged = 1;
			for(int i = 0; i < 6; i++)
			{
				Vector2 circular = new Vector2(24 * (1 - percentCharged), 0).RotatedBy(percentCharged * MathHelper.TwoPi + MathHelper.TwoPi * i / 6f);
				spriteBatch.Draw(textureGlow, drawPos + circular, NPC.frame, new Color(200, 100, 100, 0) * (1f - (NPC.alpha / 255f) * 0.5f * percentCharged) * percentCharged, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
        }
		public override void AI()
		{
			bool canSeePlayer;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
			{
				canSeePlayer = true;
			}
			else 
				canSeePlayer = false;
            Vector2 toPlayer = player.Center - NPC.Center;
			float length = toPlayer.Length();
			float speed = 3f + length * 0.00002f;
			if (length < 360)
				speed *= 0.2f + 0.8f * length / 360f;
			toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
			float sinusoid = 0.5f + 0.5f * (float)Math.Sin(NPC.ai[0]++ * MathHelper.TwoPi / 150f);
			if (NPC.velocity.Length() >= 0.1f)
			{
				NPC.rotation = NPC.velocity.ToRotation() + MathHelper.PiOver2;
			}
			Tile tile = Framing.GetTileSafely((NPC.Center / 16).ToPoint());
			if (tile.LiquidAmount > 0)
			{
				NPC.wet = true;
			}
			else
				NPC.wet = false;
			if (NPC.ai[3] > 0)
			{
				NPC.ai[3]++;
				float speedSh = 2f;
				if (!NPC.wet && NPC.ai[1] <= 0)
                {
					float bonus = NPC.ai[1] / -60f;
					if (bonus > 5)
						bonus = 5;
                    NPC.ai[3] += bonus;
                    speedSh += bonus / 4.5f;
                }
                if (NPC.ai[3] > 120)
				{
					NPC.ai[3] = 0;
                    if (Main.netMode == NetmodeID.Server)
                        NPC.netUpdate = true;
					if(Main.netMode != NetmodeID.MultiplayerClient)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(0, -22).RotatedBy(NPC.rotation), toPlayer.SafeNormalize(Vector2.Zero) * speedSh + Main.rand.NextVector2Circular(1, 1) * 0.1f * speedSh * speedSh, ModContent.ProjectileType<PhantarayBall>(), (int)(NPC.GetBaseDamage() / 2 * 0.75f), 2f, Main.myPlayer);
					}
                }
			}
			if(!NPC.wet && NPC.ai[1] <= 0)
            {
				if (NPC.ai[1] < -180)
				{
					if (NPC.ai[1] % 120 == 0)
                    {
                        if (canSeePlayer)
                        {
                            NPC.velocity += toPlayer * 18.5f;
                        }
						else
                        {
                            wanderDirection = Main.rand.NextVector2CircularEdge(1, 1);
                            wanderDirection.Y *= 0.5f;
                            wanderDirection.Y -= 0.5f;
                            NPC.velocity += wanderDirection * 18.5f;
                        }
                        if (Main.netMode == NetmodeID.Server)
                            NPC.netUpdate = true;
                    }
				}
            }
			if (canSeePlayer)
            {
                if (!NPC.wet)
				{
					NPC.velocity.Y += 0.1575f; //gravity when out of water
					if (NPC.velocity.Y < 0)
						NPC.velocity.Y *= 0.978f;
				}
				if (NPC.ai[1] <= 0)
				{
					if (previousWet != NPC.wet)
					{
						NPC.velocity += toPlayer * speed * 2.5f;
						NPC.ai[1] = 60;
						if (Main.netMode == NetmodeID.Server)
							NPC.netUpdate = true;
					}
					else if (NPC.wet)
					{
						NPC.velocity += toPlayer * speed * sinusoid * (NPC.ai[2] / 30f);
						NPC.velocity *= 0.815f;
						if(player.wet)
						{
							NPC.velocity *= 0.95f;
						}
					}
					else if (!NPC.wet)
					{
						NPC.velocity += new Vector2(toPlayer.X * 0.35f, 0.2f) * 0.35f * sinusoid;
						NPC.velocity.X *= 0.95f;
					}
				}
				else
				{
					if (NPC.wet)
                    {
                        if (NPC.velocity.Y > 5)
                            NPC.velocity.Y *= 0.978f;
                        NPC.velocity.Y += 0.04f;
                        NPC.velocity.X *= 0.9f;
                        NPC.velocity += new Vector2(toPlayer.X * 0.45f, 0.12f) * 0.35f * sinusoid * (NPC.ai[2] / 30f);
                    }
                    else
                        NPC.velocity += new Vector2(toPlayer.X * 0.45f, 0.12f) * 0.35f * sinusoid * (1 - NPC.ai[2] / 30f);
                    if (Math.Abs(NPC.velocity.X) > 1)
                        NPC.velocity.X *= 0.9925f;
                }
                if (NPC.ai[3] <= 0)
                {
                    if (Main.netMode == NetmodeID.Server)
                        NPC.netUpdate = true;
                    NPC.ai[3] = 1;
                }
			}
			else if(NPC.wet)
			{
                if(NPC.ai[0] % 150 == 0 || wanderDirection == Vector2.Zero)
				{
					wanderDirection = Main.rand.NextVector2CircularEdge(1, 1);
					wanderDirection.Y *= 0.5f;
					wanderDirection.Y += 0.5f;
                    if (Main.netMode == NetmodeID.Server)
                        NPC.netUpdate = true;
                }
                NPC.velocity += wanderDirection * speed * sinusoid * 0.3f;
                NPC.velocity *= 0.875f;
            }
			if(previousWet != NPC.wet)
                SOTSUtils.PlaySound(SoundID.Splash, NPC.Center, 0.8f, -0.4f);
            previousWet = NPC.wet;
            NPC.velocity.X /= 0.93f;
            CheckOtherCollision();
            NPC.alpha = (int)MathHelper.Lerp(195, 50, sinusoid);
			if(NPC.wet)
			{
				NPC.ai[2]++;
			}
			else
			{
				NPC.ai[2]--;
			}
			NPC.ai[2] = MathHelper.Clamp(NPC.ai[2], 0, 30);
			int alphaToGo = 235;
			if (player.wet)
				alphaToGo = 205;
			NPC.alpha = (int)MathHelper.Lerp(NPC.alpha, alphaToGo, NPC.ai[2] / 30f);
			if (NPC.alpha > 200)
				NPC.dontTakeDamage = !player.wet;
			else
				NPC.dontTakeDamage = false;
			NPC.velocity = Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true, true);
            NPC.ai[1]--;
        }
		public void CheckOtherCollision()
        {
			Vector2 nudge = Vector2.Zero;
			for(int i = 0; i < Main.maxNPCs; i++)
            {
				NPC npc = Main.npc[i];
				if (npc.active && npc.type == Type && npc.Hitbox.Intersects(NPC.Hitbox))
                {
					Vector2 away = NPC.Center - npc.Center;
					nudge += away * 0.1f;
                }
            }
			NPC.velocity += nudge;
        }
		public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 6f) 
			{
				NPC.frameCounter -= 6f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			//npcLoot.Add(ItemDropRule.Common(ItemType<SkipSoul>(), 1, 1, 2));
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life <= 0)
			{
				for (int k = 0; k < 30; k++)
				{
					Dust d = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustType<CopyDust4>(), (float)(2 * hit.HitDirection), -2f);
					d.velocity *= 1.0f;
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.5f;
					d.color = ColorHelpers.TideColor;
				}
			}		
		}
	}
}





















