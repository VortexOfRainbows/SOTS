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
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Pyramid.Aten
{
    public class Aten : BaseFlailItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aten");
            Tooltip.SetDefault("'The defunct god... now in flail form'");
        }
        public override void SafeSetDefaults()
        {
            item.Size = new Vector2(34, 30);
            item.damage = 20;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
            item.useTime = 30;
            item.useAnimation = 30;
            item.shoot = ModContent.ProjectileType<AtenProj>();
            item.shootSpeed = 16;
            item.knockBack = 4;
        }
    }
    public class AtenProj : BaseFlailProj
    {
        public AtenProj() : base(new Vector2(0.7f, 1.3f), new Vector2(0.5f, 2f), 2, 70, 8) { }

        public override void SetStaticDefaults() => DisplayName.SetDefault("Aten");

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
            Vector2 drawPos = projectile.Center - Main.screenPosition;
            Color color = new Color(255, 230, 138, 0);
            Texture2D tex = mod.GetTexture("Items/Pyramid/Aten/FlailBloom");
            spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale * 1.5f, SpriteEffects.None, 0f);
            return true;
        }
    }

    public class AtenStar : ModProjectile, IOrbitingProj
    {

        public bool inFront
        {
            get
            {
                return projectile.scale > 1;
            }
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

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.Size = new Vector2(12, 12);
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
            projectile.penetrate = 1;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            Dust.NewDustPerfect(projectile.Center, 244, Main.rand.NextVector2Circular(0.5f,0.5f));
            if (!Parent.active)
                parentActive = false;
            if (!Player.channel && !released)
            {
                released = true;
                Vector2 direction = projectile.Center - Player.Center;
                direction.Normalize();
                projectile.velocity = direction * 15;

            }
            if (released)
            {
                projectile.scale = MathHelper.Lerp(projectile.scale, 1, 0.08f);
                if (parentActive)
                {
                    Vector2 direction = Parent.Center - projectile.Center;
                    if (direction.Length() > distToFlail)
                    {
                        distToFlail = direction.Length();
                        direction.Normalize();
                        projectile.velocity = Vector2.Lerp(projectile.velocity, direction * 15, 0.06f);
                    }
                }

            }
            else
            {
                projectile.timeLeft = 180;
                angleProgression += orbitSpeed;
                angleProgression %= 6.28f;
                Vector2 offset = Vector2.UnitX.RotatedBy(Angle) * (float)Math.Sin(angleProgression) * orbitDistance;
                projectile.scale = 1 + ((float)Math.Cos(angleProgression) / 2f);
                if (projectile.scale < 1)
                    projectile.scale = (projectile.scale + 1) / 2;
                projectile.Center = Player.Center + offset;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!inFront)
                Draw(spriteBatch, Color.White);
            return false;
        }
        public void Draw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), null,
                             lightColor, projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2, projectile.scale, SpriteEffects.None, 0);
        }
    }
}