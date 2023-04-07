using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Earth;
using System;
using SOTS.Void;

namespace SOTS.Projectiles.Blades
{    
    public class ColossusSpawnAnimation : ModProjectile 
    {
        public override bool PreDraw(ref Color lightColor)
        {
			DrawIncomingCircle();
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[ItemID.BreakerBlade].Value;
			Texture2D textureColossus = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
			float compressMult = Projectile.ai[1] / 180f;
			for (int i = 0; i < 3; i++)
			{
				Vector2 circular = new Vector2((1 - compressMult) * 32, 0).RotatedBy(MathHelper.ToRadians(i * 120f + compressMult * 180));
				Main.spriteBatch.Draw(texture, circular + Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(100, 100, 100, 0) * (float)Math.Pow(compressMult, 2), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(textureColossus, circular.RotatedBy(MathHelper.ToRadians(60)) + Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), null, new Color(100, 100, 100, 0) * (float)Math.Pow(compressMult, 3), Projectile.rotation, textureColossus.Size() / 2, 1f, SpriteEffects.None, 0f);
			}
			return false;
        }
		public void DrawIncomingCircle()
		{
			Vector2 crystalCenter = new Vector2(Projectile.Center.X - 5, Projectile.ai[0] - 7);
			float percent = (Projectile.ai[1] - 20) / 130f;
			if(percent <= 1 && percent > 0)
				ConduitHelper.DrawConduitCircleFull(crystalCenter, percent, new Color(180, 230, 100, 0));
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawn Colossus");
		}
        public override void SetDefaults()
		{
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.timeLeft = 180;
			Projectile.friendly = false;
			Projectile.hostile = false;
			Projectile.scale = 1.00f;
			Projectile.tileCollide = false;
			Projectile.alpha = 255;
			Projectile.rotation = -3 * MathHelper.PiOver2;
		}
		bool runOnce = true;
		public override bool PreAI()
		{
			Vector2 crystalCenter = new Vector2(Projectile.Center.X - 5, Projectile.ai[0] - 7);
			if (Projectile.alpha > 0)
			{
				Projectile.alpha -= 3;
			}
			if (Projectile.alpha < 0)
				Projectile.alpha = 0;
			Projectile.ai[1]++;
			float speedMult = 1f - (float)Math.Sqrt(Projectile.ai[1] / 170f);
			if(speedMult > 0)
				Projectile.velocity = new Vector2(0, -speedMult * 4f);
			Projectile.rotation *= 0.99f;
			Projectile.rotation += 0.011f;
			if (Projectile.rotation > 0)
				Projectile.rotation = 0;
			if (Projectile.ai[1] >= 150)
			{
				float percent = (Projectile.ai[1] - 150) / 30f;
				Dust dust;
				if (runOnce)
				{
					SOTSUtils.PlaySound(SoundID.Item30, Projectile.Center, 0.7f, 0.2f);
					for (int a = 0; a < 32; a++)
					{
						Vector2 outward = new Vector2(0, 8f).RotatedBy(MathHelper.TwoPi * a / 32f);
						dust = Dust.NewDustDirect(crystalCenter + outward.SafeNormalize(Vector2.Zero) * 16 - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, new Color(180, 230, 100, 0), 1.1f);
						dust.scale *= 1.5f;
						dust.velocity *= 0.6f;
						dust.velocity += outward / dust.scale;
						dust.fadeIn = 0.1f;
						dust.noGravity = true;
					}
				}
				Vector2 crystalToCenter = Vector2.Lerp(crystalCenter, Projectile.Center, percent);
				dust = Dust.NewDustDirect(crystalToCenter - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, new Color(180, 230, 100, 0), 1.1f);
				dust.scale *= 1.45f;
				dust.velocity *= 0.3f;
				dust.fadeIn = 0.1f;
				dust.noGravity = true;
				runOnce = false;
			}
			return true;
		}
        public override void Kill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item30, Projectile.Center, 1f, -0.3f);
			for(int j = 1; j <= 2; j++)
			{
				for (int a = 0; a < 64; a++)
				{
					Vector2 outward = new Vector2(0, 8f * j).RotatedBy(MathHelper.TwoPi * a / 32f);
					Dust dust = Dust.NewDustDirect(Projectile.Center + outward.SafeNormalize(Vector2.Zero) * 16 - new Vector2(4, 4), 0, 0, ModContent.DustType<Dusts.AlphaDrainDust>(), 0, 0, 0, new Color(180, 230, 100, 0), 1.1f);
					dust.scale *= 1f + 0.6f * (1.5f - j * 0.5f);
					dust.velocity *= 0.7f;
					dust.velocity += outward / dust.scale;
					dust.fadeIn = 0.1f;
					dust.noGravity = true;
				}
			}
			if (Main.netMode != NetmodeID.MultiplayerClient)
            {
				Item.NewItem(Projectile.GetSource_FromThis(), Projectile.position, new Vector2(32, 32), ModContent.ItemType<Colossus>());
            }
        }
    }
}
		