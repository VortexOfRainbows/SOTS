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
    public class SolarBullet : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; //also saves rotation and spritedriection  
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = true;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.timeLeft = 3600;
			Projectile.extraUpdates = 1;
			Projectile.penetrate = 2;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 2;
			Projectile.localNPCHitCooldown = 0;
			Projectile.usesLocalNPCImmunity = true;
		}
        public override void AI()
        {
            if (Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				if(Projectile.timeLeft > 30)
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
					Projectile.timeLeft = 30;
				}
            }
			Projectile.rotation = Projectile.velocity.ToRotation();
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			int ignoreDefense = ((target.defense + 1) / 2);
			int baseDamage = (int)(damage * 0.5) - ignoreDefense;
			baseDamage = (int)MathHelper.Clamp(baseDamage, 0, damage);
			baseDamage += (int)(damage * 0.5f);
			damage = baseDamage + ignoreDefense;
			//Main.NewText(baseDamage);
		}
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == -1 ? false : base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.ai[1] > 8)
			{
				Projectile.ai[0] = -1;
				Projectile.netUpdate = true;
				return false;
			}
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.ai[1] += 1f;
			Projectile.netUpdate = true;
			Projectile.velocity *= 1.1f;
			if (Projectile.velocity.Length() > 24)
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 24;
			Projectile.rotation = Projectile.velocity.ToRotation();
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int starting = 0;
			if(Projectile.timeLeft <= 30)
            {
				starting = 30 - Projectile.timeLeft;
			}
			else
				for (int i = 0; i < 4; i++)
				{
					Vector2 offset = new Vector2(2, 0).RotatedBy(MathHelper.PiOver2 * i);
					Main.spriteBatch.Draw(texture, Projectile.Center + offset - Main.screenPosition, null, new Color(100, 90, 80, 0), Projectile.rotation + MathHelper.PiOver2, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
				}
			for (int k = starting; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(120, 80, 30, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				float rotation = Projectile.oldRot[k];
				float stretch = 2.0f;
				if ((k > 1 && rotation != Projectile.oldRot[k - 2]) || (k > 0 && rotation != Projectile.oldRot[k - 1]) || (k < 29 && rotation != Projectile.oldRot[k + 1]) || (k < 28 && rotation != Projectile.oldRot[k + 2]))
					stretch = 0.8f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.8f, rotation + MathHelper.PiOver2, drawOrigin, new Vector2(Projectile.scale * scale * 0.9f, stretch), SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(texture, drawPos, null, Projectile.GetAlpha(new Color(250, 120, 50, 0)) * scale * 0.5f, rotation + MathHelper.PiOver2, drawOrigin, new Vector2(Projectile.scale * scale * 0.75f, stretch), SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
		