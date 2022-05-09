using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class SandBullet : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Bullet");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(160, 130, 90, 0);
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
			Projectile.ranged = true;
			Projectile.friendly = true;
			Projectile.ignoreWater = false;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 720;
			Projectile.width = 12;
			Projectile.height = 12;
			Projectile.extraUpdates = 4;
			Projectile.penetrate = -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[Projectile.owner];
			if (owner.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GoldLight>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.NextFloat(1000));
			}
			UpdateEnd();
			target.immune[Projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player owner = Main.player[Projectile.owner];
			if (owner.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<GoldLight>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Main.rand.NextFloat(1000));
			}
			UpdateEnd();
			return false;
		}
		bool end = false;
		public void UpdateEnd()
		{
			if (Projectile.timeLeft > 40)
				Projectile.timeLeft = 40;
			end = true;
			Projectile.velocity *= 0;
			Projectile.friendly = false;
			Projectile.extraUpdates = 1;
			Projectile.tileCollide = false;
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
		int counter = 0;
		public override void AI()
		{
			counter++;
			if (Main.rand.NextBool(3) && counter > 12)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.color = new Color(255, 165, 0, 100);
				dust.fadeIn = 0.1f;
				dust.scale = 1.5f;
				dust.alpha = 40;
			}
			if (end == true && Projectile.timeLeft > 40)
				Projectile.timeLeft = 40;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 165, 0, 100);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 40;
				dust.velocity += Projectile.velocity * 0.2f;
			}
		}
	}
}
		
			