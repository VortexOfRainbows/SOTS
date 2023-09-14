using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Void;
using SOTS.WorldgenHelpers;
using System;
using System.IO;
using Terraria;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Tide
{
	public class WaterShark : ModProjectile
	{
		public override void SetDefaults()
		{
			Projectile.DamageType = DamageClass.Magic;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.extraUpdates = 1;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = false;
			Projectile.penetrate = 1;
            Projectile.ignoreWater = true;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
                Projectile.velocity.X = -oldVelocity.X;
            if (Projectile.velocity.Y != oldVelocity.Y)
                Projectile.velocity.Y = -oldVelocity.Y;
            counter = 0;
            originalVelocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.5f;
            return false;
        }
        public void Draw(bool outLine)
        {
            Projectile.direction = Math.Sign(originalVelocity.X);
            if (Projectile.direction == 0)
                Projectile.direction = 1;
            Texture2D texture = ModContent.Request<Texture2D>("SOTS/Projectiles/Tide/WaterShark").Value;
            Texture2D outline = ModContent.Request<Texture2D>("SOTS/Projectiles/Tide/WaterSharkOutline").Value;
            Vector2 origin = outline.Size() / 2;
            if(outLine)
                for (int k = 0; k < 4; k++)
                {
                    Vector2 circular = new Vector2(1f * Projectile.scale, 0).RotatedBy(MathHelper.PiOver2 * k);
                    Main.spriteBatch.Draw(outline, Projectile.Center - Main.screenPosition + circular, null, Color.White, Projectile.rotation, origin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                }
            else
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, Projectile.rotation, origin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
		public override bool PreDraw(ref Color lightColor)
        {
            return false;
		}
		Vector2 originalVelocity = Vector2.Zero;
        bool runOnce = true;
		float counter = 0;
		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, ColorHelpers.TideColor.ToVector3());
			if (runOnce)
            {
                for (int i = 0; i < 36; i++)
                {
                    WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 8f + Main.rand.NextVector2Circular(12, 12) * 0.5f, Main.rand.NextFloat(1.3f, 1.5f));
                    Vector2 circular = new Vector2(5, 0).RotatedBy(i / 36f * MathHelper.TwoPi);
                    circular.X *= 0.4f;
                    circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                    WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 2f + circular + Main.rand.NextVector2Circular(1, 1), Main.rand.NextFloat(1.5f, 1.8f));
                }
                SOTSUtils.PlaySound(SoundID.Item80, Projectile.Center, 0.8f, -0.5f);
                originalVelocity = Projectile.velocity;
                runOnce = false;
			}
            if(!SOTSWorldgenHelper.TrueTileSolid((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16))
            {
                Projectile.tileCollide = true;
            }
			if (!Projectile.velocity.Equals(new Vector2(0, 0)))
				Projectile.rotation = Projectile.velocity.ToRotation();
			Projectile.direction = Math.Sign(originalVelocity.X);
			if (Projectile.direction == 0)
				Projectile.direction = 1;
            float sin = -0.75f * (float)Math.Sin(MathHelper.ToRadians(Projectile.ai[1] * 5.0f)) * 2.5f * Math.Clamp((float)Math.Sqrt(counter / 15f), 0, 2f);
            if (counter < 6)
            {
                Projectile.ai[1] = 0;
                sin = 0f;
            }
			Vector2 sinV = new Vector2(0, sin * Projectile.direction).RotatedBy(Projectile.velocity.ToRotation());
			Projectile.Center += sinV;
			Projectile.rotation += sinV.Y * 0.2f * Projectile.direction;
            Projectile.ai[1] += Math.Clamp(counter / 3f, 0, 1f);
            counter += 0.15f;
			Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.Zero) * originalVelocity.Length() * (1 + counter);
            float homingDist = 320 * (float)Math.Sqrt(counter / 6f);
            int target = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, homingDist, needsLOS: true);
            if (target >= 0)
            {
                NPC npc = Main.npc[target];
                if (npc.CanBeChasedBy())
                {
                    Vector2 toNPC = npc.Center - Projectile.Center;
                    float homing = 0.015f * (1 + counter);
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC.SafeNormalize(Vector2.Zero) * originalVelocity.Length() * 4f, homing);
                }
            }
			if(Main.netMode != NetmodeID.Server)
            {
				if(Main.rand.NextBool(3))
					WaterParticle.NewWaterParticle(Projectile.Center + new Vector2(0, Main.rand.NextFloat(-12, 12)).RotatedBy(Projectile.rotation), Projectile.velocity * -0.4f + Main.rand.NextVector2Circular(0.5f, 0.5f), 0.55f);
                for (float i = 0.0f; i < 1f; i += 0.34f)
                {
                    WaterParticle.NewWaterParticle(Projectile.Center + new Vector2(13, 13 * Projectile.direction).RotatedBy(Projectile.rotation) + i * Projectile.velocity, Projectile.velocity * 0.25f + Main.rand.NextVector2Circular(0.1f, 0.1f), 0.7f);
                    if (Main.rand.NextBool(4))
                        WaterParticle.NewWaterParticle(Projectile.Center + new Vector2(13, 13 * Projectile.direction).RotatedBy(Projectile.rotation) + i * Projectile.velocity, Projectile.velocity * 0.6f + Main.rand.NextVector2Circular(4.0f, 4.0f), 0.75f);
                }
			}
        }
        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 24; i++)
            {
                WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 9f + Main.rand.NextVector2Circular(4, 4), Main.rand.NextFloat(1.4f, 1.5f));
                Vector2 circular = new Vector2(3.75f, 0).RotatedBy(i / 24f * MathHelper.TwoPi);
                circular.X *= 0.4f;
                circular = circular.RotatedBy(Projectile.velocity.ToRotation());
                WaterParticle.NewWaterParticle(Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 1.85f + circular + Main.rand.NextVector2Circular(1, 1), Main.rand.NextFloat(1.4f, 1.5f));
            }
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<HydroBubble>(), (int)(Projectile.damage * 0.2f), Projectile.knockBack * 0.1f, Projectile.owner, 3, target.whoAmI);
        }
    }
}