using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Flails;
using SOTS.Prim.Trails;
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
        public override void SetStaticDefaults() => DisplayName.SetDefault("Aten");
        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(34, 34);
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
                SOTS.primitives.CreateTrail(new AtenPrimTrail(Projectile));
            }
            if (Projectile.localAI[0] % 24 == 0 && summonedNum < 9) //prevent spawning more in multiplayer with Main.myPlayer == Projectile.owner
            {
                SOTSUtils.PlaySound(SoundID.Item30, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.8f, -0.15f);
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Vector2.Zero, ModContent.ProjectileType<AtenStar>(), (int)(Projectile.damage * 0.7f) + 1, 0, Projectile.owner, summonedNum, Projectile.identity); //use identity since it aids with server syncing (.whoAmI is client dependent)
                }
                summonedNum++;
            }
            Projectile.localAI[0]++;
            Lighting.AddLight(Projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public override void NotSpinningExtras(Player player)
        {
            Lighting.AddLight(Projectile.Center, new Color(255, 230, 138).ToVector3());
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 drawPos = Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY);
            Color color = new Color(255, 230, 138, 0);
            Texture2D tex = Mod.Assets.Request<Texture2D>("Assets/FlailBloom").Value;
            Main.spriteBatch.Draw(tex, drawPos, null, color, 0, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale * 1.50f, SpriteEffects.None, 0f);
            tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(tex, drawPos, null, lightColor, Projectile.rotation, new Vector2(tex.Width, tex.Height) / 2, Projectile.scale * 1.25f, SpriteEffects.None, 0f); //putting origin on center of ball instead of on spike + ball
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
            if (parent != null && parent.active && parent.owner == Projectile.owner && parent.type == ModContent.ProjectileType<AtenProj>() && parent.identity == (int)(Projectile.ai[1] + 0.5f)) //this is to prevent it from iterating the loop over and over
            {
                return parent;
            }
            else
                parent = null;
            for (short i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];
                if (proj.active && proj.owner == Projectile.owner && proj.type == ModContent.ProjectileType<AtenProj>() && proj.identity == (int)(Projectile.ai[1] + 0.5f)) //use identity since it aids with server syncing (.whoAmI is client dependent)
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
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.Size = new Vector2(12, 12);
            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.penetrate = -1;
            Projectile.extraUpdates = 1;
            Projectile.idStaticNPCHitCooldown = 30;
            Projectile.usesIDStaticNPCImmunity = true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[Projectile.owner] = 0;
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
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<AtenStarExplosion>(), (int)(Projectile.damage * 2), 0, Projectile.owner, Projectile.scale);
            }
            if (Projectile.timeLeft > 30)
                Projectile.timeLeft = 30;
            Projectile.velocity *= 0f;
            orbitalDistance = -1;
            Projectile.netUpdate = true;
            Projectile.friendly = false;
        }
        public override bool PreAI()
        {
            if (runOnce)
            {
                orbitalDistance = Main.rand.NextFloat(70, 90);
                Projectile.netUpdate = true;
                runOnce = false;
            }
            if(orbitalDistance == -1)
            {
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
                    if (Main.rand.NextBool(10))
                        Dust.NewDustPerfect(Projectile.Center, 244, Main.rand.NextVector2Circular(0.5f, 0.5f));
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
                angleProgression = MathHelper.ToRadians((SOTSPlayer.ModPlayer(Player).orbitalCounter + additionalCounter * 1.5f) * 2.5f + (orbitNum % 3) * 120 + (int)(orbitNum / 3) * 30);
                Vector2 offset = Vector2.UnitX.RotatedBy(Angle + MathHelper.ToRadians((int)(orbitNum / 3) * 120)) * (float)Math.Sin(angleProgression) * (orbitalDistance + additionalCounter * 1.25f * endSin);
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
                Draw(Main.spriteBatch, Color.White);
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
                float scale = Projectile.scale * (0.2f + 0.8f * (Projectile.oldPos.Length - k) / Projectile.oldPos.Length);
                if (k != 0) scale *= 0.3f;
                else scale *= 0.4f;
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + Projectile.Size / 2f + new Vector2(0, Projectile.gfxOffY);
                float lerpPercent = (float)k / Projectile.oldPos.Length;
                Color colorMan = Color.Lerp(new Color(255, 230, 140), new Color(180, 90, 20), lerpPercent);
                Color color = colorMan * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * scale;
                spriteBatch.Draw(texture, drawPos, null, color, Projectile.rotation, origin, scale, SpriteEffects.None, 0f);
            }
            if(orbitalDistance != -1)
                spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY), null, new Color(255, 255, 255, 100), Projectile.rotation, Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Size() / 2, Projectile.scale * 0.9f, SpriteEffects.None, 0);
        }
    }
}