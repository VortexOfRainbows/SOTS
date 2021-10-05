using System;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.BiomeChest
{    
    public class Rebar : ModProjectile
	{
		int counter2 = 72;
		bool startAnim = false;
		float storeRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rebar");
		}
        public override void SetDefaults()
        {
			projectile.width = 36;
			projectile.height = 6;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.alpha = 0; 
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 7200;
			projectile.extraUpdates = 20;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(projectile.alpha);
			writer.Write(projectile.tileCollide);
			writer.Write(latch);
			writer.Write(projectile.friendly);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
			writer.Write(projectile.extraUpdates);
			writer.Write(startAnim);
			writer.Write(storeRot);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			projectile.alpha = reader.ReadInt32();
			projectile.tileCollide = reader.ReadBoolean();
			latch = reader.ReadBoolean();
			projectile.friendly = reader.ReadBoolean();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
			projectile.extraUpdates = reader.ReadInt32();
			startAnim = reader.ReadBoolean();
			storeRot = reader.ReadSingle();
		}
		int counter = 0;
		bool runOnce = true;
		bool latch = false;
		float diffPosX = 0;
		float diffPosY = 0;
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox = new Rectangle((int)projectile.Center.X - 3, (int)projectile.Center.Y - 3, 6, 6);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 6;
			height = 6;
			return true;
        }
        public override void AI()
		{
			counter++;
			Player player = Main.player[projectile.owner];
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			projectile.spriteDirection = 1;
			if (runOnce)
			{
				runOnce = false;
				projectile.ai[1] = -1;
            }
			if (counter > 13 && (projectile.velocity.X != 0 || projectile.velocity.Y != 0))
			{
				Vector2 circular = new Vector2(10 * -projectile.direction, 0).RotatedBy(MathHelper.ToRadians(counter * 20));
				Vector2 circularLocation = new Vector2(0, circular.X).RotatedBy(projectile.velocity.ToRotation());
				int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 235);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].velocity += circularLocation * 0.2f;
				Main.dust[num1].scale = 1.5f;
			}
			if (startAnim)
			{
				float radians = MathHelper.ToRadians(counter2 / 4);
				if (counter2 != 0)
				{
					if (counter2 < 0)
						counter2 += 1;
					if (counter2 > 0)
						counter2 -= 1;
					counter2 *= -1;

				}
				projectile.rotation = storeRot + radians;
				if (projectile.alpha > 250)
					projectile.Kill();
			}
			Latch();
		}
		public void Latch()
		{
			if (latch)
			{
				if(projectile.extraUpdates > 2)
				{
					for (int i = 0; i < 24; i++)
					{
						int num1 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(5), 0, 0, 235);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 1.55f;
						Main.dust[num1].velocity += projectile.velocity * 0.4f;
						Main.dust[num1].scale *= 2.25f;
					}
					triggerAnim();
				}
				if ((int)projectile.ai[1] != -1)
				{
					NPC target = Main.npc[(int)projectile.ai[1]];
					projectile.alpha += projectile.timeLeft % 10 == 0 ? 1 : 0;
					if (target.active && !target.friendly)
					{
						projectile.position.X = target.Center.X - projectile.width / 2 - diffPosX;
						projectile.position.Y = target.Center.Y - projectile.height / 2 - diffPosY;
					}
					else
					{
						projectile.Kill();
					}
				}
				else
				{
					projectile.alpha += projectile.timeLeft % 2 == 0 ? 1 : 0;
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			storeRot = projectile.rotation;
			projectile.friendly = false;
			target.immune[projectile.owner] = 0;
			latch = true;
			if(diffPosX == 0)
				diffPosX = target.Center.X - projectile.Center.X;
			if(diffPosY == 0)
				diffPosY = target.Center.Y - projectile.Center.Y;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
			projectile.ai[1] = target.whoAmI;
			projectile.ai[0] = target.whoAmI;
			if(target.life <= 0)
			{
				projectile.Kill();
			}
			projectile.netUpdate = true;
		}
		public void triggerAnim()
		{
			projectile.velocity *= 0;
			projectile.tileCollide = false;
			projectile.friendly = false;
			startAnim = true;
			projectile.extraUpdates = 2;
			projectile.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			storeRot = projectile.rotation;
			latch = true;
			return false;
		}
	}
}