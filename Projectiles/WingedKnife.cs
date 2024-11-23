using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SOTS.Common.GlobalNPCs;
using SOTS.Dusts;
using Terraria.ID;
using SOTS.Items.AbandonedVillage;

namespace SOTS.Projectiles
{    
    public class WingedKnife : ModProjectile 
    {
        public override void SetDefaults()
        {
			Projectile.aiStyle = 2;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.width = 42;
			Projectile.height = 42;
			Projectile.timeLeft = 1200;
			Projectile.penetrate = 5;
			Projectile.tileCollide = true;
			Projectile.alpha = 0;
			Projectile.scale = 0.9f;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 10;
			height = 10;
			fallThrough = true;
			return true;
		}
		public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.direction == -1 ? MathHelper.ToRadians(135): MathHelper.ToRadians(45));
            int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 500, Projectile, true);
			if(target != -1)
			{
				NPC npc = Main.npc[target];
				Vector2 toNPC = npc.Center - Projectile.Center;
				Projectile.velocity += 0.4f * toNPC.SNormalize();
			}
			Vector2 away = Projectile.velocity.SNormalize();
			for(float j = 0; j < 1; j += 0.5f)
            {
                for (int i = -1; i <= 1; i += 2)
                {
                    Dust dust = PixelDust.Spawn(Projectile.Center - away.RotatedBy(MathHelper.PiOver2 * i) * 8 + j * Projectile.velocity, 0, 0, Vector2.Zero, new Color(150, 150, 170, 0), -11);
					dust.scale *= 0.9f;
                    dust.velocity = Main.rand.NextVector2Square(-0.2f, .2f) + away.RotatedBy(MathHelper.PiOver2 * i);
                }
            }
		}
		public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Dig, Projectile.Center, 0.9f, 0.2f);
			Vector2 away = Projectile.oldVelocity.SNormalize();
			for(int i = 0; i < 24; i++)
            {
                Dust d = PixelDust.Spawn(Projectile.Center + away.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-16, 16) - new Vector2(4), 8, 8, Main.rand.NextVector2Square(-0.4f, .4f), new Color(75, 90, 135, 0) * 0.5f, -5);
				d.velocity += Projectile.oldVelocity.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-.15f, .15f);
				d.scale += 0.5f;

                d = PixelDust.Spawn(Projectile.Center + away * Main.rand.NextFloat(-18, 18) - new Vector2(4), 8, 8, Main.rand.NextVector2Square(-0.4f, .4f), new Color(197, 196, 171, 0) * 0.5f, -5);
				d.velocity += Projectile.oldVelocity * Main.rand.NextFloat(.1f, .5f);
				d.noGravity = true;
				d.scale += 0.5f;
            }
        }
	}
}
		