using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Projectiles
{
    public class BrittleFlailProj : ModProjectile
    {	
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Flail Proj");
			
		}
        public override void SetDefaults()
		{
			projectile.width = 58;
			projectile.height = 50;
			projectile.penetrate = 1;
			projectile.timeLeft = 3000;
			projectile.friendly = true;
			projectile.aiStyle = 15;
			projectile.melee = true;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/BrittleChain");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
                
            Player player = Main.player[projectile.owner];
               int numberProjectiles = 4;
              for (int i = 0; i < numberProjectiles; i++)
              {
                  Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedByRandom(MathHelper.ToRadians(20)); // This defines the projectiles random spread . 30 degree spread.
                  Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BrittleShard"), projectile.damage, 0, player.whoAmI);

              }
              Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("BrittleBall"), projectile.damage, projectile.knockBack, player.whoAmI);  
                projectile.timeLeft = 0;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 36;
            height = 36;
            fallThrough = true;
            return true;
        }
    }
}