using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles
{    
    public class PinkTracer : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Putrid Tracer");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
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
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 330;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
		public override void OnKill(int timeLeft)
        {
			for(int i = 0; i < 360; i += 24)
			{
				Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
				
				int num1 = Dust.NewDust(Projectile.Center + circularLocation - new Vector2(5), 0, 0, DustID.Gastropod);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity = circularLocation * 0.45f;
			}
		}
		public override void AI()
		{
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - 1, Projectile.Center.Y - 1), 2, 2, DustID.Gastropod);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			
			Projectile.rotation += 0.1f;
		}
	}
}
		