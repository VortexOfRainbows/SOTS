using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.NPCs.Boss.Curse;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Pyramid;

namespace SOTS.Projectiles.Pyramid
{    
    public class FriendlyShadeSpear : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		}
        public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 34;
			Projectile.friendly = true;
			Projectile.timeLeft = 240;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.tileCollide = true;
			Projectile.hide = false;
			Projectile.extraUpdates = 1;
			Projectile.DamageType = ModContent.GetInstance<Void.VoidMagic>();
		}
		int bounces = 0;
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
				Projectile.velocity.X = -oldVelocity.X;
			if (Projectile.velocity.Y != oldVelocity.Y)
				Projectile.velocity.Y = -oldVelocity.Y;
			bounces++;
			if (bounces > 3)
				Projectile.Kill();
			return false;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (projHitbox.Intersects(targetHitbox))
				return true;
			for (int k = 0; k < Projectile.oldPos.Length - 4; k++)
			{
				float scale = 1f - 0.5f * (k / (float)Projectile.oldPos.Length - 4);
				Vector2 oldPos = Projectile.oldPos[k];
				int width = (int)(24 * scale);
				Rectangle hitBox = new Rectangle((int)oldPos.X + Projectile.width/2 - width/2, (int)oldPos.Y + Projectile.height / 2 - width / 2, width, width);
				if (hitBox.Intersects(targetHitbox))
					return true;
			}
			return false;
        }
        public override bool ShouldUpdatePosition()
        {
            return true;
        }
		public void TruePreDraw(SpriteBatch spriteBatch, int iterationValue)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			if(Projectile.timeLeft > 1 && Projectile.timeLeft < 239)
			{
				Vector2 oldPosition = Projectile.Center;
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] + drawOrigin;
					float newToOld = Vector2.Distance(drawPos, oldPosition) / drawOrigin.Y * 2;
					Color color = Color.White;
					float scale = 1f - 0.6f * (k / (float)Projectile.oldPos.Length);
					spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos - Main.screenPosition, new Rectangle(0, iterationValue * 34, 10, 34), color, Projectile.oldRot[k] + MathHelper.Pi / 2, drawOrigin, Projectile.scale * new Vector2(scale, 0.1f + 0.9f * newToOld), SpriteEffects.None, 0f);
					oldPosition = drawPos;
				}
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			TruePreDraw(Main.spriteBatch, 1);
			TruePreDraw(Main.spriteBatch, 0);
			return false;
		}
		bool RunOnce = true;
        public override void AI()
		{
			if (RunOnce)
			{
				RunOnce = false;
				SOTSUtils.PlaySound(SoundID.Item96, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.875f, 0.2f);
			}
			int target = Common.GlobalNPCs.SOTSNPCs.FindTarget_Basic(Projectile.Center, 300f, Projectile);
			if (target != -1)
			{
				float distance = (Main.npc[target].Center - Projectile.Center).Length();
				if (distance > 120)
				{
					var normal = (Main.npc[target].Center - Projectile.Center).SafeNormalize(Vector2.Zero);
					Projectile.velocity = Vector2.Lerp(Projectile.velocity, normal * (Projectile.velocity.Length() * 1.02f + 1.2f + Projectile.ai[0]), 0.0825f + Projectile.ai[0]);
				}
				else
                {
					Projectile.velocity *= 1.0325f;
                }
			}
			Projectile.velocity *= 1.0075f;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}
        public override void Kill(int timeLeft)
        {
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 dustPosition = Projectile.oldPos[k] + Projectile.Size / 2;
				for(int i = 0; i < 4; i++)
				{
					int num2 = Dust.NewDust(dustPosition - new Vector2(5), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>());
					Dust dust = Main.dust[num2];
					dust.color = new Color(200, 80, 80, 40);
					dust.noGravity = true;
					dust.fadeIn = 0.1f;
					dust.scale *= 1.25f;
					dust.alpha = Projectile.alpha;
					dust.velocity *= 0.5f;
					dust.velocity += Projectile.velocity * Main.rand.NextFloat(-0.2f, 0.3f);
				}
			}
		}
    }
}
		