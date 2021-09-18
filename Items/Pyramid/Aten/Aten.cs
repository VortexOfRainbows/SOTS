using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using SOTS.BaseWeapons;
using SOTS.Utilities;

namespace SOTS.Items.Pyramid.Aten
{
    public class Aten : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aten");
            Tooltip.SetDefault("Conjures stars while charging\n'The defunct god... now in flail form'");
        }
        public override void SafeSetDefaults()
        {
            item.Size = new Vector2(34, 30);
            item.damage = 20;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = ItemRarityID.LightRed;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = ModContent.ProjectileType<AtenProj>();
            item.shootSpeed = 14;
            item.knockBack = 4;
        }
    }
    public class AtenProj : BaseFlailProj
    {
        public AtenProj() : base(new Vector2(0.5f, 1.4f), new Vector2(0.5f, 1f), 1.5f, 60, 10) { }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 48;
            hitbox = new Rectangle((int)projectile.Center.X - width/2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override void SetStaticDefaults() => DisplayName.SetDefault("Aten");
        public override void SetDefaults()
        {
            projectile.Size = new Vector2(26, 32);
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
        }
        public override void SpinExtras(Player player)
        {
            if (projectile.localAI[0] == 0)
            {
                SOTS.primitives.CreateTrail(new AtenPrimTrail(projectile));
            }
            if (++projectile.localAI[0] % 60 == 0)
            {
                Projectile proj = Projectile.NewProjectileDirect(Main.player[projectile.owner].Center, Vector2.Zero, ModContent.ProjectileType<AtenStar>(), projectile.damage, 0, projectile.owner, (int)(projectile.localAI[0] / 400), projectile.whoAmI);
                if (proj.modProjectile is AtenStar modProj)
                {
                    modProj.orbitSpeed = Main.rand.NextFloat(0.05f, 0.1f);
                    modProj.orbitDistance = Main.rand.Next(60, 100);
                }
            }
            Lighting.AddLight(projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public override void NotSpinningExtras(Player player)
        {
            Lighting.AddLight(projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
            Color color = new Color(255, 230, 138, 0);
            Texture2D tex = mod.GetTexture("Items/Pyramid/Aten/FlailBloom");
            spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale * 1.50f, SpriteEffects.None, 0f);
            tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, drawPos, null, lightColor, projectile.rotation, new Vector2(tex.Width / 2, 10), projectile.scale * 1.25f, SpriteEffects.None, 0f); //putting origin on center of ball instead of on spike + ball
            return false;
        }
    }

    public class AtenStar : ModProjectile, IOrbitingProj
    {
        public bool inFront
        {
            get => projectile.scale > 1;
            set
            {

            }
        }
        private Player Player => Main.player[projectile.owner];
        private Projectile Parent => Main.projectile[(int)projectile.ai[1]];
        private float Angle => Parent.localAI[0] * 0.01f + projectile.ai[0];

        bool released = false;

        bool parentActive = true;

        float angleProgression = 0;

        public float orbitSpeed;

        public int orbitDistance;

        private float distToFlail;
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.Size = new Vector2(12, 12);
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
            projectile.penetrate = 1;
            projectile.extraUpdates = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            //Dust dust = Dust.NewDustPerfect(projectile.Center, 244, Main.rand.NextVector2Circular(0.5f,0.5f));
            //dust.velocity *= 0.1f;
            //dust.alpha = 100;
            if (!Parent.active)
                parentActive = false;
            if (!Player.channel && !released)
            {
                released = true;
                Vector2 direction = projectile.Center - Player.Center;
                direction.Normalize();
                projectile.velocity = direction * 15;

            }
            //if (released)
            //{
            //    projectile.scale = MathHelper.Lerp(projectile.scale, 1, 0.08f);
            //    if (parentActive)
            //    {
            //        Vector2 direction = Parent.Center - projectile.Center;
            //        if (direction.Length() > distToFlail)
            //        {
            //            distToFlail = direction.Length();
            //            direction.Normalize();
            //            projectile.velocity = Vector2.Lerp(projectile.velocity, direction * 15, 0.06f);
            //        }
            //    }
            //}
            Vector2 toCenter = Player.Center;
            if (released)
                toCenter = Parent.Center;
            else
            {
                projectile.timeLeft = 180;
            }
            angleProgression += orbitSpeed * 0.5f;
            angleProgression %= 6.28f;
            Vector2 offset = Vector2.UnitX.RotatedBy(Angle) * (float)Math.Sin(angleProgression) * orbitDistance;
            projectile.scale = 1 + ((float)Math.Cos(angleProgression) / 2f);
            if (projectile.scale < 1)
                projectile.scale = (projectile.scale + 1) / 2;
            Vector2 goToPos = toCenter + offset;
            goToPos -= projectile.Center;
            float speed = projectile.velocity.Length() + 1.0f;
            float dist = goToPos.Length();
            if (speed > dist)
            {
                speed = dist;
            }
            projectile.velocity = speed * goToPos.SafeNormalize(Vector2.Zero);
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!inFront)
                Draw(spriteBatch, Color.White);
            return false;
        }
        public void Draw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = ModContent.GetTexture("SOTS/Assets/Glow");
            Vector2 origin = texture.Size() / 2;
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                float scale = projectile.scale * (0.2f + 0.8f * (projectile.oldPos.Length - k) / projectile.oldPos.Length);
                if (k != 0) scale *= 0.3f;
                else scale *= 0.4f;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + Main.projectileTexture[projectile.type].Size() / 3f + new Vector2(0, projectile.gfxOffY + 2);
                float lerpPercent = (float)k / projectile.oldPos.Length;
                Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), lerpPercent);
                Color color = colorMan * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * scale;
                spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), null, lightColor, projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2, projectile.scale, SpriteEffects.None, 0);
        }
    }
}