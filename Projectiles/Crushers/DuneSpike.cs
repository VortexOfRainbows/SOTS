using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{    
    public class DuneSpike : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Dune Spike");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2; //also saves rotation and sprite direction  
		}
        public override void SetDefaults()
        {
			Projectile.tileCollide = true;
			Projectile.width = 18;
			Projectile.height = 12;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
			Projectile.penetrate = 1;
			Projectile.alpha = 0; 
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
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
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(90, 90, 90, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				float rotation = Projectile.oldRot[k];
				float stretch = 1.0f;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.9f, rotation, drawOrigin, new Vector2(Projectile.scale * scale * 0.9f, stretch), SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void AI()
        {
			Projectile.spriteDirection = 1;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(30);
			if(!Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, DustID.Sandnado);
				if (Main.rand.NextBool(4))
					dust.type = ModContent.DustType<ModSandDust>();
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = 0.6f * dust.scale + 0.7f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 0.4f;
			}
			float currentVelo = Projectile.velocity.Length();
			float minDist = 240;
			int target2 = -1;
			float dX;
			float dY;
			float distance;
			float speed = 0.9f + ((120 - Projectile.timeLeft) / 120f);
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
					Projectile.velocity = new Vector2(currentVelo + 0.15f, 0).RotatedBy(Projectile.velocity.ToRotation());
				}
			}
		}
		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 8; i++)
			{
				int num1 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, DustID.Sandnado);
				Dust dust = Main.dust[num1];
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale = dust.scale * 0.8f + 0.9f;
				dust.alpha = Projectile.alpha;
				dust.velocity *= 1.4f;
			}
		}
	}
}