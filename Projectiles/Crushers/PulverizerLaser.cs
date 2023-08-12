using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Crushers
{
	public class PulverizerLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			// DisplayName.SetDefault("Pulverizer Laser");
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 4800;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 40;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.penetrate = -1;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.localNPCHitCooldown = 15;
			Projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		Color color;
		public float scale = 0.75f;
		public const float length = 7.0f;
		public float WindUp()
        {
			float square = Projectile.timeLeft / 160f + ((Projectile.timeLeft / 40f) * (Projectile.timeLeft / 40f) * (Projectile.timeLeft / 40f) * (Projectile.timeLeft / 40f) * 0.75f); //basically cubes the timeleft as a percent
			float radians = MathHelper.Pi * square;
			return (float)Math.Sin(radians);
        }
		public override bool PreAI() 
		{
			if(runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item94, Projectile.Center, 1, 0.2f);
				color = Color.Lerp(Color.Red, Color.Maroon, Main.rand.NextFloat(1)); //epic color picking moments
				SetPostitions();
				runOnce = false;
				return true;
            }
			return true;
		}
        public override void AI()
        {
			Projectile.alpha = (int)(255 * (1 - Projectile.timeLeft / 40f));
			Projectile.ai[1]++;
		}
		//bool collided = false;
        public void SetPostitions()
        {
			Vector2 direction = new Vector2(length * scale, 0).RotatedBy(Projectile.velocity.ToRotation());
			int maxDist = 240;
			Vector2 currentPos = Projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				if(WorldgenHelpers.SOTSWorldgenHelper.TrueTileSolid((int)currentPos.X / 16, (int)currentPos.Y / 16))
				{
					for(int i = 0; i < 12; i++)
					{
						Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						dust.fadeIn = 0.2f;
						dust.noGravity = true;
						dust.alpha = 100;
						dust.color = color;
						dust.scale *= 1.75f;
						dust.velocity *= 1.7f;
					}
					break;
                }
				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.75f;
					dust.velocity *= 1.7f;
				}
				maxDist--;
			}
		}
		/*public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
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
        }*/
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(Projectile.alpha >= 150)
            {
				return false;
            }
			float width = Projectile.width * scale;
			float height = Projectile.height * scale;
			for (int i = 0; i < posList.Count - 2; i += 2)
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
			float cutOffPoint = posList.Count * 0.8f;
			for(int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if(i > posList.Count - cutOffPoint)
                {
					alpha = 1 - (i - posList.Count + cutOffPoint) / cutOffPoint;
				}
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? Projectile.velocity.ToRotation() : direction.ToRotation();
				//Vector2 sinusoid = new Vector2(0, scale * 18 * (float)Math.Sin(MathHelper.ToRadians(i * 2 + Projectile.ai[1] * 2 + j * 120))).RotatedBy(rotation);
				Color color = this.color * ((255 - Projectile.alpha) / 255f) * alpha;
				color.A = 0;
				Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, color, rotation, origin, new Vector2(scale * 2, scale * WindUp() * Projectile.ai[0]), SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}