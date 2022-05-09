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
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.hostile = false;
			Projectile.friendly = false;
			Projectile.ranged = true;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.extraUpdates = 3;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.ChaosPink;
			color.A = 0;
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * k + Projectile.velocity.ToRotation());
				Main.spriteBatch.Draw(texture, offset + Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, color * 0.7f, Projectile.rotation, drawOrigin, Projectile.scale * new Vector2(0.9f, 1.1f), SpriteEffects.None, 0f);
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
			if(deathTime == -1 && Projectile.owner == Main.myPlayer)
            {
				deathTime = Main.rand.Next(80, 105);
				Projectile.netUpdate = true;
			}
			if (Projectile.timeLeft < deathTime)
			{
				Projectile.Kill();
				return;
            }
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.alpha += 2;
			Projectile.velocity *= 0.99f - Projectile.ai[0] * 0.2f;
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(Projectile.ai[1])) * (Projectile.ai[0]);
			if (Projectile.ai[1] > 0)
			{
				Projectile.ai[1] += turnFactor;
			}
			else
            {
				Projectile.ai[1] -= turnFactor;
            }
			Dust dust2 = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0, 100);
			dust2.velocity *= 0;
			dust2.velocity -= Projectile.velocity * 0.1f;
			dust2.noGravity = true;
			dust2.color = VoidPlayer.ChaosPink;
			dust2.noGravity = true;
			dust2.fadeIn = 0.2f;
			dust2.scale = 1.8f;
		}
        public override void Kill(int timeLeft)
        {
            if(Projectile.owner == Main.myPlayer)
				Projectile.NewProjectile(Projectile.Center, Projectile.velocity, ModContent.ProjectileType<ChaosBloomExplosion>(), Projectile.damage, Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(360), Main.rand.NextFloat(360));
		}
    }
}