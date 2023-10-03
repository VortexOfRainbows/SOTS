using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Blades;
using SOTS.Void;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth.Glowmoth
{    
    public class IlluminantStaffShard : ModProjectile 
    {	
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
			Projectile.width = 14;
			Projectile.height = 14;
            Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = 1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 180;
			Projectile.extraUpdates = 3;
			Projectile.hide = true;
		}
		bool bouncedOnce = false;
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if(!bouncedOnce)
			{
				if (Projectile.velocity.X != oldVelocity.X)
					Projectile.velocity.X = -oldVelocity.X;
				if (Projectile.velocity.Y != oldVelocity.Y)
					Projectile.velocity.Y = -oldVelocity.Y;
				bouncedOnce = true;
				return false;
			}
			return true;
		}
		public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.X = (int)Projectile.Center.X;
			hitbox.Y = (int)Projectile.Center.Y;
			hitbox.Width = 28;
			hitbox.Height = 28;
			hitbox.X -= 14;
			hitbox.Y -= 14;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = TextureAssets.Projectile[Type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = new Color(100, 160, 210, 0);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			for (int j = 1; j <= 5; j++)
			{
				float scaleMult = 1.5f - j * 0.1f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.25f, Projectile.rotation, drawOrigin, new Vector2(2f, 1.1f) * scaleMult * 0.9f, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, new Vector2(2f, 1.1f) * 0.9f, SpriteEffects.None, 0f);
			return false;
		}
		bool RunOnce = true;
		public static Color getColor => Color.Lerp(new Color(166, 221, 145), new Color(84, 148, 234), Main.rand.NextFloat(1));
		public override void AI()
        {
			if(RunOnce)
            {
				RunOnce = false;
				for (int i = 0; i < 16; i++)
				{
					int num1 = Dust.NewDust(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<CopyDust4>());
					Vector2 circular = new Vector2(6, 0).RotatedBy(MathHelper.TwoPi * i / 16f);
					Dust dust2 = Main.dust[num1];
					Color color = getColor;
					dust2.color = color * 0.75f;
					dust2.noGravity = true;
					dust2.fadeIn = 0.1f;
					dust2.scale = dust2.scale * 0.8f + 1.0f;
					dust2.alpha = Projectile.alpha;
					dust2.velocity *= 0.5f;
					dust2.velocity += Projectile.velocity * 0.1f + circular * 0.75f;
				}
			}
			Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.hide = false;
			if(!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<AlphaDrainDust>());
				Color color2 = getColor;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 1.5f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.05f;
			}
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 12; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = getColor * 0.75f;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.6f + 0.8f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.2f;
				dust.velocity += Projectile.velocity * 0.75f;
			}
		}
	}
}