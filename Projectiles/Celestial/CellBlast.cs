using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Buffs;

namespace SOTS.Projectiles.Celestial
{    
    public class CellBlast : ModProjectile
	{
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Celestial/CrossLaserIndicator");
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
			Vector2 origin2 = new Vector2(texture2.Width / 2, texture2.Height / 2);
			Color color = Color.Black;
			color = new Color(100, 255, 100, 0);
			if (projectile.ai[1] > 0)
			{
				Vector2 velo = projectile.velocity.SafeNormalize(Vector2.Zero);
				float scale = 1 - (projectile.ai[1] / 120f);
				Vector2 drawPos = projectile.Center;
				for (int j = 0; j < 100; j++)
				{
					drawPos += velo * scale * (texture2.Width + 0.5f);
					Main.spriteBatch.Draw(texture2, drawPos - Main.screenPosition, null, color * scale, velo.ToRotation(), origin2, scale, SpriteEffects.None, 0.0f);
					scale *= 0.94f;
					scale -= 0.01f;
					if (scale <= 0.05f)
					{
						break;
					}
				}
			}
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 255, 100, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, null, color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(50, 122, 50);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursespire");
		}
        public override void SetDefaults()
        {
			projectile.width = 40;
			projectile.height = 14;
			projectile.friendly = false;
			projectile.timeLeft = 75;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 125;
			projectile.hide = true;
		}
		public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsOverWiresUI.Add(index);
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
		bool runOnce = true;
        public override void AI()
		{
			if (runOnce)
			{
				projectile.ai[1] = 120;
				runOnce = false;
			}
			projectile.ai[1] *= 0.98f;
			projectile.ai[1] -= 0.13f;
			if (projectile.ai[1] < 0)
				projectile.ai[1] = 0;
			projectile.position += projectile.velocity *= 1.033f;
			projectile.rotation = projectile.velocity.ToRotation();
		}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.Center.X, (int)projectile.Center.Y, 92, 0.8f);
			if (Main.netMode != 1)
			{
				Projectile.NewProjectile(projectile.Center, projectile.velocity, ModContent.ProjectileType<BabyLaser>(), projectile.damage, 0, Main.myPlayer, projectile.ai[0]);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit) 
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
	}
}
		