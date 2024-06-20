using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System;
using System.IO;
using SOTS.Dusts;

namespace SOTS.Projectiles.Chaos
{    
    public class ChimeraFireball : ModProjectile 
    {	
        public static Color ChaosPinkNoA
        {
            get
            {
                Color cPink = ColorHelpers.ChaosPink;
                cPink.A = 0;
                return cPink;
            }
        }
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 20;  
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;    
		}
		public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            int cap = Math.Min(Projectile.timeLeft, Projectile.oldPos.Length);
            for (int k = 0; k < cap; k++) 
            {
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                float colorM = (cap - k) / (float)cap;
                Color color = Projectile.GetAlpha(ChaosPinkNoA) * (0.5f + 0.5f * colorM) * .75f;
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(i * MathHelper.TwoPi / 6f + SOTSWorld.GlobalCounter);
                    Main.spriteBatch.Draw(texture, drawPos + circular * 1, null, Color.Black * 0.1f * colorM, Projectile.rotation, drawOrigin, Projectile.scale * colorM, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * colorM, SpriteEffects.None, 0f);
            }
            for(int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(i * MathHelper.TwoPi / 6f + SOTSWorld.GlobalCounter);
                    if(j == 0)
                        Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, Color.Black * 0.1f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                    else
                        Main.spriteBatch.Draw(texture, Projectile.Center + circular * 0.5f - Main.screenPosition, null, ChaosPinkNoA * 0.5f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, ChaosPinkNoA, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
		}
        public override void SetDefaults()
        {
			Projectile.height = 32;
			Projectile.width = 32;
			Projectile.friendly = false;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 240;
			Projectile.tileCollide = false;
			Projectile.hostile = true;
			Projectile.netImportant = true;
		}
		public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(SoundID.DD2_BetsyFireballImpact, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.3f);
            if (Main.myPlayer == Projectile.owner)
            {
                for(int i = 0; i < 5; i ++)
                {
                    Vector2 circular = new Vector2(0, -Main.rand.NextFloat(1f, 2f)).RotatedBy(i * MathHelper.TwoPi / 5f) + Vector2.UnitY * -1.0f + Projectile.velocity * 0.5f + Main.rand.NextVector2Circular(1, 1) * 0.8f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, circular, ModContent.ProjectileType<ChimeraFireballSmall>(), Projectile.damage, 1f, Main.myPlayer, Projectile.ai[0]);
                }
            }
            for (int i = 0; i < 36; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, i % 2 == 0 ? ModContent.DustType<CopyDust4>() : ModContent.DustType<PixelDust>());
                dust.noGravity = true;
                dust.velocity *= 1.5f;
                dust.velocity += Projectile.velocity;
                dust.scale = 2f + 2.0f * Main.rand.NextFloat();
                dust.fadeIn = i % 2 == 0 ? 0.1f : 6f;
                dust.color = i % 2 == 0 ? ChimeraFireball.ChaosPinkNoA * 0.5f : Color.Black * 0.5f;
            }
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.hostile = false;
            Projectile.velocity *= 0f;
            Projectile.tileCollide = false;
            return false;
        }
        private bool RunOnce = true;
		public override void AI()
        {
            if(RunOnce)
            {
                RunOnce = false;
                SOTSUtils.PlaySound(SoundID.DD2_BetsyFireballShot, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f, -0.3f);
            }
            int target = (int)Projectile.ai[0];
            Player player = Main.player[target];
            if (!player.active)
            {
                Projectile.Kill();
            }
            else
            {
                float speedMult = Projectile.ai[1] / 60f;
                if (speedMult > 1)
                    speedMult = 1;
                Vector2 hoverPosition = player.Center - new Vector2(0, 240);
                Vector2 toPlayer = hoverPosition - Projectile.Center;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, toPlayer.SafeNormalize(Vector2.Zero) * (2 + speedMult + Projectile.velocity.Length()), 0.02f * (1 + speedMult));
                Projectile.ai[1]++;
                Projectile.rotation += MathHelper.ToRadians(4) * Math.Sign(Projectile.velocity.X) * MathF.Sqrt(Math.Abs(Projectile.velocity.X));

                int rand = Main.rand.Next(6);
                if (rand <= 1)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, rand == 0 ? ModContent.DustType<CopyDust4>() : ModContent.DustType<PixelDust>());
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.scale = 2.0f;
                    dust.fadeIn = rand == 0 ? 0.1f : 8f;
                    dust.color = rand == 0 ? ChimeraFireball.ChaosPinkNoA * 0.5f : Color.Black * 0.5f;
                }
            }
        }
    }
    public class ChimeraFireballSmall : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(Projectile.hostile);
            writer.Write(Projectile.tileCollide);
            writer.Write(Projectile.timeLeft);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Projectile.hostile = reader.ReadBoolean();
            Projectile.tileCollide = reader.ReadBoolean();
            Projectile.timeLeft = reader.ReadInt32();
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 15;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            int starting = 0;
            if (Projectile.timeLeft <= Projectile.oldPos.Length)
            {
                starting = Projectile.oldPos.Length - Projectile.timeLeft;
            }
            for (int k = starting; k < Projectile.oldPos.Length; k++)
            {
                if (k < Projectile.oldPos.Length - 1  && Projectile.oldPos[k] == Projectile.oldPos[k + 1])
                    continue;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                float colorM = (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length;
                Color color = Projectile.GetAlpha(ChimeraFireball.ChaosPinkNoA) * (0.5f + 0.5f * colorM) * .75f;
                color.A = 0;
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(i * MathHelper.TwoPi / 6f + SOTSWorld.GlobalCounter);
                    Main.spriteBatch.Draw(texture, drawPos + circular * 1, null, Color.Black * 0.1f * colorM, Projectile.rotation, drawOrigin, Projectile.scale * colorM, SpriteEffects.None, 0f);
                }
                Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale * colorM, SpriteEffects.None, 0f);
            }
            if (!Projectile.tileCollide)
                return false;
            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(4, 0).RotatedBy(i * MathHelper.TwoPi / 6f + SOTSWorld.GlobalCounter);
                    if (j == 0)
                        Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, null, Color.Black * 0.1f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                    else
                        Main.spriteBatch.Draw(texture, Projectile.Center + circular * 0.5f - Main.screenPosition, null, ChimeraFireball.ChaosPinkNoA * 0.4f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
                }
            }
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, ChimeraFireball.ChaosPinkNoA, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SetDefaults()
        {
            Projectile.height = 18;
            Projectile.width = 18;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 330;
            Projectile.tileCollide = true;
            Projectile.hostile = true;
            Projectile.netImportant = true;
        }
        public override void OnKill(int timeLeft)
        {

        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return true;
        }
        public override void AI()
        {
            Projectile.rotation += MathHelper.ToRadians(4);
            if(Projectile.tileCollide)
                Projectile.velocity.Y += 0.2f;
            if(Projectile.timeLeft <= 15)
            {
                if(Projectile.timeLeft >= 15)
                    Decay();
                for(int i = 0; i < 2; i++)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, i == 0 ? ModContent.DustType<CopyDust4>() : ModContent.DustType<PixelDust>());
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.velocity.Y += -Main.rand.NextFloat(1.5f);
                    dust.scale = 1f + 2.0f * Main.rand.NextFloat();
                    dust.fadeIn = i == 0 ? 0.1f : 5f;
                    dust.color = i == 0 ? ChimeraFireball.ChaosPinkNoA * 0.5f : Color.Black * 0.5f;
                }
            }
            else
            {
                int rand = Main.rand.Next(7);
                if(rand <= 1)
                {
                    Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, rand == 0 ? ModContent.DustType<CopyDust4>() : ModContent.DustType<PixelDust>());
                    dust.noGravity = true;
                    dust.velocity *= 0.5f;
                    dust.scale = 1.5f;
                    dust.fadeIn = rand == 0 ? 0.1f : 8f;
                    dust.color = rand == 0 ? ChimeraFireball.ChaosPinkNoA * 0.5f : Color.Black * 0.5f;
                }
            }
        }
        public void Decay()
        {
            Projectile.hostile = false;
            Projectile.velocity *= 0f;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 15;
            Projectile.netUpdate = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Decay();
            return false;
        }
    }
}
		