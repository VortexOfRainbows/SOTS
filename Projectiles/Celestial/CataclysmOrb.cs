using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Projectiles.Lightning;

namespace SOTS.Projectiles.Celestial
{    
    public class CataclysmOrb : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Bomb");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(48);
            aiType = 48; 
			projectile.thrown = false;
			projectile.magic = false;
			projectile.melee = false;
			projectile.ranged = true;
			projectile.width = 14;
			projectile.height = 14;
			projectile.penetrate = 1;
			projectile.timeLeft = 900;
			projectile.alpha = 0;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = new Color(100, 155, 100, 0);
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(2.5f, 4f), 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, drawPos + circular, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = Color.White;
			spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}
		public override void AI()
		{
			if(projectile.timeLeft >= 800)
			{
				projectile.timeLeft = Main.rand.Next(32, 48);
			}
		}
		public override void Kill(int timeLeft)
        {
			SoundEngine.PlaySound(3, (int)projectile.Center.X, (int)projectile.Center.Y, 53, 0.625f);
			for (int i = 0; i < 10; i++)
			{
				var num371 = Dust.NewDust(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += projectile.velocity * 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(210, 255, 205, 100), new Color(30, 150, 60, 100), new Vector2(-0.5f, 0).RotatedBy(Main.rand.Next(360)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = projectile.alpha;
			}
			SoundEngine.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.55f, 0.1f);
			if (projectile.owner == Main.myPlayer)
			{
				int amt = 1;
				if (Main.rand.NextBool(3))
					amt++;
				if (Main.rand.NextBool(3))
					amt++;
				for (int i = 0; i < amt; i++)
				{
					Vector2 circular = -projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<CataclysmLightning>(), (int)(projectile.damage * 0.9f + 0.5f), 0, projectile.owner, 0);
				}
			}
		}
	}
}
			