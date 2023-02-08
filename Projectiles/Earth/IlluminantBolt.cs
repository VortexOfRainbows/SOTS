using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class IlluminantBolt : ModProjectile 
    {	
		int helixRot = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Bolt");
		}
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
			Projectile.width = 12;
			Projectile.height = 12;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = 1;
			Projectile.alpha = 10; 
			Projectile.friendly = true;
			Projectile.timeLeft = 180;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox.X = (int)Projectile.Center.X;
			hitbox.Y = (int)Projectile.Center.Y;
			hitbox.Width = 24;
			hitbox.Height = 24;
			hitbox.X -= 12;
			hitbox.Y -= 12;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			Projectile.netUpdate = true;
			Projectile.velocity *= 1.1f;
			if (Projectile.velocity.Length() > 6)
				Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 6;
			return false;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = VoidPlayer.VibrantColorAttempt(Projectile.ai[0], true);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			color = Projectile.GetAlpha(color) * 0.825f;
			for (int j = 0; j < 5; j++)
			{
				float scaleMult = 2f - j * 0.3f;
				float x = Main.rand.Next(-10, 11) * 0.1f / scaleMult;
				float y = Main.rand.Next(-10, 11) * 0.1f / scaleMult;
				Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y) + helixPosition(), null, color * (1f / scaleMult), Projectile.rotation, drawOrigin, Projectile.scale * scaleMult, SpriteEffects.None, 0f);
			}
			return false;
		}
		public Vector2 helixPosition()
		{
			float curve = (float)Math.Sin(MathHelper.ToRadians(Projectile.whoAmI * 60 + helixRot * 7)) * 9;
			float radianDir = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			Vector2 helixPos1 = new Vector2(curve, 0).RotatedBy(radianDir + MathHelper.ToRadians(90 + Projectile.ai[0]));
			return helixPos1;
		}
		int counter = 0;
		public override void AI()
        {
			Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			helixRot ++;
			int num1 = Dust.NewDust(Projectile.Center + helixPosition() - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[num1];
			Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.ai[0], true);
			dust.color = color2;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale = 0.1f * dust.scale + 1f;
			dust.alpha = Projectile.alpha;
			dust.velocity *= 0.1f;
			Projectile.alpha++;
			if (Projectile.timeLeft % 10 == 0)
			{
				float currentVelo = Projectile.velocity.Length();
				float minDist = 192;
				int target2 = -1;
				float dX;
				float dY;
				float distance;
				float speed = 2.4f + 1.9f * counter;
				if (Projectile.friendly == true && Projectile.hostile == false && Projectile.timeLeft > 10)
				{
					for (int i = 0; i < Main.npc.Length - 1; i++)
					{
						NPC target = Main.npc[i];
						if (!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
						{
							dX = target.Center.X - Projectile.Center.X;
							dY = target.Center.Y - Projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
							if (distance < minDist && lineOfSight)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}
					if (target2 != -1)
					{
						NPC toHit = Main.npc[target2];
						if (toHit.active == true)
						{
							dX = toHit.Center.X - Projectile.Center.X;
							dY = toHit.Center.Y - Projectile.Center.Y;
							distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
							speed /= distance;
							Projectile.velocity += new Vector2(dX * speed, dY * speed);
							Projectile.velocity = new Vector2(currentVelo + 0.35f, 0).RotatedBy(Projectile.velocity.ToRotation());
							counter++;
						}
					}
				}
			}
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 8; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = VoidPlayer.VibrantColorAttempt(Projectile.ai[0], true);
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.75f + 0.8f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.2f;
			}
		}
	}
}