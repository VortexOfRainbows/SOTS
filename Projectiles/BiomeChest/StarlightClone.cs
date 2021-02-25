using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.BiomeChest
{    
    public class StarlightClone : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Clone");
		}
        public override void SetDefaults()
        {
			projectile.CloneDefaults(616);
            aiType = 616; //18 is the demon scythe style
            projectile.width = 72;
            projectile.height = 72; 
            projectile.timeLeft = 6000;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Texture2D texture2 = mod.GetTexture("Projectiles/BiomeChest/StarlightCloneWhite");
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, new Color(130, 255, 255, 0), projectile.rotation, new Vector2(texture.Width/2, texture.Height/2), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture2.Width / 2, texture2.Height / 2), 0.75f, SpriteEffects.None, 0f);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 24;
            height = 24;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            hitbox = new Rectangle((int)projectile.Center.X - 12, (int)projectile.Center.Y - 12, 24, 24);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation(); // + MathHelper.ToRadians(90);
        }
	}	
}
			