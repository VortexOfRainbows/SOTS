using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceShard : ModProjectile 
    {
		bool crit = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Shard");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.width = 24;
			projectile.height = 18;
            Main.projFrames[projectile.type] = 6;
			projectile.penetrate = 2;
			projectile.friendly = false;
			projectile.timeLeft = 1800;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.alpha = 255;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, new Rectangle(0, 18 * projectile.frame, 24, 18), color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(projectile.penetrate != 1)
			{
				target.immune[projectile.owner] = 10;
			}
			else
			{
				target.immune[projectile.owner] = 0;
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			projectile.ai[1] = 1;
			projectile.netUpdate = true;
			if (Main.rand.Next(10) == 0)
			{
				target.AddBuff(BuffID.Frostburn, 300, false);
			}
			else if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.Frostburn, 90, false);
			if (this.crit)
			{
				crit = true;
			}
			else
			{
				crit = false;
			}
		}
		public override bool PreAI()
		{
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			int type = (int)projectile.ai[0];
			if (projectile.ai[1] >= 2)
			{
				projectile.frame = type;
				projectile.penetrate = 1;
				if (type == 2)
				{
					projectile.tileCollide = false;
				}
				if (type == 3)
				{
					projectile.damage += 10;
				}
				if (type == 4)
				{
					projectile.penetrate = 2;
				}
				if (type == 5)
				{
					crit = true;
					projectile.penetrate = 3;
					projectile.tileCollide = false;
				}
				projectile.friendly = true;
			}
			if (type == 1)
			{
				projectile.velocity *= 1.01f;
			}
			if (type == 5)
			{
				projectile.velocity *= 1.005f;
			}
			if (projectile.timeLeft < 1710)
			{
				projectile.tileCollide = true;
			}
			if(projectile.alpha > 0)
				projectile.alpha -= 25;
			if(projectile.ai[1] >= 1)
				return true;
			return false;
		}
		public override void Kill(int timeLeft)
        {
			Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			projectile.ai[1] = 0;
		}
		public override void AI()
		{
			if(projectile.ai[1] == 2)
			{
				Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				for(int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-7, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.45f;
					circularLocation.Y *= 0.7f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				projectile.ai[1] = 0;
				return;
			}
			Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X + circularLocation.X - 4, projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			projectile.ai[1] = 0;
		}
	}
}
		