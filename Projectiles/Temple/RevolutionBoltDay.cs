using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;

namespace SOTS.Projectiles.Temple
{    
    public class RevolutionBoltDay : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 8;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; //also saves rotation and spritedriection  
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
			Projectile.friendly = true;
			Projectile.width = 22;
			Projectile.height = 22;
			Projectile.timeLeft = 3600;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.localNPCHitCooldown = 20;
			Projectile.usesLocalNPCImmunity = true;
		}
        public override void AI()
        {
            if (Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				if(Projectile.timeLeft > 8)
				{
					for (int i = 0; i < 360; i += 45)
					{
						Vector2 circularLocation = new Vector2(-4, 0).RotatedBy(MathHelper.ToRadians(i));
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.3f;
						dust.velocity += circularLocation * 0.1f;
						dust.scale *= 1.5f;
						dust.fadeIn = 0.1f;
						dust.color = new Color(160, 80, 30, 0);
						dust.alpha = 70;
					}
					Projectile.timeLeft = 8;
				}
            }
			Projectile.rotation = Projectile.velocity.ToRotation();
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
			if(Projectile.timeLeft <= 8)
            {
				starting = 8 - Projectile.timeLeft;
			}
			else
			{
				drawMain = true;
			}
			for (int k = starting; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(110, 70, 30, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				float rotation = Projectile.oldRot[k];
				float stretch = 1.5f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.6f, rotation + MathHelper.PiOver2, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(Projectile.scale * scale * 0.9f, stretch), SpriteEffects.None, 0f);
			}
			if(drawMain)
			{
				for (int i = 0; i < 4; i++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * i);
					Main.spriteBatch.Draw(texture, Projectile.Center + offset - Main.screenPosition, null, new Color(150, 130, 100, 0), Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}
		