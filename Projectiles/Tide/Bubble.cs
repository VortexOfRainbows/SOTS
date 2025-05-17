using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SOTS.Projectiles.Tide
{    
    public class Bubble : ModProjectile 
    {
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D t = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            int trueHeight = t.Height / Main.projFrames[Projectile.type];
            Vector2 origin = new Vector2(t.Width, 0) / 2;
            Vector2 stretch = new Vector2(1, 1);
            if (Projectile.ai[2] < 0 && Projectile.timeLeft > 15)
            {
                int pix = Projectile.ai[2] >= -7 ? 4 : 12;
                float target = pix / (float)trueHeight;
                stretch.Y -= target;
            }
            Main.EntitySpriteDraw(t, new Vector2(Projectile.Center.X, Projectile.position.Y) - Main.screenPosition, new Rectangle(0, trueHeight * Projectile.frame, t.Width, trueHeight), Color.Lerp(Color.White, lightColor, 0.9f), Projectile.rotation, origin, stretch, SpriteEffects.None, 0);
            return false;
        }
		public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 8;
        }
		public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
			Projectile.friendly = Projectile.hostile = false; 
			Projectile.timeLeft = 18000;
			Projectile.alpha = 40;
			Projectile.hide = true;
		}
		public override bool? CanDamage() => false;
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 18;
			height = 18;
            return true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(oldVelocity.Y > Projectile.velocity.Y && Projectile.ai[2] >= 0)
				return false; //if I was moving down and hit a block, dont break, as that means Im hitting the floor
			if (Projectile.timeLeft > 15)
			{
				Projectile.timeLeft = 15;
			}
			return false;
        }
		private bool runOnce = true;
		public override bool PreAI()
		{
			if (runOnce)
			{
                for (int i = 0; i < Main.projectile.Length; i++) //Kill all other bubbles owned by this player when I am spawned
                {
                    Projectile other = Main.projectile[i];
                    if (other != Projectile && other.active && other.type == Type && other.owner == Projectile.owner && other.timeLeft > 15)
                    {
						other.timeLeft = 15;
                    }
                }
				Main.player[Main.myPlayer].itemRotation = 0;
				Projectile.scale = 0.1f;
                Projectile.hide = false;
				runOnce = false;
            }
			return true;
		}
        public override void AI()
        {
			Color c = Color.Lerp(new Color(95, 205, 228), Color.White, Main.rand.NextFloat() * Main.rand.NextFloat());
			c.A = 0;
			if(Main.rand.NextBool(5))
				PixelDust.Spawn(Projectile.position, Projectile.width, Projectile.height, -Projectile.velocity * Main.rand.NextFloat(0.5f) + Main.rand.NextVector2Square(-0.2f, 0.2f), c, -10).scale = Main.rand.NextFloat(0.5f, 1.0f);
            Projectile.scale = MathF.Min(1.0f, Projectile.scale * 1.02f + 0.05f);
			if (Projectile.ai[2] >= 0) //ToDo: Skip if standing on bubble
			{
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -1, 0.05f);
				float sin = MathF.Sin(MathHelper.ToRadians(Projectile.ai[1])) * 0.05f;
				Projectile.velocity.X += sin;
                Projectile.velocity.X *= 0.92f;
				Projectile.ai[1] += 2.4f;
            }
			else
            {
                float fall = Projectile.ai[2] / 7f;
                Projectile.velocity.X *= 0.1f;
				Projectile.velocity.Y = MathHelper.Lerp(Projectile.velocity.Y, -fall, 0.05f);
				if (Projectile.ai[2] <= -35)
					Projectile.timeLeft = Projectile.timeLeft > 15 ? 15 : Projectile.timeLeft;
				else
                {
                    ++Projectile.ai[2];
                }
            }
            Projectile.ai[0]++;
			if(Projectile.timeLeft <= 15)
            {
                if(Projectile.frame == 0)
                {
                    for (int i = 0; i < 12; ++i)
                    {
                        c = Color.Lerp(new Color(95, 205, 228), Color.White, Main.rand.NextFloat() * Main.rand.NextFloat());
                        c.A = 0;
                        Vector2 circ = new Vector2(3, 0).RotatedBy(i * MathF.PI / 6f);
                        PixelDust.Spawn(Projectile.Center, 0, 0, -Projectile.velocity * Main.rand.NextFloat(0.3f) + Main.rand.NextVector2Square(-0.2f, 0.2f) + circ, c, -10).scale = Main.rand.NextFloat(0.5f, 1.0f);
                    }
                    Terraria.Audio.SoundEngine.PlaySound(SoundID.Item54, Projectile.Center);
                }
                Projectile.velocity *= 0.75f;
                if (Projectile.ai[0] >= 0 && Projectile.frame < 5)
                {
                    Projectile.frame++;
                    Projectile.ai[0] = -3;
                }
            }
        }
		public override void OnKill(int timeLeft)
		{
        }
	}
}
			