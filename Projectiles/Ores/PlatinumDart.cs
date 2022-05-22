using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Ores
{    
    public class PlatinumDart : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Platinum Dart");
		}
        public override void SetDefaults()
        {
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.penetrate = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 9000;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.alpha);
			//writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(Projectile.friendly);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.alpha = reader.ReadInt32();
			//damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
		}
        public override bool ShouldUpdatePosition()
        {
            return !latch;
        }
        bool runOnce = true;
		bool latch = false;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			if(runOnce)
            {
				runOnce = false;
				Projectile.ai[1] = -1;
            }
			Player player = Main.player[Projectile.owner];
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + MathHelper.ToRadians(45);
			Projectile.spriteDirection = 1;
			
			if(latch && (int)Projectile.ai[1] != -1 && Projectile.owner == Main.myPlayer)
			{
				NPC target = Main.npc[(int)Projectile.ai[1]];
				Projectile.alpha += Projectile.timeLeft % 10 == 0 ? 1 : 0;
				if (Projectile.alpha >= 200)
				{
					Projectile.Kill();
				}
				if(target.active && !target.friendly)
				{
					Projectile.position.X = target.Center.X - Projectile.width/2 - diffPosX;
					Projectile.position.Y = target.Center.Y - Projectile.height/2 - diffPosY;
				}
				else
				{
					Projectile.Kill();
				}
			}
			if(Projectile.ai[1] == -1)
            {
				Projectile.velocity.Y += 0.09f;
				if (Projectile.velocity.Y > 30)
					Projectile.velocity.Y = 30;
            }
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 12;
			height = 12;
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			Projectile.friendly = false;
            target.immune[Projectile.owner] = 0;
			Projectile.tileCollide = false;
			latch = true;
			
			if(diffPosX == 0)
				diffPosX = target.Center.X - Projectile.Center.X;
				
			if(diffPosY == 0)
				diffPosY = target.Center.Y - Projectile.Center.Y;

			Projectile.ai[1] = target.whoAmI;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
			if (target.life <= 0)
			{
				Projectile.Kill();
			}
			Projectile.netUpdate = true;
		}
		public override void Kill(int timeLeft)
        {
			if(timeLeft > 1)
				Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
			for(int i = 0; i < 18; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Platinum);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity += Projectile.velocity * 0.2f;
				Main.dust[num1].scale = 1.25f;
			}
		}
	}
}