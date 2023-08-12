using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Permafrost
{    
    public class IceImpale : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("IceImpale");
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.width = 46;
			Projectile.height = 42;
			Projectile.alpha = 255;
			Projectile.timeLeft = 640;
		}
		bool hasHit = false;
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			Player player = Main.player[Projectile.owner];
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            target.immune[Projectile.owner] = 8;
			if(!hasHit)
            {
				if(player.whoAmI == Main.myPlayer)
					Projectile.NewProjectile(Projectile.GetSource_OnHit(target), target.Center, Vector2.Zero, ModContent.ProjectileType<Base.HealProj>(), 2, 0, Projectile.owner, 3f, 4);
				hasHit = true;
            }
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 30;
            height = 30;
            fallThrough = true;
            return true;
        }
		public override void Kill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<IcePulse>(), Projectile.damage, 0, Projectile.owner, -1);
			}
		}
		public override void AI()
		{
			Projectile.alpha -= 20;
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Projectile.spriteDirection = 1;
			if(Projectile.rotation > MathHelper.ToRadians(180))
			{
				Projectile.rotation -= MathHelper.ToRadians(180);
				Projectile.spriteDirection = -1;
			}
			
			if(Projectile.timeLeft % 4 == 0)
			{
				for(int i = 0; i < 360; i += 20)
				{
					Vector2 circularLocation = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(i));
					int num1 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<ModIceDust>());
					Main.dust[num1].noGravity = true;
					Main.dust[num1].velocity = Projectile.velocity * -0.5f;
				}
			}
			/*
			Vector2 trailLoc = new Vector2(18, 0).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
			int num1 = Dust.NewDust(new Vector2(Projectile.Center.X - trailLoc.X - 2, Projectile.Center.Y - trailLoc.Y - 2), 2, 2, 235);
			Main.dust[num1].noGravity = true;
			Main.dust[num1].velocity *= 0.1f;
			*/
		}
	}
}
		
			