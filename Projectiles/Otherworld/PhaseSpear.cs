using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Otherworld
{
	public class PhaseSpear : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phase Spear");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 7;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			projectile.width = 18;
			projectile.height = 36;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.timeLeft = 2160;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.alpha = 255;
			projectile.extraUpdates = 2;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Otherworld/OtherworldlyBolt");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if(!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, projectile.scale * (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
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
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			fallThrough = false;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 14, 0.6f);
			if (Main.netMode != 1)
			{
				for (int i = 0; i < 4 + (Main.expertMode ? 1 : 0); i++)
				{
					Vector2 circular = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.rand.Next(360)));
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, circular.X, circular.Y, ModContent.ProjectileType<ThunderColumn>(), projectile.damage, 0, Main.myPlayer);
				}
			}
			for (int i = 0; i < 40; i++)
			{
				Vector2 circularLocation = new Vector2(10, 0);
				resetVector2(ref circularLocation, i);
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 2f;
				Main.dust[dust].scale *= 6f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 4f;
				Main.dust[dust].scale *= 5;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 6.5f;
				Main.dust[dust].scale *= 4f;
				Main.dust[dust].noGravity = true;

				resetVector2(ref circularLocation, i);
				dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 3), 0, 0, 242);
				Main.dust[dust].velocity = circularLocation;
				Main.dust[dust].velocity *= 10f;
				Main.dust[dust].scale *= 3f;
				Main.dust[dust].noGravity = true;
			}
		}
	}
}