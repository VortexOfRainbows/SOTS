using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Celestial;
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
			npc.knockBackResist = 0.1f;
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(npc.width / 2, npc.height / 2);
			Texture2D texture = Main.npcTexture[npc.type];
			dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			float rotation = dir + (npc.spriteDirection - 1) * 0.5f * -MathHelper.ToRadians(180); 
			DrawFire();
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if(probes[i].degrees >= 180)
						probes[i].Draw();
				}
			}
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, drawColor, rotation, origin, npc.scale, npc.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(ModContent.GetTexture("SOTS/NPCs/Constructs/InfernoConstructGlow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), null, Color.White, rotation, origin, npc.scale, npc.spriteDirection != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			if (!runOnce)
			{
				for (int i = 0; i < probes.Count; i++)
				{
					if (probes[i].degrees < 180)
						probes[i].Draw();
				}
			}
			return false;
		}
		public void DrawFire()
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
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
			int rotateLength = 72;
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
			if(Main.rand.NextBool(7))
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
				DashAttacks(440, 1.3f, 5);
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
				aimTo = player.Center;
				float toPlayerY = (float)(Math.Sign(player.Center.Y - npc.Center.Y) * Math.Sqrt(Math.Abs(player.Center.Y - npc.Center.Y)));
				float current = timer - 40;
				float sin = (float)Math.Sin(MathHelper.ToRadians(current * 12f));
				npc.velocity *= 0.1f;
				npc.velocity.X += sin * 4.5f * direction;
				npc.velocity.Y += sin * 0.1f * toPlayerY;
				spinSpeed = 0.5f;
				spinDynamicSpeed = 2f;
			}
			if (timer == 50)
			{
				Vector2 toPlayer = player.Center - npc.Center;
				Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 62, 1.1f, 0.3f);
				npc.velocity += 23f * toPlayer.SafeNormalize(Vector2.Zero);
				npc.velocity.Y *= 0.5f;
			}
			if (timer > 50)
			{
				aimTo = npc.Center + npc.velocity * 6;
				npc.velocity += new Vector2(-1.1f * speedMult * direction, 0);
				spinSpeed = 3.5f;
				spinDynamicSpeed = 1f;
			}
			if (timer == timeBetweenDashes - 1)
			{
				direction *= -1;
			}
			if (attackTimer > timeBetweenDashes * amt + 30)
			{
				attackTimer = 0;
				attackPhase = 0;
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
				Vector2 rotational = new Vector2(-5f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
				rotational.X *= 1f;
				rotational.Y *= 0.6f;
				rotational = rotational.RotatedBy(rotation);
				particleList.Add(new FireParticle(position + new Vector2(-12, 0).RotatedBy(rotation), rotational + velocity * Main.rand.NextFloat(0.8f), Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.0f, 1.2f)));
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
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
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