using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Projectiles.Tide
{    
    public class RippleWave : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple Wave");
		}
        public override void SetDefaults()
        {
			projectile.penetrate = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.timeLeft = 31;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.idStaticNPCHitCooldown = 15;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.alpha = 100;
		}
        public override bool? CanCutTiles()
        {
			return false;
        }
        bool runOnce = true;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			int defense = target.defense;
			int extra = defense / 2;
			if (extra > 4)
				extra = 4;
			damage += extra;
		}
        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
			int defense = target.statDefense;
			int extra = defense / 2;
			if (extra > 4)
				extra = 4;
			damage += extra;
		}
		public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void AI()
		{
			UpdateList();
			length += projectile.velocity.Length();
			projectile.position += projectile.velocity;
			projectile.ai[1]++;
			projectile.alpha += 5;
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 21, 0.4f);
				runOnce = false;
            }
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.rotation += 0.04f;
		}
		float length = 0;
		const float finalDegree = 0f;
		List<Vector2> ParticlePos = new List<Vector2>();
		public void UpdateList()
		{
			Player player = Main.player[projectile.owner];
			if ((int)projectile.ai[0] >= 0)
			{
				ParticlePos = new List<Vector2>();
				Vector2 origin = player.Center;
				float rotation = projectile.velocity.ToRotation();
				float C = 2 * (float)Math.PI * length;
				float oneLength = 360f / C * 10;
				float mult = 0f;
				for (float i = 0; i < 360; i += oneLength)
				{
					float waveValue = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 8 + projectile.ai[1])).X;
					Vector2 drawArea = origin + new Vector2(length + waveValue, 0).RotatedBy(MathHelper.ToRadians(i) + rotation);
					if(i <= 360)
					ParticlePos.Add(drawArea);
				}
			}
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int i = 0; i < ParticlePos.Count; i++)
            {
				Rectangle hitbox = new Rectangle((int)ParticlePos[i].X - 5, (int)ParticlePos[i].Y - 5, 10, 10);
				if (hitbox.Intersects(targetHitbox))
					return true;
			}
			return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Tide/TidalConstructTrail");
			if ((int)projectile.ai[0] >= 0)
			{
				for(int i = 0; i < ParticlePos.Count; i++)
				{
					spriteBatch.Draw(texture, ParticlePos[i] - Main.screenPosition, null, new Color(200, 200, 255, 0) * (1f - (projectile.alpha / 255f)), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
	public class RippleWaveSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ripple Wave");
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.timeLeft = 20;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.localNPCHitCooldown = 45;
			projectile.usesLocalNPCImmunity = true;
			projectile.alpha = 70;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
		bool runOnce = true;
		public override bool ShouldUpdatePosition()
		{
			return false;
		}
		public override void AI()
		{
			UpdateList();
			length += projectile.velocity.Length();
			projectile.ai[1]++;
			projectile.alpha += 9;
			if (runOnce)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 21, 0.5f, -0.1f);
				runOnce = false;
			}
			Lighting.AddLight(projectile.Center, (255 - projectile.alpha) * 0.15f / 255f, (255 - projectile.alpha) * 0.25f / 255f, (255 - projectile.alpha) * 0.65f / 255f);
			projectile.rotation += 0.04f;
		}
		float length = 0;
		List<Vector2> ParticlePos = new List<Vector2>();
		public void UpdateList()
		{
			if ((int)projectile.ai[0] >= 0)
			{
				ParticlePos = new List<Vector2>();
				Vector2 origin = projectile.Center;
				float rotation = projectile.velocity.ToRotation();
				float C = 2 * (float)Math.PI * length;
				float oneLength = 360f / C * 10;
				for (float i = 0; i < 360; i += oneLength)
				{
					float waveValue = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 8 + projectile.ai[1])).X;
					Vector2 drawArea = origin + new Vector2(length + waveValue, 0).RotatedBy(MathHelper.ToRadians(i) + rotation);
					if (i <= 360)
						ParticlePos.Add(drawArea);
				}
			}
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			for (int i = 0; i < ParticlePos.Count; i++)
			{
				Rectangle hitbox = new Rectangle((int)ParticlePos[i].X - 5, (int)ParticlePos[i].Y - 5, 10, 10);
				if (hitbox.Intersects(targetHitbox))
					return true;
			}
			return false;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/Tide/TidalConstructTrail");
			if ((int)projectile.ai[0] >= 0)
			{
				for (int i = 0; i < ParticlePos.Count; i++)
				{
					spriteBatch.Draw(texture, ParticlePos[i] - Main.screenPosition, null, new Color(200, 200, 255, 0) * (1f - (projectile.alpha / 255f)), projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}
		
			