using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Nature
{    
    public class NatureBeat : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nature Blast");
		}
        public override void SetDefaults()
        {
			Projectile.height = 40;
			Projectile.width = 40;
            Main.projFrames[Projectile.type] = 4;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.timeLeft = 19;
			Projectile.tileCollide = false;
			Projectile.alpha = 0;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			if(Main.rand.NextBool(3))
				VoidPlayer.VoidBurn(Mod, target, 3, 90);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 120, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 4; k++)
			{
				float x = Main.rand.Next(-10, 11) * 0.15f;
				float y = Main.rand.Next(-10, 11) * 0.15f;
				Main.spriteBatch.Draw(texture,
				new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y),
				new Rectangle(0, 40 * Projectile.frame, 40, 40), color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override void AI()
        {
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 1.0f / 255f, (255 - Projectile.alpha) * 2.5f / 255f, (255 - Projectile.alpha) * 1.0f / 255f);
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 5)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 4;
            }
        }
	}
}
		