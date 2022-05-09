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
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
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
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 1f;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 10;
			Projectile.netImportant = true;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++) {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(new Color(100, 100, 100, 0)) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(texture, drawPos, null, color * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 9; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.25f;
				float y = Main.rand.Next(-10, 11) * 0.25f;
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, color * ((255 - Projectile.alpha) / 255f), 0f, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
		}
		int ofTotal2 = 0;
		public void MoveAwayFromOthers(bool andTiles = false, float movespeed = 0.09f, float widthMult = 1.5f)
		{
			float overlapVelocity = movespeed;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile other = Main.projectile[i];
				if (i != Projectile.whoAmI && other.active && other.owner == Projectile.owner && Math.Abs(Projectile.Center.X - other.Center.X) + Math.Abs(Projectile.Center.Y - other.Center.Y) < Projectile.width * widthMult && other.modProjectile as SpiritMinion != null)
				{
					if (Projectile.position.X < other.position.X) Projectile.velocity.X -= overlapVelocity;
					else Projectile.velocity.X += overlapVelocity;

					if (Projectile.position.Y < other.position.Y) Projectile.velocity.Y -= overlapVelocity;
					else Projectile.velocity.Y += overlapVelocity;
				}
			}
			if(andTiles)
			{
				float tileMult = 17.5f;
				int i = (int)Projectile.Center.X / 16;
				int j = (int)Projectile.Center.Y / 16;
				Tile tile = Framing.GetTileSafely(i, j + 1);
				if (!WorldGen.InWorld(i, j + 1, 20) || (tile.active() && !Main.tileSolidTop[tile.TileType] && Main.tileSolid[tile.TileType]))
				{
					Projectile.velocity.Y -= overlapVelocity * tileMult;
				}
			}
		}
		public void GoIdle(float speed = 10f) 
		{
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			bool found = false;
			int ofTotal = 0;
			int total = 0;
			for (int i = 0; i < Main.Projectile.Length; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.modProjectile as SpiritMinion != null && proj.active && Projectile.active && proj.owner == Projectile.owner)
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
				if (ofTotal2 != ofTotal)
					Projectile.netUpdate = true;
				ofTotal2 = ofTotal;
            }

			Vector2 idlePosition = player.Center;
			float extraDist = 5;
			idlePosition.Y -= 96f + total * extraDist;
			Vector2 toPlayer = idlePosition - Projectile.Center;
			float distance = toPlayer.Length();
			Projectile.velocity = new Vector2(-speed, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - idlePosition.Y, Projectile.Center.X - idlePosition.X));
			if (distance < 256)
			{
				Vector2 rotateCenter = new Vector2(64 + total * extraDist, 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal2 * 360f / total)));
				rotateCenter += idlePosition;
				Vector2 toRotate = rotateCenter - Projectile.Center;
				float dist2 = toRotate.Length();
				if (dist2 > 12)
				{
					dist2 = 12;
				}
				Projectile.velocity = new Vector2(-dist2, 0).RotatedBy(Math.Atan2(Projectile.Center.Y - rotateCenter.Y, Projectile.Center.X - rotateCenter.X));
			}
		}
	}
}