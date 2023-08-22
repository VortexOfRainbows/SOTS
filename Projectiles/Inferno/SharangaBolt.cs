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

namespace SOTS.Projectiles.Inferno
{    
    public class SharangaBolt : ModProjectile 
    {
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sharanga Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color * 0.8f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
        public override void SetDefaults()
        {
			Projectile.CloneDefaults(1);
            AIType = 1;
			Projectile.penetrate = 3;
			Projectile.alpha = 0;
			Projectile.width = 14;
			Projectile.height = 38;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<SharangaBlast>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 10;
			target.AddBuff(BuffID.OnFire, 60, false);
			if(Projectile.owner == Main.myPlayer && Projectile.penetrate != 1)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<SharangaBlast>(), Projectile.damage, 0, Main.myPlayer);
			}
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 8;
			height = 16;
			fallThrough = true;
			return true;
		}
		public override void AI()
		{
			float rad = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			//Vector2 curve = new Vector2(8f,0).RotatedBy(MathHelper.ToRadians(helixRot * 5f));
			Vector2 curve2 = new Vector2(8f,0).RotatedBy(MathHelper.ToRadians(helixRot * 15f));
			helixRot ++;
			
			Vector2 helixPos3 = new Vector2(18f + curve2.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(18f + curve2.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			/*
			Vector2 helixPos1 = Projectile.Center + new Vector2(curve.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			//Vector2 helixPos2 = Projectile.Center + new Vector2(curve.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			
			int num1 = Dust.NewDust(new Vector2(helixPos1.X - 4, helixPos1.Y - 4), 4, 4, 269);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.15f;
			Main.dust[num1].scale = 1.5f;
			
			//num1 = Dust.NewDust(new Vector2(helixPos2.X - 4, helixPos2.Y - 4), 4, 4, 269);
			//Main.dust[num1].noGravity = true;
			//Main.dust[num1].velocity *= 0.15f;
			//Main.dust[num1].scale = 1.5f
			*/
			int num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos3.X - 4, Projectile.Center.Y + helixPos3.Y - 4), 4, 4, DustID.Torch);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity = helixPos3 * 0.125f;
			Main.dust[num2].scale = 2;
			
			num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos4.X - 4, Projectile.Center.Y + helixPos4.Y - 4), 4, 4, DustID.Torch);
			Main.dust[num2].noGravity = true;
			Main.dust[num2].velocity = helixPos4 * 0.125f;
			Main.dust[num2].scale = 2;
		}
	}
}
		
			