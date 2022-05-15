using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Flails;
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
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 32;
            height = 32;
            return true;
        }
        public override void OnLaunch(Player player)
        {
            Projectile.velocity *= 0.85f;
        }
        public override void SetStaticDefaults() => DisplayName.SetDefault("North Star");
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(50, 68);
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 15;
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
        }
        int summonedNum = 0;
        public override void SpinExtras(Player player)
        {
            if (Projectile.localAI[0] == 0)
            {
                SOTS.primitives.CreateTrail(new NorthStarTrail(Projectile));
            }
            if (Projectile.localAI[0] % 18 == 0 && summonedNum < 8) //prevent spawning more in multiplayer with Main.myPlayer == Projectile.owner
            {
                SoundEngine.PlaySound(SoundID.Item, (int)Projectile.Center.X, (int)Projectile.Center.Y, 30, 0.8f, -0.15f);
                if (Main.myPlayer == Projectile.owner)
                {
                    for(int i = 0; i <= 1; i++)
                    {
                        Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<NorthStarStar>(), (int)(Projectile.damage * 0.7f) + 1, 0, Projectile.owner, summonedNum + i * 8, Projectile.identity); //use identity since it aids with server syncing (.whoAmI is client dependent)
                    }
                }
                summonedNum++;
            }
            Projectile.localAI[0]++;
            Lighting.AddLight(Projectile.Center, new Color(150, 180, 240).ToVector3());
        }
        public override void NotSpinningExtras(Player player)
        {
            Lighting.AddLight(Projectile.Center, new Color(150, 180, 240).ToVector3());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Color color = new Color(150, 180, 240, 0) * 0.5f;
            Texture2D tex = Mod.Assets.Request<Texture2D>("Assets/FlailBloom").Value;
            spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale * 2.0f, SpriteEffects.None, 0f);
            tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            spriteBatch.Draw(tex, drawPos, null, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, tex.Height / 2), Projectile.scale * 0.9f, SpriteEffects.FlipVertically, 0f); //putting origin on center of ball instead of on spike + ball
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
            get => Projectile.scale > 1;
            set
            {

            }
        }
        private Player Player => Main.player[Projectile.owner];
        Projectile pastParent = null;
        private Projectile Parent()
        {
            Projectile parent = pastParent;
            if (parent != null && parent.active && parent.owner == Projectile.owner && parent.type == ModContent.ProjectileType<NorthStar>() && parent.identity == (int)(Projectile.ai[1] + 0.5f)) //this is to prevent it from iterating the loop over and over
            {
                return parent;
            }
            else
                parent = null;
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Projectile.owner && proj.type == ModContent.ProjectileType<NorthStar>() && proj.identity == (int)(Projectile.ai[1] + 0.5f)) //use identity since it aids with server syncing (.whoAmI is client dependent)
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
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 36;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.Size = new Vector2(20, 20);
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.localNPCHitCooldown = 100; //actually 50 because of extraupdates
            Projectile.usesLocalNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
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
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectileDirect(Projectile.Center, Vector2.Zero, ModContent.ProjectileType<NorthStarExplosion>(), Projectile.damage * 3, 0, Projectile.owner, Projectile.scale);

            }
            if (Projectile.timeLeft > 36)
                Projectile.timeLeft = 36;
            Projectile.velocity *= 0f;
            orbitalDistance = -1;
            Projectile.friendly = false;
            Projectile.netUpdate = true;
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                orbitalDistance = Main.rand.NextFloat(120, 160);
                Projectile.netUpdate = true;
                runOnce = false;
            }
            if(orbitalDistance == -1)
            {
                parentActive = false;
                Projectile.velocity *= 0f;
                Projectile.friendly = false;
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
                        Dust dust = Dust.NewDustPerfect(Projectile.Center, ModContent.DustType<CopyDust4>(), Main.rand.NextVector2Circular(0.5f, 0.5f));
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
                    Projectile.timeLeft = 180;
                }
                float endSin = (float)Math.Sin(1.5f * additionalCounter * MathHelper.Pi / 180);
                angleProgression = MathHelper.ToRadians((SOTSPlayer.ModPlayer(Player).orbitalCounter + additionalCounter * 1.5f) * 1.75f + (orbitNum % 4) * 90 + (int)(orbitNum / 4) * 22.5f);
                Vector2 offset = Vector2.UnitX.RotatedBy(Angle + MathHelper.ToRadians((int)(orbitNum / 4) * 45)) * (float)Math.Sin(angleProgression) * (orbitalDistance + additionalCounter * 1.25f * endSin);
                Projectile.scale = 1 + ((float)Math.Cos(angleProgression) / 2f);
                if (Projectile.scale < 1)
                    Projectile.scale = (Projectile.scale + 1) / 2;
                Vector2 goToPos = toCenter + offset;
                goToPos -= Projectile.Center;
                float speed = Projectile.velocity.Length() + 1.0f;
                float dist = goToPos.Length();
                if (speed > dist)
                {
                    speed = dist;
                }
                Projectile.velocity = speed * goToPos.SafeNormalize(Vector2.Zero);
            }
            else
                DelayEnd();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (!inFront)
                Draw(spriteBatch, Color.White);
            return false;
        }
        public void Draw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Assets/Glow");
            Vector2 origin = texture.Size() / 2;
            int length = Projectile.oldPos.Length;
            int end = 0;
            if(length > Projectile.timeLeft)
            {
                end = length - Projectile.timeLeft;
            }
            for (int k = length - 1; k >= end; k--)
            {
                //Color colorR = VoidPlayer.pastelAttempt(MathHelper.ToRadians((float)(Projectile.oldPos.Length - k) / Projectile.oldPos.Length * 300f + VoidPlayer.soulColorCounter * 2)) * 1f;
                float scale = Projectile.scale * (0.25f + 0.75f * (Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
                if (k != 0) scale *= 0.3f;
                else scale *= 0.4f;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY);
                float lerpPercent = (float)Math.Sin(MathHelper.ToRadians((float)k / Projectile.oldPos.Length * 300f + VoidPlayer.soulColorCounter * 1.5f));
                Color colorMan = Color.Lerp(new Color(150, 180, 240), new Color(190, 10, 75), lerpPercent);
                Color color = colorMan * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * scale;
                spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            if(orbitalDistance != -1)
            {
                for(int i = 0; i < 6; i++)
                {
                    Color color = VoidPlayer.pastelAttempt(MathHelper.ToRadians(i * 60));
                    Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY) + Main.rand.NextVector2Circular(0.5f, 0.5f) + Vector2.UnitX.RotatedBy(MathHelper.ToRadians(i * 60)) * 2;
                    spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos, null, new Color(color.R, color.G, color.B, 0) * 0.3f, Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() / 2, Projectile.scale * 0.75f, SpriteEffects.None, 0);
                }
            }
        }
    }
}