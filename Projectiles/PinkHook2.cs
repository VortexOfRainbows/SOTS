using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles
{
    public class PinkHook2 : ModProjectile
    {	int enemyIndex = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Hook");
			
		}
        public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;
			projectile.penetrate = -1;
			projectile.timeLeft = 6;
			projectile.friendly = true;
			projectile.tileCollide = false;
        }
        public override bool? SingleGrappleHook(Player player)
        {
			return true;
        }
        public override void PostDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/PinkyHook_Chain");    //this where the chain of grappling hook is drawn
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
        }
		public override void AI()
		{
			projectile.scale = 1.2f;
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.Center.Y - player.Center.Y, (double)projectile.Center.X - player.Center.X) + MathHelper.ToRadians(45);
			projectile.spriteDirection = 1;
			if(projectile.ai[0] == -1)
			{
				if(Main.myPlayer == projectile.owner)
				{
					NPC target = Main.npc[enemyIndex];
					if(target.active)
					{
						projectile.timeLeft = 6;
						projectile.position.X = target.Center.X + (projectile.position.X - projectile.Center.X);
						projectile.position.Y = target.Center.Y - (projectile.Center.Y - projectile.position.Y);
					}
					projectile.netUpdate = true;
				}
			}
			if(target.Center.X - 220 > player.Center.X)
			{
				player.velocity.X += .75f;
			}
			if(target.Center.X + 220 < player.Center.X)
			{
				player.velocity.X -= .75f;
			}
			if(target.Center.Y - 220 > player.Center.Y)
			{
				player.velocity.Y += .75f;
			}
			if(target.Center.Y + 220 < player.Center.Y)
			{
				player.velocity.Y -= .75f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player owner = Main.player[projectile.owner];
            target.immune[projectile.owner] = 0;
			projectile.timeLeft = 6;
			projectile.friendly = false;
			enemyIndex = target.whoAmI;
			projectile.ai[0] = -1;
			projectile.netUpdate = true;
        }
    }
}