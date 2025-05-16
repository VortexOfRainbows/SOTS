using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.AbandonedVillage
{
	public class ExcavatorSaw : ModProjectile
	{
        public override void SendExtraAI(BinaryWriter writer)
        {
            base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            base.ReceiveExtraAI(reader);
        }
        public int Timer = 0;
        public static Color Color => new Color(251, 129, 13, 0);
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override void SetDefaults()
		{
			Projectile.width = 48;
			Projectile.height = 48;
			Projectile.hostile = true;
			Projectile.friendly = false;
			Projectile.timeLeft = 360;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
            Projectile.ignoreWater = true;
            Projectile.hide = true;
		}
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindNPCsAndTiles.Add(index);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float width = Projectile.width * Projectile.scale;
			float height = Projectile.width * Projectile.scale;
			width += 2;
			height += 2;
			hitbox = new Rectangle((int)(Projectile.Center.X - width/2), (int)(Projectile.Center.Y - height/2), (int)width, (int)height);
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Color color = ExcavatorOrb.Color * 0.5f;
            SpriteEffects s = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            for (int k = 0; k < 7; k++)
            {
                Vector2 circular = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
                Main.spriteBatch.Draw(texture, drawPos + circular - Main.screenPosition, null, color * 0.8f, Projectile.rotation, drawOrigin, Projectile.scale * 1.0f, s, 0f);
            }
            Main.spriteBatch.Draw(texture, drawPos - Main.screenPosition, null, Lighting.GetColor(Projectile.Center.ToTileCoordinates()), Projectile.rotation, drawOrigin, Projectile.scale * 1.0f, s, 0f);
            float deathPercent = 1 - Projectile.timeLeft / 360f;
            deathPercent = deathPercent * deathPercent * deathPercent;
            for (int k = 0; k < 7; k++)
            {
                Vector2 circular = new Vector2(2 + 2 * (1 - deathPercent), 0).RotatedBy(MathHelper.ToRadians(k * 60 + Main.GameUpdateCount));
                Main.spriteBatch.Draw(texture, drawPos + circular - Main.screenPosition, null, color * 0.1f * deathPercent, Projectile.rotation, drawOrigin, Projectile.scale * 1.0f, s, 0f);
            }
            return false;
        }
		private bool runOnce = true;
        public Vector2 drawPos = Vector2.Zero;
        public float direction = 0;
        public bool hasCollidedOnce = false;
        public override void AI()
		{
			if(runOnce)
            {
                drawPos = Projectile.Center;
                directionY = 1;
                Projectile.ai[0] = 1f;
                if(!Projectile.velocity.HasNaNs())
                    direction = SOTSUtils.SignNoZero(Projectile.velocity.X);
                Projectile.alpha = 0;
                runOnce = false;
			}
            drawPos = Vector2.Lerp(drawPos, Projectile.Center, 0.5f);
            float speed = MathF.Abs(Projectile.velocity.X);
            //Projectile.velocity *= 0.9975f;
            //Projectile.velocity += gravity * 0.1f;
            if (speed > 0.1f && !hasCollidedOnce)
            {
                Projectile.spriteDirection = MathF.Sign(Projectile.velocity.X);
            }
            else
            {
                Projectile.spriteDirection = (int)(direction * directionY * directionFlip);
            }
            Projectile.rotation += Projectile.spriteDirection * (0.05f + MathF.Sqrt(Projectile.velocity.Length()) * 0.05f);
            Timer++;
            if (hasCollidedOnce)
            {
                BlazingWheelAI();
            }
            else
            {
                speedModifier = Projectile.velocity.Length() / 2f;
                Projectile.velocity.Y += 0.09f;
            }
        }
        public bool collideX;
        public bool collideY;
        public float directionY;
        public float speedModifier = 6;
        public float directionFlip = 1;
        public void BlazingWheelAI()
        {
            if(hasCollidedOnce)
            {
                speedModifier *= 1.01f;
                speedModifier += 0.04f;
                speedModifier = MathHelper.Clamp(speedModifier, 0, 10);
            }
            if (Projectile.ai[1] == 0f)
            {
                if (collideY)
                {
                    Projectile.ai[0] = 2f;
                }
                if (!collideY && Projectile.ai[0] == 2f)
                {
                    direction = -direction;
                    Projectile.ai[1] = 1f;
                    Projectile.ai[0] = 1f;
                    directionFlip *= -1;
                }
                if (collideX)
                {
                    directionY = -directionY;
                    Projectile.ai[1] = 1f;
                    directionFlip *= -1;
                }
            }
            else
            {
                if (collideX)
                {
                    Projectile.ai[0] = 2f;
                }
                if (!collideX && Projectile.ai[0] == 2f)
                {
                    directionY = -directionY;
                    Projectile.ai[1] = 0f;
                    Projectile.ai[0] = 1f;
                    directionFlip *= -1;
                }
                if (collideY)
                {
                    direction = -direction;
                    Projectile.ai[1] = 0f;
                    directionFlip *= -1;
                }
            }
            Projectile.velocity.X = speedModifier * direction;
            Projectile.velocity.Y = speedModifier * directionY;
            Vector2 toRealPosition = Projectile.Center - drawPos;
            Vector2 perpendicular = -new Vector2(Projectile.velocity.X, Projectile.velocity.Y) * Main.rand.NextFloat(0.5f, 1.2f);
            Vector2 perpEdge = Projectile.Center + new Vector2(Main.rand.NextFloat(-0.6f, 0.6f) * Projectile.width, 8 * directionFlip).RotatedBy(toRealPosition.ToRotation() + (direction * directionY == -1 ? MathF.PI : 0));
            for(int i = 0; i < 2; i++)
            {
                PixelDust.Spawn(perpEdge, 0, 0, perpendicular + Main.rand.NextVector2Circular(1, 1), ExcavatorOrb.Color, 12).scale = Main.rand.NextFloat(0.5f, 1f);

            }
            collideX = collideY = false;     
        }
        public override void OnKill(int timeLeft)
        {
            DoDust(0.9f, 1);
        }
        public void DoDust(float scaleFactor = 1f, float dir = 1)
        {
            for (int i = 0; i < 360; i += 12)
            {
                //Vector2 circularLocation = new Vector2(Main.rand.NextFloat(10), 0).RotatedBy(MathHelper.ToRadians(i) + Projectile.rotation);
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Silver);
                d.velocity += Projectile.velocity * Main.rand.NextFloat() * Main.rand.NextFloat();
            }
            for(int i = 0; i < 20; ++i)
            {
                Vector2 circular = Main.rand.NextVector2Circular(24, 24);
                PixelDust.Spawn(Projectile.Center + circular, 0, 0, Projectile.velocity * Main.rand.NextFloat() * Main.rand.NextFloat() + circular.SNormalize() * Main.rand.NextFloat(2, 4) + Main.rand.NextVector2Circular(1, 1), ExcavatorOrb.Color, 6).scale = Main.rand.NextFloat(1, 2);
            }
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 8;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (oldVelocity.X != Projectile.velocity.X)
                collideX = true;
            if (oldVelocity.Y != Projectile.velocity.Y)
                collideY = true;
            hasCollidedOnce = true;
            return false;
        }
    }
}