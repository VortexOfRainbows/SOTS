using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.NPCs.AbandonedVillage;
using System;
using SOTS.WorldgenHelpers;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class BridgeburnerLaser : FamishedLaser
	{
        public override string Texture => "SOTS/Projectiles/AbandonedVillage/BridgeburnerLaser";
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		public override bool PreAI()
        {
			InitializeLaser();
			Projectile.ai[2] += 5;
			Projectile.ai[2] *= 1.01f;
			if (Projectile.timeLeft < 50)
				Projectile.hostile = false;
			return true;
        }
		public override void PlaySound() => SOTSUtils.PlaySound(SoundID.Item92, Projectile.Center, 0.7f, -0.25f); // SOTSUtils.PlaySound(SoundID.Item42, Projectile.Center, 0.7f, -0.25f);
        public override int DeadzoneType() => ModContent.ProjectileType<BridgeburnerDeadzone>();
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) 
		{
			Vector2 toEnd = FinalPosition - Projectile.Center;
			float dist = toEnd.Length();
			toEnd = toEnd.SNormalize();
			for(int i = 0; i < dist; i += 4)
			{
				Vector2 position = Projectile.Center + toEnd * i;
				Rectangle hitbox = new Rectangle((int)position.X - 6, (int)position.Y - 6, 12, 12);
				if (hitbox.Intersects(targetHitbox))
					return true;
			}
			return false;
		}
        public override bool PreDraw(ref Color lightColor)
        {
            if (!HasInit)
                return false;
            Texture2D texture = ModContent.Request<Texture2D>(Texture).Value;
            Vector2 origin = new Vector2(3, 0);
            float rotation = Projectile.velocity.ToRotation();
            Vector2 start = Projectile.Center;
            Vector2 final = FinalPosition;
			Vector2 toEnd = (final - start).SNormalize();
			float dist = Vector2.Distance(start, final);
            Color color = Famished.GlowColor;
			Vector2 prevPosition = Projectile.Center;
			//float prevRot = Projectile.velocity.ToRotation();
			float scale = 0.1f;
			float sinScale = 1f;
			float flatten = MathF.Max(0, 1 - Projectile.ai[2] / 900f);
            for (int i = 4; i < dist; i += 4)
            {
				if (scale < 1)
					scale += 0.2f;
				else
					scale += 0.005f;
				sinScale -= 0.003f;
				//Vector2 sin = new Vector2(0, 10 * MathF.Sin(MathHelper.ToRadians(i))).RotatedBy(prevRot);
                Vector2 position = Projectile.Center + toEnd * i;
                Vector2 toPrev = prevPosition - position;
                float rot = toPrev.ToRotation();
                float yScale = toPrev.Length() / texture.Height;
                Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, color * Percent * MathF.Min(1, scale), rot - MathHelper.PiOver2, origin, new Vector2(scale * Percent + 1.25f * flatten * sinScale * (0.5f + 0.5f * MathF.Sin(MathHelper.ToRadians(i - Projectile.ai[2]) * 6 * sinScale)) * Percent, yScale), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, color * Percent * MathF.Min(1, scale), rot - MathHelper.PiOver2, origin, new Vector2(scale * Percent + 1.5f * flatten * sinScale * (0.5f + 0.5f * MathF.Sin(MathHelper.ToRadians(i - Projectile.ai[2] - 90) * 6 * sinScale)) * Percent, yScale), SpriteEffects.None, 0f);
				prevPosition = position;
				//prevRot = rot;
            }
            return false;
		}
	}
	public class BridgeburnerDeadzone : FamishedDeadzone
	{
        public override string Texture => "SOTS/Projectiles/AbandonedVillage/BridgeburnerLaser";
        public override void SetDefaults()
        {
            Projectile.width = 64;
            Projectile.height = 64;
            Projectile.timeLeft = 120;
            Projectile.friendly = false;
            Projectile.hostile = false;
			Projectile.hide = true;
			Projectile.tileCollide = false;
        }
    }
}