using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Evil
{    
    public class SteelShrapnel : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Steel Shrapnel");
			Main.projFrames[Projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.tileCollide = true;
			Projectile.ranged = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 30;
			Projectile.width = 14;
			Projectile.height = 24;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.idStaticNPCHitCooldown = 20;
			Projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 16;
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Silver, 0, 0, 0, Color.LightGray);
				dust.noGravity = true;
				dust.scale *= 1.3f;
				dust.velocity *= 0.5f;
				dust.velocity += Projectile.velocity * 0.8f;
			}
		}
        public override void AI()
		{
			if(Projectile.scale != 0.8f)
			{
				SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 17, 1.1f, -0.1f);
				Projectile.scale = 0.8f;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.frame = (int)Projectile.ai[0];
			for (int i = 0; i < 3; i++)
			{
				Vector2 spawnPos = Vector2.Lerp(Projectile.Center, Projectile.oldPosition + Projectile.Size / 2, i * 0.34f);
				Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, DustID.Silver, 0, 0, 0, Color.LightGray);
				dust.noGravity = true;
				dust.scale = 0.65f;
				dust.velocity = Vector2.Zero;
			}
		}
	}
}
		
			