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
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(endHow);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			endHow = reader.ReadInt32();
		}
		int count = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shardstorm");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.width = 44;
			Projectile.height = 28;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 37;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 100;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			int width = 24;
			if (endHow == 1)
				width = 96;
				hitbox = new Rectangle((int)(Projectile.Center.X - width), (int)(Projectile.Center.Y - width), width * 2, width * 2);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White * ((255f - Projectile.alpha) / 255f);
		}
		public void checkPos()
		{
			float iterator = 0f;
			Vector2 current = Projectile.Center;
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				if (current == previousPosition)
				{
					iterator++;
				}
			}
			if (iterator >= trailPos.Length)
				Projectile.Kill();
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0; 
			triggerStop();
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 24;
			height = 24;
			return true;
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
			Projectile.tileCollide = false;
			Projectile.velocity *= 0f;
			Projectile.netUpdate = true;
			Projectile.ai[0] = -1;
			
		}
		public void TrailPreDraw(ref Color lightColor)
		{
			if (Projectile.ai[0] == 0)
				return;
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Permafrost/ShardstormTrail").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 previousPosition = Projectile.Center;
			float drawAmt = 1f;
			if (SOTS.Config.lowFidelityMode)
				drawAmt = 0.5f;
			for (int k = 0; k < trailPos.Length; k++)
			{
				float scale = Projectile.scale * 1.25f * (trailPos.Length - k) / (float)trailPos.Length;
				if (trailPos[k] == Vector2.Zero)
				{
					break;
				}
				Color color = new Color(80, 80, 90, 0);
				Vector2 drawPos = trailPos[k] - Main.screenPosition;
				Vector2 currentPos = trailPos[k];
				Vector2 betweenPositions = previousPosition - currentPos;
				color = color * ((trailPos.Length - k) / (float)trailPos.Length) * 0.33f;
				float max = betweenPositions.Length() / (2.5f * scale) * drawAmt;
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
						if (trailPos[k] != Projectile.Center)
							spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, betweenPositions.ToRotation(), drawOrigin, scale, SpriteEffects.None, 0f);
					}
				}
				previousPosition = currentPos;
			}
		}
		Vector2[] trailPos = new Vector2[10];
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
		public override bool PreDraw(ref Color lightColor)
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
				Projectile.netUpdate = true;
				runOnce = false;
			}
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.3f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.9f / 255f);
			if(Projectile.ai[0] == 0)
			{
				count++;
				if (count % 3 == 0) //will activate 12 times
				{
					Vector2 stormPos = Projectile.Center - new Vector2(348, 0).RotatedBy(MathHelper.ToRadians((Projectile.whoAmI + count) * 15));
					SoundEngine.PlaySound(SoundID.Item44, (int)stormPos.X, (int)stormPos.Y);
					if(Main.myPlayer == Projectile.owner)
						Projectile.NewProjectile(stormPos, Vector2.Zero, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 1, (float)(MathHelper.ToRadians(180) + MathHelper.ToRadians((Projectile.whoAmI + count) * 15)));
				}
			}
			else
			{
				cataloguePos();
				checkPos();
				Projectile.timeLeft = Projectile.timeLeft < 100 ? 720 : Projectile.timeLeft;
				if (Projectile.timeLeft >= 702)
				{
					Projectile.alpha -= 15;
					Projectile.ai[1] += MathHelper.ToRadians(10);
					Projectile.velocity = new Vector2(8, 0).RotatedBy(Projectile.ai[1]);
				}
				else if (Projectile.ai[0] == -1)
				{
					Vector2 position = Projectile.Center;
					float veloM = 1f;
					int num = 30;
					if(Projectile.alpha < 245)
					{
						SoundEngine.PlaySound(SoundID.Item, (int)position.X, (int)position.Y, 14, 0.7f, 0.1f);
					}
					else
                    {
						veloM = 0.5f;
						num = 6;
					}
					for (int i = 0; i < num; i++)
					{
						int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
						Dust dust = Main.dust[num1];
						dust.velocity *= 2.75f * veloM;
						dust.velocity += Projectile.velocity * 0.15f * veloM;
						dust.noGravity = true;
						dust.scale += 0.5f * veloM;
						dust.color = new Color(183, 218, 249, 100);
						dust.fadeIn = 0.1f;
						dust.scale *= 1.74f;
						dust.alpha = 100;
					}
					Projectile.ai[0]--;
				}
				else if(Projectile.ai[0] == 1)
				{
					if(Projectile.timeLeft <= 675)
					{
						Projectile.tileCollide = true;
					}
					else if(endHow == 0)
					{
						Projectile.timeLeft--;
						Projectile.friendly = true;
						Projectile.velocity = new Vector2(24, 0).RotatedBy(Projectile.ai[1]);
					}
					Projectile.alpha += 6;
				}
				if (Projectile.alpha > 255)
				{
					Projectile.alpha = 255;
					triggerStop();
				}
			}
			Projectile.rotation = Projectile.ai[1];
		}
	}
}
	