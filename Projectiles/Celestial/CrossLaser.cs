using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Celestial
{    
    public class CrossLaser : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unholy Light");
		}
        public override void SetDefaults()
        {
            projectile.width = 34;
            projectile.height = 34; 
            projectile.timeLeft = 90;
            projectile.penetrate = 1; 
            projectile.friendly = false; 
            projectile.hostile = false; 
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
			projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsOverWiresUI.Add(index);
        }
        float finalRotation = 0;
		float bonusRotation = 420f;
		bool runOnce = true;
		Color color = Color.White;
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/CrossLaserIndicator");
			if ((int)projectile.ai[0] % 3 == 2)
				texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SunLaser");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Color color = Color.Black;
			color = this.color;
			if(projectile.ai[1] > 0)
			{
				int amt = 4;
				float bonus = 0;
				if ((int)projectile.ai[0] % 3 == 1)
					bonus = 45f;
				if ((int)projectile.ai[0] % 3 == 2)
				{
					bonus = 22.5f;
					amt = 8;
				}
				for (int i = 0; i < amt; i++)
				{
					float deg = 90;
					if (amt == 8)
						deg = 45;
					Vector2 velo = new Vector2(1, 0).RotatedBy(MathHelper.ToRadians(deg * i + bonus));
					float scale = projectile.ai[1] / 120f;
					Vector2 drawPos = projectile.Center;
					for (int j = 0; j < 50; j++)
					{
						drawPos += velo * scale * (texture2.Width + 0.5f);
						Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition, null, color * scale, MathHelper.ToRadians(deg * i + bonus), origin2, scale, SpriteEffects.None, 0.0f);
						scale *= 0.92f;
						scale -= 0.01f;
						if (scale <= 0.05f)
						{
							break;
						}
					}
				}
			}
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = this.color;
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color((int)(this.color.R * 0.5f), (int)(this.color.G * 0.5f), (int)(this.color.B * 0.5f));
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
        }
        public override bool ShouldUpdatePosition()
        {
			return projectile.ai[0] < 3;
        }
        private float lerpMath(float point, float point2, float scale)
		{
			return point * scale + point2 * (1f - scale);
		}
		Vector2 originalLocation;
		float progressMultiplier = 0f;
		public override void AI()
		{
			if(projectile.ai[0] < 3)
				projectile.velocity *= 0.98f;
			if (projectile.timeLeft > 30)
			{
				projectile.scale += 0.0125f;
			}
			else
			{
				projectile.scale -= 0.025f;
			}
			if (runOnce)
			{
				originalLocation = projectile.Center;
				projectile.ai[1] = 120;
				color = new Color(100, 255, 100, 0);
				if ((int)projectile.ai[0] % 3 == 1)
                {
					color = new Color(255, 100, 100, 0);
					finalRotation = 45;
				}
				if ((int)projectile.ai[0] % 3 == 2)
				{
					color = new Color(255, 100, 255, 0);
					finalRotation = 22.5f;
				}
				SoundEngine.PlaySound(SoundID.Item92, (int)projectile.Center.X, (int)projectile.Center.Y);
				runOnce = false;
				DoLine();
			}
			if(projectile.timeLeft < 60 && projectile.ai[0] >= 3)
            {
				progressMultiplier += 1 / 30f;
				if (progressMultiplier > 1)
					progressMultiplier = 1;
				Vector2 toNextLocation = new Vector2(0, 0);
				toNextLocation.X += 320 * projectile.velocity.X;
				toNextLocation.Y += 320 * projectile.velocity.Y;
				projectile.Center = originalLocation + toNextLocation * (float)Math.Pow(progressMultiplier, 1.5f); 
			}
			projectile.ai[1] *= 0.9825f;
			projectile.ai[1] -= 0.125f;
			if (projectile.ai[1] < 0)
				projectile.ai[1] = 0;
			projectile.rotation = MathHelper.ToRadians(bonusRotation + finalRotation);
			bonusRotation = lerpMath(420f, 0, (projectile.timeLeft / 90f) * (projectile.timeLeft / 90f));
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 0.9f / 255f, (255 - projectile.alpha) * 0.3f / 255f);
		}
		public void DoLine()
        {
			Vector2 from = projectile.Center;
			if(projectile.ai[0] >= 3)
			{
				Vector2 to = projectile.velocity.SafeNormalize(Vector2.Zero) * 6;
				for (int i = 0; i < 30; i++)
				{
					from += to;
					if( i > 6)
					{
						int dust3 = Dust.NewDust(from - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust4 = Main.dust[dust3];
						dust4.velocity *= 0.15f;
						dust4.color = color;
						dust4.noGravity = true;
						dust4.fadeIn = 0.1f;
						dust4.scale = 3f;
					}
				}
				Vector2 savePos = from;
				for (int i = 0; i < 8; i++)
				{
					from += projectile.velocity.RotatedBy(MathHelper.ToRadians(-160)).SafeNormalize(Vector2.Zero) * 4;
					int dust3 = Dust.NewDust(from - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.1f;
					dust4.color = color;
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale = 3f;
				}
				from = savePos;
				for (int i = 0; i < 8; i++)
				{
					from += projectile.velocity.RotatedBy(MathHelper.ToRadians(160)).SafeNormalize(Vector2.Zero) * 4;
					int dust3 = Dust.NewDust(from - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					Dust dust4 = Main.dust[dust3];
					dust4.velocity *= 0.1f;
					dust4.color = color;
					dust4.noGravity = true;
					dust4.fadeIn = 0.1f;
					dust4.scale = 3f;
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item93, (int)projectile.Center.X, (int)projectile.Center.Y);
			if (Main.netMode != 1)
			{
				int amt = 4;
				if ((int)projectile.ai[0] % 3 == 2)
					amt = 8;
				for (int i = 0; i < amt; i++)
				{
					float deg = 90;
					if (amt == 8)
						deg = 45;
					Vector2 velo = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(deg * i + finalRotation));
					Projectile.NewProjectile(projectile.Center, velo, ModContent.ProjectileType<CelestialLightning>(), projectile.damage, 0, Main.myPlayer, projectile.ai[0] % 3);
				}
			}
		}
	}
}