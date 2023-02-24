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

namespace SOTS.Projectiles.Camera
{    
    public class DreamingFrame : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
		
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.friendly = true;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 20;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			/*Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}*/
			float scaleMult = 0.8f;
			Color color = Projectile.GetAlpha(new Color(86, 226, 100, 0));
			SOTSProjectile.DrawStar(Projectile.Center, color, 0.3f * windUp / windUpTime, MathHelper.PiOver4, 0f, 4, 16 * scaleMult, 15 * scaleMult, 1f, 360, 6 * scaleMult, 1);
			SOTSProjectile.DrawStar(Projectile.Center, color, 0.45f * windUp / windUpTime, 0, 0f, 4, 4 * scaleMult, 0, 1f, 180, 0, 1);
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Camera/CameraCenterCross");
			Vector2 center = Projectile.Center - Main.screenPosition;
			for(int i = 0; i < 8; i++)
            {
				Vector2 circular = new Vector2(1.5f, 0).RotatedBy(MathHelper.ToRadians(45 * i));
				Main.spriteBatch.Draw(texture, center + circular, null, color, 0, texture.Size() / 2, scaleMult, SpriteEffects.None, 0);
			}				
			return false;
		}
		private static float windUpTime = 20f;
		float windUp = 0f;
		public override void AI()
		{ 
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, new Vector3(86 / 255f, 226 / 255f, 100 / 255f) * windUp / windUpTime);
			Projectile.rotation += 0.2f;
			if (Projectile.alpha > 255)
				Projectile.alpha -= 13; //this should take 20 frames to fully remove
			else
				Projectile.alpha = 0;
			if(windUp < windUpTime)
				windUp += 1;
            if(Main.myPlayer == Projectile.owner)
            {
				Projectile.ai[0] = Main.MouseWorld.X;
				Projectile.ai[1] = Main.MouseWorld.Y;
				Projectile.netUpdate = true;
			}
			if (player.channel)
			{
				if(player.itemTime < 2)
				{
					player.itemAnimation = 2;
					player.itemTime = 2;
					Projectile.timeLeft = 2;
				}
				if(Projectile.timeLeft < 2)
					Projectile.timeLeft = 2;
			}
			if (Projectile.ai[0] > 0 && Projectile.ai[1] > 0)
            {
				Vector2 mousePos = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 toMousePos = mousePos - Projectile.Center;
				Vector2 velo = toMousePos.SafeNormalize(Vector2.Zero) * 3.3f;
				if(velo.Length() > toMousePos.Length())
					velo = toMousePos;
				Projectile.velocity *= 0.45f;
				Projectile.velocity += velo;
				Projectile.Center = Vector2.Lerp(Projectile.Center, mousePos, 0.036f);
            }
		}
	}
}
		