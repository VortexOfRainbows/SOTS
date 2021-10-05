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
			projectile.width = 22;
			projectile.height = 22;
			projectile.penetrate = 2;
			projectile.ranged = true;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 9000;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.alpha);
			//writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(projectile.friendly);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.alpha = reader.ReadInt32();
			//damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
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
				projectile.ai[1] = -1;
            }
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + MathHelper.ToRadians(45);
			projectile.spriteDirection = 1;
			
			if(latch && (int)projectile.ai[1] != -1 && projectile.owner == Main.myPlayer)
			{
				NPC target = Main.npc[(int)projectile.ai[1]];
				projectile.alpha += projectile.timeLeft % 10 == 0 ? 1 : 0;
				if (projectile.alpha >= 200)
				{
					projectile.Kill();
				}
				if(target.active && !target.friendly)
				{
					projectile.position.X = target.Center.X - projectile.width/2 - diffPosX;
					projectile.position.Y = target.Center.Y - projectile.height/2 - diffPosY;
				}
				else
				{
					projectile.Kill();
				}
			}
			if(projectile.ai[1] == -1)
            {
				projectile.velocity.Y += 0.09f;
				if (projectile.velocity.Y > 30)
					projectile.velocity.Y = 30;
            }
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 12;
			height = 12;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[projectile.owner];
			projectile.friendly = false;
            target.immune[projectile.owner] = 0;
			projectile.tileCollide = false;
			latch = true;
			
			if(diffPosX == 0)
				diffPosX = target.Center.X - projectile.Center.X;
				
			if(diffPosY == 0)
				diffPosY = target.Center.Y - projectile.Center.Y;

			projectile.ai[1] = target.whoAmI;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
			if (target.life <= 0)
			{
				projectile.Kill();
			}
			projectile.netUpdate = true;
		}
		public override void Kill(int timeLeft)
        {
			if(timeLeft > 1)
				Main.PlaySound(SoundID.Dig, projectile.Center);
			for(int i = 0; i < 18; i++)
			{
				int num1 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Platinum);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity += projectile.velocity * 0.2f;
				Main.dust[num1].scale = 1.25f;
			}
		}
	}
}