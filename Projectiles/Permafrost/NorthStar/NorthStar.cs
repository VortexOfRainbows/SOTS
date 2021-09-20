using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.BaseWeapons;
using SOTS.Prim.Trails;
using SOTS.Utilities;
using SOTS.Void;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost.NorthStar
{
    public class NorthStar : BaseFlailProj
    {
        public NorthStar() : base(new Vector2(0.2f, 2.1f), new Vector2(1f, 1f), 2f, 80, 12) { }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 64;
            hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 32;
            height = 32;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void OnLaunch(Player player)
        {
            projectile.velocity *= 0.85f;
        }
        public override void SetStaticDefaults() => DisplayName.SetDefault("North Star");
        public override void SetDefaults()
        {
            projectile.Size = new Vector2(50, 68);
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
                SOTS.primitives.CreateTrail(new NorthStarTrail(projectile));
            }
            if (projectile.localAI[0] % 18 == 0 && summonedNum < 8) //prevent spawning more in multiplayer with Main.myPlayer == projectile.owner
            {
                Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 30, 0.8f, -0.15f);
                if (Main.myPlayer == projectile.owner)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<NorthStarStar>(), (int)(projectile.damage * 0.7f) + 1, 0, projectile.owner, summonedNum + i * 8, projectile.identity); //use identity since it aids with server syncing (.whoAmI is client dependent)
                    }
                }
                summonedNum++;
            }
            projectile.localAI[0]++;
            Lighting.AddLight(projectile.Center, new Color(150, 180, 240).ToVector3());
        }
        public override void NotSpinningExtras(Player player)
        {
            Lighting.AddLight(projectile.Center, new Color(150, 180, 240).ToVector3());
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY);
            Color color = new Color(150, 180, 240, 0) * 0.5f;
            Texture2D tex = mod.GetTexture("Assets/FlailBloom");
            spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, projectile.scale * 2.0f, SpriteEffects.None, 0f);
            tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, drawPos, null, Color.White, projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), projectile.scale * 0.9f, SpriteEffects.FlipVertically, 0f); //putting origin on center of ball instead of on spike + ball
            return false;
        }
    }
    public class NorthStarStar : ModProjectile, IOrbitingProj
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(orbitalSpeed);
            writer.Write(orbitalDistance);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            orbitalSpeed = reader.ReadSingle();
            orbitalDistance = reader.ReadSingle();
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
            if (parent != null && parent.active && parent.owner == projectile.owner && parent.type == ModContent.ProjectileType<NorthStar>() && parent.identity == (int)(projectile.ai[1] + 0.5f)) //this is to prevent it from iterating the loop over and over
            {
                return parent;
            }
            else
                parent = null;
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == projectile.owner && proj.type == ModContent.ProjectileType<NorthStar>() && proj.identity == (int)(projectile.ai[1] + 0.5f)) //use identity since it aids with server syncing (.whoAmI is client dependent)
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
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 36;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.melee = true;
            projectile.Size = new Vector2(20, 20);
            projectile.tileCollide = false;
            projectile.timeLeft = 180;
            projectile.penetrate = -1;
            projectile.extraUpdates = 1;
            projectile.localNPCHitCooldown = 100; //actually 50 because of extraupdates
            projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 0;
            if(released)
            {
                DelayEnd();
            }
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
                Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ModContent.ProjectileType<NorthStarExplosion>(), projectile.damage * 3, 0, projectile.owner, projectile.scale);

            }
            if (projectile.timeLeft > 36)
                projectile.timeLeft = 36;
            projectile.velocity *= 0f;
            orbitalDistance = -1;
            projectile.friendly = false;
            projectile.netUpdate = true;
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                orbitalDistance = Main.rand.NextFloat(120, 160);
                projectile.netUpdate = true;
                runOnce = false;
            }
            if(orbitalDistance == -1)
            {
                parentActive = false;
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
            if (parentActive && orbitalDistance != -1)
            {
                if (released)
                {
                    if (Main.rand.NextBool(9))
                    {
                        Color colorMan = Color.Lerp(new Color(240, 250, 255, 100), new Color(200, 250, 255, 100), Main.rand.NextFloat(1));
                        Dust dust = Dust.NewDustPerfect(projectile.Center, ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(0.5f, 0.5f));
                        dust.color = colorMan;
                        dust.noGravity = true;
                        dust.fadeIn = 0.1f;
                        dust.scale *= 1.1f;
                        dust.alpha = 180;
                    }
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
                angleProgression = MathHelper.ToRadians((SOTSPlayer.ModPlayer(Player).orbitalCounter + additionalCounter * 1.5f) * 1.75f + (orbitNum % 4) * 90 + (int)(orbitNum / 4) * 22.5f);
                Vector2 offset = Vector2.UnitX.RotatedBy(Angle + MathHelper.ToRadians((int)(orbitNum / 4) * 45)) * (float)Math.Sin(angleProgression) * (orbitalDistance + additionalCounter * 1.25f * endSin);
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
                //Color colorR = VoidPlayer.pastelAttempt(MathHelper.ToRadians((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length * 300f + VoidPlayer.soulColorCounter * 2)) * 1f;
                float scale = projectile.scale * (0.25f + 0.75f * (projectile.oldPos.Length - k) / projectile.oldPos.Length);
                if (k != 0) scale *= 0.3f;
                else scale *= 0.4f;
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + projectile.Size / 2f + new Vector2(0, projectile.gfxOffY);
                float lerpPercent = (float)Math.Sin(MathHelper.ToRadians((float)k / projectile.oldPos.Length * 300f + VoidPlayer.soulColorCounter * 1.5f));
                Color colorMan = Color.Lerp(new Color(150, 180, 240), new Color(190, 10, 75), lerpPercent);
                Color color = colorMan * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * scale;
                spriteBatch.Draw(texture, drawPos, null, color, projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            if(orbitalDistance != -1)
            {
                for(int i = 0; i < 6; i++)
                {
                    Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60));
                    Vector2 drawPos = projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY) + Main.rand.NextVector2Circular(0.5f, 0.5f) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 60)) * 2;
                    spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, new Color(color.R, color.G, color.B, 0) * 0.3f, projectile.rotation, Main.projectileTexture[projectile.type].Size() / 2, projectile.scale * 0.75f, SpriteEffects.None, 0);
                }
            }
        }
    }
}