using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;
using SOTS.Helpers;

namespace SOTS.Projectiles.Blades
{    
    public class SkipSlash : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.friendly = true;
			Projectile.width = 70;
			Projectile.height = 70;
			Projectile.timeLeft = 54;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 0;
			Projectile.idStaticNPCHitCooldown = 10;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.alpha = 255;
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		bool runOnce = true;
        public override void AI()
        {
			if(runOnce)
            {
				runOnce = false;
				SOTSUtils.PlaySound(SoundID.Item60, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, 0.25f);
			}
            if (Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				if(Projectile.timeLeft > 8)
				{
					for (int i = 0; i < 360; i += 45)
					{
						Vector2 circularLocation = new Vector2(-8, 0).RotatedBy(MathHelper.ToRadians(i));
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.3f;
						dust.velocity += circularLocation * 0.2f + Projectile.velocity * 0.4f;
						dust.scale *= 1.5f;
						dust.fadeIn = 0.1f;
						dust.color = ColorHelper.VoidAnomaly;
						dust.color.A = 0;
					}
					Projectile.timeLeft = 8;
				}
            }
			else
			{
				if(Main.rand.NextBool(5))
				{
					int i = Main.rand.Next(3);
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, ModContent.DustType<CopyDust4>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f);
					dust.noGravity = true;
					dust.scale *= 1.6f - 0.1f * i;
					dust.fadeIn = 0.1f;
					dust.color = ColorHelper.VoidAnomaly;
					if (i == 1)
						dust.color = new Color(203, 70, 224, 0);
					if (i == 2)
						dust.color = new Color(130, 192, 236, 0);
					dust.alpha = 30;
					dust.velocity *= 0.2f + 0.1f * i;
					dust.color.A = 0;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			if (Projectile.timeLeft <= 10)
				Projectile.ai[0] = -1;
			Projectile.ai[1]++;
            if (Projectile.ai[1] > 0)
				Projectile.position += Projectile.velocity * Projectile.ai[1] * Projectile.ai[1] / 1400f;
			Projectile.alpha -= 25;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == -1 ? false : base.CanHitNPC(target);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int starting = 0;
			bool drawMain = false;
			if(Projectile.timeLeft <= 9)
            {
				starting = 9 - Projectile.timeLeft;
			}
			else
			{
				drawMain = true;
			}
			for (int k = starting; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(120, 40, 140, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.6f, Projectile.rotation, new Vector2(texture.Width / 2, texture.Width / 2), Projectile.scale * scale * 1.2f, SpriteEffects.None, 0f);
			}
			if(drawMain)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 255, 255)), Projectile.rotation, drawOrigin, Projectile.scale * 1.2f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
		