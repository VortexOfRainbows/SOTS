using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Projectiles
{
    public class TangleFlower : ModProjectile
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TangleFlower");
			
		}
        public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 22;
			projectile.penetrate = -1;
			projectile.timeLeft = 7200;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.tileCollide = false;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/TangleVine");  
			
            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.projectile[(int)projectile.ai[1]].Center;
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
		bool returnTo = false;
		int counter = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			
			projectile.rotation += 0.8f;
			
			if(!player.channel || !Main.projectile[(int)projectile.ai[1]].active || Main.projectile[(int)projectile.ai[1]].type != 17)
			{
				for(int i = 0; i < 10; i++)
				{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
				Main.dust[num1].noGravity = true;
				}
				projectile.Kill();
			}
			if(returnTo)
			{
				counter++;
				Projectile proj = Main.projectile[(int)projectile.ai[1]];
				float disX = proj.Center.X - projectile.Center.X;
				float disY = proj.Center.Y - projectile.Center.Y;
				Vector2 newVelocity = new Vector2(11, 0).RotatedBy(Math.Atan2(disY, disX));
				if(counter > 20)
				projectile.velocity = newVelocity;
				if(Math.Abs(disX) < 24f && Math.Abs(disY) < 24f && counter > 20)
				{
					for(int i = 0; i < 10; i++)
					{
					int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 2);
					Main.dust[num1].noGravity = true;
					}
					projectile.Kill();
				}
			}
			else
			{
				NPC npc = Main.npc[(int)projectile.ai[0]];
				if(!npc.active)
				returnTo = true;
				float disX = npc.Center.X - projectile.Center.X;
				float disY = npc.Center.Y - projectile.Center.Y;
				Vector2 newVelocity = new Vector2(8f, 0).RotatedBy(Math.Atan2(disY, disX));
				projectile.velocity = newVelocity;
			}
			
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			damage--;
			returnTo = true;
			projectile.velocity *= 0.75f;
        }
    }
}