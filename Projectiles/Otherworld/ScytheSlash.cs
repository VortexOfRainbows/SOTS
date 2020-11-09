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
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			var x2 = ((float)((double)projectile.localAI[0] * 6.28318548202515 / 30.0)).ToRotationVector2().X;
			var color1 = new Color(20, 40, 250, 20);
			color1 *= (float)(0.75 + 0.25 * (double)x2);
			for (var index2 = 0; index2 < 8; ++index2)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY) + projectile.rotation.ToRotationVector2().RotatedBy(0.785398185253143 * index2) * 4f, null, color1 * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);

			for (var index2 = 0; index2 < 2; ++index2)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition + new Vector2(0.0f, projectile.gfxOffY) + projectile.velocity.SafeNormalize(Vector2.Zero) * 2, null, new Color(255, 255, 255, 127) * ((255f - projectile.alpha)/255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override void SetDefaults()
        {
			projectile.friendly = true;
			projectile.melee = true;
			projectile.penetrate = 2;
			projectile.alpha = 0;
			projectile.width = 80;
			projectile.height = 80;
			projectile.timeLeft = 36;
			projectile.tileCollide = false;
		}
		bool runOnce = true;
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = projectile.width * projectile.scale;
			float height = projectile.width * projectile.scale;
			width += 4;
			height += 4;
			hitbox = new Rectangle((int)(projectile.Center.X - width / 2), (int)(projectile.Center.Y - height / 2), (int)width, (int)height);
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
				projectile.scale = 0.1f;
            }
			projectile.position += projectile.velocity * projectile.scale;
            return base.PreAI();
        }
        public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			for(int i = 0; i < 50; i++)
            {
				int num2 = Dust.NewDust(new Vector2(projectile.Center.X - 40, projectile.Center.Y - 40), 82, 82, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(20, 40, 250, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity = projectile.velocity * 0.6f;
			}
			for (int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(projectile.Center.X - 30, projectile.Center.Y - 30), 62, 62, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(255, 255, 255, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				dust.velocity = projectile.velocity * 0.5f;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 10;
			BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
			if (crit && Main.myPlayer == projectile.owner)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0, 0, mod.ProjectileType("SoulofRetaliation"), damage + modPlayer.soulDamage, 1f, player.whoAmI);
			}
		}
		public override void AI()
		{
			if(projectile.scale < 1f)
            {
				projectile.scale += 0.04f;
            }
			projectile.localAI[0] += 0.25f;
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			projectile.alpha += 2;
			float rad = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			Vector2 curve2 = new Vector2(6f,0).RotatedBy(MathHelper.ToRadians(helixRot * 15f));
			helixRot ++;
			
			Vector2 helixPos3 = new Vector2(40f + curve2.X, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(40f + curve2.X, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			helixPos3 *= projectile.scale;
			helixPos4 *= projectile.scale;
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos3.X - 4, projectile.Center.Y + helixPos3.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			Dust dust = Main.dust[num2];
			dust.color = new Color(20, 40, 250, 40);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos3 * 0.075f + projectile.velocity * 0.5f;
			dust.alpha = projectile.alpha;
			
			num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos4.X - 4, projectile.Center.Y + helixPos4.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			dust = Main.dust[num2];
			dust.color = new Color(20, 40, 250, 40);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos4 * 0.075f + projectile.velocity * 0.5f;
			dust.alpha = projectile.alpha;
		}
	}
}
		
			