using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Otherworld
{
    public class Poyoyo : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Poyo-yo");
            ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 15f;
			// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
			// Vanilla values range from 3f(Wood) to 16f(Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
			
            ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 240f;
            // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
            // Vanilla values range from 130f(Wood) to 400f(Terrarian), and defaults to 200f
			
            ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 18f;
            // YoyosTopSpeed is top speed of the yoyo Projectile.
            // Vanilla values range from 9f(Wood) to 17.5f(Terrarian), and defaults to 10f
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Otherworld/PoyoyoGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Color color = Color.White;
            Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 0;
            Projectile.width = 16;
            Projectile.height = 16;            
            Projectile.aiStyle = 99;
            Projectile.friendly = true; 
            Projectile.penetrate = -1; 
            Projectile.DamageType = DamageClass.Melee; 
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 14;
			height = 14;
			fallThrough = true;
			return true;
        }
        int storeData = -1;
        public override void PostAI()
        {
            if (storeData == -1 && Projectile.owner == Main.myPlayer)
            {
                storeData = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<RainbowTrail>(), (int)(Projectile.damage * 0.6f) + 1, 0, Projectile.owner, 0, Projectile.whoAmI);
            }
        }
    }
}