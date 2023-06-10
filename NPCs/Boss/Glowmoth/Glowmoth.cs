using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Pyramid;
using SOTS.Projectiles.Pyramid;
using SOTS.WorldgenHelpers;
using System;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Boss.Glowmoth
{
	[AutoloadBossHead]
	public class Glowmoth : ModNPC
	{
		public Color glowColor = Color.White;
		public const int InitializationPhase = -1;
		public const int WanderPhase = 0;
		public const int ScatterShotPhase = 1;
		private float AI0
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AI1
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AI2
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float AI3
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 10;
			NPCID.Sets.TrailCacheLength[Type] = 10;
			NPCID.Sets.TrailingMode[Type] = 0;
		}
        public override void SetDefaults()
		{
			NPC.lifeMax = 2400;   
            NPC.damage = 20; 
            NPC.defense = 10;  
            NPC.knockBackResist = 0f;
            NPC.width = 62;
            NPC.height = 94;
            NPC.value = 3000;
            NPC.npcSlots = 5f;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.netUpdate = true;
            NPC.HitSound = SoundID.NPCHit32;
            NPC.DeathSound = SoundID.NPCDeath5;
            NPC.netAlways = true;
			NPC.boss = true;
			NPC.dontTakeDamage = true;
			NPC.alpha = 255;
			Music = MusicID.Boss2; //MusicLoader.GetMusicSlot(Mod, "Sounds/Music/PutridPinky");
			SceneEffectPriority = SceneEffectPriority.BossLow;
		}
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.lifeMax = (int)(NPC.lifeMax * bossLifeScale * 14 / 20); //140%, 210%
			NPC.damage = (int)(NPC.damage * 0.75f); //150%, 225%
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Texture2D textureG = (Texture2D)Request<Texture2D>("SOTS/NPCs/Boss/Glowmoth/GlowmothGlow");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / Main.npcFrameCount[NPC.type] / 2);
			int frameHeight = texture.Height / Main.npcFrameCount[NPC.type];
			Vector2 drawPos = NPC.Center - screenPos;
			Color glow2 = glowColor * 0.5f;
			glow2.A = 0;
			int CurrentFrameNumber = (int)(NPC.frame.Y / frameHeight + 0.5f);
			for (int j = 0; j < NPCID.Sets.TrailCacheLength[Type]; j++)
            {
				int thisFrameNumber = (CurrentFrameNumber + j) % 10;
				Vector2 position = NPC.oldPos[j] + new Vector2(NPC.width, NPC.height) / 2;
				float alphaMult = 1 - (j / (float)NPCID.Sets.TrailCacheLength[Type]);
				spriteBatch.Draw(textureG, position - Main.screenPosition, new Rectangle(0, thisFrameNumber * frameHeight, texture.Width, NPC.height), NPC.GetAlpha(ColorHelpers.VibrantColorAttempt(j * 12)) * alphaMult * 0.4f, NPC.rotation, drawOrigin, alphaMult, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(drawColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			for (int i = 0; i < 4; i++)
			{
				Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * i + MathHelper.TwoPi / 270f * AI3);
				spriteBatch.Draw(textureG, drawPos + circular, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(glow2) * 0.75f, NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(textureG, drawPos, new Rectangle(0, NPC.frame.Y, texture.Width, NPC.height), NPC.GetAlpha(glowColor), NPC.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			return false;
		}
		public bool RunOnce = true;
        public override bool PreAI()
        {
			if(RunOnce)
            {
				SwapPhase(InitializationPhase);
				RunOnce = false;
            }
			glowColor = ColorHelpers.VibrantColor;
			return true;
        }
        public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			if (AI0 == InitializationPhase)
			{
				NPC.alpha -= 4;
				if(NPC.alpha <= 0)
                {
					NPC.alpha = 0;
					SwapPhase(WanderPhase);
					return;
				}
				if(NPC.alpha <= 20)
                {
					if(AI2 != -1)
                    {
						if(Main.netMode != NetmodeID.MultiplayerClient)
                        {
							//fire out projectile waves
                        }
						SOTSUtils.PlaySound(SoundID.Roar, NPC.Center, 1.0f, 0.3f);
                    }
					AI2 = -1;
                }
			}
			if(AI0 == WanderPhase)
            {
				Vector2 toPlayer = player.Center - NPC.Center;
				float dist = toPlayer.Length();
				toPlayer = toPlayer.SafeNormalize(Vector2.Zero) * (6.5f + dist / 80f);
				if(dist < 240)
                {
					toPlayer *= 0.25f;
                }
				NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer, 0.0425f + dist / 24000f);
				if(AI1 >= 240)
				{
					//SwapPhase(ScatterShotPhase);
					//return;
				}
				AI1++;
			}
		}
        public override void PostAI()
        {
			float sinusoid = (float)Math.Sin(MathHelper.ToRadians(AI3 * 5));
			NPC.position += new Vector2(0, -sinusoid * 0.825f);
			NPC.alpha = Math.Clamp(NPC.alpha, 0, 255);
			float scalingFactor = 1 - NPC.alpha / 255f;
			AI3 += scalingFactor * scalingFactor;
			NPC.rotation = NPC.velocity.X * 0.05f;
		}
		public void SwapPhase(int Phase)
        {
			AI1 = 0;
			AI2 = 0;
			if(Phase == InitializationPhase)
			{
				NPC.dontTakeDamage = true;
			}
			else
				NPC.dontTakeDamage = false;
			if (Phase == WanderPhase)
			{

			}
			if (Phase == ScatterShotPhase)
			{

			}
			AI0 = Phase;
			NPC.netUpdate = true;
		}
        public override void FindFrame(int frameHeight) 
		{
			NPC.frameCounter++;
			if (NPC.frameCounter >= 3.5f) 
			{
				NPC.frameCounter -= 3.5f;
				NPC.frame.Y += frameHeight;
				if(NPC.frame.Y >= Main.npcFrameCount[NPC.type] * frameHeight)
				{
					NPC.frame.Y = 0;
				}
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ItemType<GlowmothBag>()));
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < damage / NPC.lifeMax * 50.0)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silk, (float)(2 * hitDirection), -2f);
					num++;
				}
			}
            else
			{
				for (int k = 0; k < 30; k++)
				{
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Silk, (float)(2 * hitDirection), -2f);
				}
			}		
		}
	}
}





















