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
using SOTS.Buffs;

namespace SOTS.Projectiles.Celestial
{    
    public class BoxLaser : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Box Blast");
		}
		public override void SetDefaults()
        {
			projectile.width = 10;
			projectile.height = 10;
			projectile.friendly = false;
			projectile.timeLeft = 360;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = true;
			projectile.alpha = 0;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 0;
		}
		float[] rotations = new float[3] { 1.56f, 0, 0.78f };
		float[] compressions = new float[3] { 0.25f, 0.5f, 0.75f };
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			Texture2D texture = Main.projectileTexture[projectile.type];
			for(int k = 0; k < 3; k++)
            {
				float mult = 1f - 0.25f * k;
				for (int j = 0; j < 4; j++)
					for (int i = -48; i < 48; i += 3)
					{
						Color color = new Color(255, 69, 0, 0);
						Vector2 center = projectile.Center;
						Vector2 rotation = new Vector2(i, 48).RotatedBy(MathHelper.ToRadians(90 * j));
						rotation *= mult;
						rotation.Y *= compressions[k];
						rotation = rotation.RotatedBy(rotations[k] + projectile.rotation);
						Main.spriteBatch.Draw(texture, center - Main.screenPosition + rotation, null, color * 0.6f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 0.66f, SpriteEffects.None, 0f);
					}
			}
			return false;
		}
        public override void AI()
        {
			RingStuff();
            base.AI();
        }
		int charging = 0;
		float counter = 0;
		float[] nextRotations = new float[3];
		float[] nextCompressions = new float[3];
		float[] prevRotations = new float[3];
		float[] prevCompressions = new float[3];
		public void RingStuff()
		{
			if (counter == 0 || charging == -1)
			{
				counter = 0;
				charging = 0;
				for (int i = 0; i < 3; i++)
				{
					nextRotations[i] = Main.rand.NextFloat(-1 * (float)Math.PI, (float)Math.PI);
					nextCompressions[i] = Main.rand.NextFloat(0, 1);
					prevRotations[i] = rotations[i];
					prevCompressions[i] = compressions[i];
				}
			}
			if (charging == 1)
			{
				for (int i = 0; i < 3; i++)
				{
					prevRotations[i] = rotations[i];
					prevCompressions[i] = compressions[i];
					nextCompressions[i] = 0.5f;
					nextRotations[i] = 0;
				}
				charging = 2;
				counter = 0;
			}
			if (counter < 180)
				counter += 10;
			float scale = 0.5f + new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(counter)).X;
			if (counter >= 180)
			{
				if (charging != 2)
					counter = 0;
				else
				{
					counter = 180;
				}
			}
			for (int i = 0; i < 3; i++)
			{
				rotations[i] = lerpMath(prevRotations[i], nextRotations[i], scale);
				compressions[i] = lerpMath(prevCompressions[i], nextCompressions[i], scale);
			}
		}
		private float lerpMath(float point, float point2, float scale)
		{
			return point * scale + point2 * (1f - scale);
		}
		public override bool ShouldUpdatePosition()
        {
			return false;
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(ModContent.BuffType<AbyssalInferno>(), 60, false);
		}
	}
}
		