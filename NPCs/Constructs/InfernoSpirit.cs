using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Inferno;
using SOTS.Projectiles.Tide;
using SOTS.Void;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.NPCs.Constructs
{
	public class InfernoSpirit : ModNPC
	{	
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[NPC.type] = 1;
			DisplayName.SetDefault("Inferno Spirit");
			NPCID.Sets.TrailCacheLength[NPC.type] = 5;  
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(rotation);
			writer.Write(phase);
			writer.Write(counter);
			writer.Write(nextDestination.X);
			writer.Write(nextDestination.Y);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			rotation = reader.ReadSingle();
			phase = reader.ReadInt32();
			counter = reader.ReadInt32();
			nextDestination.X = reader.ReadSingle();
			nextDestination.Y = reader.ReadSingle();
		}
		public override void SetDefaults()
		{
			NPC.aiStyle =10;
            NPC.lifeMax = 4000; 
            NPC.damage = 100; 
            NPC.defense = 0;   
            NPC.knockBackResist = 0f;
            NPC.width = 70;
            NPC.height = 70;
            NPC.value = Item.buyPrice(0, 18, 0, 0);
            NPC.npcSlots = 10f;
            NPC.boss = false;
            NPC.lavaImmune = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit54;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = false;
			NPC.rarity = 2;
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			NPC.damage = NPC.damage * 27 / 40;
			NPC.lifeMax = NPC.lifeMax * 3 / 4;
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return canDamage;
        }
        Vector2 nextDestination = Vector2.Zero;
		public bool canDamage = true;
		public float rotation = 0;
		private int InitiateHealth = 12000;
		private float ExpertHealthMult = 1.5f; //18000
		public const int ProbeCount = 7;
		public const int timeToFire = 2;
		public const int timeToDash = 45;
		float verticalCompress = 1f;
		int counter2 = 0;
		int phase = 1;
		int counter = 0;
		bool runOnce = true;
		List<MiniSpirit> miniSpirits = new List<MiniSpirit>();
		public override void AI()
		{
			if(runOnce)
            {
				runOnce = false;
				for (int i = 0; i < ProbeCount; i++)
				{
					miniSpirits.Add(new MiniSpirit(0, i * 360f / ProbeCount));
				}
			}
			Lighting.AddLight(NPC.Center, (255 - NPC.alpha) * 0.15f / 255f, (255 - NPC.alpha) * 0.25f / 255f, (255 - NPC.alpha) * 0.65f / 255f);
			Player player = Main.player[NPC.target];
			if (phase == 3)
			{
				NPC.aiStyle =-1;
				NPC.dontTakeDamage = false;
				int damage = NPC.GetBaseDamage() / 2;
				NPC.ai[1]++;
				if ((int)NPC.ai[2] % 3 == 0)
				{
					verticalCompress = MathHelper.Lerp(verticalCompress, 1f, 0.04f);
					if (NPC.ai[1] < (timeToDash * 4))
					{
						rotation -= Direction();
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 96, 0.06f);
						}
						if (NPC.ai[1] >= 0)
						{
							float multiplier = NPC.ai[1] / (timeToDash * 4);
							if (nextDestination != Vector2.Zero)
							{
								int rnCounter = (int)NPC.ai[1] % timeToDash;
								if (rnCounter == 0)
								{
									Vector2 toPlayer = player.Center - NPC.Center;
									nextDestination = NPC.Center + toPlayer * (0.25f + 0.8f * multiplier) + Main.rand.NextVector2CircularEdge(160, 160) * (1.0f - 0.8f * multiplier);
									if (Main.netMode != NetmodeID.MultiplayerClient)
									{
										NPC.netUpdate = true;
									}
								}
								else
								{
									NPC.Center = Vector2.Lerp(NPC.Center, nextDestination, 0.06f);
								}
							}
							else
							{
								nextDestination = NPC.Center;
								NPC.velocity *= 0;
							}
						}
					}
					else if (NPC.ai[1] <= (timeToDash * 4) + 30)
					{
						float bonus = (NPC.ai[1] - (timeToDash * 4)) / 30f;
						rotation -= Direction() * (1.0f + bonus * 2);
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(96, 60, bonus);
						}
					}
					else if (NPC.ai[1] > (timeToDash * 4) + 30)
					{
						rotation -= Direction() * 3.0f;
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 60, 0.06f);
							if ((int)NPC.ai[3] % ProbeCount == i)
							{
								float mult = NPC.ai[1] % timeToFire / (timeToFire - 1);
								if (mult == 0)
									mult = 1;
								spirit.offset = 60 + 32 * mult;
							}
						}
						if (NPC.ai[1] % timeToFire == 0)
						{
							if(Main.netMode != NetmodeID.MultiplayerClient)
							{
								int i = (int)NPC.ai[3] % ProbeCount;
								MiniSpirit spirit = miniSpirits[i];
								Vector2 normal = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(spirit.rotation));
								Projectile.NewProjectile(NPC.GetSource_FromAI(), spirit.getCenter(NPC.Center, verticalCompress), normal, ModContent.ProjectileType<InfernoBolt>(), damage, 3.5f, Main.myPlayer, Direction(), 0);
							}
							NPC.ai[3]++;
						}
					}
					if (NPC.ai[1] > (timeToDash * 4) + 240)
					{
						NPC.ai[1] = -110;
						NPC.ai[2]++;
						NPC.ai[3] = 0;
						NPC.ai[0] = 0;
						NPC.netUpdate = true;
					}
				}
				if ((int)NPC.ai[2] % 3 == 1)
				{
					float mult = NPC.ai[1] / 60f;
					mult = MathHelper.Clamp(mult, 0, 1);
					verticalCompress = MathHelper.Lerp(verticalCompress, 0.4f, 0.04f);
					rotation -= Direction() * (1f + mult * 2);
					for (int i = 0; i < ProbeCount; i++)
					{
						MiniSpirit spirit = miniSpirits[i];
						spirit.offset = MathHelper.Lerp(spirit.offset, 96, 0.06f);
					}
					if(NPC.ai[1] < 0)
                    {
						NPC.ai[0] = player.Center.X;
						NPC.ai[3] = player.Center.Y;
                    }
					Vector2 center = new Vector2(NPC.ai[0], NPC.ai[3]);
					Vector2 dashPosition = new Vector2(800 * Direction(), -400) + center;
					Vector2 dashPositionOther = new Vector2(800 * -Direction(), -400) + center;
					if (NPC.ai[1] < 0)
					{
						NPC.Center = Vector2.Lerp(NPC.Center, dashPosition, 0.054f); 
						canDamage = false;
					}
					else
						canDamage = true;
					if (NPC.ai[1] >= 0)
					{
						mult = NPC.ai[1] / 200f;
						NPC.Center = Vector2.Lerp(dashPosition, dashPositionOther, mult);
						if ((int)NPC.ai[1] % 4 == 0)
							SOTSUtils.PlaySound(SoundID.Item34, (int)NPC.Center.X, (int)NPC.Center.Y, 1f, 0.5f);
						if ((int)NPC.ai[1] % 2 == 0)
						{
							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(Direction() * -0.5f, 4.5f), ModContent.ProjectileType<LavaLaser>(), (int)(damage * 1.5f), 3.5f, Main.myPlayer, Main.rand.NextFloat(0.65f, 0.75f), Main.rand.NextFloat(360));
							}
						}
						if (NPC.ai[1] > 200f)
						{
							NPC.ai[1] = -60;
							NPC.ai[2]++;
							NPC.ai[3] = 0;
							NPC.ai[0] = 0;
							NPC.netUpdate = true;
						}
					}
				}
				if ((int)NPC.ai[2] % 3 == 2)
				{
					verticalCompress = MathHelper.Lerp(verticalCompress, 1f, 0.04f);
					if (NPC.ai[1] < (timeToDash * 4))
					{
						rotation -= Direction();
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 96, 0.06f);
						}
						if (NPC.ai[1] >= 0)
						{
							float multiplier = NPC.ai[1] / (timeToDash * 4);
							if (nextDestination != Vector2.Zero)
							{
								int rnCounter = (int)NPC.ai[1] % timeToDash;
								if (rnCounter == 0)
								{
									Vector2 toPlayer = player.Center - NPC.Center;
									nextDestination = NPC.Center + toPlayer * (0.25f + 0.8f * multiplier) + Main.rand.NextVector2CircularEdge(160, 160) * (1.0f - 0.8f * multiplier);
									if (Main.netMode != NetmodeID.MultiplayerClient)
                                    {
										NPC.netUpdate = true;
									}
								}
								else
								{
									NPC.Center = Vector2.Lerp(NPC.Center, nextDestination, 0.06f);
								}
							}
							else
							{
								nextDestination = NPC.Center;
								NPC.velocity *= 0;
							}
						}
					}
					else if (NPC.ai[1] <= (timeToDash * 4) + 30)
					{
						float bonus = (NPC.ai[1] - (timeToDash * 4)) / 30f;
						rotation -= Direction() * (1.0f - 0.6f * bonus);
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(96, 60, bonus);
						}
					}
					else if (NPC.ai[1] > (timeToDash * 4) + 30)
					{
						rotation -= Direction() * 0.4f;
						for (int i = 0; i < ProbeCount; i++)
						{
							MiniSpirit spirit = miniSpirits[i];
							spirit.offset = MathHelper.Lerp(spirit.offset, 480, 0.05f);
							if (Main.netMode != NetmodeID.MultiplayerClient)
								if ((int)NPC.ai[1] % 30 == 0 && NPC.ai[1] > (timeToDash * 4) + 90)
								{
									int fortyfive = (int)NPC.ai[1] % 60 == 30 ? 45 : 0;
									for (int j = 0; j < 4; j++)
									{
										Vector2 normal = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(90 * j + fortyfive));
										Projectile.NewProjectile(NPC.GetSource_FromAI(), spirit.getCenter(NPC.Center, verticalCompress), normal, ModContent.ProjectileType<InfernoBolt>(), damage, 3.5f, Main.myPlayer, 0, j == 0 ? -2 : -1);
									}
								}
						}
						NPC.ai[3]++;
					}
					if (NPC.ai[1] > (timeToDash * 4) + 210)
					{
						NPC.ai[1] = -60;
						NPC.ai[2]++;
						NPC.ai[3] = 0;
						NPC.ai[0] = 0;
						NPC.netUpdate = true;
					}
				}
				counter2++;
				Vector2 idle = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(counter2 * 2));
				NPC.velocity *= 0.95f;
				NPC.velocity += idle * 0.1f;
			}
			if (phase == 2)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
					NPC.netUpdate = true;
				NPC.dontTakeDamage = false;
				NPC.aiStyle =-1;
				NPC.ai[0] = 0;
				NPC.ai[1] = -120;
				NPC.ai[2] = 0;
				NPC.ai[3] = 0;
				phase = 3;
			}
			else if(phase == 1)
			{
				counter++;
			}
			if(phase != 3)
			{
				rotation -= 1.0f;
				for (int i = 0; i < ProbeCount; i++)
				{
					MiniSpirit spirit = miniSpirits[i];
					spirit.offset = MathHelper.Lerp(spirit.offset, 60, 0.03f);
					spirit.rotation = i * 360f / ProbeCount + rotation;
				}
			}
			else
            {
				for (int i = 0; i < ProbeCount; i++)
				{
					MiniSpirit spirit = miniSpirits[i];
					spirit.rotation = i * 360f / ProbeCount + rotation;
				}
			}
			if (Main.player[NPC.target].dead)
			{
				counter++;
			}
			else if(phase != 1 && counter > 0)
			{
				counter--;
			}
			if(counter >= 1800)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.netUpdate = true;
				}
				phase = 1;
				NPC.aiStyle =-1;
				NPC.velocity.Y -= 0.014f;
				NPC.dontTakeDamage = true;
			}
			int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
			Dust dust = Main.dust[dust2];
			dust.color = new Color(255, 130, 15);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 2f;
		}
		public int Direction()
		{
			return (int)NPC.ai[2] % 2 * 2 - 1;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++) {
				Vector2 drawPos = NPC.oldPos[k] - screenPos + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = new Color(150, 80, 70, 0) * ((float)(NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, NPC.rotation, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
			if(!runOnce)
			{
				for (int k = 0; k < ProbeCount; k++)
				{
					MiniSpirit spirit = miniSpirits[k];
					spirit.Draw(spriteBatch, spirit.getCenter(NPC.Center, verticalCompress), screenPos);
				}
			}
			return false;
		}	
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
			{
				if(Main.netMode	!= NetmodeID.Server)
				{
					for (int i = 0; i < 50; i++)
					{
						Dust dust = Dust.NewDustDirect(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.RainbowMk2);
						dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 2.2f;
						dust.velocity *= 4f;
					}
					for(int i = 0; i < ProbeCount; i++)
					{
						for (int k = 0; k < 10; k++)
						{
							Dust dust = Dust.NewDustDirect(miniSpirits[i].getCenter(NPC.Center, verticalCompress) - new Vector2(4, 4), 0, 0, DustID.RainbowMk2);
							dust.color = VoidPlayer.InfernoColorAttempt(Main.rand.NextFloat(1f));
							dust.noGravity = true;
							dust.fadeIn = 0.1f;
							dust.scale *= 2.0f;
							dust.velocity *= 2.5f;
						}
					}
				}
				if(phase == 1)
				{
					phase = 2;
					NPC.lifeMax = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
					NPC.life = (int)(InitiateHealth * (Main.expertMode ? ExpertHealthMult : 1));
				}
			}
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Npc[NPC.type].Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				spriteBatch.Draw(texture, NPC.Center + Main.rand.NextVector2Circular(4f, 4f) - screenPos, null, color, 0f, drawOrigin, NPC.scale * 1.1f, SpriteEffects.None, 0f);
			}
		}
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DissolvingNether>()));
		}
	}
	public class MiniSpirit
    {
		public float offset;
		public float rotation; 
		public MiniSpirit(float offset, float rotation)
        {
			this.offset = offset;
			this.rotation = rotation;
        }
		public Vector2 getCenter(Vector2 npcCenter, float yCompress = 1f, float npcRotation = 0f)
        {
			Vector2 vectorOffset = new Vector2(offset, 0).RotatedBy(MathHelper.ToRadians(rotation));
			vectorOffset.Y *= yCompress;
			return npcCenter + vectorOffset;
		}
		public void Draw(SpriteBatch spriteBatch, Vector2 drawLocation, Vector2 screenPos)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/InfernoSpiritMini");
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				spriteBatch.Draw(texture, drawLocation + Main.rand.NextVector2Circular(4f, 4f) - screenPos, null, color, 0f, drawOrigin, 1.1f, SpriteEffects.None, 0f);
			}
		}
    }
}
