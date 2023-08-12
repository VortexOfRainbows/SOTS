using Microsoft.Xna.Framework;
using SOTS.Projectiles.Base;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class MacaroniBeam : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Crescent Beam");
		}
		public override void SetDefaults()
        {
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.penetrate = 1;
			Projectile.friendly = true;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.hostile = false;
			Projectile.netImportant = true;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 899;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
		Vector2 ogPos = new Vector2(0, 0);
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
				ogPos = Projectile.Center;
				for (int i = 0; i < 12; i++)
				{
					var num372 = Dust.NewDust(Projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
					Dust dust2 = Main.dust[num372];
					dust2.velocity += Projectile.velocity * 0.5f;
					dust2.noGravity = true;
					dust2.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 1.4f;
				}
				runOnce = false;
				Projectile.netUpdate = true;
				return;
			}
			counter++;
			Vector2 rotational = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(counter * 4));
			var num371 = Dust.NewDust(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>(), 0, 0, 100, default, 1.6f);
			Dust dust = Main.dust[num371];
			dust.position += new Vector2(0, rotational.X).RotatedBy(Projectile.velocity.ToRotation());
			dust.velocity *= 0.05f;
			dust.noGravity = true;
			dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			dust.scale *= 1.0f;
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
				dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
			}
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[Projectile.owner];
			int heal = 2;
			if (Main.rand.NextBool(6))
				heal = 3;
			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), ogPos.X, ogPos.Y, 0, 0, ModContent.ProjectileType<HealProj>(), 0, 0, player.whoAmI, heal, 8);
			}
			base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
		