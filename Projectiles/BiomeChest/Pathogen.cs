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
			// DisplayName.SetDefault("Pathogen Ball");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.friendly = true;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.timeLeft = 90;
			Projectile.penetrate = 7;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.alpha = 0;
            Main.projFrames[Projectile.type] = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 20;
		}
		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
			if(Main.rand.NextBool(7))
            {
				if (hit.Crit)
					damageDone /= 2;
				target.AddBuff(ModContent.BuffType<Infected>(), damageDone * 60);
            }
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color;
			for (int k = 0; k < 60; k++)
			{
				Vector2 lengthMod = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 10));
				Vector2 circularModifier = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				Vector2 circularLength = new Vector2(1.5f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Vector2 circularPos = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				color = new Color(250, 250, 250, 0);
				float mod = Math.Abs(circularModifier.X);
				if (mod < 0.25f) mod = 0.25f;
				circularPos.Y *= 0.5f + mod;
				circularPos += bonus;
				circularPos = circularPos.RotatedBy(Projectile.velocity.ToRotation());
				Vector2 drawPos = Projectile.Center + circularPos - Main.screenPosition;
				color = Projectile.GetAlpha(color);
				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, 0.33f, SpriteEffects.None, 0f);
			}

			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(100, 100, 100, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = Color.White;
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			hitbox = new Rectangle((int)Projectile.Center.X - 24, (int)Projectile.Center.Y - 24, 48, 48);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void AI()
		{
			if(Projectile.ai[0] == -1)
            {
				Projectile.tileCollide = false;
            }
			Projectile.ai[1]++;
			Projectile.rotation += 0.05f * Projectile.direction;
			Projectile.velocity *= 0.99f;
			Projectile.alpha += 3;		
			float minDist = 160;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.1f;
			if(Projectile.friendly == true && Projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
							if (lineOfSight || !Projectile.tileCollide)
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
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
					   
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
			dust.velocity *= 0.1f;
			dust.velocity -= 1f * Projectile.velocity.SafeNormalize(Vector2.Zero);
			dust.noGravity = true;
			dust.fadeIn = 0.2f;
			bool rand = Main.rand.NextBool(5);
			dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
			dust.scale *= rand ? 1.4f : 2;
			int alpha = Projectile.alpha;
			if (alpha > 150)
				alpha = 150;
			dust.alpha = alpha + 55;
		}
		public override void OnKill(int timeLeft)
		{
			for (int i = 0; i < 9; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.3f;
				dust.velocity += 1 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				bool rand = Main.rand.NextBool(5);
				dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
				dust.scale *= rand ? 1.5f : 2;
				dust.alpha = 100;
			}
			for (int k = 0; k < 60; k++)
			{
				Vector2 lengthMod = new Vector2(1.0f, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 10));
				Vector2 circularModifier = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				Vector2 circularLength = new Vector2(1.5f + lengthMod.X, 0).RotatedBy(MathHelper.ToRadians(k * 60));
				Vector2 circularPos = new Vector2(24, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				Vector2 bonus = new Vector2(circularLength.X, 0).RotatedBy(MathHelper.ToRadians(k * 6f) + Projectile.rotation);
				float mod = Math.Abs(circularModifier.X);
				if (mod < 0.25f) mod = 0.25f;
				circularPos.Y *= 0.5f + mod;
				circularPos += bonus;
				circularPos = circularPos.RotatedBy(Projectile.velocity.ToRotation());
				Vector2 pos = Projectile.Center + circularPos;
				if(!Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(pos - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.velocity *= 0.3f;
					dust.velocity += 1 * Projectile.velocity.SafeNormalize(Vector2.Zero);
					dust.noGravity = true;
					dust.fadeIn = 0.2f;
					bool rand = Main.rand.NextBool(5);
					dust.color = rand ? new Color(200, 100, 200, 0) : new Color(100, 200, 100, 0);
					dust.scale *= rand ? 0.85f : 1.35f;
					dust.alpha = 145;
				}
			}
			base.OnKill(timeLeft);
		}
	}
}
		