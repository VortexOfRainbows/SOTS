using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Minions
{
	public abstract class SpiritMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;   
		}

		public override void SetDefaults()
		{
			projectile.width = 34;
			projectile.height = 34;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++) {
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - projectile.alpha) / 255f), 0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public void GoIdle() 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			Vector2 idlePosition = player.Center;
			idlePosition.Y -= 96f;
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.modProjectile as SpiritMinion != null && proj.active && projectile.active && proj.owner == projectile.owner)
				{
					if (proj == projectile)
					{
						found = true;
					}
					if (!found)
						ofTotal++;
					total++;
				}
			}

			Vector2 toPlayer = idlePosition - projectile.Center;
			float distance = toPlayer.Length();
			float speed = 10f;
			projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(projectile.Center.Y - idlePosition.Y, projectile.Center.X - idlePosition.X));
			if (distance < 256)
			{
				Vector2 rotateCenter = new Vector2(64, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
				rotateCenter += idlePosition;
				Vector2 toRotate = rotateCenter - projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 12)
				{
					dist2 = 12;
				}
				projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(projectile.Center.Y - rotateCenter.Y, projectile.Center.X - rotateCenter.X));
			}
		}
	}
}