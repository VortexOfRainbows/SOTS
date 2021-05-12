using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.IO;

namespace SOTS.Projectiles.Vibrant 
{    
    public class VibrantBall : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Ball");
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.width = 34;
			projectile.height = 34;
			projectile.alpha = 0;
			projectile.timeLeft = 40;
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(projectile.tileCollide);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			projectile.tileCollide = reader.ReadBoolean();
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for(int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				for (int k = 0; k < 10; k++)
				{
					if(trailPos[k] != Vector2.Zero && trailCircular[k] != -100 && (trailPos[k] != projectile.Center))
					{
						Vector2 circularPos = trailPos[k] + new Vector2(0, trailCircular[k] * direction).RotatedBy(projectile.rotation);
						Color color = VoidPlayer.VibrantColorAttempt((90 + k * 3) * direction);
						Vector2 drawPos = circularPos - Main.screenPosition;
						color = projectile.GetAlpha(color);
						for (int j = 0; j < 2; j++)
						{
							float x = Main.rand.Next(-10, 11) * 0.10f;
							float y = Main.rand.Next(-10, 11) * 0.10f;
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale - k * 0.1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			return false;
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
			float current2 = projectile.ai[0];
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
				float previousPosition2 = trailCircular[i];
				trailCircular[i] = current2;
				current2 = previousPosition2;
			}
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
					trailCircular[i] = -100;
				}
				runOnce = false;
			}
			return true;
		}
		Vector2[] trailPos = new Vector2[10];
		float[] trailCircular= new float[10];
		float dist = 24f;
		public void Explode()
		{
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 28, 0.6f);
			for (int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = VoidPlayer.VibrantColorAttempt(0) * 0.75f;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = 50;
				dust.velocity *= 0.6f;
				dust.velocity -= projectile.velocity * 0.3f;
			}
			if (Main.myPlayer == projectile.owner)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int i = 2; i < 7; i++)
					{
						Vector2 toReach = new Vector2(i * 48, (j - 1) * 24).RotatedBy(projectile.velocity.ToRotation());
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, toReach.X / 20f, toReach.Y / 20f, mod.ProjectileType("VibrantBolt"), projectile.damage, 0, projectile.owner);
					}
				}
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			UpdateEnd();
			return false;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.immune[projectile.owner] = 0;
			UpdateEnd();
        }
        public void UpdateEnd()
		{
			if (projectile.timeLeft > 10)
				projectile.timeLeft = 11;
			projectile.friendly = false;
			projectile.tileCollide = false;
			if (Main.myPlayer == projectile.owner)
				projectile.netUpdate = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 20;
			height = 20;
			return true;
        }
        public override void AI()
		{
			if(projectile.timeLeft > 10)
			{
				projectile.velocity *= 0.94f;
				projectile.rotation = projectile.velocity.ToRotation();
				projectile.ai[1]++;
				dist *= 0.99f;
				projectile.scale += 0.025f;
				Vector2 circular = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1] * 12));
				projectile.ai[0] = circular.Y;
				cataloguePos();
			}
			else if(projectile.timeLeft == 10)
            {
				Explode();
			}
			else
			{
				projectile.velocity *= 0.0f;
				cataloguePos();
			}
		}
	}
}
		
			