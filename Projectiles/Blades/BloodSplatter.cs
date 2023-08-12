using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Blades
{    
    public class BloodSplatter : ModProjectile 
    {	       
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Blood Splatter");
		}
        public override void SetDefaults()
        {
			Projectile.height = 22;
			Projectile.width = 22;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 2;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.hide = true;
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void Kill(int timeLeft)
        {
			for(int i = 0; i < 21; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, DustID.Blood);
				dust.position += Projectile.velocity * (-10 + i) * 3;
				dust.velocity *= 0.8f;
				dust.velocity += Projectile.velocity * Main.rand.NextFloat(3, 6);
				dust.scale *= 0.5f;
				dust.scale += 1.1f;
				dust.noGravity = true;
			}
			for (int i = 0; i < 11; i++)
			{
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, DustID.LifeDrain);
				dust.position += Projectile.velocity * (-5f + i) * 6;
				dust.velocity *= 0.4f;
				dust.velocity += Projectile.velocity * Main.rand.NextFloat(1.5f, 4.5f);
				dust.scale *= 0.7f;
				dust.scale += 1.2f;
				dust.noGravity = true;
			}
		}
	}
}
		