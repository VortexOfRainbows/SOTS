using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;

namespace SOTS.Projectiles.Permafrost
{    
    public class PermafrostLinger : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Lingering Permafrost");
		}
        public override void SetDefaults()
        {
			Projectile.width = 42;
			Projectile.height = 40;
			Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.friendly = false;
			Projectile.timeLeft = 180;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			VoidPlayer.VoidBurn(Mod, target, 10, 180);
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(80, 80, 80, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 7; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.3f;
				float y = Main.rand.Next(-10, 11) * 0.3f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 40 * Projectile.frame, 40, 42), color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool PreAI()
		{
			Projectile.rotation = Projectile.ai[0];
			return base.PreAI();
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.65f / 255f);
			Projectile.frameCounter++;
            if (Projectile.frameCounter >= 8 && Projectile.frame != 3)
            {
				Projectile.friendly = false;
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
		}
		public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item50, (int)Projectile.Center.X, (int)Projectile.Center.Y);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(-Main.rand.NextFloat(5, 12), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.15f;
				dust.color = new Color(65, 136, 164);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
}