using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Laser
{
	public class VibrantRing : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibrant Ring");
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.timeLeft = 60;
			Projectile.tileCollide = false;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 120; k++)
			{
				Vector2 circularPos = new Vector2(Projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(k * 3) + Projectile.rotation);
				Color color = VoidPlayer.VibrantColorAttempt(3 * k);
				Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
				color = Projectile.GetAlpha(color) * 0.1f;
				for (int j = 0; j < 4; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.15f;
					float y = Main.rand.Next(-10, 11) * 0.15f;
					Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.5f / 255f, (255 - Projectile.alpha) * 1.75f / 255f, (255 - Projectile.alpha) * 0.2f / 255f);
			Projectile.rotation += MathHelper.ToRadians(3);
			Projectile.alpha += 4;
			if (Projectile.timeLeft < 12)
			{
				Projectile.alpha += 3;
			}
			if (Projectile.ai[1] == 0)
			{
				Projectile.ai[0]++;
				if(Projectile.ai[0] >= 22)
				{
					Projectile.ai[1] = 1;
				}
			}
			if (Projectile.ai[1] == 1)
			{
				Projectile.ai[0]--;
				if (Projectile.ai[0] <= 16)
				{
					Projectile.ai[1] = 0;
				}
			}
		}
	}
}