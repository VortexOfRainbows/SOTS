using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.BaseWeapons;
using SOTS.Items.Pyramid.Aten;
using SOTS.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Pyramid.Aten
{
    public class AtenProj : BaseFlailProj
    {
        public AtenProj() : base(new Vector2(0.5f, 1.5f), new Vector2(1f, 1f), 1.5f, 60, 10) { }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 48;
            hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override void OnLaunch(Player player)
        {
            projectile.velocity *= 0.85f;
        }
        public override void SetStaticDefaults() => DisplayName.SetDefault("Aten");
        public override void SetDefaults()
        {
            projectile.Size = new Vector2(26, 32);
            projectile.friendly = true;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.localNPCHitCooldown = 15;
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
        }
        int summonedNum = 0;
        public override void SpinExtras(Player player)
        {
            if (projectile.localAI[0] == 0)
            {
                SOTS.primitives.CreateTrail(new AtenPrimTrail(projectile));
            }
            if (projectile.localAI[0] % 24 == 0 && summonedNum < 9) //prevent spawning more in multiplayer with Main.myPlayer == projectile.owner
            {
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.8f, -0.15f);
                if (Main.myPlayer == projectile.owner)
                {
                    Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<AtenStar>(), (int)(projectile.damage * 0.7f) + 1, 0, projectile.owner, summonedNum, projectile.identity); //use identity since it aids with server syncing (.whoAmI is client dependent)
                }
                summonedNum++;
            }
            projectile.localAI[0]++;
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
            Texture2D tex = mod.GetTexture("Assets/FlailBloom");
            spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale * 1.50f, SpriteEffects.None, 0f);
            tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, drawPos, null, lightColor, projectile.rotation, new Vector2(tex.Width / 2, 10), projectile.scale * 1.25f, SpriteEffects.None, 0f); //putting origin on center of ball instead of on spike + ball
            return false;
        }
    }
    public class AtenStar : ModProjectile, IOrbitingProj
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(orbitalSpeed);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            orbitalSpeed = reader.ReadSingle();
        }
        public bool inFront
        {
            get => projectile.scale > 1;
            set
            {

            }
        }
        private Player Player => Main.player[projectile.owner];
        Projectile pastParent = null;
        private Projectile Parent()
        {
            Projectile parent = pastParent;
            if (parent != null && parent.active && parent.owner == projectile.owner && parent.type == ModContent.ProjectileType<AtenProj>() && parent.identity == (int)(projectile.ai[1] + 0.5f)) //this is to prevent it from iterating the loop over and over
            {
                return parent;
            }
            else
                parent = null;
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<AtenProj>() && proj.identity == (int)(projectile.ai[1] + 0.5f)) //use identity since it aids with server syncing (.whoAmI is client dependent)
                {
                    parent = proj;
                    break;
                }
            }
            pastParent = parent;
            return parent;
        }
        private float Angle => Parent().localAI[0] * 0.02f + additionalCounter * 0.01f;

        bool released = false;

        bool parentActive = true;
        float orbitalSpeed = 1;
        float orbitalDistance = 0;
        float angleProgression = 0;
        public float orbitNum
        {
            get => projectile.ai[0];
            set => projectile.ai[0] = value;
        }
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
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.idStaticNPCHitCooldown = 30;
            projectile.usesIDStaticNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        bool runOnce = true;
        public void DelayEnd() //so that trails can disappear properly
        {
            if (Main.myPlayer == projectile.owner)
            {
                Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<AtenStarExplosion>(), projectile.damage * 3, 0, projectile.owner, projectile.scale);

            }
            if (projectile.timeLeft > 30)
                projectile.timeLeft = 30;
            projectile.velocity *= 0f;
            orbitalDistance = -1;
            projectile.netUpdate = true;
            projectile.friendly = false;
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                orbitalDistance = Main.rand.NextFloat(70, 90);
                projectile.netUpdate = true;
                runOnce = false;
            }
            if(orbitalDistance == -1)
            {
                projectile.velocity *= 0f;
                projectile.friendly = false;
            }
            return orbitalDistance != -1;
        }
        float additionalCounter = 0;
        public override void AI()
        {
            if (Parent() == null)
                parentActive = false;
            else if (!Player.channel && !released)
            {
                released = true;
            }
            Vector2 toCenter = Player.Center;
            if (parentActive)
            {
                if (released)
                {
                    if (Main.rand.NextBool(10))
                        Dust.NewDustPerfect(projectile.Center, 244, Main.rand.NextVector2Circular(0.5f, 0.5f));
                    additionalCounter++;
                    toCenter = Parent().Center;
                    if (!Parent().tileCollide && additionalCounter > 4) //added counter here to counteract pre-emptive exploding due to place in projectile array
                    {
                        DelayEnd();
                        return;
                    }
                }
                else
                {
                    projectile.timeLeft = 180;
                }
                float endSin = (float)Math.Sin(1.5f * additionalCounter * MathHelper.Pi / 180);
                angleProgression = MathHelper.ToRadians((SOTSPlayer.ModPlayer(Player).orbitalCounter + additionalCounter * 1.5f) * 2.5f + (orbitNum % 3) * 120 + (int)(orbitNum / 3) * 30);
                Vector2 offset = Vector2.UnitX.RotatedBy(Angle + MathHelper.ToRadians((int)(orbitNum / 3) * 120)) * (float)Math.Sin(angleProgression) * (orbitalDistance + additionalCounter * 1.25f * endSin);
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
            else
                DelayEnd();
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
            int length = projectile.oldPos.Length;
            int end = 0;
            if(length > projectile.timeLeft)
            {
                end = length - projectile.timeLeft;
            }
            for (int k = length - 1; k >= end; k--)
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
            if(orbitalDistance != -1)
                spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY), null, new Color(255, 255, 255, 100), projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2, projectile.scale * 0.9f, SpriteEffects.None, 0);
        }
    }
}