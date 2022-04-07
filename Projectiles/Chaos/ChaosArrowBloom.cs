using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class ChaosArrowBloom : ModProjectile
	{
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(deathTime);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			deathTime = reader.ReadInt32();
		}
		float deathTime = -1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chaos Arrow Bloom");
		}
		public override void SetDefaults()
		{
			projectile.width = 48;
			projectile.height = 48;
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.ranged = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.extraUpdates = 3;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.ChaosPink;
			color.A = 0;
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + projectile.velocity.ToRotation());
				Main.spriteBatch.Draw(texture, offset + projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, projectile.rotation, drawOrigin, projectile.scale * new Vector2(0.9f, 1.1f), SpriteEffects.None, 0f);
			}
			return false;
		}
		float turnFactor = 0.05f;
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				runOnce = false;
            }
			if(deathTime == -1 && projectile.owner == Main.myPlayer)
            {
				deathTime = Main.rand.Next(80, 110);
				projectile.netUpdate = true;
			}
			if (projectile.timeLeft < deathTime)
			{
				projectile.Kill();
				return;
            }
			projectile.rotation = projectile.velocity.ToRotation();
			projectile.alpha += 2;
			projectile.velocity *= 0.99f - projectile.ai[0] * 0.2f;
			projectile.velocity += projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(projectile.ai[1])) * (projectile.ai[0]);
			if (projectile.ai[1] > 0)
			{
				projectile.ai[1] += turnFactor;
			}
			else
            {
				projectile.ai[1] -= turnFactor;
            }
			Dust dust2 = Dust.NewDustDirect(projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100);
			dust2.velocity *= 0;
			dust2.velocity -= projectile.velocity * 0.1f;
			dust2.noGravity = true;
			dust2.color = VoidPlayer.ChaosPink;
			dust2.noGravity = true;
			dust2.fadeIn = 0.2f;
			dust2.scale = 1.8f;
		}
        public override void Kill(int timeLeft)
        {
            if(projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<ChaosBloomExplosion>(), projectile.damage, -1, Main.myPlayer, Main.rand.NextFloat(360), Main.rand.NextFloat(360));
		}
    }
}