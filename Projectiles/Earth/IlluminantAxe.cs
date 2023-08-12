using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{    
    public class IlluminantAxe : ModProjectile 
    {
        public float AI3 = 0;
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(AI3);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            AI3 = reader.ReadSingle();
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Illuminant Axe");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 4;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((float)(Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
			}
            if(AI3 == 4)
            {
                texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Earth/IlluminantAxeBorder");
                drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
                if (Projectile.ai[0] > 0)
                    for (int i = 0; i < 6; i++)
                    {
                        Vector2 circular = new Vector2(40 - Projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians((float)Math.Pow(Projectile.ai[0], 1.25) + i * 60));
                        Vector2 drawPos = Projectile.Center - Main.screenPosition;
                        Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(60, 80, 100, 0) * (Projectile.ai[0] / 40f), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                    }
                for (int i = 0; i < 6; i++)
                {
                    Vector2 circular = new Vector2(2, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 0.25f + i * 60));
                    Vector2 drawPos = Projectile.Center - Main.screenPosition;
                    Main.spriteBatch.Draw(texture, drawPos + circular, null, new Color(255, 255, 255) * MathHelper.Clamp(Projectile.ai[1] / 32, 0, 1), Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
                }
            }
            texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/Earth/Glowmoth/IlluminantAxeGlow");
            Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Main.spriteBatch.Draw(texture, drawPos, null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            AI3 = 5;
            Projectile.netUpdate = true;
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
            height = 24;
            return true;
        }
        int counter = 0;
        bool hasExplodedOnce = false;
        public override bool PreAI()
        {
            if(AI3 == 5)
            {
                if (Projectile.velocity.X < 0)
                    Projectile.direction = -1;
                else
                    Projectile.direction = 1;
                Projectile.tileCollide = false;
                if (Projectile.velocity.Length() > 1)
                    Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity *= 0.1f;
                Projectile.aiStyle = 0;
                Projectile.ai[0] = -12;
                Projectile.ai[1] = 0;
                Projectile.extraUpdates = 0;
                Projectile.friendly = false;
                AI3 = 4;
            }
            else if (AI3 == 4)
            {
                if (Projectile.velocity.X < 0)
                    Projectile.direction = -1;
                else
                    Projectile.direction = 1;
                Projectile.aiStyle = 0;
                Projectile.tileCollide = false;
                if (Projectile.velocity.Length() > 1)
                    Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity *= 0.9f;
                Projectile.ai[0]++;
                Projectile.ai[1]++;
                if (Projectile.ai[0] > 40)
                {
                    Explode();
                    Projectile.ai[0] = -20;
                }
                Projectile.timeLeft = 180;
                Projectile.extraUpdates = 0;
            }
            else if(AI3 <= 3 && AI3 > 0 && !Projectile.friendly)
            {
                Projectile.direction = -1;
                Projectile.netUpdate = true;
                Projectile.aiStyle = 3;
                Projectile.ai[0] = -1;
                if (!hasExplodedOnce) Explode();
                AI3--;
                Projectile.extraUpdates = 2;
                Projectile.friendly = true;
            }
            else
            {
                counter++;
                if (counter % 2 != 0)
                    Projectile.soundDelay++;
                if(counter % 15 == 0 && counter > 30 && counter < 70)
                {
                    if(Projectile.owner == Main.myPlayer)
                    {
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(12, 0).RotatedBy(Projectile.rotation), Main.rand.NextVector2CircularEdge(2.75f, 2.75f), ModContent.ProjectileType<IlluminantBolt>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack * 0.2f, Main.myPlayer, Main.rand.NextFloat(180, 360));
                    }
                }  
                Projectile.rotation -= 0.1f * (float)Projectile.direction;
            }
            if (Main.rand.NextBool(12))
            {
                int num1 = Dust.NewDust(new Vector2(Projectile.Hitbox.X, Projectile.Hitbox.Y), Projectile.Hitbox.Width, Projectile.Hitbox.Height, DustID.GlowingMushroom, 0f, 0f, 150, default(Color), 1.3f);
                Main.dust[num1].velocity *= 0.2f;
                Main.dust[num1].scale = 0.8f;
            }
            else if (Main.rand.NextBool(6))
            {
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(180, 360), true);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.44f;
                dust.velocity += Projectile.velocity;
            }
            return base.PreAI();
        }
        public override void PostAI()
        {
            if(Projectile.friendly)
                if (!hasExplodedOnce)
                    Projectile.tileCollide = true;
                else
                    Projectile.tileCollide = false;
        }
        public void Explode()
        {
            SOTSUtils.PlaySound(SoundID.Item62, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, -0.3f);
            for (int i = 30; i > 0; i--)
            {
                Vector2 circular = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(i * 12));
                int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y) - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                dust.color = ColorHelpers.VibrantColorAttempt(Main.rand.NextFloat(180, 360), true);
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.5f;
                dust.velocity *= 0.5f;
                dust.velocity += 1.5f * circular + 0.5f * circular / dust.scale;
            }
            if(Main.myPlayer == Projectile.owner)
            {
                int amt = Main.rand.Next(2) + 3;
                int degrees = 360 / amt;
                for (int i = 0; i < amt; i++)
                {
                    Vector2 circular = new Vector2(2.75f, 0).RotatedBy(MathHelper.ToRadians(Main.rand.NextFloat(-degrees / 2 + 5, degrees / 2 - 5) + degrees * i) + Projectile.rotation);
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center - new Vector2(12, 0).RotatedBy(Projectile.rotation), circular, ModContent.ProjectileType<IlluminantBolt>(), (int)(Projectile.damage * 0.5f), Projectile.knockBack * 0.2f, Main.myPlayer, Main.rand.NextFloat(180, 360));
                }
            }
            hasExplodedOnce = true;
        }
        public override void SetDefaults()
        {
            Projectile.width = 54;
            Projectile.height = 46; 
            Projectile.timeLeft = 7200;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Melee; 
            Projectile.aiStyle = 3; 
			Projectile.alpha = 0;
            Projectile.extraUpdates = 1;
		}
	}
}