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
			writer.Write(Projectile.friendly);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.friendly = reader.ReadBoolean();
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Arrow");
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 50;
			Projectile.alpha = 255;
			Projectile.ranged = true;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 90;
		}
		float counter = 0;
		bool runOnce = true;
		Color color;
		float scale = 1.0f;
		public const float length = 5.5f;
		public const int timeToEnd = 15;
		public float percentDeath => Projectile.timeLeft / 50f;
		public override bool PreAI()
		{
			if (Projectile.ai[0] == 1)
				scale = 1.25f;
			if (runOnce)
			{
				if(Projectile.ai[0] != 1)
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 96, 1.0f, -0.1f);
				else
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 94, 1.0f, -0.1f);
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
			Projectile.alpha = (int)(255 * (1 - percentDeath));
			counter++;
		}
		public void SetPostitions()
		{
			Vector2 direction = new Vector2(length * scale, 0).RotatedBy(Projectile.velocity.ToRotation());
			int maxDist = (int)(300 / scale);
			Vector2 currentPos = Projectile.Center;
			int k = 0;
			Projectile.ai[1] = -1;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				if(maxDist > (int)(timeToEnd / scale))
				{
					int npcID = isHittingEnemy(currentPos);
					if (npcID != -1)
					{
						Projectile.ai[1] = npcID;
						maxDist = (int)(timeToEnd / scale);
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
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
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
			Projectile.friendly = false;
			Projectile.netUpdate = true;
			if (Main.myPlayer == Projectile.owner && !runOnce)
			{
				float dmgMult = 6f;
				if (Projectile.ai[0] == 1)
					dmgMult = 15f;
				Vector2 position = posList[posList.Count - 2];
				Projectile.NewProjectile(position, Projectile.velocity, ModContent.ProjectileType<ChaosBloomExplosion>(), (int)(dmgMult * Projectile.damage), Projectile.knockBack, Main.myPlayer, -Main.rand.NextFloat(360) - 1, Main.rand.NextFloat(360));
				if(Projectile.ai[0] == 1)
                {
					int otherDir = Main.rand.Next(2) * 2 - 1;
					float rand = Main.rand.NextFloat(360);
					for (int dir = -1; dir <= 1; dir += 2)
					{
						float speedMult = 0.8f + 0.2f * dir;
						for (int i = 0; i < 3; i++)
						{
							Vector2 circular = new Vector2(Main.rand.Next(10, 14) * speedMult, 0).RotatedBy(MathHelper.ToRadians(i * 120 + rand));
							Projectile.NewProjectile(position, circular, ModContent.ProjectileType<ChaosArrowBloom>(), (int)(dmgMult * Projectile.damage), Projectile.knockBack, Main.myPlayer, 2 * speedMult, otherDir * dir * Main.rand.NextFloat(6, 9));
						}
						rand += Main.rand.NextFloat(-20, 20);
					}
				}
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return true;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return (int)Projectile.ai[1] == target.whoAmI;
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
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Chaos/DogmaLaser");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			float alpha = 1;
			Vector2 lastPosition = Projectile.Center;
			float startingScale = 0.1f;
			for (int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if (i > posList.Count - (int)(timeToEnd / scale))
				{
					alpha = 1 - (i - posList.Count + (int)(timeToEnd / scale)) / (float)(int)(timeToEnd / scale);
				}
				Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 4 + counter * -2), this.color) * alpha * (0.2f + 0.8f * percentDeath);
				color.A = 0;
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				if(Projectile.ai[0] == 1)
				{
					for(int j = 0; j < 3; j++)
					{
						Vector2 sinusoid = new Vector2(0, percentDeath * scale * 22 * (float)Math.Sin(MathHelper.ToRadians(i * 3 + counter * 3 + j * 120)) * startingScale).RotatedBy(rotation);
						spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color * 0.4f * startingScale, rotation, origin, new Vector2(scale * 2, scale * percentDeath * 0.4f * startingScale), SpriteEffects.None, 0f);
					}
				}
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
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<Dusts.CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = VoidPlayer.ChaosPink;
				dust.noGravity = true;
				dust.alpha = 60;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.25f;
			}
		}
	}
}