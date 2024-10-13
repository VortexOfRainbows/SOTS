using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using SOTS.NPCs.AbandonedVillage;
using System;
using SOTS.WorldgenHelpers;
using Humanizer;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class FamishedLaser : ModProjectile
	{
        public override string Texture => WorldGen.crimson ? "SOTS/Projectiles/AbandonedVillage/FamishedLaserCrimson" : "SOTS/Projectiles/AbandonedVillage/FamishedLaserCorruption";
        public override void SetStaticDefaults() 
		{
			ProjectileID.Sets.DrawScreenCheckFluff[Type] = 1200;
		}
		public override void SetDefaults() 
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.timeLeft = 60;
			Projectile.penetrate = -1;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
		}
		private bool HasInit = false;
		private Vector2 FinalPosition;
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public void InitializeLaser()
		{
			if(!HasInit)
			{
				SOTSUtils.PlaySound(SoundID.Item94, Projectile.Center, 0.6f, 0.2f);
			}
			Vector2 destination = new Vector2(Projectile.ai[0], Projectile.ai[1]);
			Vector2 startingPosition = Projectile.Center;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero);
			bool ableToHitBlocks = false;
			for(int b = 0; b < 640; b++)
			{
				startingPosition += Projectile.velocity * 2.5f;
				FinalPosition = startingPosition;
				int i = (int)startingPosition.X / 16;
				int j = (int)startingPosition.Y / 16;
				if (!ableToHitBlocks)
					ableToHitBlocks = FinalPosition.Distance(Projectile.Center) > 80 && FinalPosition.Distance(Projectile.Center) > destination.Distance(Projectile.Center);
                if (ableToHitBlocks && SOTSWorldgenHelper.TrueTileSolid(i, j))
                {
					break;
                }
				bool extra = !HasInit;
				int chance = SOTS.Config.lowFidelityMode ? 50 : 25;
				if(Main.rand.NextBool(chance) || extra)
				{
					Dust dust = Dust.NewDustDirect(FinalPosition - new Vector2(11, 11), 17, 17, ModContent.DustType<PixelDust>(), 0, 0, 0, Famished.GlowColor * Percent, 0.75f);
					dust.noGravity = true;
                    dust.velocity *= 1.25f * Percent;
                    dust.velocity += Projectile.velocity * Main.rand.NextFloat(6f, 8f) * Percent;
                    dust.fadeIn = 12;
                    dust.scale *= 0.5f + 0.75f * Percent;
                }
			}
			for (int i = 2; i > 0; i--)
			{
				Dust dust = Dust.NewDustDirect(FinalPosition - new Vector2(11, 11), 17, 17, ModContent.DustType<CopyDust4>(), 0, 0, 0, Famished.GlowColor * Percent * Percent, 1.5f);
				dust.noGravity = true;
				dust.velocity = dust.velocity * 0.6f * (5 - Percent * 4) + Projectile.velocity * Main.rand.NextFloat(0.1f, 2.0f);
				if (i == 2)
					dust.velocity += new Vector2(0, -Main.rand.NextFloat(0.1f, 0.3f));
				dust.fadeIn = 0.2f;
			}
			if(!HasInit)
			{
				if(Main.myPlayer == Projectile.owner)
				{
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), FinalPosition, Vector2.Zero, ModContent.ProjectileType<FamishedDeadzone>(), Projectile.damage, 0, Main.myPlayer);
				}
			}
			HasInit = true;
        }
		public override bool PreAI()
        {
			InitializeLaser();
			Projectile.ai[2] += 4;
			Projectile.ai[2] *= 1.03f;
			if (Projectile.timeLeft < 40)
				Projectile.hostile = false;
			return true;
		}
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
		public float Percent => Projectile.timeLeft / 60f;
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
                Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, color * Percent * MathF.Min(1, scale), rot - MathHelper.PiOver2, origin, new Vector2(scale * 1.25f * Percent + 3f * flatten * sinScale * (0.5f + 0.5f * MathF.Sin(MathHelper.ToRadians(i - Projectile.ai[2]) * 3 * sinScale)) * Percent, yScale), SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, position - Main.screenPosition, null, color * Percent * MathF.Min(1, scale), rot - MathHelper.PiOver2, origin, new Vector2(scale * 1.5f * Percent + 2f * flatten * sinScale * (0.5f + 0.5f * MathF.Sin(MathHelper.ToRadians(i - Projectile.ai[2] - 90) * 3* sinScale)) * Percent, yScale), SpriteEffects.None, 0f);
				prevPosition = position;
				//prevRot = rot;
            }
            return false;
		}
	}
	public class FamishedDeadzone : ModProjectile
	{
        public override string Texture => WorldGen.crimson ? "SOTS/Projectiles/AbandonedVillage/FamishedLaserCrimson" : "SOTS/Projectiles/AbandonedVillage/FamishedLaserCorruption";
        public override void SetDefaults()
        {
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.timeLeft = 300;
            Projectile.friendly = false;
            Projectile.hostile = false;
			Projectile.hide = true;
			Projectile.tileCollide = false;
        }
        public override bool CanHitPlayer(Player target)
        {
			return !Collision.IsClearSpotTest(target.position - new Vector2(2, 2), 16, target.width + 4, target.height + 4, true, true, 1, true, true);
        }
        public override bool PreAI()
        {
			Projectile.ai[0]++;
			float percent = Projectile.ai[0] / 60f;
			if (percent > 1.2f)
				Projectile.hostile = true;
            if (percent > 1)
                percent = 1;
			if (Projectile.timeLeft < 60)
				return true;
            Vector2 circular = Main.rand.NextVector2Square(-Projectile.width / 2, Projectile.width / 2);
            Vector2 center = Projectile.Center + circular;
			if(percent < 0.9f)
				PixelDust.Spawn(center, 0, 0, circular * Main.rand.NextFloat(0.05f), Famished.GlowColor * 0.5f * percent, 6);
			for(int x = 0; x < 48; x++)
            {
                center = Projectile.Center + Main.rand.NextVector2Square(-Projectile.width / 2, Projectile.width / 2);
                int i = (int)center.X / 16;
                int j = (int)center.Y / 16;
                Vector2? dustPosition = SOTSTile.GetWorldPositionOnTile(i, j, x % 4, center.X - i * 16, center.Y - j * 16);
                if (dustPosition != null)
                    PixelDust.Spawn(dustPosition.Value, 0, 0, Main.rand.NextVector2Circular(0.1f, 0.1f) + (dustPosition.Value - center).SNormalize() * Main.rand.NextFloat(0.5f) * Main.rand.NextFloat(0.5f), Famished.GlowColor * 0.8f * percent, 3).scale = Main.rand.NextFloat(1.5f, 2f);
            }
            return true;
        }
    }
}