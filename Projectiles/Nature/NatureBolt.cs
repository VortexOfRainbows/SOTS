using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Nature
{    
    public class NatureBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("NatureBotl");
			
		}
		
        public override void SetDefaults()
        {
			projectile.height = 18;
			projectile.width = 18;
			projectile.penetrate = 4;
			projectile.thrown = false;
			projectile.melee = false;
			projectile.magic = false;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.hostile = false;
			projectile.alpha = 50;
			projectile.timeLeft = 110;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if(projectile.alpha >= 250)
			{
			Player player = Main.player[(int)(projectile.ai[1])];
			projectile.position = player.Center;
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Nature/NatureReticle");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 drawPos = trueTarget - Main.screenPosition + new Vector2(0f, projectile.gfxOffY);
			spriteBatch.Draw(texture, drawPos, null, drawColor, projectile.rotation, drawOrigin, 0.30f - projectile.ai[0], SpriteEffects.None, 0f);
			}
		}
		Vector2 targetPos;
		Vector2 trueTarget;
		public override void AI()
		{
			if(projectile.alpha >= 250)
			{
			projectile.ai[0] -= 0.01f;
			}
			projectile.velocity *= 0.97f;
			projectile.rotation += 0.3f;
			Player player = Main.player[(int)(projectile.ai[1])];
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.1f / 255f, (255 - projectile.alpha) * 1.1f / 255f, (255 - projectile.alpha) * 0.1f / 255f);
			if(projectile.ai[0] > 0)
			{
				targetPos = new Vector2(projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
				projectile.ai[0] = 0;
				trueTarget = player.Center + targetPos;
			}
			projectile.alpha += 5;
		}
		public override void Kill(int timeLeft)
		{
			Projectile.NewProjectile(trueTarget.X, trueTarget.Y, 0, 0, mod.ProjectileType("NatureBeat"), projectile.damage, 0, 0);
			Main.PlaySound(2, (int)(projectile.Center.X), (int)(projectile.Center.Y), 93, 0.75f);
		}
	}
}
		