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
			// DisplayName.SetDefault("Ripple Wave");
		}
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.timeLeft = 31;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.idStaticNPCHitCooldown = 15;
			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.alpha = 100;
		}
        public override bool? CanCutTiles()
        {
			return false;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
			int defense = target.defense;
			int extra = defense / 2;
			if (extra > 4)
				extra = 4;
			damage += extra;
		}
        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
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
			length += Projectile.velocity.Length();
			Projectile.position += Projectile.velocity;
			Projectile.ai[1]++;
			Projectile.alpha += 5;
			//if (runOnce)
			//{
			//	SOTSUtils.PlaySound(SoundID.Item21, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.4f);
			//	runOnce = false;
			//}
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.65f / 255f);
			Projectile.rotation += 0.04f;
		}
		float length = 0;
		const float finalDegree = 0f;
		List<Vector2> ParticlePos = new List<Vector2>();
		public void UpdateList()
		{
			Player player = Main.player[Projectile.owner];
			if ((int)Projectile.ai[0] >= 0)
			{
				ParticlePos = new List<Vector2>();
				Vector2 origin = player.Center;
				float rotation = Projectile.velocity.ToRotation();
				float C = 2 * (float)Math.PI * length;
				float oneLength = 360f / C * 10;
				float mult = 0f;
				for (float i = 0; i < 360; i += oneLength)
				{
					float waveValue = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 8 + Projectile.ai[1])).X;
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
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Tide/TidalConstructTrail").Value;
			if ((int)Projectile.ai[0] >= 0)
			{
				for(int i = 0; i < ParticlePos.Count; i++)
				{
					Main.spriteBatch.Draw(texture, ParticlePos[i] - Main.screenPosition, null, new Color(200, 200, 255, 0) * (1f - (Projectile.alpha / 255f)), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
	public class RippleWaveSummon : ModProjectile
	{
		public override string Texture => "SOTS/Projectiles/Tide/RippleWave";
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Ripple Wave");
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.timeLeft = 20;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.localNPCHitCooldown = 45;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.alpha = 70;
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
			length += Projectile.velocity.Length();
			Projectile.ai[1]++;
			Projectile.alpha += 9;
			if (runOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item21, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.5f, -0.1f);
				runOnce = false;
			}
			Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.15f / 255f, (255 - Projectile.alpha) * 0.25f / 255f, (255 - Projectile.alpha) * 0.65f / 255f);
			Projectile.rotation += 0.04f;
		}
		float length = 0;
		List<Vector2> ParticlePos = new List<Vector2>();
		public void UpdateList()
		{
			if ((int)Projectile.ai[0] >= 0)
			{
				ParticlePos = new List<Vector2>();
				Vector2 origin = Projectile.Center;
				float rotation = Projectile.velocity.ToRotation();
				float C = 2 * (float)Math.PI * length;
				float oneLength = 360f / C * 10;
				for (float i = 0; i < 360; i += oneLength)
				{
					float waveValue = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 8 + Projectile.ai[1])).X;
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
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Projectiles/Tide/TidalConstructTrail").Value;
			if ((int)Projectile.ai[0] >= 0)
			{
				for (int i = 0; i < ParticlePos.Count; i++)
				{
					Main.spriteBatch.Draw(texture, ParticlePos[i] - Main.screenPosition, null, new Color(200, 200, 255, 0) * (1f - (Projectile.alpha / 255f)), Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}
		
			