using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
 
namespace SOTS.Projectiles
{
    public class HungryHunter : ModProjectile
    {	int enemyIndex = -1;
		bool latch;
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Hungry Hunter");
		}
        public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 30;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 3000;
			Projectile.friendly = true;
			Projectile.aiStyle = 15;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
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
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Hungry_Chain");    //this where the chain of grappling hook is drawn
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
					Projectile.position.X = target.Center.X - Projectile.width/2;
					Projectile.position.Y = target.Center.Y - Projectile.height/2;
				}
				else
				{
					enemyIndex = -1;
				}
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			Player player = Main.player[Projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);	
			Projectile.timeLeft = 3000;
			Projectile.friendly = true;
			latch = true;
			if(player.whoAmI == Main.myPlayer)
			Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center.X, target.Center.Y, 0, 0, ModContent.ProjectileType<Base.HealProj>(), 2, 0, Projectile.owner, 1, 5);
			enemyIndex = target.whoAmI;
			if(target.life <= 0)
			{
				enemyIndex = -1;
			}
        }
    }
}