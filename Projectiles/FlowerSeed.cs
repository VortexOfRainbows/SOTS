using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class FlowerSeed : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Flower Seed");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 1;
			Projectile.width = 34;
			Projectile.height = 38;
            Main.projFrames[Projectile.type] = 2;
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.timeLeft = 3600;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Magic;
			// Projectile.ranged = false /* tModPorter - this is redundant, for more info see https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide#damage-classes */ ;
			Projectile.alpha = 0;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.rotation);
			writer.Write(Projectile.spriteDirection);
			writer.Write(damageCounter);
			writer.Write(latch);
			writer.Write(Projectile.frame);
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
			Projectile.frame = reader.ReadInt32();
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
			if(!latch)
			{
				Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
				Projectile.spriteDirection = 1;
				if(Projectile.velocity.X < 0)
				{
					Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) - MathHelper.ToRadians(180);
					Projectile.spriteDirection = -1;
				}
			}
			if(latch && enemyIndex != -1)
			{
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
					latch = true;
					Projectile.tileCollide = true;
					Projectile.friendly = false;
				}
			}
			if(!Projectile.tileCollide)
			{
				Projectile.velocity *= 0.9f;
			}				
			if(!Projectile.friendly && latch || damageCounter >= 310)
			{
				damageCounter++;
				if(damageCounter % 11 == 0 && damageCounter < 300)
				{
					Projectile.scale -= 0.02f;
				}
				if(damageCounter >= 300 && damageCounter < 315)
				{
					if(damageCounter == 301)
						Projectile.frame = 1;
					Projectile.scale += 0.035f;
					Projectile.rotation += MathHelper.ToRadians(12);
				}
				if(damageCounter == 310)
				{
					Projectile.friendly = true;
					for(int i = 0; i < 360; i += 15)
					{
						Vector2 circularLocation = new Vector2(22, 0).RotatedBy(MathHelper.ToRadians(i));
						
						int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, 231);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 0.1f;
					}
				}
				if(damageCounter >= 315)
				{
					Projectile.rotation += MathHelper.ToRadians(12);
					Projectile.scale -= 0.047f;
					Projectile.alpha += 2;
				}
				if(damageCounter >= 335)
				{
					Projectile.Kill();
				}
			}
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			hitbox = new Rectangle((int)(Projectile.Center.X - 11), (int)(Projectile.Center.Y- 8), 22, 16);
			if(Projectile.frame == 1)
			{
				hitbox = new Rectangle((int)(Projectile.position.X), (int)(Projectile.position.Y), Projectile.width, Projectile.height);
			}
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Projectile.friendly = false;
            target.immune[Projectile.owner] = 0;
			Projectile.tileCollide = false;
			latch = true;
			Projectile.damage = (int)(Projectile.damage * Projectile.ai[1]);
			Projectile.velocity *= 0.1f;
			Projectile.aiStyle = 0;
			enemyIndex = target.whoAmI;
			if (diffPosX == 0)
				diffPosX = target.Center.X - Projectile.Center.X;
			if (diffPosY == 0)
				diffPosY = target.Center.Y - Projectile.Center.Y;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
			if (target.life <= 0)
			{
				enemyIndex = -1;
				Projectile.aiStyle = 1;
				latch = false;
				Projectile.tileCollide = true;
			}
			Projectile.netUpdate = true;
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac) 
		{
			width = 2;
			height = 2;
			fallThrough = true;
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.damage = (int)(Projectile.damage * Projectile.ai[1]);
			enemyIndex = -1;
			Projectile.aiStyle = 0;
			latch = true;
			Projectile.friendly = false;
			Projectile.velocity *= 0.3f;
			Projectile.tileCollide = false;
			return false;
		}
		public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 15; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 2);
				Main.dust[num1].noGravity = true;
			}
		}
	}
}
		