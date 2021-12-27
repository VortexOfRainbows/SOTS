using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Permafrost
{    
    public class Shardstorm : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(endHow);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
		int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shardstorm");
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.width = 44;
			projectile.height = 28;
			projectile.penetrate = -1;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.timeLeft = 37;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 100;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 24;
			if (endHow == 1)
				width = 96;
				hitbox = new Rectangle((int)(projectile.Center.X - width), (int)(projectile.Center.Y - width), width * 2, width * 2);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255f - projectile.alpha) / 255f);
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				projectile.Kill();
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0; 
			triggerStop();
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 24;
			height = 24;
			return base.TileCollideStyle(ref width, ref height, ref fallThrough);
		}
		int endHow = 0;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			triggerStop();
			return false;
		}
		public void triggerStop()
		{
			if (endHow != 0)
				return;
			endHow = 1;
			projectile.tileCollide = false;
			projectile.velocity *= 0f;
			projectile.netUpdate = true;
			projectile.ai[0] = -1;
			
		}
		public void TrailPreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.ai[0] == 0)
				return;
			Texture2D texture = mod.GetTexture("Projectiles/Permafrost/ShardstormTrail");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = projectile.Center;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = projectile.scale * 1.25f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(80, 80, 90, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (2.5f * scale);
				for (int i = 0; i < max; i++)
				{
					drawPos = previousPosition + -betweenPositions * (i / max) - Main.screenPosition;
					for (int j = 0; j < 3; j++)
					{
						float x = Main.rand.Next(-10, 11) * 0.1f * scale;
						float y = Main.rand.Next(-10, 11) * 0.1f * scale;
						if (j == 0)
						{
							x = 0;
							y = 0;
						}
						if (trailPos[k] != projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
		Vector2[] trailPos = new Vector2[10];
		public void cataloguePos()
		{
			Vector2 current = projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			TrailPreDraw(spriteBatch, lightColor);
			return endHow == 0;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
				}
				projectile.netUpdate = true;
				runOnce = false;
			}
			Player player = Main.player[projectile.owner];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.3f / 255f, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f);
			if(projectile.ai[0] == 0)
			{
				count++;
				if (count % 3 == 0) //will activate 12 times
				{
					Vector2 stormPos = projectile.Center - new Vector2(348, 0).RotatedBy(MathHelper.ToRadians((projectile.whoAmI + count) * 15));
					Main.PlaySound(SoundID.Item44, (int)stormPos.X, (int)stormPos.Y);
					if(Main.myPlayer == projectile.owner)
						Projectile.NewProjectile(stormPos, Vector2.Zero, projectile.type, projectile.damage, projectile.knockBack, player.whoAmI, 1, (float)(MathHelper.ToRadians(180) + MathHelper.ToRadians((projectile.whoAmI + count) * 15)));
				}
			}
			else
			{
				cataloguePos();
				checkPos();
				projectile.timeLeft = projectile.timeLeft < 100 ? 720 : projectile.timeLeft;
				if (projectile.timeLeft >= 702)
				{
					projectile.alpha -= 15;
					projectile.ai[1] += MathHelper.ToRadians(10);
					projectile.velocity = new Vector2(8, 0).RotatedBy(projectile.ai[1]);
				}
				else if (projectile.ai[0] == -1)
				{
					Vector2 position = projectile.Center;
					float veloM = 1f;
					int num = 30;
					if(projectile.alpha < 245)
					{
						Main.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 14, 0.7f, 0.1f);
					}
					else
                    {
						veloM = 0.5f;
						num = 6;
					}
					for (int i = 0; i < num; i++)
					{
						int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(5), projectile.width, projectile.height, mod.DustType("CopyDust4"));
						Dust dust = Main.dust[num1];
						dust.velocity *= 2.75f * veloM;
						dust.velocity += projectile.velocity * 0.15f * veloM;
						dust.noGravity = true;
						dust.scale += 0.5f * veloM;
						dust.color = new Color(183, 218, 249, 100);
						dust.fadeIn = 0.1f;
						dust.scale *= 1.74f;
						dust.alpha = 100;
					}
					projectile.ai[0]--;
				}
				else if(projectile.ai[0] == 1)
				{
					if(projectile.timeLeft <= 675)
					{
						projectile.tileCollide = true;
					}
					else if(endHow == 0)
					{
						projectile.timeLeft--;
						projectile.friendly = true;
						projectile.velocity = new Vector2(24, 0).RotatedBy(projectile.ai[1]);
					}
					projectile.alpha += 6;
				}
				if (projectile.alpha > 255)
				{
					projectile.alpha = 255;
					triggerStop();
				}
			}
			projectile.rotation = projectile.ai[1];
		}
	}
}
	