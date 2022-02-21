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
			Main.projFrames[projectile.type] = 3;
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 30;
			projectile.width = 14;
			projectile.height = 24;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 20;
			projectile.extraUpdates = 1;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 16;
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 8; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center + new Vector2(-4, -4), 0, 0, DustID.Silver, 0, 0, 0, Color.LightGray);
				dust.noGravity = true;
				dust.scale *= 1.3f;
				dust.velocity *= 0.5f;
				dust.velocity += projectile.velocity * 0.8f;
			}
		}
        public override void AI()
		{
			if(projectile.scale != 0.8f)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 17, 1.1f, -0.1f);
				projectile.scale = 0.8f;
			}
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			projectile.frame = (int)projectile.ai[0];
			for (int i = 0; i < 3; i++)
			{
				Vector2 spawnPos = Vector2.Lerp(projectile.Center, projectile.oldPosition + projectile.Size / 2, i * 0.34f);
				Dust dust = Dust.NewDustDirect(spawnPos + new Vector2(-4, -4), 0, 0, DustID.Silver, 0, 0, 0, Color.LightGray);
				dust.noGravity = true;
				dust.scale = 0.65f;
				dust.velocity = Vector2.Zero;
			}
		}
	}
}
		
			