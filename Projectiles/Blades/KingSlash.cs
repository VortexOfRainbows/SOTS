using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Void;
using SOTS.Dusts;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Prim.Trails;
using SOTS.Prim;
using System.IO;
using SOTS.Projectiles.Laser;

namespace SOTS.Projectiles.Blades
{    
    public class KingSlash : SOTSBlade
    {
        public override Color? DrawColor => null;
        public static readonly Color Red = new(187, 11, 76, 0); 
        public static readonly Color SecondRed = new(202, 234, 247, 0);
        public static readonly Color Blue = new(64, 74, 204, 0);
        public static readonly Color SecondBlue = new(255, 221, 233, 0);
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 300);
        }
        private Color GetColors(int i, int j)
        {
            if (i == 1)
            {
                if(j == 0)
                    return Red * 0.9f;
                else
                    return SecondRed * 0.9f;
            }
            else if (j == 0)
                return Blue * 0.9f;
            else
                return SecondBlue * 0.9f;
        }
        public override Color color1 => GetColors(thisSlashNumber % 2, 0);
        public override Color color2 => GetColors(thisSlashNumber % 2, 1);
        public override void SafeSetDefaults()
        {
            Projectile.timeLeft = 7200;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.friendly = true;
            Projectile.localNPCHitCooldown = 11;
            Projectile.extraUpdates = 2;
        }
        public override void SwingSound(Player player)
        {
            SOTSUtils.PlaySound(SoundID.Item71, (int)player.Center.X, (int)player.Center.Y, 0.75f, 0.75f * Projectile.ai[1]);
        }
        public override float AdditionalTipLength => base.AdditionalTipLength;
        public override float HitboxWidth => 44f;
        public override Vector2 drawOrigin => new Vector2(12, 60);
        public override float ArmAngleOffset => 12;
        public override float MaxSwipeDistance => 180;
        public override float MinSwipeDistance => 180;
        public override float MeleeSpeedMultiplier => 0.75f;
        public override float GetBaseSpeed(float swordLength)
        {
			if((int)Math.Abs(Projectile.ai[0]) == 2)
                spinSpeed *= 0.6f;
            return 2.2f + (1.2f / (float)Math.Pow(swordLength / MaxSwipeDistance, 2f));
        }
        public override float ArcStartDegrees => 200 + 15f / speedModifier;
        public bool RunOnce = true;
        public override void PostAI()
        {
            base.PostAI();
            if(RunOnce)
            {
                if(Projectile.owner == Main.myPlayer)
                {
                    Player player = Main.player[Projectile.owner];
                    Vector2 spawnPos = new Vector2((float)(player.Center.X + (240f * -player.direction * Main.rand.Next(8, 13)) + (Main.mouseX + Main.screenPosition.X - player.position.X)), player.Center.Y - 26f * Main.rand.Next(8, 13));
                    spawnPos.Y += 0.5f * (player.Center.Y - Main.MouseWorld.Y);
                    Vector2 circularPos = new Vector2(196, 0).RotatedBy(Main.rand.NextFloat(MathF.PI * 2));
                    spawnPos += circularPos;
                    Vector2 speed = Main.MouseWorld - spawnPos;
                    speed = speed.SNormalize();
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, speed * 8f, ModContent.ProjectileType<LightspeedKingblade>(), Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer, 0.0f, Main.MouseWorld.X);
                    RunOnce = false;
                }
            }
        }
        public override void SlashPattern(Player player, int slashNumber)
        {
            int damage = Projectile.damage;
            if (slashNumber == 2)
            {
                damage = (int)(damage * 1.0f);
                Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, (Main.MouseWorld - player.Center).SafeNormalize(Vector2.Zero) * 9f, ModContent.ProjectileType<KingSlashThrow>(), (int)(damage * 0.8f), Projectile.knockBack, player.whoAmI, 0, FetchDirection);
            }
            else
            {
                float speedBonus = 0.1f;
                if (slashNumber == 3)
                    speedBonus = -0.4f;
                Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), player.Center, Projectile.velocity, Type, damage, Projectile.knockBack, player.whoAmI, -FetchDirection * slashNumber, Projectile.ai[1] + speedBonus);
                if (proj.ModProjectile is KingSlash a)
                    a.distance = distance;
            }
        }
        public override Vector2 ModifySwingVector2(Vector2 original, float yDistanceCompression, int swingNumber)
        {
            original.Y *= 0.85f / speedModifier * yDistanceCompression; //turn circle into an oval by compressing the y value
            if (swingNumber == 3)
                original.Y *= 1.1f;
            return original;
        }
        public override float swingSizeMult => 0.7f + 0.3f * Projectile.ai[1];
        public override float swipeDegreesTotal => 270.0f + (1800f / distance) + ((int)Math.Abs(Projectile.ai[0]) == 3 ? 10 : 0);
        public override void SpawnDustDuringSwing(Player player, float bladeLength, Vector2 bladeDirection)
        {
            if (dustAway != Vector2.Zero)
            {
                if(Main.rand.NextBool(1))
                {
                    float dustScale = 1f;
                    float rand = Main.rand.NextFloat(0.9f, 1.1f);
                    int type = SOTSUtils.TypeHelper.CopyDust4Type;
                    Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 12, Projectile.Center.Y - 12) - dustAway.SafeNormalize(Vector2.Zero) * 4, 16, 16, type);
                    dust.velocity *= 0.02f / rand;
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero) * 1.2f * rand;
                    dust.noGravity = true;
                    dust.scale *= 0.1f / rand;
                    dust.scale += 1.1f / rand * dustScale;
                    dust.fadeIn = 0.2f;
                    if (type == SOTSUtils.TypeHelper.CopyDust4Type)
                        dust.color = Color.Lerp(color1, color2, Main.rand.NextFloat(1f) * Main.rand.NextFloat(1f));
                }
                Vector2 toProjectile = Projectile.Center - player.RotatedRelativePoint(player.MountedCenter, true);
                for(int i = 0; i < 2; ++i)
                {
                    float percent = Main.rand.NextFloat(0.9f) * Main.rand.NextFloat();
                    Dust dust = PixelDust.Spawn(Projectile.Center - toProjectile.SafeNormalize(Vector2.Zero) * 4 - toProjectile * percent, 0, 0, Main.rand.NextVector2Circular(0.05f, 0.05f),
                        Color.Lerp(color1, color2, Main.rand.NextFloat(0.9f) * Main.rand.NextFloat(0.9f)), (int)(12 - percent * 4));
                    dust.velocity += dustAway.SafeNormalize(Vector2.Zero).RotatedBy(MathHelper.ToRadians(90 * FetchDirection)) * Main.rand.NextFloat(0.65f, 1f);
                    dust.scale = 1.5f - percent;
                }
            }
        }
        public override float TrailOffsetFromTip => 1f;
    }
    public class KingSlashThrow : ModProjectile
    {
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 300);
        }
        public override string Texture => "SOTS/Projectiles/Blades/KingSlash";
        private float rotation = 0;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(initialVelo);
            writer.WriteVector2(Projectile.velocity);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            initialVelo = reader.ReadVector2();
            Projectile.velocity = reader.ReadVector2();
        }
        public override void SetDefaults()
        {
            Projectile.height = 74;
            Projectile.width = 74;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.DamageType = ModContent.GetInstance<VoidMelee>();
            Projectile.timeLeft = 240;
            Projectile.tileCollide = true;
            Projectile.hostile = false;
            Projectile.alpha = 0;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 9;
            Projectile.extraUpdates = 2;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            int width = 196;
            hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
            return false;
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 24;
            height = 24;
            return true;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.HitDirectionOverride = initialDirection;
        }
        private bool runOnce = true;
        private Vector2 initialVelo;
        private Vector2 initialCenter;
        public int initialDirection = 0;
        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];
            if (runOnce)
            {
                if(player.whoAmI == Main.myPlayer)
                {
                    Projectile.ai[1] = Main.MouseWorld.X;
                    Projectile.ai[2] = Main.MouseWorld.Y;
                    Vector2 mouse = new Vector2(Projectile.ai[1], Projectile.ai[2]);
                    float toMouse = mouse.Distance(player.Center);
                    Projectile.ai[1] = toMouse;
                    Projectile.ai[2] = 0;
                }
                SOTSUtils.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)player.Center.X, (int)player.Center.Y, 1.6f, -0.1f);
                runOnce = false;
                initialVelo = Projectile.velocity;
                if (Projectile.velocity.X < 0)
                {
                    rotation = -MathHelper.ToRadians(90);
                    initialDirection = 1;
                }
                else
                {
                    rotation = 0;
                    initialDirection = -1;
                }
                initialCenter = player.Center;
                Projectile.ai[0] = -180 * initialDirection;
                Projectile.scale = 1.65f;
                if(Main.netMode != NetmodeID.Server)
                {
                    BladeTrail myTrail = new BladeTrail(Projectile, clockWise: initialDirection, KingSlash.Blue.ToVector4(), KingSlash.SecondBlue.ToVector4(), 40, 2);
                    SOTS.primitives.CreateTrail(myTrail);
                }
            }
            else if (Projectile.timeLeft % 20 == 0)
                SOTSUtils.PlaySound(SoundID.DD2_MonkStaffSwing, (int)Projectile.Center.X, (int)Projectile.Center.Y, 1.2f, 0.1f);
            player.itemAnimation = 3;
            player.itemTime = 3;
            player.itemRotation = MathHelper.WrapAngle((Projectile.Center - player.Center).ToRotation() + (initialDirection == -1 ? MathHelper.ToRadians(180) : 0));
            if (Projectile.ai[0] * initialDirection < -30)
            {
                Projectile.ai[0] += 2.4f * initialDirection;
                if ((int)(Projectile.ai[0] / 2.4f) % 5 == 0)
                    Projectile.netUpdate = true;
            }
            else
            {
                Projectile.ai[2]++;
                if (Projectile.ai[2] >= 12 && Projectile.timeLeft >= 90)
                {
                    Projectile.netUpdate = true;
                    Projectile.ai[2] = 0;
                    if (Projectile.owner == Main.myPlayer)
                    {
                        Vector2 spawnPos = Projectile.Center + new Vector2(1200, 0).RotatedByRandom(MathF.PI);
                        Vector2 circularPos = new Vector2(32, 0).RotatedByRandom(MathF.PI);
                        spawnPos += circularPos;
                        Vector2 speed = (Projectile.Center - spawnPos).SNormalize();
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), spawnPos, speed * 8f, ModContent.ProjectileType<LightspeedKingblade>(), Projectile.damage, Projectile.knockBack * 0.5f, Main.myPlayer, 0.0f, Projectile.Center.X);
                    }
                }
                Projectile.ai[0] += 0.6f * initialDirection;
            }
            if (Projectile.timeLeft >= 90)
            {
                Vector2 initialCenter = this.initialCenter;
                float length = MathF.Max(Projectile.ai[1] * 0.5f, 64);
                float multiplier = 64f / length;
                float rad = MathHelper.ToRadians(Projectile.ai[0]);
                Vector2 ovalArea = new Vector2(length, 0).RotatedBy(initialVelo.ToRotation());
                Vector2 ovalArea2 = new Vector2(length, 0).RotatedBy(rad);
                ovalArea2.Y *= -multiplier;
                ovalArea2 = ovalArea2.RotatedBy(initialVelo.ToRotation());
                ovalArea.X += ovalArea2.X;
                ovalArea.Y += ovalArea2.Y;
                Vector2 goTo = initialCenter + ovalArea;
                float dist = (Projectile.Center - goTo).Length();
                Vector2 circular = new Vector2(-(dist > 18 ? 18 : dist), 0).RotatedBy((Projectile.Center - goTo).ToRotation());
                Projectile.velocity = circular + new Vector2(0, -1.5f);
            }
            else
            {
                float dist = (Projectile.Center - player.Center).Length();
                float maxSpeed = 12 + (12 * (90 - Projectile.timeLeft) / 90f);
                Vector2 toPlayer = player.Center - Projectile.Center;
                Vector2 circular = toPlayer.SNormalize() * (dist > maxSpeed ? maxSpeed : dist);
                Projectile.velocity = circular;
                Projectile.tileCollide = false;
                if (toPlayer.LengthSquared() <= 144)
                {
                    Projectile.Kill();
                }
            }
            return base.PreAI();
        }
        public override void AI()
        {
            Lighting.AddLight(Projectile.Center, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 0.6f / 255f, (255 - Projectile.alpha) * 1.8f / 255f);
            Player player = Main.player[Projectile.owner];
            float incrementAmount = -initialDirection * MathHelper.ToRadians(16);
            rotation += incrementAmount;
            Projectile.rotation = rotation;
            Projectile.spriteDirection = initialDirection;

            for(float i = 0; i < 1; i += 0.34f)
            {
                Vector2 circular = new Vector2(80, 0).RotatedBy(Projectile.rotation + incrementAmount * i) * Main.rand.NextFloat(0.9f, 1.2f);
                int type = SOTSUtils.TypeHelper.CopyDust4Type;
                Dust dust = Dust.NewDustDirect(new Vector2(Projectile.Center.X - 4, Projectile.Center.Y - 4) + circular, 0, 0, type);
                dust.velocity *= 0.02f;
                dust.velocity += circular.SafeNormalize(Vector2.Zero) * 1.1f;
                dust.noGravity = true;
                dust.scale *= 0.1f;
                dust.scale += 1.0f;
                dust.fadeIn = 0.2f;
                if (type == SOTSUtils.TypeHelper.CopyDust4Type)
                    dust.color = Color.Lerp(KingSlash.Red, KingSlash.Blue, circular.X / 80f * 0.5f + 0.5f);
            }

            if (Projectile.timeLeft >= 90)
            {
                Vector2 postTileCollision = Collision.TileCollision(Projectile.Center - new Vector2(12, 12), Projectile.velocity, 24, 24, true);
                if (postTileCollision.X != 16 || Projectile.velocity.X == 16)
                    Projectile.Center += postTileCollision;
            }
            else
                Projectile.Center += Projectile.velocity;

            if (Main.netMode != NetmodeID.Server)
            {
                foreach (PrimTrail trail in SOTS.primitives._trails.ToArray())
                {
                    if (trail is BladeTrail fireTrail)
                    {
                        if (trail.Entity is Projectile proj && proj.whoAmI == Projectile.whoAmI && proj.type == Projectile.type && initialDirection == fireTrail.ClockWiseOrCounterClockwise)
                        {
                            trail.Update();
                        }
                    }
                }
            }
        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (runOnce)
                return true;
            Texture2D texture2 = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Texture2D textureGlow = ModContent.Request<Texture2D>("SOTS/Items/Permafrost/KingBladeGlow").Value;
            Main.spriteBatch.Draw(texture2, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation + (initialDirection == 1 ? MathHelper.PiOver2 : 0), new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, initialDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            Main.spriteBatch.Draw(textureGlow, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.White), Projectile.rotation + (initialDirection == 1 ? MathHelper.PiOver2 : 0), new Vector2(texture2.Width / 2, texture2.Height / 2), Projectile.scale, initialDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
    }
}
		
			