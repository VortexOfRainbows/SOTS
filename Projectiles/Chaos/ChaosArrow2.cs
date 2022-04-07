using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{    
    public class ChaosArrow2 : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.friendly);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.friendly = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Arrow");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 50;
			projectile.alpha = 255;
			projectile.ranged = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 90;
		}
		bool runOnce = true;
		Color color;
		float scale = 1.0f;
		public const float length = 5.5f;
		public float percentDeath => projectile.timeLeft / 50f;
		public override bool PreAI()
		{
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 96, 1.0f, -0.1f + Main.rand.NextFloat(-0.1f, 0.1f));
				DustOut();
				color = VoidPlayer.ChaosPink;
				SetPostitions();
				runOnce = false;
				return true;
			}
			return true;
		}
		public override void AI()
		{
			projectile.alpha = (int)(255 * (1 - percentDeath));
			projectile.ai[1]++;
		}
		public void SetPostitions()
		{
			Vector2 direction = new Vector2(length * scale, 0).RotatedBy(projectile.velocity.ToRotation());
			int maxDist = 300;
			Vector2 currentPos = projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				if(maxDist > 20)
				{
					int npcID = isHittingEnemy(currentPos);
					if (npcID != -1)
					{
						maxDist = 20;
					}
				}
				if (!Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.4f;
					dust.velocity *= 1.5f;
				}
				maxDist--;
			}
		}
		public int isHittingEnemy(Vector2 position)
		{
			float width = projectile.width * scale;
			float height = projectile.height * scale;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC npc = Main.npc[i];
				if (npc.active && !npc.friendly && !npc.dontTakeDamage)
				{
					Rectangle projHitbox = new Rectangle((int)position.X - (int)width / 2, (int)position.Y - (int)height / 2, (int)width, (int)height);
					Rectangle targetHitbox = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					if (projHitbox.Intersects(targetHitbox))
					{
						return npc.whoAmI;
					}
				}
			}
			return -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.friendly = false;
			projectile.netUpdate = true;
			if (Main.myPlayer == projectile.owner && !runOnce)
			{
				Vector2 position = posList[posList.Count - 2];
				Projectile.NewProjectile(position, projectile.velocity, ModContent.ProjectileType<ChaosBloomExplosion>(), projectile.damage, projectile.knockBack, Main.myPlayer, -Main.rand.NextFloat(360) - 1, Main.rand.NextFloat(360));
				if(projectile.ai[0] == 1)
                {
					int otherDir = Main.rand.Next(2) * 2 - 1;
					float rand = Main.rand.NextFloat(360);
					for (int dir = -1; dir <= 1; dir += 2)
					{
						float speedMult = 0.8f + 0.2f * dir;
						for (int i = 0; i < 3; i++)
						{
							Vector2 circular = new Vector2(Main.rand.Next(7, 10) * speedMult, 0).RotatedBy(MathHelper.ToRadians(i * 120 + rand));
							Projectile.NewProjectile(position, circular, ModContent.ProjectileType<ChaosArrowBloom>(), projectile.damage, projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(1.8f, 2.4f) * speedMult, otherDir * dir * Main.rand.NextFloat(6, 11));
						}
						rand += Main.rand.NextFloat(-20, 20);
					}
				}
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.alpha >= 100)
			{
				return false;
			}
			float width = projectile.width * scale;
			float height = projectile.height * scale;
			for (int i = 0; i < posList.Count - 10; i += 2)
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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Chaos/DogmaLaser");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float alpha = 1;
			Vector2 lastPosition = projectile.Center;
			float startingScale = 0.1f;
			for (int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if (i > posList.Count - 20)
				{
					alpha = 1 - (i - posList.Count + 20) / 20f;
				}
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 4 + projectile.ai[1] * -2), this.color) * alpha * (0.2f + 0.8f * percentDeath);
				color.A = 0;
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? projectile.velocity.ToRotation() : direction.ToRotation();
				//Vector2 sinusoid = new Vector2(0, percentDeath * scale * 18 * (float)Math.Sin(MathHelper.ToRadians(i * 3 + projectile.ai[1] * 3))).RotatedBy(rotation);
				//spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color * 0.4f * startingScale, rotation, origin, new Vector2(scale * 2, scale * percentDeath * 0.5f * startingScale), SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color * 0.6f * startingScale, rotation, origin, new Vector2(scale * 2, scale * percentDeath * startingScale), SpriteEffects.None, 0f);
				if (startingScale < 1f || alpha != 1)
					startingScale += 0.05f;
				if(alpha == 1)
					startingScale = MathHelper.Clamp(startingScale, 0, 1);
			}
			return false;
		}
        public void DustOut()
		{
			for (int i = 0; i < 5; i++)
			{
				Vector2 circularLocation = Main.rand.NextVector2Circular(4, 4);
				int dust2 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += projectile.velocity * 0.2f;
				dust.color = VoidPlayer.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}