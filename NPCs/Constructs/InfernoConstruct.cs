using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class InfernoConstruct : ModNPC
	{
		int direction = 1;
		float dir = 0f;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(direction);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			direction = reader.ReadInt32();
		}
		private float attackPhase
		{
			get => NPC.ai[0];
			set => NPC.ai[0] = value;
		}
		private float attackTimer
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
        public override void SetStaticDefaults()
		{
			NPCID.Sets.MPAllowedEnemies[Type] = true;
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetDefaults()
		{
			NPC.aiStyle = 0;
			NPC.lifeMax = 5000;  
			NPC.damage = 70; 
			NPC.defense = 26;  
			NPC.knockBackResist = 0f;
			NPC.width = 98;
			NPC.height = 78;
			NPC.value = Item.buyPrice(0, 10, 0, 0);
			NPC.npcSlots = 4f;
			NPC.lavaImmune = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.netAlways = true;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.rarity = 5;
		}
		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)/* tModPorter Note: bossLifeScale -> balance (bossAdjustment is different, see the docs for details) */
		{
			NPC.damage = NPC.damage * 6 / 7;
			NPC.lifeMax = NPC.lifeMax * 3 / 4;
		}
		List<InfernoProbe> probes = new List<InfernoProbe>();
		List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
				else
					particle.velocity *= 0.96f;
			}
		}
		public const float infernoDuration = 60f;
		public const float infernoEndDuration = 720f;
		public void DrawBall(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			if (attackPhase == 2 && attackTimer > 0)
			{
				float percent = attackTimer / infernoDuration;
				if (attackTimer > infernoDuration)
				{
					percent = 1f - 0.9f * (attackTimer - infernoDuration) / infernoEndDuration;
				}
				if (percent > 0 && attackTimer < infernoDuration + infernoEndDuration - 120)
				{
					Vector2 fireFrom = NPC.Center + (aimTo - NPC.Center).SafeNormalize(Vector2.Zero) * 90;
					Texture2D texture = Mod.Assets.Request<Texture2D>("Effects/Masks/Extra_49").Value;
					Color color = ColorHelper.InfernoColorGradientDegrees(attackTimer * 3);
					color.A = 0;
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
					SOTS.GodrayShader.Parameters["distance"].SetValue(6);
					SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
					SOTS.GodrayShader.Parameters["noise"].SetValue(Mod.Assets.Request<Texture2D>("TrailTextures/noise").Value);
					SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(percent * 480));
					SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * percent);
					SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
					spriteBatch.Draw(texture, fireFrom - screenPos, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 2f, SpriteEffects.None, 0f);
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
					Color inferno1 = ColorHelper.Inferno1;
					inferno1.A = 0;
					Color inferno2 = ColorHelper.Inferno2;
					inferno2.A = 0;
					for (int i = 0; i < 2; i++)
						spriteBatch.Draw(texture, fireFrom - screenPos, null, i == 1 ? inferno1 : inferno2, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 1f, SpriteEffects.None, 0f);
				}
			}
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Vector2 origin = new Vector2(NPC.width / 2, NPC.height / 2);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			dir = (float)Math.Atan2(aimTo.Y - NPC.Center.Y, aimTo.X - NPC.Center.X);
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			DrawFire(spriteBatch, screenPos);
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if(probes[i].degrees >= 180)
						probes[i].Draw(spriteBatch, screenPos);
				}
			}
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, drawColor, dir - bonusDir, origin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/InfernoConstructGlow"), NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), null, Color.White, dir - bonusDir, origin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if (probes[i].degrees < 180)
						probes[i].Draw(spriteBatch, screenPos);
				}
			}
			DrawBall(spriteBatch, screenPos);
			return false;
		}
		public void DrawFire(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = ColorHelper.Inferno1;
				color.A = 0;
				Vector2 drawPos = particleList[i].position - screenPos;
				color = NPC.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				if(Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 0.7f);
						Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Torch, 2.5f * (float)hit.HitDirection, -2.5f, 0, default(Color), 2.2f);
					}
					for (int i = 1; i <= 7; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/InfernoConstruct/InfernoConstructGore" + i), 1f);
					for (int i = 0; i < 9; i++)
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Main.rand.Next(61, 64), 1f);
					for (int i = 0; i < probes.Count; i++)
					{
						Gore.NewGore(NPC.GetSource_Death(), probes[i].position - new Vector2(13, 13), NPC.velocity, ModGores.GoreType("Gores/InfernoConstruct/InfernoChildGore"), 1f);
						for (int k = 0; k < 6; k++)
						{
							Dust dust = Dust.NewDustDirect(probes[i].position, 0, 0, DustID.Torch);
							dust.scale *= 2.1f;
						}
					}
                }
			}
		}
		public bool runOnce = true;
		Vector2 aimTo = Vector2.Zero;
		public const int ProbeCount = 7;
		float spinSpeed = 1;
		float spinDynamicSpeed = 1;
		float rotateLength = 72;
		float targetRotateLength = 72;
		int probeNumberShootCount = 0;
		int probeAttackTimer = 0;
		public override bool PreAI()
		{
			Player player = Main.player[NPC.target];
			NPC.TargetClosest(false);
			aimTo = player.Center;
			if (runOnce)
            {
				for(int i = 0; i < ProbeCount; i++)
                {
					probes.Add(new InfernoProbe(NPC.Center, aimTo));
                }
				runOnce = false;
			}
			float xCompress = 0.4f;
			NPC.ai[1] += spinSpeed;
			NPC.ai[2] += spinDynamicSpeed;
			spinSpeed = MathHelper.Lerp(spinSpeed, 1, 0.05f);
			spinDynamicSpeed = MathHelper.Lerp(spinDynamicSpeed, 1, 0.05f);
			float dynamicDegrees = 15 * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[2]));
			for (int i = 0; i < ProbeCount; i++)
			{
				float degrees = NPC.ai[1] + i * (360f / ProbeCount);
				probes[i].aimTo = player.Center;
				Vector2 circularLocation = new Vector2(0, rotateLength).RotatedBy(MathHelper.ToRadians(degrees));
				circularLocation.X *= xCompress;
				circularLocation = circularLocation.RotatedBy(NPC.rotation + MathHelper.ToRadians(dynamicDegrees));
				probes[i].position = NPC.Center + circularLocation;
				probes[i].degrees = degrees % 360;
				probes[i].velocity = NPC.velocity;
				probes[i].Update();
			}
			rotateLength = MathHelper.Lerp(rotateLength, targetRotateLength, 0.05f);
			targetRotateLength = 72;
			if (Main.rand.NextBool(7))
            {
				Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Torch);
				dust.scale *= 1.6f;
				dust.noGravity = true;
				dust.velocity *= 0.2f;
            }
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.45f / 155f, (255 - NPC.alpha) * 0.25f / 155f, (255 - NPC.alpha) * 0.45f / 155f);
			Vector2 toPlayer = player.Center - NPC.Center;
			if(attackPhase == 0)
			{
				aimTo = player.Center;
				attackTimer++;
				float distToPlayer = toPlayer.Length();
				float speed = 12 + distToPlayer * 0.0005f;
				if (speed > distToPlayer)
				{
					speed = distToPlayer;
				}
				if (distToPlayer > 880)
				{
					speed *= 2.8f;
				}
				if (distToPlayer < 380 && distToPlayer > 320)
				{
					speed *= 0.1f;
				}
				if (distToPlayer < 320)
				{
					speed = -2 + distToPlayer * -0.001f;
				}
				if (player.Center.X < NPC.Center.X)
					direction = 1;
				else
					direction = -1;
				NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer.SafeNormalize(Vector2.Zero) * speed, 0.1f);
				if(attackTimer > 180)
                {
					attackTimer = 0;
					attackPhase = 1;
                }
			}
			if(attackPhase == 1)
            {
				attackTimer++;
				DashAttacks(440, 1.2f, 5);
			}
			if(attackPhase == 2)
			{
				if(attackTimer < 0)
				{
					probeAttackTimer = 0;
					if (aimTo.X < NPC.Center.X)
						direction = 1;
					else
						direction = -1;
				}
				attackTimer++; 
				probeAttackTimer++;
				Vector2 normal = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(attackTimer * 0.5f * direction));
				NPC.velocity *= 0.9f;
				NPC.velocity = normal * 0.2f;
				Vector2 targetAimTo = NPC.Center + normal * 32;
				Vector2 fireFrom = NPC.Center + normal.SafeNormalize(Vector2.Zero) * 90;
				float lerp = 1 + attackTimer / 90f;
				if (lerp > 1)
					lerp = 1;
				if (attackTimer > infernoDuration + infernoEndDuration - 120)
				{
					float timer = attackTimer - infernoDuration - infernoEndDuration + 120;
					lerp = 1 - timer / 120f;
				}
				else
				{
					if (attackTimer > 0)
					{
						if ((int)attackTimer % (int)(infernoDuration / 3) == 0 && attackTimer < infernoDuration)
						{
							SOTSUtils.PlaySound(SoundID.Item15, (int)fireFrom.X, (int)fireFrom.Y, 1.3f, -0.3f);
						}
						if(!Main.rand.NextBool(3))
						{
							float sizeMult = attackTimer / infernoDuration;
							if (sizeMult > 1)
								sizeMult = 1;
							float dustDirection = 1;
							if (attackTimer > infernoDuration)
							{
								dustDirection = -1;
								sizeMult = 0.6f - 0.5f * (attackTimer - infernoDuration) / infernoEndDuration;
							}
							Vector2 circular = new Vector2(90 * sizeMult, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
							int dust2 = Dust.NewDust(fireFrom - new Vector2(12, 12) + circular, 16, 16, ModContent.DustType<CopyDust4>());
							Dust dust = Main.dust[dust2];
							dust.color = ColorHelper.InfernoColorGradient(Main.rand.NextFloat(1));
							dust.noGravity = true;
							dust.fadeIn = 0.1f;
							dust.scale *= 2.2f;
							dust.velocity = dust.velocity * 0.1f - circular * 0.06f * dustDirection;
							dust.alpha = 125;
						}
					}
				}
				if ((int)attackTimer == (int)(infernoDuration + infernoEndDuration - 120))
                {
					for(int i = 0; i < 20; i++)
                    {
						int dust2 = Dust.NewDust(fireFrom - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = ColorHelper.InfernoColorGradient(Main.rand.NextFloat(1));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.5f;
						dust.velocity *= 2.5f;
						dust.alpha = 125;
					}
				}
				aimTo = Vector2.Lerp(aimTo, targetAimTo, lerp);
				if (attackTimer > infernoDuration + infernoEndDuration)
				{
					attackTimer = 0;
					attackPhase = 0;
				}
				float slowDownMult = 1f;
				if (attackTimer > infernoDuration + infernoEndDuration - 360)
				{
					slowDownMult = 1f - (attackTimer - infernoDuration - infernoEndDuration + 360) / 240f;
				}
				int fireRate = (int)(10 - 8f * slowDownMult);
				if (attackTimer > infernoDuration && attackTimer < infernoDuration + infernoEndDuration - 120)
				{
					Vector2 launchvelo = normal * Main.rand.NextFloat(2.75f, 3.25f);
					if (attackTimer % (fireRate * 2) == 0)
					{
						NPC.Center -= normal * 1.5f;
						SOTSUtils.PlaySound(SoundID.Item34, (int)fireFrom.X, (int)fireFrom.Y, 1f, 0.5f);
					}
					int damage = NPC.GetBaseDamage() / 2;
					if (Main.netMode != NetmodeID.MultiplayerClient && attackTimer % fireRate == 0)
					{
						Projectile.NewProjectile(NPC.GetSource_FromAI(), fireFrom, launchvelo * (0.5f + 0.5f * slowDownMult), ModContent.ProjectileType<LavaLaser>(), (int)(damage * 3f), 3.5f, Main.myPlayer, Main.rand.NextFloat(0.65f, 0.75f) * (0.1f + 0.9f * slowDownMult), Main.rand.NextFloat(360)); //lava beam should do a ludicrous amount of damage
					}
					if (probeAttackTimer >= (int)(fireRate * 5))
					{
						probeAttackTimer = 0;
						int num = (int)probeNumberShootCount % probes.Count;
						InfernoProbe probe = probes[num];
						probe.Shoot(NPC, damage, 3f, 2.7f);
						probeNumberShootCount++;
					}
				}
			}
			dir = (float)Math.Atan2(aimTo.Y - NPC.Center.Y, aimTo.X - NPC.Center.X);
			NPC.rotation = dir;
			if (Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < (SOTS.Config.lowFidelityMode ? 2 : 3); i++)
				{
					Vector2 rotational = new Vector2(-5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
					if (i <= 1)
					{
						rotational.X *= 1f;
						rotational.Y *= 0.6f;
					}
					else
					{
						rotational.X *= 0.4f;
						rotational.Y *= 1.1f;
					}
					rotational = rotational.RotatedBy(NPC.rotation);
					particleList.Add(new FireParticle(NPC.Center + new Vector2(-30, 0).RotatedBy(NPC.rotation), rotational + NPC.velocity * Main.rand.NextFloat(0.8f), Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.6f, 2f)));
				}
				cataloguePos();
			}
			NPC.spriteDirection = -direction;
		}
		public const int timeBetweenDashes = 80;
		public void DashAttacks(float distance, float speedMult, int amt = 4)
		{
			Player player = Main.player[NPC.target];
			int timer = (int)attackTimer % timeBetweenDashes;
			if (timer <= 30)
			{
				targetRotateLength = 90;
				Vector2 dashArea = player.Center + new Vector2(distance * direction, 0);
				Vector2 toPlayer = dashArea - NPC.Center;
				float distToPlayer = toPlayer.Length();
				float speed = 10 * speedMult + distToPlayer * 0.0005f;
				if (speed > distToPlayer)
				{
					speed = distToPlayer;
				}
				NPC.velocity = Vector2.Lerp(NPC.velocity, toPlayer.SafeNormalize(Vector2.Zero) * speed, 0.1f);
				if (NPC.velocity.Length() < 2)
					aimTo = player.Center;
				aimTo = Vector2.Lerp(NPC.Center + NPC.velocity * 6, player.Center, timer / 30f);
			}
			else if (timer < 50)
			{
				targetRotateLength = 112;
				aimTo = player.Center;
				float toPlayerY = (float)(Math.Sign(player.Center.Y - NPC.Center.Y) * Math.Sqrt(Math.Abs(player.Center.Y - NPC.Center.Y)));
				float current = timer - 40;
				float sin = (float)Math.Sin(MathHelper.ToRadians(current * 12f));
				NPC.velocity *= 0.1f;
				NPC.velocity.X += sin * 5f * direction;
				NPC.velocity.Y += sin * 0.125f * toPlayerY;
				spinSpeed = 0.5f;
				spinDynamicSpeed = 2f;
			}
			if (timer == 50)
			{
				targetRotateLength = 112;
				Vector2 toPlayer = player.Center - NPC.Center;
				SOTSUtils.PlaySound(SoundID.Item62, (int)NPC.Center.X, (int)NPC.Center.Y, 1.1f, 0.3f);
				NPC.velocity += 23f * toPlayer.SafeNormalize(Vector2.Zero);
				NPC.velocity.Y *= 0.5f;
			}
			if (timer > 50)
			{
				targetRotateLength = 112;
				aimTo = NPC.Center + NPC.velocity * 6;
				NPC.velocity += new Vector2(1.1f * speedMult * Math.Sign(NPC.velocity.X), 0);
				spinSpeed = 3.5f;
				spinDynamicSpeed = 1f;
				if(attackTimer % 4 == 0)
                {
					int num = (int)attackTimer % probes.Count;
					InfernoProbe probe = probes[num];
					int damage = NPC.GetBaseDamage() / 2;
					probe.Shoot(NPC, damage, 3f);
				}
			}
			if (timer == timeBetweenDashes - 1)
			{
				direction *= -1;
			}
			if (attackTimer > timeBetweenDashes * amt + 30)
			{
				attackTimer = -90;
				attackPhase = 2;
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FragmentOfInferno>(), 1, 4, 7));
		}
		public override void OnKill()
        {
			int n = NPC.NewNPC(NPC.GetSource_Death(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<InfernoSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
		}	
	}
	public class InfernoProbe
    {
		public float degrees = 0;
		public Vector2 position;
		public Vector2 aimTo;
		public Vector2 velocity;
		public List<FireParticle> particleList = new List<FireParticle>();
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
				}
				else
					particle.velocity *= 0.96f;
			}
		}
		public void Shoot(NPC npc, int damage, float knockBack, float speedMod = 1f)
		{
			SOTSUtils.PlaySound(SoundID.Item62, (int)position.X, (int)position.Y, 0.6f, 1.25f);
			Vector2 toAim = aimTo - position;
			Vector2 launchvelo = toAim.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3f, 4.5f);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.NewProjectile(npc.GetSource_FromAI(), position, launchvelo * speedMod, ModContent.ProjectileType<LingeringFlame>(), damage, knockBack, Main.myPlayer);
			}
			for (int i = 0; i < 6; i++)
			{
				int dust2 = Dust.NewDust(position - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = new Color(255, 75, 0, 0);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
				dust.velocity = dust.velocity * 0.7f + launchvelo * 1.5f;
				dust.alpha = 125;
			}
		}
		public InfernoProbe(Vector2 position, Vector2 aimTo)
        {
			this.position = position;
			this.aimTo = aimTo;
        }
		public void Update()
		{
			float rotation = (float)Math.Atan2(aimTo.Y - position.Y, aimTo.X - position.X);
			if (Main.netMode != NetmodeID.Server)
			{
				Vector2 rotational = new Vector2(-5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20f, 20f)));
				rotational.X *= 1f;
				rotational.Y *= 0.6f;
				rotational = rotational.RotatedBy(rotation);
				particleList.Add(new FireParticle(position + new Vector2(-8, 0).RotatedBy(rotation), rotational + velocity * Main.rand.NextFloat(0.8f), Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.0f, 1.2f)));
				cataloguePos();
			}
		}
		public void Draw(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			DrawFire(spriteBatch, screenPos);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/InfernoChild");
			Texture2D textureGlow = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/InfernoChildGlow");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			int spriteDirection = 1;
			if (position.X > aimTo.X)
				spriteDirection = -1;
			float dir = (float)Math.Atan2(aimTo.Y - position.Y, aimTo.X - position.X);
			float rotation = dir + (spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			spriteBatch.Draw(texture, position - screenPos, null, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16), rotation, origin, 1f, spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			spriteBatch.Draw(textureGlow, position - screenPos, null, Color.White, rotation, origin, 1f, spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public void DrawFire(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = ColorHelper.Inferno1;
				color.A = 0;
				Vector2 drawPos = particleList[i].position - screenPos;
				color *= (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.0f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}