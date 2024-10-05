using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Helpers;
using SOTS.Items.Flails;
using SOTS.Prim.Trails;
using SOTS.Utilities;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Earth
{
    public class Shattershine : BaseFlailProj
    {
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 40;
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
            height = 24;
            return true;
        }
        public override void OnLaunch(Player player)
        {
            Projectile.velocity *= 1.2f;
        }
        // public override void SetStaticDefaults() => DisplayName.SetDefault("Shattershine");
        public override void SetDefaults()
        {
            SetFlailStats(new Vector2(0.5f, 1.5f), new Vector2(1f, 1f), 2f, 50, 11);
            Projectile.Size = new Vector2(46, 46);
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 15;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if(Main.myPlayer == Projectile.owner && released)
            {
                float rand2 = Main.rand.NextFloat(360f);
                int rand = Main.rand.Next(2) + 2;
                for(int i = 0; i < rand; i++)
                { 
                    Projectile.NewProjectile(Projectile.GetSource_OnHit(target), Projectile.Center, new Vector2(4 + Main.rand.NextFloat(2), 0).RotatedBy(MathHelper.ToRadians(rand2 + Main.rand.NextFloat(-10, 10f) + i * 360f / rand)), ModContent.ProjectileType<ShattershinePulse>(), (int)(Projectile.damage * 0.6f), Projectile.knockBack, Main.myPlayer, Main.rand.NextFloat(6.28f), Main.rand.Next(11) + 10);
                }
            }
            target.immune[Projectile.owner] = 0;
        }
        public override void SpinExtras(Player player)
        {
            MakeDust(0);
            MakeDust(3);
            Lighting.AddLight(Projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public void MakeDust(int rand = 3)
        {
            if (rand <= 1)
            {
                for (float i = 0f; i < 1f; i += 0.334f)
                {
                    Vector2 velo = new Vector2(8, 0).RotatedBy(MathHelper.ToRadians(11 * i));
                    if (rand > 0)
                        velo = Projectile.velocity;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 8, Projectile.Center.Y - 8) + velo * i, 8, 8, ModContent.DustType<CopyDust4>());
                    Color color2 = ColorHelper.VibrantColorGradient(dustCounter + i * 6);
                    dust.color = color2;
                    dust.noGravity = true;
                    dust.fadeIn = 0.1f;
                    dust.scale = 1.2f;
                    dust.alpha = Projectile.alpha;
                    dust.velocity *= 0.12f;
                    dust.velocity -= Projectile.velocity * 0.08f;
                }
            }
            else if (Main.rand.NextBool(rand))
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.position.X - 4, Projectile.position.Y - 4), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
                Color color2 = ColorHelper.VibrantColorGradient(Main.rand.NextFloat(360));
                dust.color = color2;
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.8f;
                dust.alpha = Projectile.alpha;
                dust.velocity *= 0.3f;
                dust.velocity += Projectile.velocity * 0.2f;
            }
        }
        float dustCounter = 0;
        public override void NotSpinningExtras(Player player)
        {
            dustCounter += 6;
            MakeDust(2);
            MakeDust(1);
            Lighting.AddLight(Projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, drawPos, null, Color.White, Projectile.rotation, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale * 1.0f, SpriteEffects.None, 0f); //putting origin on center of ball instead of on spike + ball
            return false;
        }
    }
}