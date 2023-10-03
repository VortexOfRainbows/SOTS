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
    public class GoldBolt : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Gold Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
		
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.width = 14;
			Projectile.height = 14;
			Projectile.timeLeft = 236;
			Projectile.penetrate = 1;
			Projectile.alpha = 55;
			Projectile.tileCollide = false;
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
		public override void AI()
		{ 
			Player player = Main.player[Projectile.owner];
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.85f / 255f, (255 - Projectile.alpha) * 0.1f / 255f, (255 - Projectile.alpha) * 0.2f / 255f);
			Projectile.rotation += 0.2f;
			
			if(player.whoAmI == Main.myPlayer)
			{
				Projectile.netUpdate = true;
				Vector2 cursorArea = Main.MouseWorld;
				float dX = 0f;
				float dY = 0f;
				float distance = 0;
				float speed = 3f;
					
				if(Projectile.timeLeft > 192 && Projectile.timeLeft < 212)
				{
					dX = cursorArea.X - Projectile.Center.X;
					dY = cursorArea.Y - Projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
					Projectile.velocity *= 0.9325f;
					Projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
			if(Projectile.timeLeft == 212)
			{				
				for(int i = 0; i < 360; i += 12)
				{
					Vector2 circularLocation = new Vector2(-10, 0).RotatedBy(MathHelper.ToRadians(i));
					
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 235);
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = circularLocation * 0.2f;
				}
			}
				
			if(Projectile.timeLeft == 192) Projectile.tileCollide = true;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 0;
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 360; i += 8)
			{
				Vector2 circularLocation = new Vector2(-16, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.25f;
			}
		}
	}
}
		