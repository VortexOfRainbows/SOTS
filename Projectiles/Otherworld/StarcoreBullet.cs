using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class StarcoreBullet : ModProjectile 
    {
		Color color = Color.White;
		bool end = false;
		int bounceCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starcore Bullet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 32;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(150, 150, 150, 0).MultiplyRGBA(color);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color2 = Projectile.GetAlpha(color2) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 3; j++)
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
			Projectile.height = 14;
			Projectile.width = 14;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 7200;
			Projectile.tileCollide = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 24 * (1 + Projectile.extraUpdates);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bounceCount++;
			if (bounceCount > 3)
				UpdateEnd();
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if (bounceCount > 3)
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
			for (int i = 0; i < 16; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 110, 110, 0).MultiplyRGBA(color);
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
			if (bounceCount > 3)
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
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Projectile.tileCollide);
			writer.Write(Projectile.friendly);
			writer.Write(end);
			writer.Write(Projectile.extraUpdates);
			writer.Write(bounceCount);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Projectile.tileCollide = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			end = reader.ReadBoolean();
			Projectile.extraUpdates = reader.ReadInt32();
			bounceCount = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				initialVelo = Projectile.velocity;
				runOnce = false;
				color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
			}
			if (end == true && Projectile.timeLeft > 40)
				Projectile.timeLeft = 40;
			if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(24))
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(4, 4), Projectile.width, Projectile.height, Mod.Find<ModDust>("CopyDust4").Type);
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 110, 110, 0).MultiplyRGBA(color);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (Projectile.timeLeft / 40f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
			}
			if (bounceCount > 3)
			{
				UpdateEnd();
				return false;
			}
			return base.PreAI();
        }
        public override void AI()
		{
			int red = color.R;
			int green = color.G;
			int blue = color.B;
			Projectile.ai[0] += 5f;
			if(red > green && red > blue)
            {
				Vector2 circularLocation = new Vector2(6, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
				Projectile.velocity = initialVelo.RotatedBy(MathHelper.ToRadians(circularLocation.X));
			}
			if (green > red && green > blue)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[0]));
				Projectile.velocity = initialVelo.RotatedBy(MathHelper.ToRadians(circularLocation.X));
			}
		}
	}
}
		