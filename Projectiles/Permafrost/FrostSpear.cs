using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class FrostSpear : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Spear");	
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = true;
			Projectile.alpha = 0;
			Projectile.width = 14;
			Projectile.height = 22;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 40;
			Projectile.alpha = 0;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 40;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				float x = Main.rand.NextFloat(-1, 1);
				float y = Main.rand.NextFloat(-1, 1);
				Main.spriteBatch.Draw(texture, new Vector2((float)(Projectile.Center.X - (int)Main.screenPosition.X) + x, (float)(Projectile.Center.Y - (int)Main.screenPosition.Y) + y), null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void AI()
		{
			if(Projectile.timeLeft < 27)
            {
				Projectile.tileCollide = true;
				Projectile.alpha += 8;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.velocity *= 1.04f;
			Projectile.velocity.Y += 0.1f;
			if(!Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
				dust.velocity *= 0.5f;
				dust.velocity -= Projectile.velocity * 0.05f;
				dust.color = new Color(180 - Main.rand.Next(50), 190 - Main.rand.Next(50), 250, 150);
				dust.alpha = 100;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.25f;
			}
		}
		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 50, 0.7f, 0.3f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
				int dust2 = Dust.NewDust(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
				Dust dust = Main.dust[dust2];
				dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = new Color(180 - Main.rand.Next(50), 190 - Main.rand.Next(50), 250, 150);
				dust.noGravity = true;
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
}