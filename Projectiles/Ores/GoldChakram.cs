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

namespace SOTS.Projectiles.Ores
{    
    public class GoldChakram : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Chakram");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;    
		}
        public override void SetDefaults()
        {
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 715;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.extraUpdates = 2;
		}
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			Projectile.rotation += 0.35f;
			if(Projectile.timeLeft < 700)
			{
				if(Projectile.timeLeft > 610)
				{
					Projectile.velocity *= 0.91f;
				}
				else
				{
					Projectile.velocity = new Vector2(-22.5f, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - player.Center.Y, Projectile.Center.X - player.Center.X));
					Vector2 toPlayer = player.Center - Projectile.Center;
					float distance = toPlayer.Length();
					if(distance < 20)
					{
						Projectile.Kill();
					}
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
            target.immune[Projectile.owner] = 8;
			if(Projectile.timeLeft < 690)
			{
				if(Projectile.timeLeft > 620)
				{
					target.immune[Projectile.owner] = 2;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.timeLeft = Projectile.timeLeft > 705 ? 705 : Projectile.timeLeft;
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item10, Projectile.Center);
			Projectile.tileCollide = false;
			return false;
		}
	}
}
		