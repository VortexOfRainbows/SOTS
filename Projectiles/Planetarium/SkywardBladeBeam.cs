using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{    
    public class SkywardBladeBeam : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Skyward Blade");
		}
		public override void SetDefaults()
        {
			Projectile.width = 10;
			Projectile.height = 40;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 0;
			Projectile.extraUpdates = 899;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
		Vector2 aimTo = new Vector2(0, 0);
		bool runOnce = true;
		int counter = 0;
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.dead)
			{
				Projectile.Kill();
			}
			if (runOnce)
			{
				for (int i = 0; i < 12; i++)
				{
					var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
					Dust dust = Main.dust[num371];
					dust.velocity += Projectile.velocity * 0.5f;
					dust.noGravity = true;
					dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(40, 70, 180, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					dust.alpha = Projectile.alpha;
				}
				runOnce = false;
				Projectile.netUpdate = true;
				return;
			}
			counter++;
			Vector2 rotational = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(counter * 4));
			for (int i = -1; i < 2; i += 2)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5), 4, 4, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.position += new Vector2(0, rotational.X * i).RotatedBy(Projectile.velocity.ToRotation());
				dust.velocity *= 0.1f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(40, 70, 180, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.scale *= 0.8f;
				dust.alpha = Projectile.alpha;
			}
			if(counter > 5)
            {
				Projectile.tileCollide = true;
            }
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				var num371 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += Projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(0, 200, 220, 100), new Color(40, 70, 180, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.alpha = Projectile.alpha;
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.skywardBlades++;
			modPlayer.SendClientChanges(modPlayer);
        }
    }
}
		