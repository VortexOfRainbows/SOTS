using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;
using Terraria.Audio;
using SOTS.Common.GlobalNPCs;

namespace SOTS.Projectiles.Blades
{    
    public class GuardianGreatswordBeam : ModProjectile
    {
        public static Color color1 => new Color(179, 33, 68);
        public static Color color2 => new Color(243, 200, 186);
		public static int TrailCount => 30;
        public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = TrailCount;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
        public override void SetDefaults()
        {
            Projectile.DamageType = ModContent.GetInstance<Void.VoidMelee>();
			Projectile.friendly = true;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 85;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.extraUpdates = 1;
			Projectile.localNPCHitCooldown = 50;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.alpha = 255;
		}
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
		bool runOnce = true;
        public override void AI()
        {
			Vector2 trueVelocity = Projectile.velocity * Projectile.ai[1] * Projectile.ai[1] / 3600f;
            if (runOnce)
            {
				runOnce = false;
			}
			if (Projectile.ai[1] > 6)
			{
				int target = SOTSNPCs.FindTarget_Basic(Projectile.Center, 360, Projectile, true);
				if(target != -1)
				{
					NPC npc = Main.npc[target];
					Vector2 toNPC = npc.Center - Projectile.Center;
					toNPC = toNPC.SafeNormalize(Vector2.Zero) * Projectile.velocity.Length() * 1.15f;
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, toNPC, 0.0665f);
				}
			}
            if (Projectile.ai[0] == -1)
            {
				Projectile.friendly = false;
				Projectile.velocity *= 0;
				Projectile.tileCollide = false;
				if(Projectile.timeLeft > TrailCount - 1)
				{
					for (int i = 0; i < 360; i += 45)
					{
						Vector2 circularLocation = new Vector2(-6, 0).RotatedBy(MathHelper.ToRadians(i));
						circularLocation.Y *= 0.4f;
						circularLocation = circularLocation.RotatedBy(Projectile.rotation - MathHelper.PiOver4);
						Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, ModContent.DustType<PixelDust>());
						dust.noGravity = true;
						dust.velocity *= 0.1f;
						dust.velocity += circularLocation * 0.1f + trueVelocity * 0.4f;
						dust.scale *= 1.6f;
						dust.fadeIn = 6f;
						dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(1));
                        dust.alpha = 100;
                        dust.color.A = 0;
					}
					Projectile.timeLeft = TrailCount - 1;
				}
            }
			else
			{
				for (float i = 0; i < 1.0f; i += 0.5f)
				{
					Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) - trueVelocity * i, 0, 0, ModContent.DustType<PixelDust>(), Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f);
					dust.noGravity = true;
					dust.scale = Main.rand.Next(1, 3);
					dust.fadeIn = 12f;
                    dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(1));
                    dust.alpha = 100;
					dust.velocity *= 0.1f;
					dust.velocity += trueVelocity * 0.3f;
                    dust.color.A = 0;
				}
				Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;
			}
			if (Projectile.timeLeft <= TrailCount + 1)
				Projectile.ai[0] = -1;
			Projectile.ai[1]++;
			if (Projectile.ai[1] > 0)
				Projectile.position += trueVelocity;
			Projectile.alpha -= 25;
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return Projectile.ai[0] == -1 ? false : base.CanHitNPC(target);
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.ai[0] = -1;
			Projectile.netUpdate = true;
			return false;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			int starting = 0;
			bool drawMain = false;
			if(Projectile.timeLeft <= TrailCount)
            {
				starting = TrailCount - Projectile.timeLeft;
			}
			else
			{
				drawMain = true;
			}
			for (int k = starting; k < Projectile.oldPos.Length; k++)
			{
				float scale = ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Color toChange = new Color(120, 40, 60, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + new Vector2(Projectile.width / 2, Projectile.gfxOffY + Projectile.height / 2);
				Color color = Projectile.GetAlpha(toChange) * scale;
				Main.spriteBatch.Draw(texture, drawPos, null, color * 0.4f, Projectile.oldRot[k], new Vector2(texture.Width / 2, texture.Width / 2), Projectile.scale * scale * 0.75f, SpriteEffects.None, 0f);
			}
			if(drawMain)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(.5f, .5f, .5f, 0f)), Projectile.rotation, drawOrigin, Projectile.scale * 0.6f, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}
		