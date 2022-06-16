using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using SOTS.Projectiles.Celestial;
using SOTS.Projectiles.Inferno;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Inferno
{
	public class LesserWisp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Wisp");
		}
		public override void SetDefaults()
		{
            NPC.aiStyle =0; 
            NPC.lifeMax = 35;   
            NPC.damage = 35; 
            NPC.defense = 16;  
            NPC.knockBackResist = 0.5f;
            NPC.width = 26;
            NPC.height = 32;
			Main.npcFrameCount[NPC.type] = 1;  
            NPC.value = 50;
            NPC.npcSlots = 0.25f;
            NPC.HitSound = SoundID.NPCHit30;
            NPC.DeathSound = SoundID.NPCDeath6;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.buffImmune[BuffID.OnFire] = true;
			NPC.buffImmune[BuffID.Frostburn] = true;
			NPC.buffImmune[BuffID.Ichor] = true;
			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.ai[0] = 0;
			NPC.ai[2] = 1;
			Banner = NPC.type;
			BannerItem = ItemType<LesserWispBanner>();
		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public bool sans = false;
		public override bool StrikeNPC(ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
		{
			if(sans)
			{
				if (NPC.ai[3] >= 24)
				{
					damage = 999999 + defense / 2;
					crit = false;
					return true;
				}
				else
				{
					CombatText.NewText(new Rectangle((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height), new Color(125, 125, 125, 125), "MISS", false, false);
					damage = 0;
					NPC.ai[3]++;
					NPC.netUpdate = true;
				}
				return false;
			}
			return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			Texture2D texture = (Texture2D)Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color;
			for (int i = 0; i < particleList.Count; i++)
			{
				color = new Color(255, 69, 0, 0);
				if(sans)
					color = new Color(60, 90, 115, 0);
				Vector2 drawPos = particleList[i].position - screenPos;
				color = NPC.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					float x = Main.rand.NextFloat(-2f, 2f);
					float y = Main.rand.NextFloat(-2f, 2f);
					spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.1f, SpriteEffects.None, 0f);
				}
			}
			texture = sans ? (Texture2D)Request<Texture2D>("SOTS/NPCs/Inferno/SansWispOutline") : (Texture2D)Request<Texture2D>("SOTS/NPCs/Inferno/LesserWispOutline");
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			color = sans ? new Color(50, 50, 90, 0) : new Color(80, 80, 80, 0);
			for (int k = 0; k < 12; k++)
			{
				Vector2 drawPos = NPC.Center - screenPos;
				Vector2 circular = new Vector2(Main.rand.NextFloat(0, 3), 0).RotatedBy(Math.PI / 6 * k);
				spriteBatch.Draw(texture, drawPos + circular, null, color * 0.9f, NPC.rotation, drawOrigin, NPC.scale, SpriteEffects.None, 0f);
			}
			texture = sans ? (Texture2D)Request<Texture2D>("SOTS/NPCs/Inferno/SansWisp") : Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			spriteBatch.Draw(texture, NPC.Center - screenPos, null, sans ? Color.White : new Color(180, 180, 180), NPC.rotation, drawOrigin, NPC.scale * 0.9f, SpriteEffects.None, 0f);
			return false;
		}
        public override bool PreAI()
        {
			if(NPC.ai[0] == 0)
            {
				if(Main.netMode != 1)
				{
					NPC.ai[1] = -240;
					if (Main.rand.NextBool(100))
                    {
						NPC.ai[2] = -1;
						NPC.ai[1] = -120;
					}
					else
						for (int i = 0; i < Main.rand.Next(2, 4); i++)
							NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.position.Y + NPC.height, this.NPC.type, 0, -1, -Main.rand.Next(120) - 240);
					NPC.netUpdate = true;
				}
				NPC.ai[0] = -1;
			}
			if(NPC.ai[2] == -1)
            {
				sans = true;
				NPC.HitSound = SoundID.DD2_SkeletonHurt;
				NPC.DeathSound = SoundID.DD2_SkeletonDeath;
			}
			else
            {
				sans = false;
				NPC.HitSound = SoundID.NPCHit30;
				NPC.DeathSound = SoundID.NPCDeath6;
			}
			if(Main.netMode != NetmodeID.Server)
			{
				for (int i = 0; i < (SOTS.Config.lowFidelityMode ? 1 : 1 + Main.rand.Next(2)); i++)
				{
					Vector2 rotational = new Vector2(0, -4.4f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-20f, 20f)));
					rotational.X *= 0.5f;
					rotational.Y *= 1f;
					particleList.Add(new FireParticle(NPC.Center, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.2f, 1.6f)));
				}
				cataloguePos();
			}
			if (Main.rand.NextBool(20))
			{
				Color color = new Color(155, 69, 0, 0);
				if (sans)
					color = new Color(80, 150, 200, 0);
				int dust2 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y) - new Vector2(5), NPC.width, NPC.height, 267);
				Dust dust = Main.dust[dust2];
				dust.color = color;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
			return base.PreAI();
		}
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
			}
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int orbital = modPlayer.orbitalCounter;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC other = Main.npc[i];
				if (NPC.type == other.type && other.active && NPC.active && NPC.target == other.target)
				{
					if (NPC == other)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}
			NPC.localAI[1]++;
			NPC.rotation = NPC.velocity.X * 0.03f;

			float dynamicAddition = (float)Math.Sin(MathHelper.ToRadians(NPC.localAI[1] * 2)) * 6;
			Vector2 circular = player.Center + new Vector2(0, dynamicAddition) + new Vector2(156 + 4 * total, 0).RotatedBy(MathHelper.ToRadians(orbital * 0.75f + (360f * ofTotal / total)));
			Vector2 toPlayer = circular - NPC.Center;
			float speed = 7.8f;
			if (sans)
				speed = 20f;
			float length = toPlayer.Length();
			if(length < speed)
            {
				speed = length;
            }
			NPC.velocity *= 0.5f;
			NPC.velocity += toPlayer.SafeNormalize(Vector2.Zero) * speed * 0.5f;
			NPC.ai[1]++;
			if (NPC.ai[1] > 0)
            {
				float num = NPC.ai[1];
				float chargeMult = (40f - Math.Abs(num)) / 40f;
				float min = 0;
				if (sans)
					min = 0.2f;
				if (chargeMult < min)
				{
					chargeMult = min;
				}
				NPC.velocity *= chargeMult;
				if (NPC.ai[1] == 40 || (sans && NPC.ai[1] > 40 && NPC.ai[1] % 8 == 0))
                {
					if(Main.netMode != 1)
					{
						toPlayer = player.Center - NPC.Center;
						toPlayer = toPlayer.SafeNormalize(Vector2.Zero);
						int Damage = NPC.GetBaseDamage() / 2;
						if(sans)
							Damage = (int)(Damage * 1.5f);
						if(!sans)
						{
							Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + toPlayer * 40, toPlayer * 5, ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, 0);
						}
						else
						{
							Vector2 spawnPos = NPC.Center + toPlayer * 40;
							Vector2 velo = toPlayer * 5;
							int randMod1 = Main.rand.Next(4);
							int randMod2 = Main.rand.Next(11);
							if(randMod1 == 0)
								velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
							if (randMod1 == 1 || randMod2 == 4)
								spawnPos += new Vector2(Main.rand.NextFloat(-350, 350), Main.rand.NextFloat(-350, 350));
							if(randMod2 <= 1)
                            {
								for(int i = -1; i < 2; i++)
								{
									Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velo.RotatedBy(MathHelper.ToRadians(22.5f * i)), ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
									if (randMod1 == 0)
										velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
								}
                            }
							else if(randMod2 == 10)
							{
								for (int i = 0; i < 8; i++)
								{
									Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velo.RotatedBy(MathHelper.ToRadians(45 * i)), ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
									if (randMod1 == 0)
										velo = velo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-25, 25)));
								}
							}
							else
								Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, velo, ProjectileType<LesserWispLaser>(), Damage, 1f, Main.myPlayer, 0, -1);
						}
					}
                }
				if (NPC.ai[1] >= 80)
                {
					if (sans)
						NPC.ai[1] = -120;
					else
						NPC.ai[1] = -(Main.rand.Next(35, 80 + 20 * total) + total * 10);
					if (Main.netMode != 1)
						NPC.netUpdate = true;
				}
            }
		}
        public override void OnKill()
        {
            base.OnKill();
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			LeadingConditionRule isSans = new LeadingConditionRule(new Common.ItemDropConditions.IsSansWisp());
			LeadingConditionRule notSans = new LeadingConditionRule(new Common.ItemDropConditions.NotSansWisp());
			isSans.OnSuccess(ItemDropRule.Common(ItemType<BookOfVirtues>()));
			isSans.OnSuccess(ItemDropRule.Common(ItemID.LivingUltrabrightFireBlock, 2, 10, 20));
			isSans.OnSuccess(ItemDropRule.Common(ItemType<FragmentOfChaos>(), 1, 1, 1));
			notSans.OnSuccess(ItemDropRule.Common(ItemID.LivingFireBlock, 20, 10, 20));
			notSans.OnSuccess(ItemDropRule.Common(ItemType<FragmentOfInferno>(), 25, 1, 1));
			npcLoot.Add(notSans);
			npcLoot.Add(isSans);
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				int num = 0;
				while ((double)num < damage / (double)NPC.lifeMax * 20.0)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, sans ? 26 : DustID.Torch, (float)(2 * hitDirection), -2f, 0, default, 0.5f);
					dust.scale *= sans? 1.1f : 1.75f;
					num++;
				}
			}
			else
			{
				for (int i = 0; i < particleList.Count; i++)
				{
					Color color = new Color(155, 69, 0, 0);
					if (sans)
						color = new Color(80, 150, 200, 0);
					FireParticle particle = particleList[i];
					Dust dust = Dust.NewDustDirect(new Vector2(particle.position.X - 4, particle.position.Y - 4), 4, 4, DustType<CopyDust4>());
					dust.noGravity = true;
					dust.velocity *= 1.35f;
					dust.scale = particleList[i].scale * 1.25f + 0.25f;
					dust.fadeIn = 0.1f;
					dust.color = color;
				}
				for (int k = 0; k < 30; k++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, sans ? 26 : DustID.Torch, (float)(2 * hitDirection), -2f, 0, default, 1f);
					dust.scale *= sans ? 1.25f : 2.5f;
				}
			}
		}
	}
}