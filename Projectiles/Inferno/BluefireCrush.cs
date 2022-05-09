using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Projectiles.Celestial;
using SOTS.Dusts;

namespace SOTS.Projectiles.Inferno
{    
    public class BluefireCrush : ModProjectile 
    {
		public Color blue = new Color(51, 95, 179, 0);
		public Color orange = new Color(255, 130, 8, 0);
		public bool useBoth = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bluefire Crush");
		}
        public override void SetDefaults()
        {
			Projectile.height = 70;
			Projectile.width = 70;
            Main.projFrames[Projectile.type] = 5;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.timeLeft = 140;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.hide = false;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 30;
		}
		List<FireParticle> particleList = new List<FireParticle>();
		int removedCounter = 0;
		public void cataloguePos()
		{
			for (int i = 0; i < particleList.Count; i++)
			{
				FireParticle particle = particleList[i];
				particle.Update();
				if (!particle.active)
				{
					particleList.RemoveAt(i);
					i--;
					removedCounter++;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = useBoth ? (((removedCounter + i) % 2) == 0 ? blue : orange) : toUseColor;
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = Projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(2, 2), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
			return false;
        }
		public Color randColor()
        {
			return Main.rand.NextBool(2) ? blue : orange;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			int width = 160;
			hitbox = new Rectangle((int)(Projectile.Center.X - width / 2), (int)(Projectile.Center.Y - width / 2), width, width);
		}
		int counter = 0;
		bool runOnce = true;
		public Color toUseColor = Color.White;
		public override void AI()
		{
			if(runOnce)
			{
				if (Projectile.ai[0] == 2)
				{
					useBoth = true;
				}
				else if (Projectile.ai[0] == 1)
				{
					toUseColor = orange;
				}
				else
					toUseColor = blue;
				runOnce = false;
			}
			counter++;
			if (counter > 50)
				Projectile.friendly = false;
			else if(counter >= 30)
			{
				if(counter == 30)
					SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 62, 0.65f, -0.15f);
				int currentCounter = counter - 30;
				Projectile.friendly = true;
				int baseRate = 90;
				if (SOTS.Config.lowFidelityMode)
					baseRate = 140;
				if (useBoth)
					baseRate -= 20;
				float sphereRadius = 80;
				float mult = currentCounter / 20f;
				if (mult > 1)
					mult = 1;
				float radius = currentCounter / 14f * sphereRadius;
				if (radius > sphereRadius)
					radius = sphereRadius;
				Vector2 rotational = new Vector2(0, -1.0f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
				rotational.X *= 0.25f;
				rotational.Y *= 0.75f;
				rotational = rotational.SafeNormalize(Vector2.Zero) * 3f;
				particleList.Add(new FireParticle(Projectile.Center - rotational * 1.5f, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(1.1f, 1.4f) + (useBoth ? 0.1f : 0)));
				for (int i = 0; i < 360; i++)
				{
					Vector2 circular = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(i));
					rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
					if (Main.rand.NextBool(baseRate - currentCounter))
					{
						int i2 = (int)(circular.X + Projectile.Center.X) / 16;
						int j2 = (int)(circular.Y + Projectile.Center.Y) / 16;
						if (!SOTSWorldgenHelper.TrueTileSolid(i2, j2))
							particleList.Add(new FireParticle(Projectile.Center + circular - rotational * 2, rotational + circular * 0.03f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 0.9f) + (useBoth ? 0.1f : 0)));
					}
					if(Main.rand.NextBool(baseRate - currentCounter))
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.6f;
						dust.velocity += circular * 0.15f;
						dust.scale *= 1.4f + 0.4f * mult + (useBoth ? 0.2f : 0);
						dust.fadeIn = 0.1f;
						dust.color = useBoth ? randColor() : toUseColor;
					}
				}
			}
			else
			{
				float mult = counter / 30f;
				Vector2 rotational = new Vector2(0, -1.0f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
				rotational.X *= 0.25f;
				rotational.Y *= 0.75f;
				rotational = rotational.SafeNormalize(Vector2.Zero) * 3f;
				particleList.Add(new FireParticle(Projectile.Center - rotational * 1.5f, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), mult * (Main.rand.NextFloat(1.1f, 1.4f) + (useBoth ? 0.1f : 0))));
			}
			cataloguePos();
        }
	}
}