using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class EvilConstruct : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Evil Construct");
		}
		public override void SetDefaults()
		{
			npc.aiStyle = 0;
			npc.lifeMax = 3500;  
			npc.damage = 60; 
			npc.defense = 20;  
			npc.knockBackResist = 0.1f;
			npc.width = 86;
			npc.height = 82;
			Main.npcFrameCount[npc.type] = 1;  
			npc.value = Item.buyPrice(0, 5, 0, 0);
			npc.npcSlots = 4f;
			npc.lavaImmune = true;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.netAlways = true;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.rarity = 5;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.damage = 100;
			npc.lifeMax = 6000;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == ModContent.ProjectileType<EvilArm>() && proj.active && (int)proj.ai[0] == npc.whoAmI)
				{
					Vector2 toNPC = npc.Center - proj.Center;
					Draw(proj.Center - toNPC.SafeNormalize(Vector2.Zero) * 16);
				}
			}
			float dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			npc.rotation = dir;
			float rotation = dir + (npc.spriteDirection - 1) * 0.5f * MathHelper.ToRadians(180);
			Texture2D texture = Main.npcTexture[npc.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Rectangle(0, npc.frame.Y, npc.width, npc.height), lightColor * ((255 - npc.alpha) / 255f), rotation, drawOrigin, npc.scale * 0.95f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public void Draw(Vector2 to, bool gore = false)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilDrillArm");
			Texture2D texture2 = ModContent.GetTexture("SOTS/NPCs/Constructs/EvilDrill");
			Vector2 position = to;
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float height = (float)texture.Height;
			Vector2 betweenPos = npc.Center - position;
			float rotation = betweenPos.ToRotation() - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(betweenPos.X) && float.IsNaN(betweenPos.Y))
				flag = false;
			bool flag2 = false;
			bool first = true;
			while (flag)
			{
				if ((double)betweenPos.Length() - texture.Height < 2.0)
				{
					flag = false;
				}
				else
				{
					float length = height;
					if (flag2)
					{
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height;
						betweenPos = npc.Center - position;
					}
					else
					{
						Vector2 vector2_1 = betweenPos;
						vector2_1.Normalize();
						position += vector2_1 * height * 0.5f;
						betweenPos = npc.Center - position;
					}
					if (!gore)
					{
						Color color2 = Lighting.GetColor((int)position.X / 16, (int)(position.Y / 16.0));
						color2 = npc.GetAlpha(color2);
						if(first)
							Main.spriteBatch.Draw(texture2, position - Main.screenPosition, null, color2, rotation, texture2.Size()/2, 1.1f, SpriteEffects.None, 0.0f);
						else
							Main.spriteBatch.Draw(texture, position - Main.screenPosition, new Rectangle(0, 0, texture.Width, (int)length), color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
						first = false;
					}
					else
					{
						Gore.NewGore(position, Vector2.Zero, mod.GetGoreSlot("Gores/EvilConstruct/EvilDrillArmGore" + Main.rand.Next(1, 3)), 1f);
					}
					flag2 = true;
				}
			}
		}
		float glowTimer = 0;
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = mod.GetTexture("NPCs/Constructs/EvilConstructGlow");
			Texture2D texture2 = mod.GetTexture("NPCs/Constructs/EvilConstructGlow2");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float dir = (float)Math.Atan2(aimTo.Y - npc.Center.Y, aimTo.X - npc.Center.X);
			float rotation = dir + (npc.spriteDirection - 1) * 0.5f * MathHelper.ToRadians(180);
			Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * ((255 - npc.alpha) / 255f), rotation, drawOrigin, npc.scale * 0.95f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			if(glowTimer > 0)
            {
				color = new Color(100, 100, 100, 0);
				float percent = glowTimer / 60f;
				if (percent > 1)
					percent = 1;
				int amt = (int)(1 + 6 * percent);
				for(int i = 0; i < amt; i++)
				{
					Main.spriteBatch.Draw(texture2, npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY) + Main.rand.NextVector2Circular(2, 2), new Rectangle(0, npc.frame.Y, npc.width, npc.height), color * ((255 - npc.alpha) / 255f) * 0.6f, rotation, drawOrigin, npc.scale * 0.95f, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				}
            }
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				for (int k = 0; k < 20; k++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Lead, 2.5f * (float)hitDirection, -2.5f, 0, default(Color), 0.7f);
				}
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/OtherworldlyConstructs/OtherworldlyConstructGore6"), 1f);
				for (int i = 0; i < 9; i++)
					Gore.NewGore(npc.position, npc.velocity, Main.rand.Next(61, 64), 1f);
			}
		}
		Vector2 aimTo = new Vector2(-1, -1);
		bool runOnce = true;
		int currentArmID = -1;
		public override bool PreAI()
		{
			Player player = Main.player[npc.target];
			Vector2 toPlayer = player.Center - npc.Center;
			int dmg2 = npc.damage / 2;
			if (Main.expertMode)
				dmg2 /= 2;
			int amt = 8;
			if (runOnce)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					for (int i = 0; i < amt; i++)
					{
						Vector2 center = npc.Center;
						Vector2 circular = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(22.5f + i * 360f / amt));
						for (int j = 0; j < 60; j++)
						{
							int i2 = (int)center.X / 16;
							int j2 = (int)center.Y / 16;
							center += circular;
							if (SOTSWorldgenHelper.TrueTileSolid(i2, j2))
							{
								Projectile.NewProjectile(new Vector2(i2, j2) * 16 + new Vector2(8, 8), Vector2.Zero, ModContent.ProjectileType<EvilArm>(), dmg2, 0, Main.myPlayer, npc.whoAmI);
								break;
							}
							else if (j == 59)
							{
								Projectile.NewProjectile(npc.Center + circular * 5, Vector2.Zero, ModContent.ProjectileType<EvilArm>(), dmg2, 0, Main.myPlayer, npc.whoAmI);
							}
						}
					}
				}
				runOnce = false;
				return false;
			}
			Vector2 targetPosition = npc.Center;
			bool alreadyHasArmOut = false;
			int count = 1;
			int numStuck = 0;
			if (npc.ai[1] > 0 && currentArmID >= 0)
			{
				npc.ai[1]++;
				float percent = npc.ai[1] / 120f;
				if(npc.ai[1] > 120)
                {
					npc.ai[1] = 0;
					glowTimer = 60f;
                }
                else
				{
					Projectile proj = Main.projectile[currentArmID];
					Dust dust = Dust.NewDustPerfect(Vector2.Lerp(proj.Center, npc.Center, percent), DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
					dust.velocity *= 0.5f;
					dust.color = VoidPlayer.EvilColor;
					dust.color.A = 160;
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.35f;
				}
			}
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == ModContent.ProjectileType<EvilArm>() && proj.active && (int)proj.ai[0] == npc.whoAmI)
				{
					EvilArm arm = proj.modProjectile as EvilArm;
					float speed = 4f;
					if(arm.startAnim)
                    {
						npc.ai[1]++;
						currentArmID = i; //not syncing this because unsure if whoAmI syncs correctly here.
						arm.startAnim = false;
						if(Main.netMode == NetmodeID.Server)
							npc.netUpdate = true;
					}
					if (arm.stabbyCounter != 0)
					{
						speed = 7f;
						if (arm.stabbyCounter >= 0)
							alreadyHasArmOut = true;
					}
					if (arm.stuck)
					{
						numStuck++;
						targetPosition += proj.Center;
						if (!arm.launch)
						{
							if (arm.stabbyCounter > 120)
								arm.stabbyCounter = -120;
							else if (arm.stabbyCounter < 0)
								arm.stabbyCounter++;
							else arm.stabbyCounter = 0;
						}
						proj.velocity *= 0;
					}
					else if (proj.ai[1] != -1)
					{
						Vector2 circular = new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(22.5f + i * 360f / amt) + npc.rotation);
						arm.MoveTowards(npc.Center + circular * 5, speed);
						Vector2 target = proj.Center * 2 - npc.Center;
						targetPosition += target;
						if (!arm.launch)
						{
							if (arm.stabbyCounter > 120)
								arm.stabbyCounter = -120;
							else if (arm.stabbyCounter < 0)
								arm.stabbyCounter++;
							else arm.stabbyCounter = 0;
						}
					}
					else
					{
						alreadyHasArmOut = true;
						int counter = arm.stabbyCounter;
						if (counter < 30)
						{
							Vector2 safeToPlayer = toPlayer.SafeNormalize(Vector2.Zero);
							Vector2 target = npc.Center + safeToPlayer * 96;
							arm.MoveTowards(target, 4f);
						}
						else if (counter < 90)
						{
							if (counter == 30 || counter == 60)
								Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 15, 1f, -0.05f);
							float degrees = (counter - 30) * 6f;
							float sin = (float)Math.Sin(degrees * Math.PI / 180f);
							Vector2 safeToPlayer = toPlayer.SafeNormalize(Vector2.Zero);
							Vector2 target = npc.Center + safeToPlayer * (96 + sin * 48);
							arm.MoveTowards(target, 4f);
						}
						else
						{
							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 96, 1.3f, 0.1f);
							arm.Launch(player.Center, 25.5f);
						}
						targetPosition += proj.Center;
					}
					arm.Update(npc.Center);
					count++;
				}
			}
			float distanceToPlayer = toPlayer.Length();
			if (distanceToPlayer < 800 && distanceToPlayer > 240 && numStuck >= 2)
			{
				targetPosition += player.Center;
				count++;
			}
			targetPosition /= (float)count;
			Vector2 toPos = Vector2.Lerp(npc.Center, targetPosition, 0.08f);
			Vector2 goToPos = toPos - npc.Center;
			float reduction = 1f;
			if (npc.ai[3] > 50)
			{
				reduction -= (npc.ai[3] - 50) * 0.015f;
			}
			npc.velocity = npc.velocity * 0.1f + goToPos * reduction;
			npc.TargetClosest(true);
			aimTo = player.Center;
			int typeAttack = (int)npc.ai[2] % 2;
			bool canSee = Collision.CanHitLine(npc.Center - new Vector2(5, 5), 10, 10, player.position, player.width, player.height);
			if (npc.ai[3] > 0 || (!alreadyHasArmOut && canSee))
			{
				npc.ai[3]++;
			}
			if (typeAttack == 1 || (!canSee && distanceToPlayer < 480))
			{
				if (glowTimer < 60f && npc.ai[3] > 0)
					glowTimer++;
				if (npc.ai[3] >= 38)
				{
					if ((int)npc.ai[3] % 8 == 0)
					{
						int num = ((int)npc.ai[3] - 38) / 8;
						int trueNum = 0;
						if (num >= 4)
						{
							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 1.3f, 0.1f);
							trueNum = 3;
						}
						else
						{
							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 98, 1.1f + 0.1f * num, 0.2f - 0.05f * num);
						}
						if (Main.netMode != NetmodeID.MultiplayerClient)
						{
							for (int i = -trueNum; i <= trueNum; i++)
							{
								float spread = i * 15f;
								Vector2 fireSpread = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(spread) + toPlayer.ToRotation());
								Projectile.NewProjectile(npc.Center + fireSpread * 20, fireSpread, ModContent.ProjectileType<EvilWave>(), dmg2, 0, Main.myPlayer, 0.15f + 0.04f * num);
							}
						}
						npc.velocity = npc.velocity * 0.5f + toPlayer.SafeNormalize(Vector2.Zero) * -(3 + num);
					}
					else if ((int)npc.ai[3] >= 78f)
					{
						npc.ai[3] = 0;
						npc.ai[2]++;
					}
				}
			}
			else if (typeAttack == 0)
			{
				if (glowTimer > 0)
					glowTimer -= 3;
				else
					glowTimer = 0;
				if (npc.ai[3] >= 36)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
						LaunchFarArm();
					npc.ai[3] = 0;
					npc.ai[2]++;
				}
			} 
			return true;
		}
		public override void PostAI()
		{
			Player player = Main.player[npc.target]; 
			npc.velocity = Collision.TileCollision(npc.position, npc.velocity, npc.width, npc.height, true, true);
		}
		public override void AI()
		{
			Lighting.AddLight(npc.Center, (255 - npc.alpha) * 0.4f / 155f, (255 - npc.alpha) * 0.1f / 155f, (255 - npc.alpha) * 0.1f / 155f);
		}
		public void LaunchFarArm()
		{
			Player player = Main.player[npc.target];
			bool notUseStuck = false;
			EvilArm trueArm = null;
			float dist = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.type == ModContent.ProjectileType<EvilArm>() && proj.active && (int)proj.ai[0] == npc.whoAmI)
				{
					EvilArm arm = proj.modProjectile as EvilArm;
					if(!arm.launch && proj.ai[1] != -1 && arm.stabbyCounter == 0)
					{
						float num = Vector2.Distance(proj.Center, player.Center);
						if (!arm.stuck)
						{
							if (!notUseStuck)
							{
								dist = 2000;
								notUseStuck = true;
							}
							if (num < dist)
							{
								dist = num;
								trueArm = arm;
							}
						}
						else if (num > dist && !notUseStuck)
						{
							dist = num;
							trueArm = arm;
						}
					}
				}
			}
			if(trueArm != null)
            {
				trueArm.Launch();
            }
		}
		public override void NPCLoot()
		{
			int n = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<EvilSpirit>());	
			Main.npc[n].velocity.Y = -10f;
			Main.npc[n].localAI[1] = -1;
			if (Main.netMode != NetmodeID.MultiplayerClient)
				Main.npc[n].netUpdate = true;
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height,  ModContent.ItemType<FragmentOfEvil>(), Main.rand.Next(4) + 4);
		}	
	}
	public class EvilArm : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(stuck);
			writer.Write(stabbyCounter);
			writer.Write(startAnim);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			stuck = reader.ReadBoolean();
			stabbyCounter = reader.ReadInt32();
			startAnim = reader.ReadBoolean();
		}
        public override string Texture => "SOTS/NPCs/Constructs/EvilDrillArm";
        public override void SetDefaults()
        {
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.netImportant = true;
			projectile.hide = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 120;
        }
        public override bool ShouldUpdatePosition()
        {
            return false; 
        }
        public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Evil Construct Arm");
		}
		public bool stuck = false;
		public int stabbyCounter = 0;
		public bool launch = false;
		public bool startAnim = false;
		public void Launch(Vector2 goTo, float speed)
		{
			projectile.ai[1] = 0;
			launch = true;
			if (Main.netMode == NetmodeID.Server)
				projectile.netUpdate = true;
			Vector2 dirVector = goTo - projectile.Center;
			projectile.velocity = dirVector.SafeNormalize(Vector2.Zero) * speed;
		}
		public void MoveTowards(Vector2 goTo, float speed)
		{
			if (launch)
				return;
			projectile.velocity *= 0.89f;
			Vector2 dirVector = goTo - projectile.Center;
			float length = dirVector.Length();
			speed += length * 0.001f;
			if (length < speed)
			{
				projectile.velocity *= 0.5f;
				speed = length;
			}
			if(length < 24 && projectile.ai[1] == -1)
				stabbyCounter++;
			projectile.velocity += dirVector.SafeNormalize(Vector2.Zero) * speed * 0.5f;
        }
		public void Update(Vector2 center)
        {
			projectile.timeLeft = 6;
			if (runOnce)
				return;
			Vector2 fromNPC = center - projectile.Center;
			if(fromNPC.Length() < 64)
            {
				float npcLength = 64 - fromNPC.Length();
				projectile.Center -= fromNPC.SafeNormalize(Vector2.Zero) * npcLength;
            }
			if(!launch)
				projectile.velocity *= 0.97f;
			projectile.Center += projectile.velocity;
        }
		bool runOnce = true;
        public override bool PreAI()
        {
			if(runOnce || launch)
            {
				int i = (int)projectile.Center.X / 16;
				int j = (int)projectile.Center.Y / 16;
				if (SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					launch = false;
					stuck = true;
					startAnim = true;
					if (NetmodeID.Server == Main.netMode)
						projectile.netUpdate = true;
					Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 22, 1f, -0.1f);
				}
				runOnce = false;
            }
			if(launch)
            {
				stabbyCounter++;
				if(stabbyCounter > 110)
                {
					projectile.velocity *= 0.982f;
					if(stabbyCounter > 160)
						launch = false;
                }
				else
					projectile.velocity *= 0.995f;
			}
            return base.PreAI();
        }
		public void Launch()
        {
			projectile.ai[1] = -1;
			stuck = false;
			projectile.netUpdate = true;
        }
        public override void AI()
        {
            base.AI();
        }
    }
}