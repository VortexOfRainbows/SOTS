using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld;
using SOTS.Items.Otherworld.FromChests;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{    
    public class MacaroniBeam : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crescent Beam");
		}
		public override void SetDefaults()
        {
			projectile.width = 10;
			projectile.height = 40;
			projectile.penetrate = 1;
			projectile.friendly = true;
			projectile.timeLeft = 900;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.hostile = false;
			projectile.netImportant = true;
			projectile.alpha = 255;
			projectile.extraUpdates = 899;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 8;
			height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
		Vector2 ogPos = new Vector2(0, 0);
		bool runOnce = true;
		int counter = 0;
		public override void AI()
		{
			Player player  = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (player.dead)
			{
				projectile.Kill();
			}
			if (runOnce)
			{
				ogPos = projectile.Center;
				for (int i = 0; i < 12; i++)
				{
					var num372 = Dust.NewDust(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
					Dust dust2 = Main.dust[num372];
					dust2.velocity += projectile.velocity * 0.5f;
					dust2.noGravity = true;
					dust2.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
					dust2.noGravity = true;
					dust2.fadeIn = 0.2f;
					dust2.scale *= 1.4f;
				}
				runOnce = false;
				projectile.netUpdate = true;
				return;
			}
			counter++;
			Vector2 rotational = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(counter * 4));
			var num371 = Dust.NewDust(projectile.Center - new Vector2(5), 0, 0, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
			Dust dust = Main.dust[num371];
			dust.position += new Vector2(0, rotational.X).RotatedBy(projectile.velocity.ToRotation());
			dust.velocity *= 0.05f;
			dust.noGravity = true;
			dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			dust.scale *= 1.0f;
			if(counter > 5)
            {
				projectile.tileCollide = true;
            }
		}
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				var num371 = Dust.NewDust(projectile.Center - new Vector2(5) - new Vector2(10, 10), 24, 24, mod.DustType("CopyDust4"), 0, 0, 100, default, 1.6f);
				Dust dust = Main.dust[num371];
				dust.velocity += projectile.velocity * 0.3f;
				dust.noGravity = true;
				dust.color = Color.Lerp(new Color(255, 240, 50, 100), new Color(235, 240, 50, 100), new Vector2(-0.5f, 0).RotatedBy(MathHelper.ToRadians(counter * 3)).X + 0.5f);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
			}
		}
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			int heal = 1;
			if (Main.rand.Next(2) == 0) heal = 2;

			if (Main.rand.Next(6) == 0) heal = 3;

			if (player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(ogPos.X, ogPos.Y, 0, 0, mod.ProjectileType("HealProj"), 0, 0, player.whoAmI, heal, 8);
			}
			base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
		