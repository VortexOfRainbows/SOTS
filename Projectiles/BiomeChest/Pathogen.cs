using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using SOTS.Dusts;
using SOTS.Buffs;

namespace SOTS.Projectiles.BiomeChest
{    
    public class Pathogen : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pathogen Ball");
		}
        public override void SetDefaults()
        {
			projectile.aiStyle = 0;
			projectile.melee = true;
			projectile.friendly = true;
			projectile.width = 16;
			projectile.height = 16;
			projectile.timeLeft = 90;
			projectile.penetrate = 7;
			projectile.tileCollide = true;
			projectile.ignoreWater = true;
			projectile.alpha = 0;
            Main.projFrames[projectile.type] = 5;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = projectile.localNPCHitCooldown;
			target.immune[projectile.owner] = 0;
			if(Main.rand.NextBool(7))
            {
				if (crit)
					damage /= 2;
				target.AddBuff(ModContent.BuffType<Infected>(), damage * 60);
            }
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width / 2, projectile.height / 2);
			Color color;
			for (int k = 0; k < 60; k++)
			{
				Vector2 lengthMod = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1] * 10));
				Vector2 circularModifier = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				Vector2 circularLength = new Vector2(1.5f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Vector2 circularPos = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				color = new Color(250, 250, 250, 0);
				float mod = Math.Abs(circularModifier.X);
				if (mod < 0.25f) mod = 0.25f;
				circularPos.Y *= 0.5f + mod;
				circularPos += bonus;
				circularPos = circularPos.RotatedBy(projectile.velocity.ToRotation());
				Vector2 drawPos = projectile.Center + circularPos - Main.screenPosition;
				color = projectile.GetAlpha(color);
				Main.spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, drawOrigin, 0.33f, SpriteEffects.None, 0f);
			}

			Vector2 origin = new Vector2(texture.Width / 2, projectile.height / 2);
			color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 100, 100, 0);
				Main.spriteBatch.Draw(texture, projectile.Center + circular - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color * ((255f - projectile.alpha) / 255f), projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = Color.White;
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle(0, projectile.height * projectile.frame, projectile.width, projectile.height), color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox = new Rectangle((int)projectile.Center.X - 24, (int)projectile.Center.Y - 24, 48, 48);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void AI()
		{
			if(projectile.ai[0] == -1)
            {
				projectile.tileCollide = false;
            }
			projectile.ai[1]++;
			projectile.rotation += 0.05f * projectile.direction;
			projectile.velocity *= 0.99f;
			projectile.alpha += 3;		
			float minDist = 160;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.1f;
			if(projectile.friendly == true && projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - projectile.Center.X;
						dY = target.Center.Y - projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
							if (lineOfSight || !projectile.tileCollide)
							{
								minDist = distance;
								target2 = i;
							}
						}
					}
				}
				if(target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - projectile.Center.X;
						dY = toHit.Center.Y - projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
					   
						projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
			dust.velocity *= 0.1f;
			dust.velocity -= 1f * projectile.velocity.SafeNormalize(Vector2.Zero);
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			bool rand = Main.rand.NextBool(5);
			dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
			dust.scale *= rand ? 1.4f : 2;
			int alpha = projectile.alpha;
			if (alpha > 150)
				alpha = 150;
			dust.alpha = alpha + 55;
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 9; i++)
			{
				Dust dust = Dust.NewDustDirect(projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.3f;
				dust.velocity += 1 * projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				bool rand = Main.rand.NextBool(5);
				dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
				dust.scale *= rand ? 1.5f : 2;
				dust.alpha = 100;
			}
			for (int k = 0; k < 60; k++)
			{
				Vector2 lengthMod = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1] * 10));
				Vector2 circularModifier = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				Vector2 circularLength = new Vector2(1.5f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Vector2 circularPos = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + projectile.rotation);
				float mod = Math.Abs(circularModifier.X);
				if (mod < 0.25f) mod = 0.25f;
				circularPos.Y *= 0.5f + mod;
				circularPos += bonus;
				circularPos = circularPos.RotatedBy(projectile.velocity.ToRotation());
				Vector2 pos = projectile.Center + circularPos;
				if(!Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(pos - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.velocity *= 0.3f;
					dust.velocity += 1 * projectile.velocity.SafeNormalize(Vector2.Zero);
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					bool rand = Main.rand.NextBool(5);
					dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
					dust.scale *= rand ? 0.85f : 1.35f;
					dust.alpha = 145;
				}
			}
			base.Kill(timeLeft);
		}
	}
}
		