using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using SOTS.Void;
using Terraria;
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
			get => npc.ai[0];
			set => npc.ai[0] = value;
		}
		private float attackTimer
		{
			get => npc.ai[3];
			set => npc.ai[3] = value;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Inferno Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 5000;  
			npc.damage = 70; 
			npc.defense = 26;  
			npc.knockBackResist = 0f;
			npc.width = 98;
			npc.height = 78;
			Main.npcFrameCount[npc.type] = 1;
			npc.value = Item.buyPrice(0, 10, 0, 0);
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 100;
			npc.lifeMax = 7500;
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
		public void DrawBall(SpriteBatch spriteBatch)
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
					Vector2 fireFrom = npc.Center + (aimTo - npc.Center).SafeNormalize(Vector2.Zero) * 90;
					Texture2D texture = mod.GetTexture("Effects/Masks/Extra_49");
					Color color = VoidPlayer.InfernoColorAttemptDegrees(attackTimer * 3);
					color.A = 0;
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
					SOTS.GodrayShader.Parameters["distance"].SetValue(24 * percent);
					SOTS.GodrayShader.Parameters["colorMod"].SetValue(color.ToVector4());
					SOTS.GodrayShader.Parameters["noise"].SetValue(mod.GetTexture("TrailTextures/noise"));
					SOTS.GodrayShader.Parameters["rotation"].SetValue(MathHelper.ToRadians(percent * 480));
					SOTS.GodrayShader.Parameters["opacity2"].SetValue(1f * percent);
					SOTS.GodrayShader.CurrentTechnique.Passes[0].Apply();
					Main.spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 2f, SpriteEffects.None, 0f);
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
					Color inferno1 = VoidPlayer.Inferno1;
					inferno1.A = 0;
					Color inferno2 = VoidPlayer.Inferno2;
					inferno2.A = 0;
					for (int i = 0; i < 2; i++)
						spriteBatch.Draw(texture, fireFrom - Main.screenPosition, null, i == 1 ? inferno1 : inferno2, 0f, new Vector2(texture.Width / 2, texture.Height / 2), percent * 1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(npc.width / 2, npc.height / 2);
			Texture2D texture = Main.npcTexture[npc.type];
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			DrawFire();
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if(probes[i].degrees >= 180)
						probes[i].Draw();
				}
			}
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, drawColor, dir - bonusDir, origin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/NPCs/Constructs/InfernoConstructGlow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, Color.White, dir - bonusDir, origin, npc.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if (probes[i].degrees < 180)
						probes[i].Draw();
				}
			}
			DrawBall(spriteBatch);
			return false;
		}
		public void DrawFire()
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = VoidPlayer.Inferno1;
				color.A = 0;
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = npc.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				if(Main.netMode != NetmodeID.Server)
				{
					for (int k = 0; k < 30; k++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
						Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 2.2f);
					}
					for (int i = 1; i <= 7; i++)
						Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/InfernoConstruct/InfernoConstructGore" + i), 1f);
					for (int i = 0; i < 9; i++)
						Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
					for (int i = 0; i < probes.Count; i++)
					{
						Gore.NewGore(probes[i].position - new Vector2(13, 13), npc.velocity, mod.GetGoreSlot("Gores/InfernoConstruct/InfernoChildGore"), 1f);
						for (int k = 0; k < 6; k++)
						{
							Dust dust = Dust.NewDustDirect(probes[i].position, 0, 0, DustID.Fire);
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
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			npc.TargetClosest(false);
			aimTo = player.Center;
			if (runOnce)
            {
				for(int i = 0; i < ProbeCount; i++)
                {
					probes.Add(new InfernoProbe(npc.Center, aimTo));
                }
				runOnce = false;
			}
			float xCompress = 0.4f;
			npc.ai[1] += spinSpeed;
			npc.ai[2] += spinDynamicSpeed;
			spinSpeed = MathHelper.Lerp(spinSpeed, 1, 0.05f);
			spinDynamicSpeed = MathHelper.Lerp(spinDynamicSpeed, 1, 0.05f);
			float dynamicDegrees = 15 * (float)Math.Sin(MathHelper.ToRadians(npc.ai[2]));
			for (int i = 0; i < ProbeCount; i++)
			{
				float degrees = npc.ai[1] + i * (360f / ProbeCount);
				probes[i].aimTo = player.Center;
				Vector2 circularLocation = new Vector2(0, rotateLength).RotatedBy(MathHelper.ToRadians(degrees));
				circularLocation.X *= xCompress;
				circularLocation = circularLocation.RotatedBy(npc.rotation + MathHelper.ToRadians(dynamicDegrees));
				probes[i].position = npc.Center + circularLocation;
				probes[i].degrees = degrees % 360;
				probes[i].velocity = npc.velocity;
				probes[i].Update();
			}
			rotateLength = MathHelper.Lerp(rotateLength, targetRotateLength, 0.05f);
			targetRotateLength = 72;
			if (Main.rand.NextBool(7))
            {
				Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire);
				dust.scale *= 1.6f;
				dust.noGravity = true;
				dust.velocity *= 0.2f;
            }
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[npc.target];
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.45f / 155f, (255 - npc.alpha) * 0.25f / 155f, (255 - npc.alpha) * 0.45f / 155f);
			Vector2 toPlayer = player.Center - npc.Center;
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
				if (player.Center.X < npc.Center.X)
					direction = 1;
				else
					direction = -1;
				npc.velocity = Vector2.Lerp(npc.velocity, toPlayer.SafeNormalize(Vector2.Zero) * speed, 0.1f);
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
					if (aimTo.X < npc.Center.X)
						direction = 1;
					else
						direction = -1;
				}
				attackTimer++;
				Vector2 normal = new Vector2(0, 1).RotatedBy(MathHelper.ToRadians(attackTimer * 0.5f * direction));
				npc.velocity *= 0.9f;
				npc.velocity = normal * 0.2f;
				Vector2 targetAimTo = npc.Center + normal * 32;
				Vector2 fireFrom = npc.Center + normal.SafeNormalize(Vector2.Zero) * 90;
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
							Main.PlaySound(SoundID.Item, (int)fireFrom.X, (int)fireFrom.Y, 15, 1.3f, -0.3f);
						}
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
						dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity = dust.velocity * 0.1f - circular * 0.06f * dustDirection;
						dust.alpha = 125;
					}
				}
				if ((int)attackTimer == (int)(infernoDuration + infernoEndDuration - 120))
                {
					for(int i = 0; i < 30; i++)
                    {
						int dust2 = Dust.NewDust(fireFrom - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1));
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
						npc.Center -= normal * 1.5f;
						Main.PlaySound(SoundID.Item, (int)fireFrom.X, (int)fireFrom.Y, 34, 1f, 0.5f);
					}
					if (Main.netMode != NetmodeID.MultiplayerClient && attackTimer % fireRate == 0)
					{
						int damage = npc.damage / 2;
						if (Main.expertMode)
						{
							damage = (int)(damage / Main.expertDamage);
						}
						Projectile.NewProjectile(fireFrom, launchvelo * (0.5f + 0.5f * slowDownMult), ModContent.ProjectileType<LavaLaser>(), (int)(damage * 1.5f), 3f, Main.myPlayer, Main.rand.NextFloat(0.65f, 0.75f) * (0.1f + 0.9f * slowDownMult), Main.rand.NextFloat(360));
					}
				}
			}
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			npc.rotation = dir;
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
					rotational = rotational.RotatedBy(npc.rotation);
					particleList.Add(new FireParticle(npc.Center + new Vector2(-30, 0).RotatedBy(npc.rotation), rotational + npc.velocity * Main.rand.NextFloat(0.8f), Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.6f, 2f)));
				}
				cataloguePos();
			}
			npc.spriteDirection = -direction;
		}
		public const int timeBetweenDashes = 80;
		public void DashAttacks(float distance, float speedMult, int amt = 4)
		{
			Player player = Main.player[npc.target];
			int timer = (int)attackTimer % timeBetweenDashes;
			if (timer <= 30)
			{
				targetRotateLength = 90;
				Vector2 dashArea = player.Center + new Vector2(distance * direction, 0);
				Vector2 toPlayer = dashArea - npc.Center;
				float distToPlayer = toPlayer.Length();
				float speed = 10 * speedMult + distToPlayer * 0.0005f;
				if (speed > distToPlayer)
				{
					speed = distToPlayer;
				}
				npc.velocity = Vector2.Lerp(npc.velocity, toPlayer.SafeNormalize(Vector2.Zero) * speed, 0.1f);
				if (npc.velocity.Length() < 2)
					aimTo = player.Center;
				aimTo = Vector2.Lerp(npc.Center + npc.velocity * 6, player.Center, timer / 30f);
			}
			else if (timer < 50)
			{
				targetRotateLength = 112;
				aimTo = player.Center;
				float toPlayerY = (float)(Math.Sign(player.Center.Y - npc.Center.Y) * Math.Sqrt(Math.Abs(player.Center.Y - npc.Center.Y)));
				float current = timer - 40;
				float sin = (float)Math.Sin(MathHelper.ToRadians(current * 12f));
				npc.velocity *= 0.1f;
				npc.velocity.X += sin * 5f * direction;
				npc.velocity.Y += sin * 0.125f * toPlayerY;
				spinSpeed = 0.5f;
				spinDynamicSpeed = 2f;
			}
			if (timer == 50)
			{
				targetRotateLength = 112;
				Vector2 toPlayer = player.Center - npc.Center;
				Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 62, 1.1f, 0.3f);
				npc.velocity += 23f * toPlayer.SafeNormalize(Vector2.Zero);
				npc.velocity.Y *= 0.5f;
			}
			if (timer > 50)
			{
				targetRotateLength = 112;
				aimTo = npc.Center + npc.velocity * 6;
				npc.velocity += new Vector2(1.1f * speedMult * Math.Sign(npc.velocity.X), 0);
				spinSpeed = 3.5f;
				spinDynamicSpeed = 1f;
				if(attackTimer % 4 == 0)
                {
					int num = (int)attackTimer % probes.Count;
					InfernoProbe probe = probes[num];
					int damage = npc.damage / 2;
					if (Main.expertMode)
					{
						damage = (int)(damage / Main.expertDamage);
					}
					probe.Shoot(damage, 3f);
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
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<InfernoSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<FragmentOfInferno>(), Main.rand.Next(4) + 4);
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
		public void Shoot(int damage, float knockBack)
		{
			Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 62, 0.6f, 1.25f);
			Vector2 toAim = aimTo - position;
			Vector2 launchvelo = toAim.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(3f, 4.5f);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				Projectile.NewProjectile(position, launchvelo, ModContent.ProjectileType<LingeringFlame>(), damage, knockBack, Main.myPlayer);
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
		public void Draw()
		{
			DrawFire();
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/InfernoChild");
			Texture2D textureGlow = ModContent.GetTexture("SOTS/NPCs/Constructs/InfernoChildGlow");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			int spriteDirection = 1;
			if (position.X > aimTo.X)
				spriteDirection = -1;
			float dir = (float)Math.Atan2(aimTo.Y - position.Y, aimTo.X - position.X);
			float rotation = dir + (spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180);
			Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, Lighting.GetColor((int)position.X / 16, (int)position.Y / 16), rotation, origin, 1f, spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(textureGlow, position - Main.screenPosition, null, Color.White, rotation, origin, 1f, spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public void DrawFire()
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = VoidPlayer.Inferno1;
				color.A = 0;
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color *= (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.0f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}