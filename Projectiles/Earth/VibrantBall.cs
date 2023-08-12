using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System.IO;
using SOTS.Dusts;
using Terraria.ID;

namespace SOTS.Projectiles.Earth 
{    
    public class VibrantBall : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Vibrant Ball");
		}
        public override void SetDefaults()
        {
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.width = 34;
			Projectile.height = 34;
			Projectile.alpha = 0;
			Projectile.timeLeft = 40;
		}
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(Projectile.tileCollide);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			Projectile.tileCollide = reader.ReadBoolean();
        }
        public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for(int i = 0; i < 2; i++)
			{
				int direction = i * 2 - 1;
				for (int k = 0; k < 10; k++)
				{
					if(trailPos[k] != Vector2.Zero && trailCircular[k] != -100 && (trailPos[k] != Projectile.Center))
					{
						Vector2 circularPos = trailPos[k] + new Vector2(0, trailCircular[k] * direction).RotatedBy(Projectile.rotation);
						Color color = ColorHelpers.VibrantColorAttempt((90 + k * 3) * direction);
						Vector2 drawPos = circularPos - Main.screenPosition;
						color = Projectile.GetAlpha(color);
						for (int j = 0; j < 2; j++)
						{
							float x = Main.rand.Next(-10, 11) * 0.10f;
							float y = Main.rand.Next(-10, 11) * 0.10f;
							Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, Projectile.scale - k * 0.1f, SpriteEffects.None, 0f);
						}
					}
				}
			}
			return false;
		}
		bool runOnce = true;
		public void cataloguePos()
		{
			Vector2 current = Projectile.Center;
			float current2 = Projectile.ai[0];
			for (int i = 0; i < trailPos.Length; i++)
			{
				Vector2 previousPosition = trailPos[i];
				trailPos[i] = current;
				current = previousPosition;
				float previousPosition2 = trailCircular[i];
				trailCircular[i] = current2;
				current2 = previousPosition2;
			}
		}
		public override bool PreAI()
		{
			if (runOnce)
			{
				for (int i = 0; i < trailPos.Length; i++)
				{
					trailPos[i] = Vector2.Zero;
					trailCircular[i] = -100;
				}
				runOnce = false;
			}
			return true;
		}
		Vector2[] trailPos = new Vector2[10];
		float[] trailCircular= new float[10];
		float dist = 24f;
		public void Explode()
		{
			SOTSUtils.PlaySound(SoundID.Item28, (int)Projectile.Center.X, (int)Projectile.Center.Y, 0.6f);
			for (int i = 0; i < 30; i++)
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, ModContent.DustType<CopyDust4>());
				Dust dust = Main.dust[num2];
				Color color2 = ColorHelpers.VibrantColorAttempt(0) * 0.75f;
				dust.color = color2;
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.75f;
				dust.alpha = 50;
				dust.velocity *= 0.6f;
				dust.velocity -= Projectile.velocity * 0.3f;
			}
			if (Main.myPlayer == Projectile.owner)
			{
				for (int j = 0; j < 3; j++)
				{
					for (int i = 2; i < 7; i++)
					{
						Vector2 toReach = new Vector2(i * 48, (j - 1) * 24).RotatedBy(Projectile.velocity.ToRotation());
						Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center.X, Projectile.Center.Y, toReach.X / 20f, toReach.Y / 20f, ModContent.ProjectileType<VibrantBolt>(), Projectile.damage, 0, Projectile.owner);
					}
				}
			}
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			UpdateEnd();
			return false;
		}
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
			target.immune[Projectile.owner] = 0;
			UpdateEnd();
        }
        public void UpdateEnd()
		{
			if (Projectile.timeLeft > 10)
				Projectile.timeLeft = 11;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			if (Main.myPlayer == Projectile.owner)
				Projectile.netUpdate = true;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 20;
			height = 20;
			return true;
        }
        public override void AI()
		{
			if(Projectile.timeLeft > 10)
			{
				Projectile.velocity *= 0.94f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				Projectile.ai[1]++;
				dist *= 0.99f;
				Projectile.scale += 0.025f;
				Vector2 circular = new Vector2(dist, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1] * 12));
				Projectile.ai[0] = circular.Y;
				cataloguePos();
			}
			else if(Projectile.timeLeft == 10)
            {
				Explode();
			}
			else
			{
				Projectile.velocity *= 0.0f;
				cataloguePos();
			}
		}
	}
}
		
			