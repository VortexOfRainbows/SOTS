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
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bluefire Crush");
		}
        public override void SetDefaults()
        {
			projectile.height = 70;
			projectile.width = 70;
            Main.projFrames[projectile.type] = 5;
			projectile.penetrate = -1;
			projectile.friendly = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.hide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
		}
		List<FireParticle> particleList = new List<FireParticle>();
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
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/Celestial/SubspaceLingeringFlame");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int i = 0; i < particleList.Count; i++)
			{
				Color color = blue;
				Vector2 drawPos = particleList[i].position - Main.screenPosition;
				color = projectile.GetAlpha(color) * (0.35f + 0.65f * particleList[i].scale);
				for (int j = 0; j < 2; j++)
				{
					Main.spriteBatch.Draw(texture, drawPos + Main.rand.NextVector2Circular(2, 2), null, color, particleList[i].rotation, drawOrigin, particleList[i].scale * 1.15f, SpriteEffects.None, 0f);
				}
			}
			return false;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox) 
		{
			int width = 160;
			hitbox = new Rectangle((int)(projectile.Center.X - width / 2), (int)(projectile.Center.Y - width / 2), width, width);
		}
		int counter = 0;
		bool runOnce = true;
		public override void AI()
		{
			if(runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 62, 0.65f, -0.15f);
				runOnce = false;
			}
			counter++;
			if (counter > 20)
				projectile.friendly = false;
			else
			{
				int baseRate = 70;
				if (SOTS.Config.lowFidelityMode)
					baseRate = 110;
				float sphereRadius = 80;
				float mult = counter / 20f;
				if (mult > 1)
					mult = 1;
				float radius = counter / 14f * sphereRadius;
				if (radius > sphereRadius)
					radius = sphereRadius;
				Vector2 rotational = new Vector2(0, -1.0f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-30f, 30f)));
				rotational.X *= 0.25f;
				rotational.Y *= 0.75f;
				rotational = rotational.SafeNormalize(Vector2.Zero) * 3f;
				particleList.Add(new FireParticle(projectile.Center - rotational * 2.5f, rotational, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.9f, 1.3f)));
				for (int i = 0; i < 360; i++)
				{
					Vector2 circular = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(i));
					rotational = new Vector2(0, -1.5f).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(360f)));
					if (Main.rand.NextBool(baseRate - counter))
					{
						int i2 = (int)(circular.X + projectile.Center.X) / 16;
						int j2 = (int)(circular.Y + projectile.Center.Y) / 16;
						if (!(!WorldGen.InWorld(i2, j2, 20) || Main.tile[i2, j2].active() && Main.tileSolidTop[Main.tile[i2, j2].type] == false && Main.tileSolid[Main.tile[i2, j2].type] == true))
							particleList.Add(new FireParticle(projectile.Center + circular - rotational * 2, rotational + circular * 0.03f, Main.rand.NextFloat(-3f, 3f), Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(0.8f, 0.9f)));
					}
					if(Main.rand.NextBool(baseRate - counter))
                    {
						Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 4, 4, ModContent.DustType<CopyDust4>());
						dust.noGravity = true;
						dust.velocity *= 0.6f;
						dust.velocity += circular * 0.15f;
						dust.scale *= 1.4f + 0.4f * mult;
						dust.fadeIn = 0.1f;
						dust.color = blue;
					}
				}
			}
			cataloguePos();
        }
	}
}