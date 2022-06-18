using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class SupernovaLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Supernova Laser");
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 2400;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 40;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 40;
			Projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		Color color;
		float scale = 0.8f;
		public const float length = 7f;
		public override bool PreAI() 
		{
			if(runOnce)
			{
				color = Color.Lerp(new Color(238, 145, 219), new Color(81, 170, 247), Main.rand.NextFloat(1));
				SetPostitions();
				runOnce = false;
				return true;
            }
			return true;
		}
        public override void AI()
        {
			Projectile.alpha = (int)(255 * (1 - Projectile.timeLeft / 40f));
			Projectile.ai[1] ++;
		}
		//bool collided = false;
        public void SetPostitions()
        {
			Vector2 direction = new Vector2(length * scale, 0).RotatedBy(Projectile.velocity.ToRotation());
			int maxDist = 480;
			Vector2 currentPos = Projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				/*
				if (!collided && k > 20)
				{
					int npc = FindClosestEnemy(currentPos, k);
					if (npc != -1)
					{
						NPC target = Main.npc[npc];
						if (target.CanBeChasedBy())
						{
							direction = new Vector2(length * scale, 0).RotatedBy(Redirect(direction.ToRotation(), currentPos, target.Center));
							float width = Projectile.width * scale;
							float height = Projectile.height * scale;
							Rectangle projHitbox = new Rectangle((int)currentPos.X - (int)width / 2, (int)currentPos.Y - (int)height / 2, (int)width, (int)height);
							if (target.Hitbox.Intersects(projHitbox))
							{
								collided = true;
							}
						}
					}
				}
				else if (maxDist > 120 && collided)
					maxDist = 120;*/
				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.4f;
					dust.velocity *= 1.5f;
				}
				maxDist--;
			}
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if (Projectile.owner == Main.myPlayer)
			{
				float rand = Main.rand.Next(120);
				for (int i = 0; i < 3; i++)
                {
					Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(i * 120 + rand));
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), target.Center + circular.SafeNormalize(Vector2.Zero) * 0.5f * target.Size.Length(), circular * 4f, ModContent.ProjectileType<SupernovaScatter>(), (int)(Projectile.damage * 1.4f), Projectile.knockBack, Main.myPlayer, target.whoAmI, rand * 3 + i * 120);
                }
            }
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Projectile.alpha >= 150)
            {
				return false;
            }
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
			for (int i = 0; i < posList.Count; i += 2)
			{
				Vector2 pos = posList[i];
				projHitbox = new Rectangle((int)pos.X - (int)width/2, (int)pos.Y - (int)height/2, (int)width, (int)height);
				if(projHitbox.Intersects(targetHitbox))
                {
					return true;
                }
			}
			return false;
		}
		List<Vector2> posList = new List<Vector2>();
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
			float alpha = 1;
			Vector2 lastPosition = Projectile.Center;
			for(int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if(i > posList.Count - 120)
                {
					alpha = 1 - (i - posList.Count + 120) / 120f;
				}
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				for(int j = 0; j < 3; j++)
				{
					Vector2 sinusoid = new Vector2(0, scale * 18 * (float)Math.Sin(MathHelper.ToRadians(i * 2 + Projectile.ai[1] * 2 + j * 120))).RotatedBy(rotation);
					Color color = this.color * ((255 - Projectile.alpha) / 255f) * alpha * 0.7f;
					color.A = 0;
					Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color, rotation, origin, new Vector2(scale * 3, scale), SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}