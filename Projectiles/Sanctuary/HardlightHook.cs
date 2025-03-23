using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Sanctuary
{
    public class HardlightHook : ModProjectile
    {
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.WriteVector2(Target);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            Target = reader.ReadVector2();
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = height = 14;
            return true;
        }
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.SingleGrappleHook[Type] = true;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
            ProjectileID.Sets.TrailingMode[Type] = 0;
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.GemHookDiamond);
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 1000000;
        }
        public override float GrappleRange()
        {
            return 480f; //30 distance
        }
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed += 4f;
        }
        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed += 4f;
        }
        public override bool PreDrawExtras()
        {
            return false;
        }
        public override bool? GrappleCanLatchOnTo(Player player, int x, int y)
        {
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            float velo = Projectile.velocity.Length() + 2;
            float distToTarget = Projectile.Center.Distance(Target);
            if(Projectile.ai[0] == 2 || mountedCenter.Distance(Projectile.Center) + velo > GrappleRange() || distToTarget < velo)
            {
                return true;
            }
            return null;
        }
        private int initialDirection = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = SOTSUtils.WhitePixel;

            Color color = new Color(122, 243, 255, 0) * 0.6f;
            
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            float actualDist = Projectile.Center.Distance(mountedCenter);
            Vector2 position = Projectile.Center;
            Vector2 backOffset = new Vector2(0, MathF.Min(9, actualDist)).RotatedBy(Projectile.rotation);
            Vector2 endPosition = position + backOffset;
            Vector2 origin = new Vector2(0, 1);
            Vector2 prev = mountedCenter;
            float distToPlayer = mountedCenter.Distance(endPosition);
            float total = distToPlayer / 10f;

            for (int i = 1; i <= total + 1; ++i)
            {
                float percent = MathF.Min(1, i / total);
                float cosScale = 0.5f + 0.5f * MathF.Cos(MathF.PI * i * 0.4f + SOTSWorld.GlobalCounter * 0.15f) * MathF.Sin(percent * MathF.PI);
                Vector2 circular = new Vector2(12 * cosScale * MathF.Sin(MathHelper.ToRadians(i * 60 + SOTSWorld.GlobalCounter * 5)), 0).RotatedBy(Projectile.rotation);
                Vector2 inBetweenPoints = Vector2.Lerp(mountedCenter, endPosition, percent);
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector2 prevToThis = inBetweenPoints + circular * j - prev;
                    float length = prevToThis.Length();
                    Main.spriteBatch.Draw(texture, prev - Main.screenPosition, null, color, prevToThis.ToRotation(), origin, new Vector2(length / 2f, 1 + cosScale), SpriteEffects.None, 0f);
                    prev = inBetweenPoints + circular * j;
                }
            }

            float rotateSpeed = 12;
            float timeLeftMult = MathF.Min(Projectile.timeLeft / 50f, 1);
            int min = Math.Max(0, 50 - Projectile.timeLeft);
            float expansion = Math.Min(1, Projectile.localAI[2] / 20f);
            for (int a = -1; a <= 1; a += 2)
            {
                Vector2 previous = Projectile.Center;
                for (int i = min; i < Projectile.oldPos.Length; i++)
                {
                    if (Projectile.oldPos[i] == Vector2.Zero)
                        break;
                    float dist = (2 + i - min) * expansion;
                    float perc = 1 - i / (float)Projectile.oldPos.Length;
                    Vector2 offset = new Vector2(dist * a, dist * a).RotatedBy(MathHelper.ToRadians((Projectile.localAI[2] - i) * rotateSpeed * initialDirection));
                    Vector2 center = Projectile.oldPos[i] + Projectile.Size / 2 + offset + backOffset;
                    if (i != min)
                    {
                        float scale = 1f + perc;
                        Vector2 toPrev = previous - center;
                        float length = toPrev.Length() / 2f;
                        Main.spriteBatch.Draw(SOTSUtils.WhitePixel, center - Main.screenPosition, null, color * perc * timeLeftMult * Projectile.localAI[1] * 1.5f, toPrev.ToRotation(),
                            origin, new Vector2(length, scale), SpriteEffects.None, 0f);
                    }
                    previous = center;
                }
            }

            position = Projectile.Center - Main.screenPosition;
            texture = ModContent.Request<Texture2D>(Texture + "Outline").Value;
            origin = texture.Size() / 2;
            color = new Color(100, 100, 110, 0);
            Texture2D texture2 = ModContent.Request<Texture2D>(Texture + "Fill").Value;
            for (int k = 0; k < 5; k++)
            {
                float x = Main.rand.Next(-10, 11) * 0.03f;
                float y = Main.rand.Next(-10, 11) * 0.03f;
                if (k == 0)
                    Main.spriteBatch.Draw(texture2, position, null, color * 0.5f, Projectile.rotation - MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None, 0f);
                Main.spriteBatch.Draw(texture, position + new Vector2(x, y), null, color, Projectile.rotation - MathHelper.PiOver4, origin, Projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }
        private Vector2 Target = Vector2.Zero;
        public override void AI()
        {
            Color color = new Color(122, 243, 255, 0);
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            if(Target == Vector2.Zero)
            {
                if(Projectile.owner == Main.myPlayer)
                    Target = Main.MouseWorld;
                Projectile.netUpdate = true;
            }
            Projectile.spriteDirection = 1;
            if(initialDirection == 0)
                initialDirection = Math.Sign(Projectile.velocity.X);
            Projectile.localAI[1] = 1;
            ++Projectile.localAI[2];
            if (Projectile.ai[0] == 0 && Projectile.localAI[2] > 1) //grapple traveling outward
            {
                float percent = MathF.Min(1, Projectile.localAI[2] / 40f);
                Dust d = PixelDust.Spawn(Projectile.Center - Projectile.velocity, 0, 0, Main.rand.NextVector2Circular(3, 3), color, 12);
                d.scale = Main.rand.NextFloat(1.25f, 1.5f);
            }
            if (Projectile.ai[0] == 2 && Projectile.localAI[0] != -1)
            {
                for(int i = 0; i < 30; i++)
                {
                    Dust d = PixelDust.Spawn(Projectile.Center - Projectile.velocity, 0, 0, Main.rand.NextVector2Circular(8, 8), color, 12);
                    d.scale = Main.rand.NextFloat(1.5f, 2f);
                }
                Projectile.localAI[0] = -1;
            }
        }
    }
}