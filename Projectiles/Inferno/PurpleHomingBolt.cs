using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Inferno
{    
    public class PurpleHomingBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Homing Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color2 = projectile.GetAlpha(color2) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, projectile.rotation, drawOrigin, 1.5f * (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
		{
			projectile.height = 12;
			projectile.width = 12;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 1800;
			projectile.tileCollide = false;
			projectile.extraUpdates = 3;
		}
		bool runOnce = true;
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			UpdateEnd();
			target.immune[projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			UpdateEnd();
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			return false;
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(160, 95, 198, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
				dust.velocity += projectile.velocity * 0.2f;
			}
		}
		bool end = false;
		public void UpdateEnd()
		{
			if (projectile.timeLeft > 40)
				projectile.timeLeft = 40;
			end = true;
			projectile.velocity *= 0;
			projectile.friendly = false;
			projectile.extraUpdates = 1;
			if (Main.myPlayer == projectile.owner)
				projectile.netUpdate = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(end);
			writer.Write(projectile.extraUpdates);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			end = reader.ReadBoolean();
			projectile.extraUpdates = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool PreAI()
		{
			if(runOnce)
			{
				Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 91, 0.8f, 0.1f);
				runOnce = false;
            }
			if (end == true && projectile.timeLeft > 40)
				projectile.timeLeft = 40;
			if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(22))
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(4, 4), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(160, 95, 198, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
			}
			return projectile.timeLeft < 1760;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return (projectile.timeLeft < 1760 ? (bool?)null : false);
        }
        public override void AI()
		{
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.75f;
			if (projectile.friendly == true && projectile.hostile == false)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						if (distance < minDist)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}

				if (target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if (toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						projectile.velocity *= 0.825f;
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
		}
    }
}
		