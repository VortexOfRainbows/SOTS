using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class EmeraldBoltHoming : ModProjectile 
    {
		bool end = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color2 = Projectile.GetAlpha(color2) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
		{
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 1800;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 3;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			UpdateEnd();
			target.immune[Projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			UpdateEnd();
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			initialVelo = Projectile.velocity;
		
			return false;
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 210, 90, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (Projectile.timeLeft / 40f));
				dust.velocity += Projectile.velocity * 0.2f;
			}
		}
		bool runOnce = true;
		Vector2 initialVelo;
		public void UpdateEnd()
		{
			if (Projectile.timeLeft > 40)
				Projectile.timeLeft = 40;
			end = true;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			Projectile.extraUpdates = 1;
			if (Main.myPlayer == Projectile.owner)
				Projectile.netUpdate = true;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(end);
			writer.Write(Projectile.extraUpdates);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			end = reader.ReadBoolean();
			Projectile.extraUpdates = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool PreAI()
		{
			Projectile.velocity.Y += 0.02f;
			if (runOnce)
			{
				initialVelo = Projectile.velocity;
				runOnce = false;
			}
			if (end == true && Projectile.timeLeft > 40)
				Projectile.timeLeft = 40;
			if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(22))
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(4, 4), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 210, 90, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (Projectile.timeLeft / 40f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
			}
			return Projectile.timeLeft < 1680;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return (Projectile.timeLeft < 1680 ? (bool?)null : false);
        }
        public override void AI()
		{
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 1f;
			if (Projectile.friendly == true && Projectile.hostile == false)
			{
				for (int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
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
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
						Projectile.velocity *= 0.85f;
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
		}
    }
}
		