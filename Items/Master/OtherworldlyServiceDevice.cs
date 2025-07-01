using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs.Pet;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Master
{
    public class OtherworldlyServiceDevice : ModItem
    {
        public override void SetDefaults()
        {
            Item.damage = 0;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<Boopy>();
            Item.width = 22;
            Item.height = 24;
            Item.UseSound = SoundID.Item2;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.LightPurple;
            Item.master = true;
            Item.noMelee = true;
            Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.buffType = ModContent.BuffType<BeepBoop>();
        }
        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }
    }
    public class Boopy : ModProjectile
    {
        private static Texture2D glow = null;
        private static Texture2D vineTexture = null;
        private static Texture2D vineGlow = null;
        public float directionFollower;
        public float directionFollower2;
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            Main.projPet[Projectile.type] = true;
            ProjectileID.Sets.LightPet[Projectile.type] = true;
        }
        public override void SetDefaults()
        {
            Projectile.netImportant = true;
            Projectile.width = 38;
            Projectile.height = 48;
            Projectile.timeLeft = 255;
            Projectile.penetrate = -1;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Player owner = Main.player[Projectile.owner];
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            glow ??= ModContent.Request<Texture2D>("SOTS/Items/Master/BoopyGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            vineGlow ??= ModContent.Request<Texture2D>("SOTS/Items/Master/BoopyVineGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            vineTexture ??= ModContent.Request<Texture2D>("SOTS/Items/Master/BoopyVine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Vector2 origin = vineTexture.Size() / 2;
            Vector2 start = owner.MountedCenter + new Vector2(-4 * directionFollower2, 0); 
            Vector2 start2 = owner.MountedCenter + new Vector2(-10 * directionFollower2, -30); 
            Vector2 end = Projectile.Center + new Vector2(-60 * directionFollower, 50).RotatedBy(Projectile.rotation);
            Vector2 end2 = Projectile.Center + new Vector2(-18 * directionFollower, 2).RotatedBy(Projectile.rotation);
            Vector2 previous = Projectile.Center;
            for (int i = 0; i <= 24; ++i)
            {
                float percent = 1 - i / 24f;
                float rad = MathHelper.ToRadians(percent * 540 + Projectile.ai[0] * 1.5f);
                Vector2 dynamicAddition = new Vector2(0.5f + 2 * MathF.Sin(percent * MathF.PI), 0).RotatedBy(rad);
                Vector2 p = SOTS.CalculateBezierPoint(percent, start, start2, end, end2) + dynamicAddition;
                Vector2 toPrev = previous - p;
                float r = toPrev.ToRotation() - MathHelper.PiOver4;
                Main.EntitySpriteDraw(vineTexture, p - Main.screenPosition, null, Lighting.GetColor(p.ToTileCoordinates()), r, origin, Projectile.scale, SpriteEffects.None, 0f);
                for (int h = 0; h < 3; ++h)
                {
                    Vector2 circular = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(h * 120 + Projectile.ai[0]));
                    float sin = MathF.Sin(rad) * 0.5f + 0.5f;
                    sin *= sin;
                    Main.EntitySpriteDraw(vineGlow, p + circular - Main.screenPosition, null, new Color(200, 200, 200, 0) * sin, r, origin, Projectile.scale, SpriteEffects.None, 0f);
                }
                previous = p;
            }
            origin = texture.Size() / 2;
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            for(int i = 0; i < 4; ++i)
            {
                Vector2 circular = new Vector2(0.5f, 0).RotatedBy(MathHelper.ToRadians(i * 90 + Projectile.ai[0]));
                Main.EntitySpriteDraw(glow, Projectile.Center + circular - Main.screenPosition, null, new Color(20, 20, 20, 0), Projectile.rotation, origin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            }
            return false;
        }
        public override bool PreAI()
        {
            Lighting.AddLight(Projectile.Center, Helpers.ColorHelper.PurpleOtherworldColor.ToVector3() + Vector3.One * 0.15f);
            Player owner = Main.player[Projectile.owner];
            if(owner.HasBuff<BeepBoop>())
            {
                Projectile.timeLeft = 100;
            }
            else
            {
                Projectile.Kill();
            }
            Vector2 idlePosition = owner.MountedCenter + new Vector2(-36 * owner.direction, -76 * owner.gravDir);
            Vector2 dynamicAddition = new Vector2(0.015f, 0).RotatedBy(MathHelper.ToRadians(++Projectile.ai[0] * 0.7f));
            Vector2 toIdle = idlePosition - Projectile.Center;
            float dist = toIdle.Length();
            float speed = Projectile.velocity.Length();
            bool turnLikePlayer = true;
            if(dist > 2000)
            {
                Projectile.Center = idlePosition;
            }
            else if (dist > 36)
            {
                float baseSpeed = MathF.Min(dist, 7f + dist * 0.045f);
                float inertia = 0.05f;
                Projectile.velocity *= 1 - inertia;
                Projectile.velocity += toIdle.SNormalize() * baseSpeed * inertia;
                if (speed > dist)
                {
                    Projectile.velocity *= dist / speed;
                }
                turnLikePlayer = false;
            }
            else
            {
                Projectile.velocity *= 0.93f;
                Projectile.velocity += toIdle.SNormalize() * 0.008f;
            }
            Projectile.velocity += dynamicAddition;
            if (speed > 1f)
            {
                float veloR = new Vector2(Projectile.velocity.X * Projectile.spriteDirection, Projectile.velocity.Y * Projectile.spriteDirection).ToRotation();
                veloR *= 0.6f;
                Projectile.rotation = SOTSUtils.AngularLerp(Projectile.rotation, veloR, 0.06f);
                turnLikePlayer = false;
            }
            else
            {
                Projectile.rotation = SOTSUtils.AngularLerp(Projectile.rotation, 0, 0.1f);
            }
            if(turnLikePlayer)
            {
                Projectile.spriteDirection = owner.direction;
            }
            else
            {
                Projectile.spriteDirection = SOTSUtils.SignNoZero(Projectile.velocity.X);
            }
            directionFollower = MathHelper.Lerp(directionFollower, Projectile.spriteDirection, 0.07f);
            directionFollower2 = MathHelper.Lerp(directionFollower2, owner.direction, 0.07f);
            return true;
        }
    }
}