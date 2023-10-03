using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Planetarium
{
	public class PhaseSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phase Spear");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 18;
			Projectile.height = 36;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 2160;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Planetarium/OtherworldlyBolt").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if(!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale * (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public void resetVector2(ref Vector2 loc, int i)
		{
			loc = new Vector2(10, 0).RotatedBy(MathHelper.ToRadians(i * 9));
			loc.X += Main.rand.Next(-5, 6);
			loc.Y += Main.rand.Next(-5, 6);
			loc *= 0.1f;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			fallThrough = false;
            return true;
        }
        public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item14, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				for (int i = 0; i < 4 + (Main.expertMode ? 1 : 0); i++)
				{
					Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<ThunderColumn>(), Projectile.damage, 0, Main.myPlayer);
				}
			}
			for (int i = 0; i < 40; i++)
			{
				Vector2 circularLocation = new Vector2(10, 0);
				resetVector2(ref circularLocation, i);
				int dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 6f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 4f;
				Main.dust[dust].scale *= 5;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 6.5f;
				Main.dust[dust].scale *= 4f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale *= 3f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}