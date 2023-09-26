using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using FullSerializer.Internal;
using System;
using static Humanizer.In;
using static Terraria.ModLoader.PlayerDrawLayer;

namespace SOTS.Projectiles.Permafrost
{    
    public class PolarisMines : ModProjectile 
    {
        public override void SetStaticDefaults()
		{
			Main.projFrames[Type] = 2;
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.height = 36;
			Projectile.width = 36;
			Projectile.penetrate = -1;
			Projectile.friendly = false;
			Projectile.hostile = true;
			Projectile.timeLeft = 600;
			Projectile.tileCollide = true;
			Projectile.netImportant = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (Projectile.velocity.X != oldVelocity.X)
            {
                Projectile.velocity.X = -oldVelocity.X;
            }
            if (Projectile.velocity.Y != oldVelocity.Y)
            {
                Projectile.velocity.Y = -oldVelocity.Y;
            }
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Texture2D textureG = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Permafrost/PolarisMinesGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Width / 2);
            for (int i = 0; i < 4; i++)
            {
                Vector2 circular = new Vector2(2 + Projectile.ai[1] / 200f, 0).RotatedBy(i * MathHelper.PiOver2 + Projectile.rotation);
                circular *= 1 + -1 * (float)Math.Cos((float)Math.Pow(Projectile.ai[1] / 600f, 4f) * MathHelper.TwoPi * 4f);
                Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 38 * Projectile.frame, 36, 36), new Color(100, 100, 100, 0), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 38 * Projectile.frame, 36, 36), lightColor, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(textureG, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), new Rectangle(0, 38 * Projectile.frame, 36, 36), Color.White, Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            if (Projectile.ai[1] < 590)
                Projectile.ai[1] = 590;
        }
        public override bool PreAI()
        {
            if (Projectile.ai[0] > 0)
            {
                Projectile.frame = 1;
            }
            Projectile.velocity *= 0.97f;
            if (Projectile.ai[1] > 600)
            {
                Projectile.Kill();
            }
            Projectile.rotation += Projectile.velocity.X * 0.025f;
            if(Projectile.velocity.Length() > 1)
            {
                int chance = 20;
                chance -= (int)Projectile.velocity.Length();
                if (chance <= 1 || Main.rand.NextBool(chance))
                {
                    Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<Dusts.CopyDust4>());
                    dust.velocity *= 0.25f;
                    dust.velocity += Projectile.velocity * 0.1f;
                    dust.noGravity = true;
                    dust.scale += 0.25f;
                    dust.color = Projectile.ai[0] == 1 ? new Color(100, 100, 250, 200) : new Color(250, 100, 100, 200);
                    dust.fadeIn = 0.1f;
                    dust.scale *= 1.25f;
                    dust.alpha = Projectile.alpha;
                }
            }
            Projectile.ai[1]++;
            if (Projectile.ai[1] > 600)
                Projectile.Kill();
            return true;
        }
        public override void Kill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.Item62, Projectile.Center, 1f, -0.5f);
            for(int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                dust.velocity *= 2.5f;
                dust.velocity += Projectile.velocity * 0.1f;
                dust.noGravity = true;
                dust.scale += 0.25f;
                dust.color = Projectile.ai[0] == 1 ? new Color(100, 100, 250, 200) : new Color(250, 100, 100, 200);
                dust.fadeIn = 0.1f;
                dust.scale *= 1.25f;
                dust.alpha = Projectile.alpha;
            }
            if(Main.netMode != NetmodeID.MultiplayerClient)
            {
                for(int i = 0; i < 4; i++)
                {
                    Vector2 circular = new Vector2(2.0f, 0).RotatedBy(i * MathHelper.TwoPi / 4f + Projectile.rotation);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center + circular * 12, circular, ModContent.ProjectileType<PolarBullet>(), Projectile.damage, 0, Main.myPlayer, 0, 1 - Projectile.ai[0]);
                }
            }
        }
    }
}
	