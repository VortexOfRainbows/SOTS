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
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.width = 24;
			Projectile.height = 18;
            Main.projFrames[Projectile.type] = 6;
			Projectile.penetrate = 2;
			Projectile.friendly = false;
			Projectile.timeLeft = 1800;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.alpha = 255;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, new Rectangle(0, 18 * Projectile.frame, 24, 18), color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if(Projectile.penetrate != 1)
			{
				target.immune[Projectile.owner] = 10;
			}
			else
			{
				target.immune[Projectile.owner] = 0;
			}
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[Projectile.owner];
			Projectile.ai[1] = 1;
			Projectile.netUpdate = true;
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
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			int type = (int)Projectile.ai[0];
			if (Projectile.ai[1] >= 2)
			{
				Projectile.frame = type;
				Projectile.penetrate = 1;
				if (type == 2)
				{
					Projectile.tileCollide = false;
				}
				if (type == 3)
				{
					Projectile.damage += 10;
				}
				if (type == 4)
				{
					Projectile.penetrate = 2;
				}
				if (type == 5)
				{
					crit = true;
					Projectile.penetrate = 3;
					Projectile.tileCollide = false;
				}
				Projectile.friendly = true;
			}
			if (type == 1)
			{
				Projectile.velocity *= 1.01f;
			}
			if (type == 5)
			{
				Projectile.velocity *= 1.005f;
			}
			if (Projectile.timeLeft < 1710)
			{
				Projectile.tileCollide = true;
			}
			if(Projectile.alpha > 0)
				Projectile.alpha -= 25;
			if(Projectile.ai[1] >= 1)
				return true;
			return false;
		}
		public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(SoundID.Item50, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			Projectile.ai[1] = 0;
		}
		public override void AI()
		{
			if(Projectile.ai[1] == 2)
			{
				SoundEngine.PlaySound(SoundID.Item50, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
				for(int i = 0; i < 360; i += 30)
				{
					Vector2 circularLocation = new Vector2(-7, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].scale = 1.45f;
					circularLocation.Y *= 0.7f;
					Main.dust[num1].velocity = circularLocation * 0.35f;
				}
				Projectile.ai[1] = 0;
				return;
			}
			SoundEngine.PlaySound(SoundID.Item50, (int)(Projectile.Center.X), (int)(Projectile.Center.Y));
			for(int i = 0; i < 360; i += 20)
			{
				Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyIceDust>());
				Main.dust[num1].noGravity = true;
				Main.dust[num1].scale = 1f;
				Main.dust[num1].velocity = circularLocation * 0.35f;
			}
			Projectile.ai[1] = 0;
		}
	}
}
		