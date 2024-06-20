using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Chaos
{    
    public class ChimeraFireball : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
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
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 330;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
		public override void OnKill(int timeLeft)
        {

		}
		public override void AI()
		{

		}
    }
    public class ChimeraFireballSmall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 12;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
        public override void SetDefaults()
        {
            Projectile.height = 18;
            Projectile.width = 18;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 330;
            Projectile.tileCollide = false;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override void AI()
        {

        }
    }
}
		