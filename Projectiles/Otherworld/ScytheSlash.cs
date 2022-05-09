using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Items.Otherworld;
using System;
using System.Runtime.InteropServices;

namespace SOTS.Projectiles.Otherworld
{    
    public class ScytheSlash : ModProjectile 
    {
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scythe Slash");
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			var x2 = ((float)((double)Projectile.localAI[0] * 6.28318548202515 / 30.0)).ToRotationVector2().X;
			var color1 = new Color(20, 40, 250, 20);
			color1 *= (float)(0.75 + 0.25 * (double)x2);
			for (var index2 = 0; index2 < 8; ++index2)
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY) + Projectile.rotation.ToRotationVector2().RotatedBy(0.785398185253143 * index2) * 4f, null, color1 * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);

			for (var index2 = 0; index2 < 2; ++index2)
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0.0f, Projectile.gfxOffY) + Projectile.velocity.SafeNormalize(Vector2.Zero) * 2, null, new Color(255, 255, 255, 127) * ((255f - Projectile.alpha)/255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override void SetDefaults()
        {
			Projectile.friendly = true;
			Projectile.melee = true;
			Projectile.penetrate = 2;
			Projectile.alpha = 0;
			Projectile.width = 80;
			Projectile.height = 80;
			Projectile.timeLeft = 36;
			Projectile.tileCollide = false;
		}
		bool runOnce = true;
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			width += 4;
			height += 4;
			hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - height / 2), (int)width, (int)height);
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreAI()
        {
			if(runOnce)
            {
				runOnce = false;
				Projectile.scale = 0.1f;
            }
			Projectile.position += Projectile.velocity * Projectile.scale;
            return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			for(int i = 0; i < 50; i++)
            {
				int num2 = Dust.NewDust(new Vector2(Projectile.Center.X - 40, Projectile.Center.Y - 40), 82, 82, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(20, 40, 250, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity = Projectile.velocity * 0.6f;
			}
			for (int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.Center.X - 30, Projectile.Center.Y - 30), 62, 62, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 255, 255, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity = Projectile.velocity * 0.5f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[Projectile.owner];
            target.immune[Projectile.owner] = 10;
			BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
			if (crit && Main.myPlayer == Projectile.owner)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulofRetaliation"), damage + modPlayer.soulDamage, 1f, player.whoAmI);
			}
		}
		public override void AI()
		{
			if(Projectile.scale < 1f)
            {
				Projectile.scale += 0.04f;
            }
			Projectile.localAI[0] += 0.25f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			Projectile.alpha += 2;
			float rad = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Vector2 curve2 = new Vector2(6f,0).RotatedBy(MathHelper.ToRadians(helixRot * 15f));
			helixRot ++;
			
			Vector2 helixPos3 = new Vector2(40f + curve2.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(40f + curve2.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			helixPos3 *= Projectile.scale;
			helixPos4 *= Projectile.scale;
			int num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos3.X - 4, Projectile.Center.Y + helixPos3.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			Dust dust = Main.dust[num2];
			dust.color = new Color(20, 40, 250, 40);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos3 * 0.075f + Projectile.velocity * 0.5f;
			dust.alpha = Projectile.alpha;
			
			num2 = Dust.NewDust(new Vector2(Projectile.Center.X + helixPos4.X - 4, Projectile.Center.Y + helixPos4.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			dust = Main.dust[num2];
			dust.color = new Color(20, 40, 250, 40);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos4 * 0.075f + Projectile.velocity * 0.5f;
			dust.alpha = Projectile.alpha;
		}
	}
}
		
			