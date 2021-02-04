using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class EmeraldBolt : ModProjectile 
    {
		bool end = false;
		int bounceCount = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Emerald Bolt");
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
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color2, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.extraUpdates = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20 * (1 + projectile.extraUpdates);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bounceCount++;
			if (bounceCount > 3)
				UpdateEnd();
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
			bounceCount++;
			if (bounceCount > 3)
				UpdateEnd();
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			initialVelo = projectile.velocity;
		
			return false;
        }
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 14; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 210, 90, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
				dust.velocity += projectile.velocity * 0.2f;
			}
		}
		bool runOnce = true;
		Vector2 initialVelo;
		public void UpdateEnd()
		{
			if (bounceCount > 3)
			{
				if (projectile.timeLeft > 40)
					projectile.timeLeft = 40;
				end = true;
				projectile.velocity *= 0;
				projectile.friendly = false;
				projectile.extraUpdates = 1;
				if(Main.myPlayer == projectile.owner)
					projectile.netUpdate = true;
			}
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.tileCollide);
			writer.Write(projectile.friendly);
			writer.Write(end);
			writer.Write(projectile.extraUpdates);
			writer.Write(bounceCount);
			base.SendExtraAI(writer);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.tileCollide = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			end = reader.ReadBoolean();
			projectile.extraUpdates = reader.ReadInt32();
			bounceCount = reader.ReadInt32();
			base.ReceiveExtraAI(reader);
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				initialVelo = projectile.velocity;
				runOnce = false;
			}
			if (end == true && projectile.timeLeft > 40)
				projectile.timeLeft = 40;
			if ((Main.rand.NextBool(2) && end) || Main.rand.NextBool(22))
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) - new Vector2(4, 4), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				Color color2 = new Color(110, 210, 90, 0);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 40f));
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
	}
}
		