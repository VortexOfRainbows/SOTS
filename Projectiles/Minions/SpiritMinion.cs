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
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(ofTotal2);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			ofTotal2 = reader.ReadInt32();
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
				Color color = projectile.GetAlpha(new Color(100, 100, 100, 0)) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
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
		int ofTotal2 = 0;
		int updateNetCounter = 0;
		public void MoveAwayFromOthers(bool andTiles = false, float movespeed = 0.09f, float widthMult = 1.5f)
		{
			float overlapVelocity = movespeed;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.Center.X - other.Center.X) + Math.Abs(projectile.Center.Y - other.Center.Y) < projectile.width * widthMult && other.modProjectile as SpiritMinion != null)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			if(andTiles)
			{
				float tileMult = 17.5f;
				int i = (int)projectile.Center.X / 16;
				int j = (int)projectile.Center.Y / 16;
				Tile tile = Framing.GetTileSafely(i, j + 1);
				if (!WorldGen.InWorld(i, j + 1, 20) || (tile.active() && !Main.tileSolidTop[tile.type] && Main.tileSolid[tile.type]))
				{
					projectile.velocity.Y -= overlapVelocity * tileMult;
				}
			}
		}
		public void GoIdle(float speed = 10f) 
		{
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
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
			if (Main.myPlayer == player.whoAmI)
            {
				ofTotal2 = ofTotal;
				updateNetCounter++;
				if (updateNetCounter % 60 == 0)
					projectile.netUpdate = true;
            }

			Vector2 idlePosition = player.Center;
			float extraDist = 5;
			idlePosition.Y -= 96f + total * extraDist;
			Vector2 toPlayer = idlePosition - projectile.Center;
			float distance = toPlayer.Length();
			projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(projectile.Center.Y - idlePosition.Y, projectile.Center.X - idlePosition.X));
			if (distance < 256)
			{
				Vector2 rotateCenter = new Vector2(64 + total * extraDist, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal2 * 360f / total)));
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