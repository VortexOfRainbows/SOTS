using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Banners;
using SOTS.Items.Chaos;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Projectiles.Chaos;
using SOTS.Projectiles.Planetarium;
using SOTS.Void;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SOTS.NPCs.Phase
{
	public class PhaseSpeeder : ModNPC
	{
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !NPC.dontTakeDamage;
        }
        private float tracerPosX
		{
			get => NPC.ai[2];
			set => NPC.ai[2] = value;
		}
		private float tracerPosY
		{
			get => NPC.ai[3];
			set => NPC.ai[3] = value;
		}
		public override void SetStaticDefaults()
        {
            NPCID.Sets.NoMultiplayerSmoothingByType[NPC.type] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Venom] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Ichor] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.BetsysCurse] = true;
        }
		public override void SetDefaults()
		{
            NPC.aiStyle = -1; 
            NPC.lifeMax = 650;   
            NPC.damage = 64; 
            NPC.defense = 42;  
            NPC.knockBackResist = 0f; //take no knockback
            NPC.width = 62;
            NPC.height = 54;
            NPC.value = Item.buyPrice(0, 0, 40, 0);
            NPC.npcSlots = 1.5f;
			NPC.HitSound = SoundID.NPCHit4;
			NPC.DeathSound = SoundID.NPCDeath14;
			NPC.lavaImmune = true;
			NPC.netAlways = true;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
			Banner = NPC.type;
			BannerItem = ItemType<PhaseSpeederBanner>();
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
			TrailPreDraw(spriteBatch, screenPos);
			Texture2D texture = Terraria.GameContent.TextureAssets.Npc[NPC.type].Value;
			//Texture2D texture2 = GetTexture("SOTS/NPCs/Phase/PhaseSpeederGlow");
			Texture2D texture3 = (Texture2D)Request<Texture2D>("SOTS/NPCs/Phase/PhaseSpeederPink");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height / 2);
			float dir = NPC.rotation;
			bool flip = false;
			if (Math.Abs(MathHelper.WrapAngle(dir)) <= MathHelper.ToRadians(90))
			{
				flip = true;
			}
			float bonusDir = !flip ? MathHelper.ToRadians(180) : 0;
			for(int i = 0; i < 4; i++)
            {
				Vector2 circular = new Vector2(2, 0).RotatedBy(NPC.rotation + i * MathHelper.PiOver2);
				spriteBatch.Draw(texture3, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY) + circular, NPC.frame, new Color(100, 100, 100, 0) * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f)), dir - bonusDir, drawOrigin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White * ((255 - NPC.alpha) / 255f), dir - bonusDir, drawOrigin, NPC.scale, !flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public void TrailPreDraw(SpriteBatch spriteBatch, Vector2 screenPos)
		{
			Texture2D texture = (Texture2D)Request<Texture2D>("SOTS/NPCs/Phase/PhaseSpeederTrail");
			Vector2 drawOrigin = new Vector2(0, texture.Height * 0.5f);
			Vector2 previousPosition = NPC.Center + new Vector2(-24, 0).RotatedBy(NPC.rotation);
			for (int k = 0; k < trailPos.Length; k++)
			{
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = ColorHelpers.ChaosPink * (0.1f + 0.9f * ((255 - NPC.alpha) / 255f));
				color.A = 0;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				Vector2 drawPos = trailPos[k] - screenPos;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				float lengthTowards = betweenPositions.Length() / texture.Height;
				spriteBatch.Draw(texture, drawPos, null, color, betweenPositions.ToRotation(), drawOrigin, new Vector2(lengthTowards * 3, 2f), SpriteEffects.None, 0f);
				previousPosition = currentPos;
			}
		}
		Vector2[] trailPos = new Vector2[40];
		public void cataloguePos()
		{
			Vector2 current = NPC.Center + new Vector2(-24, 0).RotatedBy(NPC.rotation);
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
			if(Main.rand.NextBool(4))
			{
				Vector2 from = NPC.Center + new Vector2(-24, 0).RotatedBy(NPC.rotation);
				Dust dust = Dust.NewDustDirect(from - new Vector2(5), 0, 0, DustType<CopyDust4>(), 0, 0, NPC.alpha, ColorHelpers.ChaosPink, 1.4f);
				dust.velocity *= 0.3f;
				dust.velocity += new Vector2(-2, 0).RotatedBy(NPC.rotation);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.alpha = (int)MathHelper.Clamp(NPC.alpha - 20, 0, 255);
			}
		}
		public void MoveCursorToPlayer()
		{
			Player player = Main.player[NPC.target];
			Vector2 between = player.Center - new Vector2(tracerPosX, tracerPosY);
			float length = between.Length();
			float speed = 32f;
			if (speed > length)
			{
				speed = length;
			}
			between = between.SafeNormalize(Vector2.Zero) * speed;
			tracerPosX += between.X;
			tracerPosY += between.Y;
		}
		int direction = 0;
		float spinny = 0;
		public bool runOnce = true;
		public override bool PreAI()
		{
			NPC.TargetClosest(false);
			Player player = Main.player[NPC.target];
			if (runOnce)
			{
				direction = NPC.whoAmI % 2 * 2 - 1;
				tracerPosX = NPC.Center.X;
				tracerPosY = NPC.Center.Y;
				NPC.ai[0] = Main.rand.Next(360);
				if (Main.netMode == NetmodeID.Server)
					NPC.netUpdate = true;
				runOnce = false;
			}
			cataloguePos();
			return true;
		}
		public override void AI()
		{
			Player player = Main.player[NPC.target];
			MoveCursorToPlayer();
			NPC.ai[1]++;
			//Vector2 toPlayer = player.Center - npc.Center;
			if (NPC.ai[1] >= 90)
			{
				if (NPC.ai[1] >= 135)
				{
					NPC.ai[1]++;
				}
				float scale = (float)Math.Sqrt((NPC.ai[1] - 90) / 90f); //make the curve better
				float sinusoid = scale * 6 * (float)Math.Sin(MathHelper.ToRadians(NPC.ai[1] - 90) * 6f); // 6 * 90 = 540
				Vector2 rotatePos = toTracer.SafeNormalize(Vector2.Zero) * sinusoid;
				NPC.Center += rotatePos;
				if (NPC.ai[1] >= 180)
				{
					spinny = 1080;
					NPC.ai[1] = -110;
					rotatePos = rotatePos.SafeNormalize(Vector2.Zero);
					NPC.velocity = rotatePos * -30f;
					if(Main.netMode != NetmodeID.MultiplayerClient)
						NPC.netUpdate = true;
					SOTSUtils.PlaySound(SoundID.Item92, (int)NPC.Center.X, (int)NPC.Center.Y);
					return;
				}
			}
			else if (NPC.ai[1] > -40)
			{
				if (NPC.ai[1] < 30)
                {
					if(NPC.ai[1] == -29)
					{
						direction *= -1;
						NPC.ai[0] = MathHelper.ToDegrees((NPC.Center - player.Center).ToRotation());
					}
					NPC.ai[0] += direction;
                }
				else
                {
					NPC.alpha -= 8;
                }
				float speed = (float)Math.Sin(MathHelper.ToRadians((NPC.ai[1] + 30) * 1.2f)); //finished 180 degree
				NPC.ai[0] += speed * direction * 1.33f;
				Vector2 rotatePos = new Vector2(320, 0).RotatedBy(MathHelper.ToRadians(NPC.ai[0] * (NPC.whoAmI % 2 * 2 - 1))); //rotates cw or ccw depending on index
				Vector2 toPos = rotatePos + tracerPos;
				Vector2 goToPos = NPC.Center - toPos;
				float length = goToPos.Length();
				if (length > 12)
				{
					length = 12;
				}
				goToPos = goToPos.SafeNormalize(Vector2.Zero);
				NPC.velocity = Vector2.Lerp(NPC.velocity, goToPos * -length, 0.05f);
			}
			if(NPC.ai[1] < 0)
			{
				if (NPC.ai[1] < -70)
				{
					NPC.alpha = 0;
				}
				else
					NPC.alpha += 8;
				NPC.velocity *= 0.97725f;
				if(NPC.ai[1] > -90)
					NPC.velocity = NPC.velocity.RotatedBy(MathHelper.ToRadians(direction * -2.4f));
				NPC.rotation = toTracer.ToRotation() + MathHelper.ToRadians(spinny);
				spinny = MathHelper.Lerp(0, 1440, (NPC.ai[1] / 110f) * (NPC.ai[1] / 110f));
				if(Math.Abs(NPC.ai[1]) % 10 == 0 && NPC.ai[1] < -70 && Main.netMode != NetmodeID.MultiplayerClient)
				{
					int damage = NPC.GetBaseDamage() / 2;
					for (int i = -2; i <= 2; i++)
                    {
						Vector2 spread = new Vector2(2.5f, 0).RotatedBy(MathHelper.ToRadians(i * 30) + NPC.rotation);
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + spread.SafeNormalize(Vector2.Zero) * 20, spread, ProjectileType<PhaseDart>(), damage, 0, Main.myPlayer, 0);
					}
                }
			}
			NPC.alpha = (int)MathHelper.Clamp(NPC.alpha, 0, 255);
			if(NPC.alpha > 200)
            {
				NPC.dontTakeDamage = true;
            }
			else
				NPC.dontTakeDamage = false;
		}
        public override void PostAI()
		{
			if (NPC.ai[1] >= 0)
				NPC.rotation = toTracer.ToRotation();
		}
		public Vector2 tracerPos => new Vector2(tracerPosX, tracerPosY);
		public Vector2 toTracer => tracerPos - NPC.Center;
		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemType<PhaseOre>(), 5, 4, 6));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfChaos>(), 8, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemType<TwilightShard>(), 12, 1, 1));
			npcLoot.Add(ItemDropRule.Common(ItemType<FragmentOfOtherworld>(), 20, 1, 1));
		}
        public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			if (NPC.life > 0)
			{
				int num = 0;
				while (num < hit.Damage / NPC.lifeMax * 60.0)
				{
					if(Main.rand.NextBool(3))
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Gold, (float)(2 * hit.HitDirection), -2f, 0, default, 0.9f);
					else
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Platinum, (float)(2 * hit.HitDirection), -2f, 0, default, 0.9f);
					}
					num++;
				}
			}
			else
			{
				if (Main.netMode != NetmodeID.Server)
				{
					for (int i = 0; i < trailPos.Length; i++)
					{
						if (Main.rand.NextBool(3))
						{
							Dust.NewDust(trailPos[i] - new Vector2(8, 8), 8, 8, DustID.PinkTorch, (float)(2 * hit.HitDirection), -2f, 0, default, 2f);
						}
					}
				}
				for (int k = 0; k < 50; k++)
				{
					if (k % 4 == 0)
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.PinkTorch, (float)(2 * hit.HitDirection), -2f, 0, default, 2f);
					}
					if (Main.rand.NextBool(3))
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Gold, (float)(2 * hit.HitDirection), -2f, 0, default, 0.9f);
					else
					{
						Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Platinum, (float)(2 * hit.HitDirection), -2f, 0, default, 0.9f);
					}
				}
				for (int i = 1; i <= 3; i++)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, ModGores.GoreType("Gores/PhaseSpeeder/PhaseSpeederGore" + i), 1f);
			}
		}
	}
}