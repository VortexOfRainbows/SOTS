using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Lightning
{    
    public class PinkLightning : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink Lightning");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1; 
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.timeLeft = 1200;
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.extraUpdates = 3;
		}
		bool runOnce = true;
		Vector2[] trailPos = new Vector2[4];
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					return false;
				}
				Color color = new Color(140, 130, 140, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = Projectile.GetAlpha(color) * ((trailPos.Length - k) / (float)trailPos.Length) * 0.5f;
				float max = betweenPositions.Length() / (texture.Width * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 5; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.2f * scale;
						float y = Main.rand.Next(-10, 11) * 0.2f * scale;
						if (j < 2)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != Projectile.Center)
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation() + MathHelper.ToRadians(90), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
			return false;
		}
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public void checkPos()
		{
			bool flag = false;
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
					flag = true;
				}
			}
			if (flag || endHow == 1)
			{
				int dust = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 255);
				Main.dust[dust].scale *= 1.2f * (trailPos.Length - iterator) / (float)trailPos.Length;
				Main.dust[dust].velocity *= 1.2f;
				Main.dust[dust].noGravity = true;
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			TriggerStop();
			return false;
		}
		public void TriggerStop()
		{
			endHow = 1;
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
			Projectile.friendly = false;
			Projectile.netUpdate = true;
		}
		int counter = 0;
		public override bool PreAI()
		{
			if (Projectile.timeLeft < 220)
			{
				TriggerStop();
			}
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				runOnce = false;
			}
			Projectile.ai[0] -= 0.02f / 4f;
			if (Main.myPlayer == Projectile.owner && endHow == 0)
			{
				Projectile.velocity.Y += 0.06f * Main.rand.Next(-3, 4);
				Projectile.velocity.X += 0.06f * Main.rand.Next(-3, 4);
			}
			return base.PreAI();
		}
		public override void AI()
		{
			checkPos();
			counter++;
			if (counter >= 0)
			{
				cataloguePos();
				counter = -14;
				if (Projectile.owner == Main.myPlayer)
				{
					if (Projectile.velocity.Length() != 0f)
					{
						Projectile.velocity = new Vector2(Projectile.velocity.Length(), 0).RotatedBy(Projectile.velocity.ToRotation() + MathHelper.ToRadians(Projectile.ai[1]));
						Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
					}
					Projectile.ai[1] = Main.rand.Next(-8, 9);
					Projectile.netUpdate = true;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[Projectile.owner] = 0;
			TriggerStop();
			if (Projectile.ai[0] <= 0)
			{
				return;
			}
			Player player = Main.player[Projectile.owner];
			target.immune[Projectile.owner] = 0;
			if (Projectile.owner == Main.myPlayer)
			{
				int npcIndex = -1;
				double distanceTB = 228;
				for (int i = 0; i < 200; i++) //find first enemy
				{
					NPC npc = Main.npc[i];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						if (npcIndex != i && target.whoAmI != i && npc.whoAmI != (int)Projectile.ai[0])
						{
							float disX = Projectile.Center.X - npc.Center.X;
							float disY = Projectile.Center.Y - npc.Center.Y;
							double dis = Math.Sqrt(disX * disX + disY * disY);
							if (dis < distanceTB)
							{
								distanceTB = dis;
								npcIndex = i;
							}
						}
					}
				}
				if (npcIndex != -1)
				{
					NPC npc = Main.npc[npcIndex];
					if (!npc.friendly && npc.lifeMax > 5 && npc.active && !npc.dontTakeDamage)
					{
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PinkLightningZap>(), (int)(Projectile.damage * 0.6f) + 1, target.whoAmI, Projectile.owner, npc.whoAmI, Projectile.ai[0]);
					}
				}
			}
		}
	}
}
		