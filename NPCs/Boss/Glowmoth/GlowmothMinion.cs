using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Earth.Glowmoth;
using System;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Glowmoth	
{
	public class GlowmothMinion : ModNPC
	{
		public bool Blue => NPC.ai[3] == 0;
		public override string Texture => "SOTS/Projectiles/Earth/Glowmoth/ArrowMoth";
        private float ownerID
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float aiCounter
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float aiCounter2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		public float deathCounter = 0;
		public override void SetStaticDefaults()
		{
			NPCID.Sets.TrailCacheLength[Type] = 10;
			NPCID.Sets.TrailingMode[Type] = 0;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 1;  
            NPC.damage = 28; 
            NPC.knockBackResist = 0.0f;
            NPC.width = 24;
            NPC.height = 20;
			Main.npcFrameCount[NPC.type] = 3;  
            NPC.value = 0;
            NPC.npcSlots = 0f;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit11;
			NPC.DeathSound = SoundID.NPCDeath7;
			NPC.alpha = 255;
			NPC.dontTakeDamage = true;
			//Banner = NPC.type;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return NPC.alpha <= 0;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Texture2D texture2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/ArrowMothTrail").Value;
			Vector2 trailOrigin = new Vector2(texture2.Width - 6, texture2.Height * 0.5f);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 3 * 0.5f);
			Vector2 drawPos = NPC.Center - screenPos;
			float scaleOffSpeed = MathHelper.Clamp(NPC.velocity.Length() / 15f, 0, 1);
			Color trailColor = new Color(60, 60, 70, 0);
			if (!Blue)
			{
				trailColor = new Color(60, 70, 60, 0);
				texture2 = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/MothMinionTrail").Value;
				texture = (Texture2D)Request<Texture2D>("SOTS/Projectiles/Earth/Glowmoth/MothMinion");
			}
			for (int k = 1; k < NPC.oldPos.Length - 1; k++)
			{
                if (NPC.oldPos[k + 1].Distance(NPC.position) > 160)
                {
					break;
                }
				for (int j = 0; j < (SOTS.Config.lowFidelityMode ? 1 : 2); j++)
				{
					float scaleMult = ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) * (1.0f - j * 0.2f);
					Vector2 toNextPosition = NPC.oldPos[k] - NPC.oldPos[k + 1];
					Vector2 drawPos2 = NPC.oldPos[k] + drawOrigin - Main.screenPosition;
					Color color = trailColor * scaleMult * (0.5f + scaleOffSpeed * 0.5f);
					spriteBatch.Draw(texture2, drawPos2 + toNextPosition.SafeNormalize(Vector2.Zero) * 3 * j, null, color * 0.9f, toNextPosition.ToRotation(), trailOrigin, new Vector2(toNextPosition.Length() / texture2.Width * 2, NPC.scale * (1.0f - 0.25f * scaleOffSpeed) * scaleMult), SpriteEffects.None, 0);
				}
			}
			spriteBatch.Draw(texture, drawPos, NPC.frame, NPC.GetAlpha(Color.White), NPC.rotation, drawOrigin, NPC.scale, NPC.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreAI()
		{
			return true;
		}
		public override void AI()
		{
			deathCounter++;
			NPC.TargetClosest(false);
			NPC owner = Main.npc[(int)ownerID];
			if (owner.type != ModContent.NPCType<Glowmoth>() || !owner.active || deathCounter > 1100 + (NPC.whoAmI % 21 * 4))
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.StrikeNPC(10, 0, Main.rand.Next(2) * 2 - 1, false, true, false);
				}
				return;
			}
			Vector2 circular = new Vector2(128, 0).RotatedBy(MathHelper.ToRadians(aiCounter));
			circular.X *= 0.35f;
			circular = circular.RotatedBy(MathHelper.ToRadians(aiCounter2));
			Vector2 orbit = owner.Center + circular;
			Vector2 toOrbit = orbit - NPC.Center;
			float dist = toOrbit.Length();
			Vector2 velocity = toOrbit.SafeNormalize(Vector2.Zero);
			float speed = 8f + (NPC.whoAmI % 10 * 0.3f) + (float)Math.Sqrt(dist);
			if(speed > dist)
            {
				speed = dist + speed / 20f;
            }
			velocity *= speed * (1 - NPC.alpha / 255f);
			NPC.velocity = Vector2.Lerp(NPC.velocity, velocity, 0.075f - (NPC.whoAmI % 6) / 300f);
			aiCounter += 1.5f;
			aiCounter2 += 0.3f;
			NPC.direction = NPC.velocity.X > 0 ? 1 : -1;
			NPC.rotation = NPC.velocity.X * 0.05f;
			if(NPC.alpha > 0)
            {
				NPC.alpha -= 6;
			}
			if (NPC.alpha < 0)
			{
				NPC.dontTakeDamage = false;
				NPC.alpha = 0;
			}
		}
        public override void FindFrame(int frameHeight)
        {
			NPC.frameCounter += 1f + (NPC.whoAmI % 4 * 0.25f);
			if(NPC.frameCounter >= 5 + NPC.whoAmI % 10)
            {
				NPC.frameCounter -= 5;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y > frameHeight * 2)
                {
					NPC.frame.Y = 0;
                }
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			Color color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(180), true);
			if(Blue)
				color = ColorHelpers.VibrantColorAttempt(180 + Main.rand.NextFloat(180), true);
			color.A = 0;
			for (int k = 0; k < 15; k++)
			{
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustType<PixelDust>(), (float)(2 * hitDirection), 0f, 0, default, 1.2f);
				dust.fadeIn = 6;
				dust.noGravity = true;
				dust.color = color;
			}
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				Dust dust = Dust.NewDustDirect(NPC.oldPos[k] + NPC.Size / 2 - new Vector2(5, 5), 0, 0, DustType<CopyDust4>(), (float)(2 * hitDirection));
				dust.fadeIn = 0.2f;
				dust.noGravity = true;
				dust.velocity *= 0.8f;
				dust.velocity += Main.rand.NextVector2Circular(1, 1);
				dust.scale *= 1.5f;
				dust.color = color;
			}
		}
        public override void OnKill()
		{
        }
        public override bool PreKill()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient && Main.expertMode)
			{
				Vector2 toPlayer = Main.player[NPC.target].Center - NPC.Center;
				Projectile.NewProjectileDirect(NPC.GetSource_FromAI(), NPC.Center, toPlayer.SafeNormalize(Vector2.Zero) * 2f, ModContent.ProjectileType<WaveBall>(), 10, 1f, Main.myPlayer, -0.75f, 0);
			}
			return false;
		}
	}
}