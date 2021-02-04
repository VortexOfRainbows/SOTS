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
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color2 = new Color(160, 130, 90, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color2 = projectile.GetAlpha(color2) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.ranged = true;
			projectile.friendly = true;
			projectile.ignoreWater = false;
			projectile.tileCollide = true;
			projectile.timeLeft = 720;
			projectile.width = 12;
			projectile.height = 12;
			projectile.extraUpdates = 4;
			projectile.penetrate = -1;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if (owner.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<GoldLight>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.NextFloat(1000));
			}
			UpdateEnd();
			target.immune[projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Player owner = Main.player[projectile.owner];
			if (owner.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<GoldLight>(), projectile.damage, projectile.knockBack, projectile.owner, Main.rand.NextFloat(1000));
			}
			UpdateEnd();
			return false;
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
			projectile.tileCollide = false;
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
		int counter = 0;
		public override void AI()
		{
			counter++;
			if (Main.rand.NextBool(3) && counter > 12)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num1];
				dust.velocity *= 0.7f;
				dust.noGravity = true;
				dust.color = new Color(255, 165, 0, 100);
				dust.fadeIn = 0.1f;
				dust.scale = 1.5f;
				dust.alpha = 40;
			}
			if (end == true && projectile.timeLeft > 40)
				projectile.timeLeft = 40;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 10; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 165, 0, 100);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 40;
				dust.velocity += projectile.velocity * 0.2f;
			}
		}
	}
}
		
			