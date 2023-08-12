using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Tide
{    
    public class Coconut : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Coconut");
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.rand.NextBool(3))
			{
				target.AddBuff(BuffID.OnFire, 600); //fire for 10 seconds
			}
		}
		public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
            Projectile.hostile = false; 
			Projectile.timeLeft = 60;
			Projectile.alpha = 0;
			Projectile.penetrate = 1;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 14;
			height = 14;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			return false;
        }
		bool runOnce = true;
        public override bool PreAI()
		{
			if(runOnce)
            {
				if (Projectile.ai[0] >= 3)
				{
					Projectile.friendly = true;
				}
				else
                {
					Projectile.friendly = false;
                }
				runOnce = false;
				Projectile.ai[1] = Main.rand.Next(24);
				Projectile.netUpdate = true;
            }
			float scaleMod = Projectile.ai[0];
			if (scaleMod < 0) scaleMod = 0;

			Projectile.scale = 0.6f + 0.5f * scaleMod / 3f;
			Projectile.rotation += Projectile.velocity.X * 0.1f;
			if(Projectile.timeLeft < 30 + Projectile.ai[1] - (scaleMod / 3 * 20))
            {
				Projectile.Kill();
            }
			Projectile.velocity.X *= 0.99f;
			Projectile.velocity.Y += 0.09f * Projectile.scale;
			return true;
		}
		public override void AI()
        {
			for(int i = 0; i < 2; i++)
			{
				Vector2 circularPos = new Vector2(7, -7).RotatedBy(Projectile.rotation) * Projectile.scale;
				int num1 = Dust.NewDust(new Vector2(Projectile.Center.X, Projectile.Center.Y) + circularPos - new Vector2(5), 0, 0, DustID.Torch);
				Main.dust[num1].noGravity = true;
				Main.dust[num1].velocity *= 0.1f;
				Main.dust[num1].scale = Projectile.scale + 0.2f;
			}
		}
		public override void Kill(int timeLeft)
		{
			Terraria.Audio.SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
			Player owner = Main.player[Projectile.owner];
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(owner);
			int RandMod = (int)Projectile.ai[0];
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[num1].velocity *= 1.8f * (0.2f + 0.7f * Projectile.scale);
				Main.dust[num1].scale = Projectile.scale + 0.6f;
			}
			for (int i = 0; i < 10 + RandMod * 2; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, 212);
				Main.dust[num1].velocity *= 1.6f * (0.2f + 0.7f * Projectile.scale);
				Main.dust[num1].scale = Projectile.scale * 0.8f + 0.5f;
			}
			for (int i = 0; i < 5 + RandMod; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, DustID.t_BorealWood);
				Main.dust[num1].velocity *= 1.4f * (0.2f + 0.6f * Projectile.scale);
				Main.dust[num1].scale = Projectile.scale * 0.8f + 0.5f;
			}
			if (Projectile.owner == Main.myPlayer)
			{
				Vector2 randVelo = new Vector2(Main.rand.NextFloat(1, 3 * Projectile.scale), 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				randVelo += Projectile.velocity.SafeNormalize(Vector2.Zero) * Main.rand.NextFloat(-10, 3) * 0.2f;
				randVelo.Y -= 1.4f;
				if (Projectile.ai[0] > 0 || Main.rand.NextBool(20 - RandMod))
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randVelo, Projectile.type, Projectile.damage, Projectile.knockBack * 0.8f, Projectile.owner, Projectile.ai[0] - 1f);
					bool triple = false;
					if ((Projectile.ai[0] > 2) || Main.rand.NextBool(40 - RandMod))
					{
						triple = true;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-90, 90) + 240)) * Main.rand.NextFloat(0.8f, 1.2f), Projectile.type, Projectile.damage, Projectile.knockBack * 0.6f, Projectile.owner, Projectile.ai[0] - 1f);
					}
					if ((Projectile.ai[0] > 1 && Main.rand.NextBool(3)) || Main.rand.NextBool(30 - RandMod))
					{
						float num = 180;
						if (!triple)
							num = 120;
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, randVelo.RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-90, 90) + num)) * Main.rand.NextFloat(0.8f, 1.2f), Projectile.type, Projectile.damage, Projectile.knockBack * 0.7f, Projectile.owner, Projectile.ai[0] - 1);
					}
				}
				for(int i = 0; i < Main.rand.Next(1, 4); i++)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(5, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360))) * Main.rand.NextFloat(0.8f, 1.2f) + new Vector2(0, -3), ModContent.ProjectileType<CoconutShrapnel>(), Projectile.damage, Projectile.knockBack * 0.7f, Projectile.owner);
				}
			}		
        }
	}
}
			