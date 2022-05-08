using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Otherworld
{    
    public class CataclysmDisc : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cataclysm Disc");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;    
		}
        public override void SetDefaults()
        {
			projectile.height = 48;
			projectile.width = 48;
			projectile.penetrate = -1;
			projectile.ranged = true;
			projectile.tileCollide = true;
			projectile.timeLeft = 715;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.extraUpdates = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 7;
		}
		int helixRot = 0;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.rotation += 0.125f;
			if(projectile.timeLeft < 700)
			{
				if(projectile.timeLeft > 610)
				{
					projectile.velocity *= 0.91f;
				}
				else
				{
					projectile.velocity = new Vector2(-22.5f, 0).RotatedBy(Math.Atan2(projectile.Center.Y - player.Center.Y, projectile.Center.X - player.Center.X));
					Vector2 toPlayer = player.Center - projectile.Center;
					float distance = toPlayer.Length();
					if(distance < 20)
					{
						projectile.Kill();
					}
				}
				if(projectile.timeLeft == 650)
				{
					if (projectile.owner == Main.myPlayer)
					{
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("CataclysmDamage"), (int)(3f * projectile.damage) + 1, projectile.knockBack, Main.myPlayer);
					}
				}
			}

			projectile.localAI[0] += 10f;
			float rad = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X);
			helixRot++;

			Vector2 helixPos3 = new Vector2(22f, 0).RotatedBy(rad + MathHelper.ToRadians(90));
			Vector2 helixPos4 = new Vector2(22f, 0).RotatedBy(rad - MathHelper.ToRadians(90));
			helixPos3 *= projectile.scale;
			helixPos4 *= projectile.scale;
			int num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos3.X - 4, projectile.Center.Y + helixPos3.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			Dust dust = Main.dust[num2];
			dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(projectile.localAI[0])).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos3 * 0.065f + projectile.velocity * 0.35f;
			dust.alpha = projectile.alpha;

			num2 = Dust.NewDust(new Vector2(projectile.Center.X + helixPos4.X - 4, projectile.Center.Y + helixPos4.Y - 4), 4, 4, mod.DustType("CopyDust4"));
			dust = Main.dust[num2];
			dust.color = Color.Lerp(new Color(220, 60, 10, 40), new Color(255, 250, 130, 40), -new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(projectile.localAI[0])).X + 0.5f);
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale *= 1.7f;
			dust.velocity = helixPos4 * 0.065f + projectile.velocity * 0.35f;
			dust.alpha = projectile.alpha;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = 21;
			if (projectile.timeLeft < 690)
			{
				if(projectile.timeLeft > 610)
				{
					projectile.localNPCImmunity[target.whoAmI] = 6;
				}
			}
			if(Main.rand.NextBool(10))
				target.AddBuff(BuffID.OnFire, 900, false);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{	
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.timeLeft = projectile.timeLeft > 705 ? 705 : projectile.timeLeft;
				SoundEngine.PlaySound(SoundID.Item10, projectile.position);
			projectile.tileCollide = false;
			return false;
		}
	}
}
		