using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Prim.Trails;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.BiomeChest
{    
    public class StarBolt : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starlight Bolt");
		}
        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 54; 
            projectile.timeLeft = 120;
            projectile.penetrate = 1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = false;    
            projectile.ignoreWater = true; 
		}
        public Color projColor(bool reverse = false)
        {
            bool fat = projectile.ai[0] == 0;
            if (reverse)
                fat = !fat;
            return fat ? new Color(85, 167, 237) : new Color(220, 139, 226);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            Color color = projColor();
            color.A = 0;
            float scale = projectile.scale;
            for(int i = 8; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 45));
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + circular, null, color * 0.5f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            color = new Color(100, 100, 100, 0);
            for (int i = 4; i > 0; i--)
            {
                Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(i * 90));
                Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + circular, null, color, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = (int)(projectile.width * projectile.scale);
            hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        bool runOnce = true;
        public override bool PreAI()
        {
            if (runOnce)
            {
                projectile.scale = 0.6f;
                SOTS.primitives.CreateTrail(new StarTrail(projectile, projColor(), projColor(true), 12));
                runOnce = false;
            }
            return true;
        }
        public override void AI()
        {

        }
	}	
}
			