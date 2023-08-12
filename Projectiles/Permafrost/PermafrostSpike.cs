using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class PermafrostSpike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Permafrost Spike");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = 1;
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = true;
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			VoidPlayer.VoidBurn(Mod, target, 5, 180);
		}
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.65f / 255f);
			Projectile.rotation += 0.04f;
			Projectile.ai[0]--;
			if(Projectile.ai[0] <= 0)
			{
				Projectile.position += Projectile.velocity;
			}
			if(Main.rand.NextBool(72))
			{
				int dust2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, 267);
				Dust dust = Main.dust[dust2];
				dust.color = new Color(65, 136, 164);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			return false;
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
				null, color * (1f - (Projectile.alpha / 255f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void Kill(int timeLeft)
        {
			if(Projectile.owner == Main.myPlayer)
			{
				Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, 0, 0, ModContent.ProjectileType<PermafrostLinger>(), Projectile.damage, 0, Projectile.owner, Projectile.rotation);
			}
		}
	}
}
		
			