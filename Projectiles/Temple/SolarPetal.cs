using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using SOTS.Dusts;

namespace SOTS.Projectiles.Temple
{    
    public class SolarPetal : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Solar Petal");
		}
        public override void SetDefaults()
        {
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.width = 60;
			Projectile.height = 60;
			Projectile.alpha = 0;
			Projectile.timeLeft = 1200;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = (int)(100 * Projectile.scale * Projectile.ai[1]);
			hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - width / 2), width, width);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(oldVelocity.X != Projectile.velocity.X)
            {
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (oldVelocity.Y != Projectile.velocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.velocity *= 1.4f;
			if(Projectile.velocity.Length() > 64)
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 64;
			return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			height = 18;
			width = 18;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, (int)(texture.Height * 0.75f));
			Color drawColor;
			float length;
			int amount = 48;
			float scale = Projectile.scale * Projectile.ai[1];
			for (int i = 0; i < amount; i++)
            {
				int direction = i % 3;
				drawColor = new Color(120, 80, 60, 0);
				if (direction == 1)
					drawColor = new Color(200, 40, 30, 0);
				if (direction == 2)
					drawColor = new Color(160, 60, 40, 0);
				float sinusoid = (float)Math.Sin(MathHelper.ToRadians(AICounter * 3 + 120 * direction));
				length = (18 + 18 * sinusoid) * scale;
				float rotation = MathHelper.ToRadians(AICounter * Projectile.direction + i * 360f / amount);
				Vector2 rotational = new Vector2(0, length).RotatedBy(rotation);
				Vector2 drawPos = Projectile.Center - Main.screenPosition;
				Main.spriteBatch.Draw(texture, drawPos + rotational * 0.25f, null, Color.Lerp(drawColor, Color.Red, 0.8f) * 0.6f, rotation, drawOrigin, scale * 0.75f, SpriteEffects.FlipVertically, 0f);
				Main.spriteBatch.Draw(texture, drawPos + rotational * 0.5f, null, Color.Lerp(drawColor, Color.Red, 0.4f) * 0.8f, rotation, drawOrigin, scale * 0.9f, SpriteEffects.FlipVertically, 0f);
				Main.spriteBatch.Draw(texture, drawPos + rotational, null, drawColor, rotation, drawOrigin, scale, SpriteEffects.FlipVertically, 0f);
			}
			texture = ModContent.Request<Texture2D>("SOTS/Effects/Masks/Extra_49").Value;
			Vector2 drawPos2 = Projectile.Center - Main.screenPosition;
			drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			for(int i = 0; i < 4; i++)
            {
				drawColor = new Color(180, 70, 40, 0);
				Main.spriteBatch.Draw(texture, drawPos2, null, drawColor, 0, drawOrigin, scale * 0.3f, SpriteEffects.None, 0f);
			}
			for (int i = 0; i < 2; i++)
			{
				drawColor = new Color(255, 70, 70, 0);
				Main.spriteBatch.Draw(texture, drawPos2, null, drawColor * 0.9f, 0, drawOrigin, scale * 0.6f, SpriteEffects.None, 0f);
			}
			drawColor = new Color(160, 120, 40, 0);
			Main.spriteBatch.Draw(texture, drawPos2, null, drawColor * 0.8f, 0, drawOrigin, scale * 1.1f, SpriteEffects.None, 0f);
			return false;
        }
        Vector2 initialVelo;
		bool runOnce = true;
		public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.65f, -0.3f);
			if(Main.myPlayer == Projectile.owner)
			{
				int projectileType = (int)Projectile.ai[0];
				int baseTotal = 30;
				if (projectileType == ProjectileID.ChlorophyteBullet)
					baseTotal = 18;
				if (projectileType == ModContent.ProjectileType<Planetarium.ChargedCataclysmBullet>())
				{
					baseTotal = 15;
					Projectile.damage *= 2;
				}
				int max = (int)(baseTotal * Projectile.ai[1]);
				for(int i = 0; i < max; i++)
				{
					Vector2 direction = initialVelo.RotatedBy(MathHelper.ToRadians(360f / max * i)) * (0.36f + 0.24f * Projectile.ai[1]);
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, direction, projectileType, Projectile.damage, Projectile.knockBack, Main.myPlayer);
				}
			}
			for (float i = 40 * Projectile.ai[1]; i > 0; i--)
			{
				Vector2 circular = new Vector2(48 * Projectile.ai[1], 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360)));
				int dust2 = Dust.NewDust(Projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = getDustColor;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.4f;
				dust.velocity = dust.velocity * 0.2f - circular * 0.2f * Main.rand.NextFloat(1.5f);

				dust2 = Dust.NewDust(Projectile.Center - new Vector2(12, 12), 16, 16, ModContent.DustType<CopyDust4>());
				dust = Main.dust[dust2];
				dust.color = getDustColor;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
				dust.velocity = dust.velocity * 0.4f - circular * 0.1f * Main.rand.NextFloat(1.5f);
			}
		}
		public Color getDustColor
        {
            get
            {
				return Color.Lerp(ColorHelpers.InfernoColorAttempt(0.5f + Main.rand.NextFloat(0.5f)), new Color(190, 80, 60), 0.3f + Main.rand.NextFloat(0.3f));
            }
        }
        public override bool PreAI()
		{
			if (runOnce)
			{
				Projectile.scale = 0.1f;
				initialVelo = Projectile.velocity;
				runOnce = false;
			}
            else if(Projectile.ai[1] >= Main.rand.NextFloat(1))
			{
				int dust2 = Dust.NewDust(Projectile.Center - new Vector2(24, 24), 40, 40, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[dust2];
				dust.color = getDustColor;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2.0f;
				dust.velocity *= 1.5f * Projectile.scale * Projectile.ai[1];
				dust.alpha = 125;
			}
			return true;
        }
		float AICounter = 0;
        public override void AI()
		{
			Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.12f * AICounter / 18f);
			AICounter += 0.8f + 0.2f * Projectile.ai[1];
			if(Projectile.ai[1] != 1)
            {
				int direction = (Projectile.ai[1] < 0.45f || Projectile.ai[1] > 0.55f) ? -1 : 1;
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.ToRadians(2.4f * direction));
			}
            if (AICounter > 40)
            {
                if (AICounter < 63)
				{
					Projectile.scale = MathHelper.Lerp(Projectile.scale, 1.9f, 0.14f * Projectile.ai[1]);
				}
				else
                {
					Projectile.scale *= 0.9f;
					Projectile.scale -= 0.11f;
					if (Projectile.scale <= 0.1f)
						Projectile.Kill();
                }
            }
            else
			{
				Projectile.scale = MathHelper.Lerp(Projectile.scale, 0.9f, 0.08f);
			}
		}
	}
}
		
			