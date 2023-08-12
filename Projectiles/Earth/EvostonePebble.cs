using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Projectiles.Blades;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class EvostonePebble : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Evostone Pebble");
		}
        public override void SetDefaults()
        {
			Projectile.tileCollide = false;
			Projectile.width = 14;
			Projectile.height = 14;
            Projectile.DamageType = ModContent.GetInstance<Void.VoidGeneric>(); //generic because multiple classes use this projectile
			Projectile.penetrate = 1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 140;
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
			Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/EvostonePebbleGlow").Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Color color = new Color(100, 100, 100, 0);
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			for (int j = 1; j <= 2; j++)
			{
				float scaleMult = 1.25f - j * 0.25f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * (1f / j) * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale * scaleMult, SpriteEffects.None, 0f);
			}
			return true;
		}
		int counter = 0;
		public Color getColor => Color.Lerp(new Color(166, 221, 145), new Color(46, 63, 77), Main.rand.NextFloat(1));
		public override void AI()
        {
			Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.ToRotation();
			int num1 = Dust.NewDust(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
			Dust dust = Main.dust[num1];
			Color color2 = getColor;
			dust.color = color2;
			dust.noGravity = true;
			dust.fadeIn = 0.1f;
			dust.scale = 0.1f * dust.scale + 0.85f;
			dust.alpha = Projectile.alpha;
			dust.velocity *= 0.1f;
			if (Projectile.timeLeft > 132)
			{
				float currentVelo = Projectile.velocity.Length();
				float minDist = 240;
				int target2 = -1;
				float dX;
				float dY;
				float distance;
				float speed = 2.1f;
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
							Projectile.velocity += new Vector2(dX * speed, dY * speed) + new Vector2(0, -0.5f);
							Projectile.velocity = new Vector2(currentVelo + 0.1f, 0).RotatedBy(Projectile.velocity.ToRotation());
						}
					}
				}
			}
			else
            {
				Projectile.tileCollide = true;
            }
			Projectile.velocity.Y += 0.1f;
		}
		public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 5; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num1];
				Color color2 = getColor;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.5f + 0.8f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.2f;
			}
			for(int i = 0; i < 9; i++)
            {
				Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Obsidian);
				dust.color = Color.LightGray;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.5f + 0.6f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.4f;
			}
			if(timeLeft < 134)
				SOTSUtils.PlaySound(SoundID.Tink, Projectile.Center, 0.8f, -0.2f, 0.1f);
		}
	}
}