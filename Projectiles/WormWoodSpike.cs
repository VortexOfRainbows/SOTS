using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Projectiles
{
    public class WormWoodSpike : ModProjectile
    {	int enemyIndex = -1;
		bool latch;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("WormWoodSpike");
			
		}
        public override void SetDefaults()
		{
			projectile.width = 28;
			projectile.height = 32;
			projectile.penetrate = -1;
			projectile.timeLeft = 3000;
			projectile.friendly = true;
			projectile.aiStyle = 15;
			projectile.melee = true;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/WormWoodVine");    //this where the chain of grappling hook is drawn
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
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if(latch && player.channel && enemyIndex != -1)
			{
				NPC target = Main.npc[enemyIndex];
				if(target.active && !target.friendly)
				{
					target.position.X = projectile.Center.X - target.width/2;
					target.position.Y = projectile.Center.Y - target.height/2;
				}
				else
				{
					enemyIndex = -1;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			
			Player player = Main.player[projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
            target.immune[projectile.owner] = 15;
			projectile.timeLeft = 3000;
			projectile.friendly = true;
			latch = true;
			voidPlayer.voidMeter += 1;
			for(int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if(npc == target && npc.lifeMax > 10 && !npc.boss)
				{
					enemyIndex = i;
					break;
				}
			}
			if(target.life <= 0)
			{
					enemyIndex = -1;
			}
        }
    }
}