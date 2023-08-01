using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using SOTS.Dusts;
using System;

namespace SOTS.Projectiles.Anomaly
{    
    public class SkipArrow : ModProjectile
	{
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Anomaly/SkipArrow");
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Color.White, Projectile.rotation, drawOrigin, Projectile.scale, Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}
		public override void SetDefaults()
        {
			Projectile.CloneDefaults(ProjectileID.WoodenArrowFriendly);
			Projectile.DamageType = ModContent.GetInstance<Void.VoidRanged>();
			Projectile.light *= 0.25f;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
			Projectile.width = 16;
			Projectile.height = 16;
			Projectile.arrow = true;
			Projectile.scale = 0.75f;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 8;
			height = 8;
            return true;
        }
        public override void Kill(int timeLeft)
		{
			for(int i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(9, 9), 8, 8, ModContent.DustType<PixelDust>(), 0, 0);
				d.velocity *= 1.5f;
				d.velocity += Projectile.velocity * 0.4f;
				d.fadeIn = 8f;
				d.noGravity = true;
				d.scale *= 1f;
				d.color = ColorHelpers.VoidAnomaly;
				d.color.A = 0;
			}
			base.Kill(timeLeft);
        }
        public override void AI()
		{
			Projectile.spriteDirection = Projectile.direction;
			Projectile.rotation = Projectile.velocity.ToRotation() + 1.57f;
			for(float i = 0; i < 1; i += 0.2f)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(5, 5) + Projectile.velocity * i, 0, 0, ModContent.DustType<PixelDust>(), 0, 0);
				d.velocity *= 0.05f;
				d.fadeIn = 12f;
				d.noGravity = true;
				d.scale *= 1f;
				d.color = ColorHelpers.VoidAnomaly;
				d.color.A = 0;
			}
			if(Projectile.tileCollide)
				Projectile.velocity.Y += 0.1f;
			for(int i = 0; i < Main.npc.Length; i++)
            {
				NPC npc = Main.npc[i];
				if(npc.active && npc.CanBeChasedBy() && npc.Distance(Projectile.Center) < 480)
                {
					if(npc.position.Y > Projectile.Center.Y - 16)
                    {
						if(Math.Abs(npc.Center.X - Projectile.Center.X) < npc.width / 2 + 12 && Collision.CanHitLine(npc.Center - new Vector2(4, 4), 8, 8, Projectile.Center - new Vector2(4, 4), 8, 8))
                        {
							TeleportToNPC(npc);	
                        }
                    }
                }
            }
		}
		public void TeleportToNPC(NPC target)
		{
			for (int i = 0; i < 20; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.Center - new Vector2(9, 9), 8, 8, ModContent.DustType<PixelDust>(), 0, 0);
				d.velocity *= 2.0f;
				d.fadeIn = 8f;
				d.noGravity = true;
				d.scale *= 1f;
				d.color = ColorHelpers.VoidAnomaly;
				d.color.A = 0;
			}
			Vector2 targetPos = new Vector2(target.Center.X, target.position.Y);
			float dist = (targetPos - Projectile.Center).Length();
			for(int i = 0; i < dist; i += 4)
            {
				float percent = i / dist;
				Vector2 position = Vector2.Lerp(Projectile.Center, targetPos, percent);
				if(!Main.rand.NextBool(3))
				{
					Dust d = Dust.NewDustDirect(position - new Vector2(5, 5), 0, 0, ModContent.DustType<PixelDust>(), 0, 0);
					d.velocity *= 0.25f;
					d.fadeIn = 8f;
					d.noGravity = true;
					d.scale *= 1.0f;
					d.color = ColorHelpers.VoidAnomaly;
					d.color.A = 0;
				}
                else
				{
					Dust d = Dust.NewDustDirect(position - new Vector2(5, 5), 0, 0, ModContent.DustType<CopyDust4>(), 0, 0);
					d.velocity *= 0.25f;
					d.fadeIn = 0.2f;
					d.noGravity = true;
					d.scale *= 1.25f;
					d.color = ColorHelpers.VoidAnomaly;
					d.color.A = 0;
				}
			}
			Projectile.Center = targetPos;
			Projectile.velocity *= 0.05f;
			Projectile.velocity += new Vector2(0, 2f);
			Projectile.tileCollide = false;
        }
	}
}
		
			