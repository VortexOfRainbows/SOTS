using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.NPCs;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class HyperlightLaser : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.friendly);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.friendly = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hyperlight Laser");
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 85;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 5;
		}
		bool runOnce = true;
		Color color;
		float scale = 1.0f;
		public const float length = 11f;
		public float percentDeath => Projectile.timeLeft / 85f;
		public override bool PreAI()
		{
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item94, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.0f, -0.1f);
				DustOut();
				color = ColorHelper.Pastel(MathHelper.ToRadians(Projectile.ai[0] * 90 + SOTSWorld.GlobalCounter * 3));
				SetPostitions();
				runOnce = false;
				return true;
			}
			int amt = Main.rand.Next(2) + 1;
			for(int i = 0; i < amt; i++)
			{
				Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				dust.fadeIn = 0.2f;
				dust.noGravity = true;
				dust.alpha = Projectile.alpha;
				dust.color = color;
				dust.scale *= 1.8f;
				dust.velocity *= 1.15f;
			}
			return true;
		}
		public override void AI()
		{
			Projectile.alpha = (int)(255 * (1 - percentDeath));
		}
		List<int> npcIDs = new List<int>();
		public void SetPostitions()
		{
			float radians = Projectile.velocity.ToRotation();
			int maxDist = (int)(150 / scale);
			Vector2 currentPos = Projectile.Center;
			float originalRadians = radians;
			int k = 0;
			Projectile.ai[1] = -1;
			while (maxDist > 0)
			{
				float redirectStrength = k / 50f;
				redirectStrength = MathHelper.Clamp(redirectStrength, 0, 1);
				Vector2 direction = new Vector2(length * scale, 0).RotatedBy(radians);
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				int i = (int)currentPos.X / 16;
				int j = (int)currentPos.Y / 16;
				if(SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					if (maxDist > 1)
						maxDist = 1;
				}
				if(npcIDs.Count < 10 && maxDist > 10)
				{
					int npcTarget = FindTarget(currentPos, npcIDs, 480 * redirectStrength);
					if (npcTarget != -1)
					{
						NPC target = Main.npc[npcTarget];
						radians = Redirect(radians, currentPos, target.Center, redirectStrength);
						Rectangle projHitbox = new Rectangle((int)currentPos.X - Projectile.width / 2, (int)currentPos.Y - Projectile.height / 2, Projectile.width, Projectile.height);
						Rectangle targetHitbox = new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height);
						if (projHitbox.Intersects(targetHitbox))
						{
							npcIDs.Add(npcTarget);
							redirectGrowth = 0;
						}
						maxDist++;
					}
					else
					{
						Vector2 rnVelo = new Vector2(length * scale, 0).RotatedBy(radians);
						rnVelo += new Vector2(1.5f, 0).RotatedBy(originalRadians);
						radians = rnVelo.ToRotation();
					}
				}
				maxDist--;
				Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.fadeIn = 0.2f;
				dust.noGravity = true;
				dust.alpha = 100;
				dust.color = color;
				dust.scale *= 1.6f;
				dust.velocity *= 1.7f;
			}
		}
		public int FindTarget(Vector2 center, List<int> ignore, float minDistance = 2000f)
		{
			int target = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].CanBeChasedBy() && !ignore.Contains(i) && Collision.CanHitLine(center - new Vector2(16, 16), 32, 32, Main.npc[i].position, Main.npc[i].width, Main.npc[i].height))
				{
					float distance = (center - Main.npc[i].Center).Length();
					if (distance < minDistance)
					{
						target = i;
						minDistance = distance;
					}
				}
			}
			return target;
		}
		float redirectGrowth = 0.0f;
		public float Redirect(float radians, Vector2 pos, Vector2 npc, float speedMult)
		{
			Vector2 toNPC = npc - pos;
			float speed = (2.0f + redirectGrowth) * speedMult;
			Vector2 rnVelo = new Vector2(length * scale, 0).RotatedBy(radians);
			rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
			float npcRad = rnVelo.ToRotation();
			redirectGrowth += 0.1f;
			return npcRad;
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (Projectile.alpha >= 150)
			{
				return false;
			}
			float width = Projectile.width * scale * 1.5f;
			float height = Projectile.height * scale * 1.5f;
			for (int i = 0; i < posList.Count; i++)
			{
				Vector2 pos = posList[i];
				projHitbox = new Rectangle((int)pos.X - (int)width / 2, (int)pos.Y - (int)height / 2, (int)width, (int)height);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}
        List<Vector2> posList = new List<Vector2>();
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/HyperlightOrb");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			for (int i = 0; i < 8; i++)
			{
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Color color = ColorHelper.PastelGradient(MathHelper.ToRadians(i * 45 - SOTSWorld.GlobalCounter), this.color);
				color.A = 0;
				Vector2 dynamicAddition = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(45 * i) + SOTSWorld.GlobalCounter * 2);
				Main.spriteBatch.Draw(texture, drawPos + dynamicAddition, null, color * 0.5f * percentDeath, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0f);
			}
			texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/DogmaLaser");
			origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float alpha = 1;
			Vector2 lastPosition = Projectile.Center;
			float startingScale = 0.1f;
			for (int i = 2; i < posList.Count - 2; i++)
			{
				float otherScale = 1;
				if (i > posList.Count - (int)(15 / scale))
				{
					otherScale = 1 - (i - posList.Count + (int)(15 / scale)) / (float)(int)(15 / scale);
				}
				Vector2 drawPos = posList[i];
				Color color = this.color * alpha * percentDeath;
				color.A = 0;
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				for (int j = 0; j < 2; j++)
				{
					Vector2 sinusoid = new Vector2(0, scale * 24 * (float)Math.Sin(MathHelper.ToRadians(i * 2 - SOTSWorld.GlobalCounter * 3 + j * 180)) * startingScale * otherScale).RotatedBy(rotation);
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color, rotation, origin, new Vector2(scale * 2, scale * 0.4f), SpriteEffects.None, 0f);
				}
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color, rotation, origin, new Vector2(scale * 2, scale), SpriteEffects.None, 0f);
				if (startingScale < 1f || alpha != 1)
					startingScale += 0.05f;
				startingScale = MathHelper.Clamp(startingScale, 0, 1);
			}
			return false;
		}
        public void DustOut()
		{
			for (int i = 0; i < 5; i++)
			{
				Vector2 circularLocation = Main.rand.NextVector2Circular(4, 4);
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = ColorHelper.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}