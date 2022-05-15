using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles
{
    public class WormWoodSpike : ModProjectile
    {	int enemyIndex = -1;
		bool latch;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wormwood Spike");
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			//writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(enemyIndex);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			//damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			enemyIndex = reader.ReadInt32();
		}
        public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 32;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3000;
			Projectile.friendly = true;
			Projectile.aiStyle = 15;
			Projectile.DamageType = DamageClass.Melee;
        }
        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/WormWoodVine");    //this where the chain of grappling hook is drawn
                                                      //change YourModName with ur mod name/ and CustomHookPr_Chain with the name of ur one
            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
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
                    color2 = Projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1f, SpriteEffects.None, 0.0f);
                }
            }
			return true;
        }
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			if(latch && player.channel && enemyIndex != -1)
			{
				Projectile.netUpdate = true;
				NPC target = Main.npc[enemyIndex];
				if(target.active && !target.friendly)
				{
					target.position.X = Projectile.Center.X - target.width/2;
					target.position.Y = Projectile.Center.Y - target.height/2;
				}
				else
				{
					enemyIndex = -1;
				}
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
            target.immune[Projectile.owner] = 15;
			Projectile.timeLeft = 3000;
			Projectile.friendly = true;
			latch = true;
			if(target.lifeMax > 10 && !target.boss && target.CanBeChasedBy())
			{
				enemyIndex = target.whoAmI;
			}
			if(target.life <= 0)
			{
				enemyIndex = -1;
			}
        }
    }
}