using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Permafrost;
using SOTS.WorldgenHelpers;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Boss.Polaris
{	[AutoloadBossHead]
	public class Polaris : ModNPC
	{	
		int despawn = 0;
		private float AICycle
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float AICycle2
		{
			get => NPC.ai[1];
			set => NPC.ai[1] = value;
		}
		private float AICycle3
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float transition
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
        public override Color? GetAlpha(Color drawColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			NPCID.Sets.DebuffImmunitySets.Add(Type, new Terraria.DataStructures.NPCDebuffImmunityData
			{
				SpecificallyImmuneTo = new int[]
				{
					BuffID.Frostburn,
					BuffID.OnFire,
					BuffID.Ichor
				}
			});
		}
		public override void SetDefaults()
		{
            NPC.lifeMax = 36000;
            NPC.damage = 80; 
            NPC.defense = 28;  
            NPC.knockBackResist = 0f;
            NPC.width = 162;
            NPC.height = 162;
            Main.npcFrameCount[NPC.type] = 1;
            NPC.value = 100000;
            NPC.npcSlots = 1f;
            NPC.boss = true;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            Music = MusicLoader.GetMusicSlot(Mod, "Sounds/Music/Polaris");
			NPC.netAlways = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleListRed.Count; i++)
			{
				Color color = new Color(250, 100, 100, 0);
				Vector2 drawPos = particleListRed[i].position - screenPos;
				color = color * (0.3f + 0.7f * particleListRed[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleListRed[i].rotation, drawOrigin, particleListRed[i].scale * 1.0f, SpriteEffects.None, 0f);
				}
			}
			for (int i = 0; i < particleListBlue.Count; i++)
			{
				Color color = new Color(200, 250, 250, 0);
				Vector2 drawPos = particleListBlue[i].position - screenPos;
				color = color * (0.3f + 0.7f * particleListBlue[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleListBlue[i].rotation, drawOrigin, particleListBlue[i].scale * 1.0f, SpriteEffects.None, 0f);
				}
			}
			return true;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Mod.Assets.Request<Texture2D>("NPCs/Boss/Polaris/PolarisThruster").Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			for (int i = 0; i < 4; i++)
			{
				int direction = (i % 2) * 2 - 1;
				int yDir = 1;
				if(i >= 2)
                {
					yDir = -1;
				}
				Vector2 rotationOrigin = new Vector2(-3f * -direction, 6f) - NPC.velocity * 2.4f;
				float overrideRotation = rotationOrigin.ToRotation() - MathHelper.ToRadians(90);
				Vector2 fromBody = NPC.Center + new Vector2(direction * 32, 32 * yDir).RotatedBy(NPC.rotation);
				Vector2 drawPos = fromBody - screenPos + new Vector2(0, 2);
				spriteBatch.Draw(texture, drawPos, null, Color.White, NPC.rotation + overrideRotation, drawOrigin, 0.9f, direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
		List<FireParticle> particleListRed = new List<FireParticle>();
		List<FireParticle> particleListBlue = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleListBlue.Count; i++)
			{
				FireParticle particle = particleListBlue[i];
				particle.Update();
				if (!particle.active)
				{
					particleListBlue.RemoveAt(i);
					i--;
				}
			}
			for (int i = 0; i < particleListRed.Count; i++)
			{
				FireParticle particle = particleListRed[i];
				particle.Update();
				if (!particle.active)
				{
					particleListRed.RemoveAt(i);
					i--;
				}
			}
		}
		public override void PostAI()
		{
			for (int i = 0; i < 4; i++)
			{
				int direction = (i % 2) * 2 - 1;
				int yDir = 1;
				if (i >= 2)
				{
					yDir = -1;
				}
				Vector2 rotationOrigin = new Vector2(-3f * -direction, 6f) - NPC.velocity * 2.4f;
				float overrideRotation = rotationOrigin.ToRotation();
				Vector2 dustVelo = new Vector2(6.0f, 0).RotatedBy(overrideRotation);
				Vector2 fromBody = NPC.Center + new Vector2(direction * 32, 32 * yDir).RotatedBy(NPC.rotation);
				if(i >= 2)
					particleListBlue.Add(new FireParticle(fromBody + dustVelo * NPC.scale, dustVelo, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 1.1f)));
				else
					particleListRed.Add(new FireParticle(fromBody + dustVelo * NPC.scale, dustVelo, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 1.1f)));
			}
			cataloguePos();
		}
        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
        {
            NPC.lifeMax = (int)(NPC.lifeMax * 0.63889f * bossLifeScale);  //boss life scale in expertmode
            NPC.damage = (int)(NPC.damage * 0.75f);  //boss damage increase in expermode
        }
        public override void OnKill()
		{
			SOTSWorld.downedAmalgamation = true;
		}
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<PolarisBossBag>()));
			LeadingConditionRule notExpertRule = new LeadingConditionRule(new Conditions.NotExpert());
			notExpertRule.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AbsoluteBar>(), 1, 26, 34));
			notExpertRule.OnSuccess(ItemDropRule.Common(ItemID.FrostCore, 1, 1, 2));
			npcLoot.Add(notExpertRule);
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<PolarisRelic>()));
		}
        public override void BossLoot(ref string name, ref int potionType)
		{ 
			potionType = ItemID.GreaterHealingPotion;
		}
		public void SpawnShard(int amt = 1)
		{
			Player player = Main.player[NPC.target];
			SOTSUtils.PlaySound(SoundID.Item44, (int)NPC.Center.X, (int)NPC.Center.Y);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				int damage = NPC.GetBaseDamage() / 2;
				for (int i = 0; i < amt; i++)
				{
					float max = 250 + 100 * i;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + new Vector2(Main.rand.NextFloat(max), 0).RotatedBy(Main.rand.NextFloat(2f * (float)Math.PI)), Vector2.Zero, ModContent.ProjectileType<PolarMortar>(), (int)(damage * 0.8f), 0, Main.myPlayer, player.Center.X + Main.rand.NextFloat(-100, 100), player.Center.Y - Main.rand.NextFloat(100));
				}
			}
		}
		public void SpawnCannons()
		{
			SOTSUtils.PlaySound(SoundID.Item50, (int)NPC.Center.X, (int)NPC.Center.Y);
			if (Main.netMode != NetmodeID.MultiplayerClient)
				for (int i = 0; i < 4; i++)
				{
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolarisCannon>(), 0, i, NPC.whoAmI);
				}
		}
		public void SpawnDragon()
		{
			Player player = Main.player[NPC.target];
			SOTSUtils.PlaySound(SoundID.Item119, (int)NPC.Center.X, (int)NPC.Center.Y);
			if (Main.netMode != 1)
			{
				Vector2 vectorToPlayer = player.Center - NPC.Center;
				vectorToPlayer = vectorToPlayer.SafeNormalize(Vector2.Zero) * -1200;
				vectorToPlayer += NPC.Center;
				NPC.NewNPC(NPC.GetSource_FromAI(), (int)vectorToPlayer.X, (int)vectorToPlayer.Y, ModContent.NPCType<BulletSnakeHead>());
			}
		}
		float variance = 0;
		public void MovetoPlayer()
		{
			variance++;
			float idleAnim = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(variance * 3)).Y;
			Player player = Main.player[NPC.target];
			NPC.velocity *= 0.725f;
			Vector2 vectorToPlayer = player.Center - NPC.Center;
			float yDist = vectorToPlayer.Y * 1.35f;
			float xDist = vectorToPlayer.X;
			float length = (float)Math.Sqrt(xDist * xDist + yDist * yDist);
			float baseSpeed = -9.5f;
			int i = (int)NPC.Center.X / 16;
			int j = (int)NPC.Center.Y / 16;
			if (SOTSWorldgenHelper.TrueTileSolid(i, j))
            {
				baseSpeed = -1;
			}
			float speedMult = baseSpeed + idleAnim + (float)Math.Pow(length, 1.035) * 0.014f;
			if(speedMult < 0)
            {
				speedMult *= 0.5f;
            }
			NPC.velocity += vectorToPlayer.SafeNormalize(Vector2.Zero) * speedMult * 0.6f;
		}
		public override void AI()
		{
			MovetoPlayer();
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.9f / 255f, (255 - NPC.alpha) * 0.1f / 255f, (255 - NPC.alpha) * 0.3f / 255f);
			Player player = Main.player[NPC.target];
			if(player.dead)
			{
				despawn++;
			}
			if(despawn >= 600)
			{
				NPC.active = false;
			}
			float shardRate = 120;
			AICycle++;
			if(NPC.life < NPC.lifeMax * 0.45f)
			{
				AICycle = 0;
				if(transition > 300)
				{
					AICycle2++;
					AICycle3++;
					shardRate *= (float)(NPC.life + 500) / (float)(NPC.lifeMax * 0.5f + 2500);
				}
				else
				{
					transition++;
				}
			}
			if(!player.ZoneSnow || Main.expertMode)
			{
				shardRate *= 0.8f;
			}
			if (AICycle3 >= shardRate)
			{
				SpawnShard(1);
				AICycle3 = 0;
			}
			if (transition > 0 && transition < 250)
			{
				NPC.velocity *= 0.9f;
				NPC.dontTakeDamage = true;
				if(transition % 30 == 0)
				{
					int index = (int)(transition / 30);
					if(Main.netMode != 1)
						NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolarisSpike>(), 0, index, NPC.whoAmI);
					SOTSUtils.PlaySound(SoundID.Item50, (int)(NPC.Center.X), (int)(NPC.Center.Y));
				}
			}
			else
			{
				NPC.dontTakeDamage = false;
			}
			if (AICycle == 400)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<PolarisCannon>()))
					SpawnCannons();
				int extra = Main.expertMode ? 1 : 0;
				SpawnShard(2 + extra);
			}
			if (AICycle >= 800 && AICycle <= 1200 && AICycle % 100 == 0)
			{
				int extra = Main.expertMode ? 1 : 0;
				SpawnShard(2 + extra);
			}
			if (AICycle == 1600)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<PolarisCannon>()))
					SpawnCannons();
				else if (!NPC.AnyNPCs(ModContent.NPCType<BulletSnakeHead>()))
					SpawnDragon();
				else
				{
					int extra = Main.expertMode ? 1 : 0;
					SpawnShard(2 + extra);
				}
			}
			if (AICycle >= 1900 && AICycle <= 2000 && AICycle % 50 == 0)
			{
				int extra = Main.expertMode ? 1 : 0;
				SpawnShard(2 + extra);
			}
			if (AICycle >= 2400 && AICycle <= 2600)
			{
				NPC.rotation = MathHelper.ToRadians(Main.rand.NextFloat(360));
				if (AICycle % 20 == 0)
				{
					int extra = Main.expertMode ? 1 : 0;
					SpawnShard(2 + extra);
				}
			}
			else
            {
				NPC.rotation = NPC.velocity.X * 0.08f;
				NPC.direction = 1;
				NPC.spriteDirection = 1;
            }
			if(AICycle >= 2600)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<BulletSnakeHead>()))
					SpawnDragon();
				AICycle = 0;
			}
			if(AICycle2 == 360)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<PolarisCannon>()))
					SpawnCannons();
			}
			if(AICycle2 == 900)
			{
				if (Main.netMode != 1)
					NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<PolarisLaser>(), 0, 0, NPC.whoAmI);
			}
			if(AICycle2 == 1200)
			{
				if (!NPC.AnyNPCs(ModContent.NPCType<BulletSnakeHead>()))
					SpawnDragon();
			}
			if(AICycle2 >= 1400)
			{
				AICycle2 = 0;
			}
		}
	}
}





















