using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using SOTS.Void;
using SteelSeries.GameSense.DeviceZone;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static System.Net.Mime.MediaTypeNames;

namespace SOTS.Projectiles.Evil
{    
    public class EvilBolt : ModProjectile 
    {	
        public override void SetDefaults()
        {
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.alpha = 0;
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.timeLeft = 180;
			Projectile.alpha = 255;
		}
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 20;
			hitbox = new Rectangle((int)Projectile.Center.X - width/2, (int)Projectile.Center.Y - width/2, width, width);
            base.ModifyDamageHitbox(ref hitbox);
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			VoidPlayer.VoidBurn(Mod, target, 10, 240);
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Color color = this is EvilBolt2 ? ColorHelper.RedEvilColor : ColorHelper.EvilColor;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < 5; k++)
			{
				Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + Main.rand.NextVector2Circular(1, 1), null, Projectile.GetAlpha(color), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		bool runOnce = true;
		public override void AI()
		{
			if (runOnce)
			{
				DustOut();
				Projectile.scale = 0.1f;
				Projectile.alpha = 0;
				runOnce = false;
			}
			else if (Projectile.scale < 1f)
				Projectile.scale += 0.05f;
			if(Projectile.timeLeft < 42)
            {
				Projectile.alpha += 6;
			}
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			Projectile.velocity += Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.ai[0];
            if(this is EvilBolt2)
            {
                if (Main.rand.NextBool(3))
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
                    dust.velocity *= 0.1f;
                    dust.velocity -= Projectile.velocity * 0.05f;
                    dust.color = ColorHelper.RedEvilColor;
                    dust.color.A = 100;
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.25f;
                }
            }
            else
            {
                if (Main.rand.NextBool(8))
                {
                    Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.RainbowMk2, Main.rand.NextVector2Circular(1, 1));
                    dust.velocity *= 0.5f;
                    dust.velocity -= Projectile.velocity * 0.05f;
                    dust.color = ColorHelper.EvilColor;
                    dust.color.A = 100;
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.25f;
                }
            }
		}
		public override void OnKill(int timeLeft)
		{
			DustOut();
		}
		public void DustOut()
        {
			for (int i = 0; i < 360; i += 60)
			{
				Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
                dust.velocity = circularLocation * 0.4f;
				dust.velocity += Projectile.velocity * 0.2f;
				dust.color = ColorHelper.EvilColor;
				dust.color.A = 100;
				dust.noGravity = true;
				dust.alpha = 100;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
			}
		}
	}
	public class EvilBolt2 : EvilBolt
	{
        public override void SetDefaults()
        {
            base.SetDefaults();
			Projectile.width = 18;
		}
    }
	public class EvilEye : ModProjectile
    {
        public Texture2D texture;
        public Texture2D texturePupil;
        public override string Texture => "SOTS/NPCs/Constructs/EvilEye";
        public override void SetDefaults()
        {
            Projectile.penetrate = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.alpha = 0;
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            texture ??= Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            texturePupil ??= ModContent.Request<Texture2D>("SOTS/NPCs/Constructs/EvilEyePupil").Value;
            Draw();
            return false;
        }
        public void Draw()
        {
            Color color = ColorHelper.RedEvilColor;
            color.A = 50;
            Vector2 drawPosition = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2;
            float percent = MathF.Min(1, Projectile.ai[0] / 30f);
            float percent2 = MathF.Min(1, Projectile.ai[0] / 50f);
            float sin = MathF.Sin(percent * MathF.PI);
            float sin2 = MathF.Sin(percent2 * MathF.PI);
            float size = 1.5f * sin + percent;
            float mult = 0.8f + size * 0.4f;
            float alpha = MathF.Min(1, Projectile.ai[0] / 20f) * sin2;
            for (int i = 0; i < 5; i++)
            {
                int length = 0;
                if (i != 0)
                    length = 1;
                Vector2 circular = new Vector2(length, 0).RotatedBy(i * MathHelper.Pi / 2f);
                Main.spriteBatch.Draw(texture, drawPosition + circular, null, color * alpha, 0f, origin, mult, SpriteEffects.None, 0f);
            }
            color = ColorHelper.RedEvilColor;
            color.A = 50;
            drawPosition += Projectile.velocity.SNormalize() * 2;
            for (int i = 0; i < 4; i++)
            {
                int length = 1;
                Vector2 circular = new Vector2(length, 0).RotatedBy(i * MathHelper.Pi / 2f);
                Main.spriteBatch.Draw(texturePupil, drawPosition + circular, null, color * alpha, 0f, origin, mult, SpriteEffects.None, 0f);
            }
        }
        public override void AI()
        {
            Projectile.ai[0]++;
            if (Projectile.ai[0] >= 15)
            {
                if (Projectile.ai[1] != -1)
                {
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity.SafeNormalize(Vector2.Zero) * 0.1f, ModContent.ProjectileType<EvilBolt2>(), Projectile.damage, 0, Main.myPlayer, 0.05f);
                    }
                    Projectile.ai[1] = -1;
                    SOTSUtils.PlaySound(SoundID.Item46, Projectile.Center, 0.9f, -0.1f);
                }
            }
            if (Projectile.ai[0] >= 50)
            {
                Projectile.Kill();
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override void OnKill(int timeLeft)
        {
            DustOut();
        }
        public void DustOut()
        {
            for (int i = 0; i < 360; i += 60)
            {
                Vector2 circularLocation = new Vector2(Main.rand.NextFloat(4), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X + circularLocation.X - 4, Projectile.Center.Y + circularLocation.Y - 4), 4, 4, DustID.RainbowMk2);
                dust.velocity = circularLocation * 0.4f;
                dust.velocity += Projectile.velocity * 0.2f;
                dust.color = ColorHelper.EvilColor;
                dust.color.A = 100;
                dust.noGravity = true;
                dust.alpha = 100;
                dust.fadeIn = 0.1f;
                dust.scale *= 2f;
            }
        }
    }
}