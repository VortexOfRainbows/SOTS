using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Conduit;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Anomaly;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Tide
{
	public class PhantarayBig : ModNPC
	{
		public const float AttackWindup = 75;
        public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 11;
		}
        public override void SetDefaults()
		{
            NPC.lifeMax = 900;   
            NPC.damage = 75; 
            NPC.defense = 24;  
            NPC.knockBackResist = 0f;
            NPC.width = 100;
            NPC.height = 100;
            NPC.value = Item.buyPrice(0, 0, 5, 0);
            NPC.npcSlots = 1f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.Splash;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
			//Banner = NPC.type;
			//BannerItem = ItemType<UltracapBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / (2 * Main.npcFrameCount[NPC.type]));
			Vector2 drawPos = NPC.Center - screenPos;
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
        }
		Vector2 wanderDirection = Vector2.Zero;
		bool previousWet;
		public override void AI()
		{
			bool canSeePlayer = false;
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (Collision.CanHitLine(NPC.position, NPC.width, NPC.height, player.position, player.width, player.height))
			{
				canSeePlayer = true;
			}
			else canSeePlayer = false;
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
                    NPC.ai[1]--;
                    if (Math.Abs(NPC.velocity.X) > 1)
                        NPC.velocity.X *= 0.9925f;
				}
			}
			else
			{
                if(NPC.ai[0] % 150 == 0 || wanderDirection == Vector2.Zero)
				{
					wanderDirection = Main.rand.NextVector2CircularEdge(1, 1);
					wanderDirection.Y *= 0.5f;
					wanderDirection.Y += 0.5f;
                }
                NPC.velocity += wanderDirection * speed * sinusoid * 0.4f;
                NPC.velocity *= 0.92f;
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
			NPC.position -= NPC.velocity;
			NPC.position += Collision.TileCollision(NPC.position + new Vector2(8, 8), NPC.velocity, NPC.width - 16, NPC.height - 16, true, true);
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





















