using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles
{    
    public class Razorwater : ModProjectile 
    {
		
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Razorwater");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;    
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
			Projectile.DamageType = DamageClass.Melee;
			Projectile.width = 62;
			Projectile.height = 62;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.alpha = 140;
			Projectile.ai[1] = -1;
		}
		public override void AI()
		{
			Projectile.rotation -= 0.37f;
			if(Projectile.ai[1] != -1)
			{
				Projectile proj = Main.projectile[(int)Projectile.ai[1]];
				if(proj.active && proj.type == ModContent.ProjectileType<Zeppelin>() && proj.owner == Projectile.owner)
				{
					Projectile.position.X = proj.Center.X - Projectile.width/2;
					Projectile.position.Y = proj.Center.Y - Projectile.height/2;
					Projectile.timeLeft = 6;
				}
			}
		}
	}
}
		