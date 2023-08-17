using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Void
{    
    public class VoidHealEffect : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Void Heal Effect");
		}
        public override void SetDefaults()
        {
			Projectile.height = 4;
			Projectile.width = 4;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.timeLeft = 2;
			Projectile.alpha = 255;
			Projectile.tileCollide = false;
			Projectile.hide = true;
		}
		/**
		* The reason I use a projectile to execute server syncing is because I don't know enough to do it a different way
		* It also allows me to generate dust in a special way, so it isn't just a workaround, but also useful in other ways
		*/
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			int voidAmt = (int)Projectile.ai[1];
			bool dot = (int)Projectile.ai[0] == -1;
			if(voidAmt > 0)
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(110, 90, 125, 255), string.Concat((int)Projectile.ai[1]), false, dot);
				for (int i = 0; i < (int)(2 * Math.Sqrt((int)Projectile.ai[1])); i++)
				{
					Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 37);
					dust.noGravity = true;
					dust.scale *= 1.5f;
					dust.velocity *= 1.2f;
				}
			}
			else
			{
				CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), new Color(125, 35, 35, 255), string.Concat((int)Projectile.ai[1]), false, dot);
				for (int i = 0; i < (int)(Math.Sqrt(-2 * (int)Projectile.ai[1])); i++)
				{
					Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.FireflyHit);
					dust.noGravity = true;
					dust.scale *= 2.0f;
					dust.velocity *= 1.3f;
				}
			}
		}
	}
}
		