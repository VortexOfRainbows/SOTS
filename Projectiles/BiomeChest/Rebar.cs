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
			Projectile.width = 36;
			Projectile.height = 6;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.timeLeft = 7200;
			Projectile.extraUpdates = 20;
		}
		public override void SendExtraAI(BinaryWriter writer) 
		{
			writer.Write(Projectile.alpha);
			writer.Write(Projectile.tileCollide);
			writer.Write(latch);
			writer.Write(Projectile.friendly);
			writer.Write(diffPosX);
			writer.Write(diffPosY);
			writer.Write(Projectile.extraUpdates);
			writer.Write(startAnim);
			writer.Write(storeRot);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{	
			Projectile.alpha = reader.ReadInt32();
			Projectile.tileCollide = reader.ReadBoolean();
			latch = reader.ReadBoolean();
			Projectile.friendly = reader.ReadBoolean();
			diffPosX = reader.ReadSingle();
			diffPosY = reader.ReadSingle();
			Projectile.extraUpdates = reader.ReadInt32();
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
			hitbox = new Rectangle((int)Projectile.Center.X - 3, (int)Projectile.Center.Y - 3, 6, 6);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 6;
			height = 6;
			return true;
        }
        public override void AI()
		{
			counter++;
			Player player = Main.player[Projectile.owner];
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Projectile.spriteDirection = 1;
			if (runOnce)
			{
				runOnce = false;
				Projectile.ai[1] = -1;
            }
			if (counter > 13 && (Projectile.velocity.X != 0 || Projectile.velocity.Y != 0))
			{
				Vector2 circular = new Vector2(10 * -Projectile.direction, 0).RotatedBy(MathHelper.ToRadians(counter * 20));
				Vector2 circularLocation = new Vector2(0, circular.X).RotatedBy(Projectile.velocity.ToRotation());
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, 235);
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
				Projectile.rotation = storeRot + radians;
				if (Projectile.alpha > 250)
					Projectile.Kill();
			}
			Latch();
		}
		public void Latch()
		{
			if (latch)
			{
				if(Projectile.extraUpdates > 2)
				{
					for (int i = 0; i < 24; i++)
					{
						int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) - new Vector2(5), 0, 0, 235);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].velocity *= 1.55f;
						Main.dust[num1].velocity += Projectile.velocity * 0.4f;
						Main.dust[num1].scale *= 2.25f;
					}
					triggerAnim();
				}
				if ((int)Projectile.ai[1] != -1)
				{
					NPC target = Main.npc[(int)Projectile.ai[1]];
					Projectile.alpha += Projectile.timeLeft % 10 == 0 ? 1 : 0;
					if (target.active && !target.friendly)
					{
						Projectile.position.X = target.Center.X - Projectile.width / 2 - diffPosX;
						Projectile.position.Y = target.Center.Y - Projectile.height / 2 - diffPosY;
					}
					else
					{
						Projectile.Kill();
					}
				}
				else
				{
					Projectile.alpha += Projectile.timeLeft % 2 == 0 ? 1 : 0;
				}
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			storeRot = Projectile.rotation;
			Projectile.friendly = false;
			target.immune[Projectile.owner] = 0;
			latch = true;
			if(diffPosX == 0)
				diffPosX = target.Center.X - Projectile.Center.X;
			if(diffPosY == 0)
				diffPosY = target.Center.Y - Projectile.Center.Y;
			diffPosX *= 0.9f;
			diffPosY *= 0.9f;
			Projectile.ai[1] = target.whoAmI;
			Projectile.ai[0] = target.whoAmI;
			if(target.life <= 0)
			{
				Projectile.Kill();
			}
			Projectile.netUpdate = true;
		}
		public void triggerAnim()
		{
			Projectile.velocity *= 0;
			Projectile.tileCollide = false;
			Projectile.friendly = false;
			startAnim = true;
			Projectile.extraUpdates = 2;
			Projectile.netUpdate = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			storeRot = Projectile.rotation;
			latch = true;
			return false;
		}
	}
}