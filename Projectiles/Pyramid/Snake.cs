using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid
{    
    public class Snake : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Snakey Boi");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 50;
			Projectile.height = 18;
			Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = 10;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(enemyIndex);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.rotation = reader.ReadSingle();
			Projectile.spriteDirection = reader.ReadInt32();
			damageCounter = reader.ReadInt32();
			latch = reader.ReadBoolean();
			enemyIndex = reader.ReadInt32();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
		}
		int damageCounter = 0;
		bool latch = false;
		int enemyIndex = -1;
		float diffPosX = 0;
		float diffPosY = 0;
		public override void AI()
        {
			Player player = Main.player[Projectile.owner];
			Projectile.tileCollide = true;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Projectile.spriteDirection = 1;
			if(Projectile.velocity.X < 0)
			{
				Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(180);
				Projectile.spriteDirection = -1;
			}
			
			if(latch && enemyIndex != -1)
			{
				Projectile.netUpdate = true;
				NPC target = Main.npc[enemyIndex];
				if(target.active && !target.friendly)
				{
					Projectile.aiStyle = 0;
					Projectile.position.X = target.Center.X - Projectile.width/2 - diffPosX;
					Projectile.position.Y = target.Center.Y - Projectile.height/2 - diffPosY;
				}
				else
				{
					enemyIndex = -1;
					Projectile.aiStyle = 1;
					latch = false;
					Projectile.tileCollide = true;
					Projectile.friendly = true;
				}
			}
			if(!Projectile.friendly)
			{
				damageCounter++;
				if(damageCounter >= 60)
				{
					damageCounter = 0;
					Projectile.friendly = true;
				}
			}
			if(Projectile.damage <= 1)
			{
				Projectile.Kill();
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Player player = Main.player[Projectile.owner];
			Projectile.friendly = false;
            target.immune[Projectile.owner] = 0;
			Projectile.tileCollide = false;
			latch = true;
			Projectile.damage--;
			Projectile.damage = (int)(Projectile.damage * 0.7f);
			for(int i = 0; i < 200; i++)
			{
				NPC npc = Main.npc[i];
				if(npc == target)
				{
					if(diffPosX == 0)
					diffPosX = npc.Center.X - Projectile.Center.X;
				
					if(diffPosY == 0)
					diffPosY = npc.Center.Y - Projectile.Center.Y;
				
					enemyIndex = i;
					break;
				}
			}
			if(target.life <= 0)
			{
				enemyIndex = -1;
				Projectile.aiStyle = 1;
				latch = false;
				Projectile.tileCollide = true;
				Projectile.friendly = true;
			}
        }
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), 42, 24, 251);
				Main.dust[num1].noGravity = true;
			}
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 12;
			height = 12;
			return true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.penetrate--;
			if(Projectile.penetrate < 1)
			{
				Projectile.Kill();
			}
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X * 0.45f; 
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y * 0.4f;
			}
			return false;
		}
	}
}
		