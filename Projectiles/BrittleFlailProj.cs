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
    {	double prevdis = -1.0;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brittle Flail");
			
		}
        public override void SetDefaults()
		{
			projectile.CloneDefaults(301);
            aiType = 301; 
			projectile.width = 38;
			projectile.height = 38;
			projectile.penetrate = -1;
			projectile.timeLeft = 3000;
			projectile.friendly = true;
			projectile.melee = true;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/BrittleChain");  
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
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
			float xdis = player.Center.X - projectile.Center.X;
			float ydis = player.Center.Y - projectile.Center.Y;
			double dis = Math.Sqrt(xdis * xdis + ydis * ydis);
			if(dis > 200 || !player.channel)
			{
				Snap(0);
			}
			
			prevdis = dis;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
			Snap(0);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			return false;
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Snap(180f);
        }
		public void Snap(float degree)
		{
			Player player = Main.player[projectile.owner];
			if(projectile.alpha != 255)
			{
				Main.PlaySound(SoundID.Item50, (int)(projectile.Center.X), (int)(projectile.Center.Y));
				int numberProjectiles = Main.rand.Next(2,4) + 1;
				if(Main.myPlayer == projectile.owner)
				{
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = new Vector2(projectile.velocity.X * 4, projectile.velocity.Y * 4).RotatedByRandom(MathHelper.ToRadians(7 * numberProjectiles)).RotatedBy(MathHelper.ToRadians(degree));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BrittleShard"), (int)(.65f * projectile.damage), 0, player.whoAmI);
			
					}
					Vector2 perturbedSpeed2 = new Vector2(projectile.velocity.X * 4, projectile.velocity.Y * 4).RotatedBy(MathHelper.ToRadians(degree));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed2.X, perturbedSpeed2.Y, mod.ProjectileType("BrittleBall"), projectile.damage, projectile.knockBack, player.whoAmI);  
				}
				projectile.alpha = 255;
			}
			player.channel = false;
			projectile.friendly = false;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 24;
            height = 24;
            fallThrough = true;
            return true;
        }
    }
}