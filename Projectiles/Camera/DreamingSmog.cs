using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;
using System.Collections.Generic;
using SOTS.Buffs.Debuffs;

namespace SOTS.Projectiles.Camera
{    
    public class DreamingSmog : ModProjectile
	{
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			height = 24;
			width = 24;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<VoidMagic>();
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.width = 36;
			Projectile.height = 36;
			Projectile.alpha = 0;
			Projectile.timeLeft = 120;
			Projectile.localNPCHitCooldown = 120;
			Projectile.usesLocalNPCImmunity = true;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(48 * Projectile.scale);
			hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - width / 2), width, width);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Projectile.velocity *= 0.5f;
			Projectile.tileCollide = false;
			return false;
        }
		public static float LockOntoEnemyTime = 24;
		public static float StarMinDistance = 60f;
		public static float StarAddedDistance = 4f;
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
			Texture2D ProjTexture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
			Color color = DreamingFrame.Green1;
			float scaleMult = MathHelper.Clamp(Counter / 10f, 0, 1);
			float starWindUp = MathHelper.Clamp(Counter / 25f, 0, 1);
			float starWindUp2 = 0;
			float starSizeBonus = 0.3f;
			if(timerTimeLeft < 24)
			{
				starWindUp2 = MathHelper.Clamp(1 - timerTimeLeft / 24f, 0, 1);
			}
			float xCompress = 1 - MathHelper.Clamp((Projectile.velocity.Length() - Counter * 0.15f) / 30f, 0, 0.5f);
			float colorMult = scaleMult * 0.6f + starWindUp * 0.4f;
			if (Projectile.timeLeft < 20)
			{
				scaleMult += (float)Math.Sqrt(MathHelper.Lerp(0.66f, 0, Projectile.timeLeft / 20f));
				colorMult *= Projectile.timeLeft / 20f;
			}
			scaleMult *= (1.0f + (starSizeBonus - starSizeBonus * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp))) - 1.1f * (float)Math.Sin(MathHelper.ToRadians(300 * starWindUp2 * starWindUp2)));
			scaleMult *= 0.65f;
			Main.spriteBatch.Draw(ProjTexture, Projectile.Center - Main.screenPosition, null, color * 0.12f * colorMult, 0, ProjTexture.Size() / 2, scaleMult * 1.75f, SpriteEffects.None, 0);
			for (int i = 0; i < 8; i++)
			{
				Vector2 circular = new Vector2(6f, 0).RotatedBy(MathHelper.ToRadians(45 * i) + Projectile.rotation);
				Main.spriteBatch.Draw(ProjTexture, Projectile.Center + circular - Main.screenPosition, null, color * 0.06f * colorMult, 0, ProjTexture.Size() / 2, scaleMult * 1.5f, SpriteEffects.None, 0);
			}
			if (starWindUp > 0)
			{
				Vector2 offSet = Projectile.velocity * MathHelper.Clamp(-5 + Counter * 0.35f, -5, 0);
				SOTSProjectile.DrawStar(Projectile.Center + offSet, color, 0.3f * colorMult, Projectile.rotation, MathHelper.ToRadians(Counter * 3 * Projectile.direction), 4, 10f * scaleMult, 6f * scaleMult, xCompress, 480, 2f * scaleMult, 1);
				SOTSProjectile.DrawStar(Projectile.Center + offSet * 2, color, 0.45f * colorMult, Projectile.rotation, MathHelper.ToRadians(-Counter * 2.4f * Projectile.direction), 4, 4f * scaleMult, 2, xCompress, 240, 0, 1);
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.225f * colorMult, Projectile.rotation, MathHelper.ToRadians(-Counter * Projectile.direction), 2, 8f * scaleMult, 12, xCompress, 180, 0, 1);
				SOTSProjectile.DrawStar(Projectile.Center, color, 0.225f * colorMult, Projectile.rotation, MathHelper.ToRadians(-Counter * Projectile.direction) + MathHelper.PiOver2, 2, 8f * scaleMult, 16, xCompress, 210, 0, 1);
				for (int i = 0; i < 8; i++)
				{
					Vector2 circular = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(45 * i));
					Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, color * 0.9f * starWindUp * colorMult * colorMult, 0, texture.Size() / 2, 0.5f + 0.5f * scaleMult, SpriteEffects.None, 0);
				}
			}
			if(timerTimeLeft > 0)
				DrawStars(colorMult);
			return false;
		}
		public void DrawStars(float alphaMult)
		{
			float add = 0;
			for (int i = 0; i < npcVectors.Count; i++)
			{
				float progress = npcVectors[i].Y / (float)LockOntoEnemyTime;
				progress = MathHelper.Clamp(progress, 0, 1f);
				DrawStarPointingTowardEnemy(npcVectors[i].center + new Vector2(0, -2), progress * alphaMult, StarMinDistance + add, npcVectors[i].Z, i);
				if (i > 3)
					add += StarAddedDistance;
			}
		}
		public void DrawStarPointingTowardEnemy(Vector2 npcCenter, float progress, float radius, float degrees, int i)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
			Vector2 drawPosition;
			Color color = DreamingFrame.Green1;
			float scaleMult = 0.5f + 0.5f * 32f / radius;
			float starWindUp = progress;
			float colorMult = 0.75f;
			drawPosition = getStarPosition(npcCenter, radius, degrees);
			float toCenter = (Projectile.Center - drawPosition).ToRotation();
			if (starWindUp > 0)
			{
				scaleMult *= (1.3f - 0.3f * (float)Math.Cos(MathHelper.ToRadians(420 * starWindUp)));
				if(npcVectors[i].X >= 0)
					DrawChainsBetween(npcCenter, Projectile.Center, progress, degrees);
				int pointNumber = 4;
				if (i > 3)
					pointNumber = 2;
				else
					colorMult = 0.375f;
				SOTSProjectile.DrawStar(drawPosition, color * colorMult * starWindUp, starWindUp, toCenter, 0, pointNumber, 1f * scaleMult, 0, 1f, 25 * pointNumber, 0, 1, pointNumber == 2);
				if(pointNumber == 4)
					for (int j = 0; j < 4; j++)
					{
						Vector2 circular = new Vector2(2f, 0).RotatedBy(MathHelper.PiOver2 * j);
						Main.spriteBatch.Draw(texture, drawPosition + circular - Main.screenPosition, null, color * 0.5f * starWindUp * starWindUp, toCenter + MathHelper.PiOver4, texture.Size() / 2, 0.25f + 0.25f * scaleMult, SpriteEffects.None, 0);
					}
			}
		}
		public Vector2 getStarPosition(Vector2 npcCenter, float radius, float degrees)
		{
			float sinusoid = 0;
			if (timerTimeLeft < 30)
			{
				float sProgress = 1 - timerTimeLeft / 30f;
				float size = 16 + 24 * (1 - timerTimeLeft / 30f);
				sinusoid = size * (float)Math.Sin(MathHelper.ToRadians(430 * sProgress));
				radius += sinusoid;
			}
			Vector2 drawPos = Projectile.Center;
			Vector2 toNPC = npcCenter - Projectile.Center;
			float startingRadians = 240 + degrees;
			startingRadians = MathHelper.Lerp(startingRadians, 0, Math.Clamp(Counter / 82f, 0, 1));
			float rotation = toNPC.ToRotation();
			Vector2 circularPosition = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(startingRadians));
			circularPosition.Y *= 0.3f + 0.4f * timerTimeLeft / 90f;
			circularPosition = circularPosition.RotatedBy(rotation);
			drawPos += circularPosition;
			return drawPos;
		}
		public void DrawChainsBetween(Vector2 start, Vector2 destination, float progress, float degrees)
		{
			Texture2D textureGradient = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/DuelGradient"); //This could be swapped for a more suitable chain texture in the future, but it looks decent and fits the current asthetic
			Color color = DreamingFrame.Green1;
			float addedHeight = 1f;
			if (timerTimeLeft < 30)
			{
				addedHeight += 1.5f * (1 - timerTimeLeft / 30f);
			}
			progress = Math.Clamp(progress, 0, 1);
			if (progress > 0)
			{
				Vector2 toOther = destination - start;
				float dist = toOther.Length();
				float multiplier = Math.Clamp((1 - (dist / (DendroChainNPCOperators.maxPullDistance + 120))), 0, 1) * progress * (1.0f - 0.5f * (float)Math.Cos(MathHelper.ToRadians(2 * Counter + degrees)));
				Main.spriteBatch.Draw(textureGradient, start - Main.screenPosition, null, color * (0.25f * multiplier * (0.6f + 0.4f * addedHeight)), toOther.ToRotation(), new Vector2(1, 1), new Vector2(1f / textureGradient.Width * dist, addedHeight + 1f * (1 - multiplier)), SpriteEffects.None, 0);
			}
		}
		public int type => (int)Projectile.ai[0];
		public int timerTimeLeft => Projectile.timeLeft - 20;
		public float Counter
		{
			get => Projectile.ai[1];
			set => Projectile.ai[1] = value;
		}
		bool runOnce = true;
		public void Activate()
		{
			float add = 0;
			SOTSUtils.PlaySound(SoundID.Item84, Projectile.Center, 0.6f, 0.4f);
			int total = 8;
			for (int i = 0; i < total; i++)
			{
				float scaleMult = 1f;
				Vector2 FlowerVe = new Vector2(-StarMinDistance - 40f, 0f).RotatedBy(MathHelper.ToRadians(i * 360f / total) + MathHelper.PiOver4);
				if (i % 2 != 0)
					scaleMult = 0.6f;
				Vector2 velo = Main.rand.NextVector2Circular(1, 1) + FlowerVe.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(2f, 3f);
				SOTSProjectile.DustStar(Projectile.Center + FlowerVe, velo * scaleMult, DreamingFrame.Green1 * scaleMult, 0f, 40, 0, 4, 8f, 5f, 1f, 1f * (0.2f + 0.8f * scaleMult));
			}
			for (int i = 0; i < 45; i++)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(5f, 12f), 0).RotatedBy(MathHelper.ToRadians(i * 8f + Main.rand.NextFloat(-4f, 4f)));
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
				dust.color = DreamingFrame.Green1;
				dust.velocity = dust.velocity * 0.2f + circular + Projectile.velocity * 0.75f;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.5f + 1.0f;
			}
			if (Main.myPlayer == Projectile.owner)
			{
				for (int i = 0; i < npcVectors.Count; i++)
				{
					int ID = (int)npcVectors[i].X;
					if (ID >= 0)
					{
						NPC npc = Main.npc[ID];
						if (isNPCValidTarget(npc))
						{
							Vector2 toOther = npc.Center - Projectile.Center;
							int damage = Projectile.damage;
							Vector2 position = Projectile.Center + new Vector2(40 + StarMinDistance + add, 0).RotatedBy(toOther.ToRotation());
							float dist = (npc.Center - position).Length();
							Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, toOther.SafeNormalize(Vector2.Zero), ModContent.ProjectileType<DreamLaser>(), damage, Projectile.knockBack, Main.myPlayer, -ID - 0.5f, dist);
						}
					}
					if (i > 3)
						add += StarAddedDistance;
				}
			}
		}
		public Color getDustColor => DreamingFrame.Green1;
		public List<StarSelector> npcVectors = new List<StarSelector>();
		public bool RunOnce = true;
		public bool RunOnce2 = true;
		public float initialYSpeed;
        public override bool PreAI()
		{
			if(runOnce)
            {
				runOnce = false;
				initialYSpeed = Projectile.velocity.Y;
            }
			Projectile.velocity.X *= MathHelper.Clamp(0.88f + Counter * 0.008f, 0, 0.98f);
			Projectile.velocity.Y *= MathHelper.Clamp(1f - Counter * 0.005f, 0, 1);
			float fallSpeed = 0.05f;
			if(initialYSpeed > 0)
            {
				fallSpeed = 0.05f - initialYSpeed * 0.025f;
            }
			Projectile.velocity.Y += MathHelper.Clamp(-0.225f + Counter * 0.01f, -0.15f, fallSpeed - 0.015f);
			Projectile.rotation = Projectile.velocity.ToRotation();
			Counter++;
			if(Projectile.timeLeft <= 21 && RunOnce2)
			{ 
				Activate();
				RunOnce2 = false;
				Counter += 8 * (1 - Projectile.timeLeft / 21f);
			}
			return true;
        }
        public override void AI()
		{
			if((int)type == 1)
            {
				ManageNPCList();
				if(timerTimeLeft > 30 && timerTimeLeft < 78 && Counter % 3 == 0)
					AddClosestNPCInRange();
				if(Counter % 3 == 0 && Counter > 0 && Counter <= 12) //5
                {
					int uniqueID = -(1 + (int)Counter / 3);
					npcVectors.Add(new StarSelector(uniqueID, 0, Main.rand.NextFloat(360), Projectile.Center + new Vector2(360, 360).RotatedBy(MathHelper.ToRadians(uniqueID * 90f))));
                }
            }
		}
		public static bool isNPCValidTarget(NPC npc)
		{
			return npc.active && !npc.friendly && !npc.immortal;// && npc.HasBuff<DendroChain>();
		}
		public bool StarSelectorListContainsX(List<StarSelector> sList, int x)
		{
			for (int i = 0; i < sList.Count; i++)
			{
				if (sList[i].X == x)
				{
					return true;
				}
			}
			return false;
		}
		public void AddClosestNPCInRange()
		{
			float maxLength = DendroChainNPCOperators.maxPullDistance;
			float bestLength = maxLength + 120;
			int bestID = -1;
			Vector2 bestCenter = Vector2.Zero;
			for (int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
				if (!StarSelectorListContainsX(npcVectors, i) && isNPCValidTarget(target))
				{
					float currentDistance = Vector2.Distance(Projectile.Center, target.Center);
					if(currentDistance < bestLength)
                    {
						bestID = i;
						bestLength = currentDistance;
						bestCenter = target.Center;
					}
				}
			}
			if(bestID != -1)
            {
				npcVectors.Add(new StarSelector(bestID, 0, Main.rand.NextFloat(360), bestCenter));
			}
		}
		public void ManageNPCList()
		{
			float add = 0;
			for (int i = 0; i < npcVectors.Count; i++)
			{
                if (npcVectors[i].X >= 0)
				{
					NPC npc = Main.npc[(int)npcVectors[i].X];
					if (!isNPCValidTarget(npc))
					{
						npcVectors[i].X = -1;
					}
					else
						npcVectors[i].center = npc.Center;
				}
				else
                {
                    if (npcVectors[i].X <= -2)
					{
						float degrees = npcVectors[i].X * 90f;
						float radians = MathHelper.ToRadians(degrees);
						npcVectors[i].center = Projectile.Center + new Vector2(360, 360).RotatedBy(radians);
					}
                }
				if (npcVectors[i].Y < LockOntoEnemyTime)
				{
					npcVectors[i].Y++;
					if (npcVectors[i].Y == 6)
					{
						SOTSUtils.PlaySound(SoundID.MenuTick, Projectile.Center, 1.2f, -0.1f);
					}
				}
				Vector2 position = getStarPosition(npcVectors[i].center, StarMinDistance + add, npcVectors[i].Z);
				if (npcVectors[i].Y >= 5 && timerTimeLeft > 0)
				{
					float scaleFactor = 0.75f;
					int maxJ = 1;
					if (npcVectors[i].Y == 5)
                    {
						scaleFactor = 1.0f;
						maxJ = 12;
					}
					for (int j = 0; j < maxJ; j++)
					{
						Vector2 outward = Vector2.Zero;
						if (j >= 1)
						{
							outward = Main.rand.NextVector2Circular(6, 6);
						}
						Dust dust = Dust.NewDustDirect(position - new Vector2(5) + outward, 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
						dust.color = DreamingFrame.Green1 * 1f;
						dust.velocity = dust.velocity * 0.2f * scaleFactor + outward + Projectile.velocity * 0.75f;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale = dust.scale * 0.2f + scaleFactor + 0.1f;
					}
				}
				if (i > 3)
					add += StarAddedDistance;
			}
		}
	}
	public class StarSelector
    {
		public float X = 0;
		public float Y = 0;
		public float Z = 0;
		public Vector2 center = Vector2.Zero;
		public StarSelector(float x, float y, float z, Vector2 center)
        {
            X = x;
            Y = y;
            Z = z;
            this.center = center;
        }
    }
}
		
			