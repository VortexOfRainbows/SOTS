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
        public static Texture2D vineTexture;
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
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            vineTexture ??= ModContent.Request<Texture2D>("SOTS/Items/Master/BoopyVine", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(lightColor), Projectile.rotation, texture.Size() / 2, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreAI()
        {
            Lighting.AddLight(Projectile.Center, Helpers.ColorHelper.PurpleOtherworldColor.ToVector3() + Vector3.One * 0.6f);
            Player owner = Main.player[Projectile.owner];
            if(owner.HasBuff<BeepBoop>())
            {
                Projectile.timeLeft = 100;
            }
            else
            {
                Projectile.Kill();
            }
            Vector2 idlePosition = owner.MountedCenter + new Vector2(-52 * owner.direction, -96 * owner.gravDir);
            Vector2 dynamicAddition = new Vector2(0.015f, 0).RotatedBy(MathHelper.ToRadians(++Projectile.ai[0] * 0.7f));
            Vector2 toIdle = idlePosition - Projectile.Center;
            float dist = toIdle.Length();
            float speed = Projectile.velocity.Length();
            bool turnLikePlayer = true;
            if (dist > 36)
            {
                float baseSpeed = MathF.Min(dist, 9f + dist * 0.0016f);
                float inertia = 0.05f;
                Projectile.velocity *= 1 - inertia;
                Projectile.velocity += toIdle.SNormalize() * baseSpeed * inertia;
                if (speed > baseSpeed)
                {
                    Projectile.velocity *= baseSpeed / speed;
                }
                turnLikePlayer = false;
            }
            else
            {
                Projectile.velocity *= 0.9f;
                Projectile.velocity += toIdle.SNormalize() * 0.01f;
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
            return true;
        }
    }
}