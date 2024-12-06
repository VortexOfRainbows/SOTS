using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Permafrost
{    
    public class ShatterShard : ModProjectile 
    {
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Type].Value;
            Color color = Projectile.GetAlpha(new Color(60, 60, 125, 0));
            Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for(int i = 0; i < 4; i++)
			{
				Vector2 offset = new Vector2(1.5f, 0).RotatedBy(i * MathHelper.PiOver2);
                Main.EntitySpriteDraw(texture, Projectile.Center + offset - Main.screenPosition, null, color, Projectile.rotation, drawOrigin, 1.1f, SpriteEffects.None, 0f);
            }
            Main.EntitySpriteDraw(texture, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(Color.Lerp(lightColor, Color.White, 0.5f)), Projectile.rotation, drawOrigin, 1f, SpriteEffects.None, 0f);
            return false;
        }
        private bool RunOnce = true;
        public override void SetDefaults()
        {
			Projectile.height = 12;
			Projectile.width = 12;
			Projectile.penetrate = 8;
			Projectile.timeLeft = 1800;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.localNPCHitCooldown = 20;
			Projectile.usesLocalNPCImmunity = true;
            Projectile.friendly = true;
            Projectile.tileCollide = false;
            Projectile.hostile = false;
			Projectile.alpha = 35;
        }
		public override bool PreAI()
		{
			if (RunOnce)
			{
				SOTSUtils.PlaySound(SoundID.Item50, Projectile.Center, 0.8f, 0.9f, 0.04f);
				Projectile.ai[0] = 96;
				RunOnce = false;
            }
			Projectile.rotation += MathF.PI / 24f;
			return true;
		}
		public override void AI()
		{
			Player player  = Main.player[Projectile.owner];
			if(player.dead)
			{
				Projectile.Kill();
			}
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (Projectile.timeLeft < 1740)
			{
				Vector2 toPlayer = player.Center - Projectile.Center;
				float distance = toPlayer.Length();
				if (distance < 128)
				{
					bool found = false;
					int ofTotal = 0;
					int total = 0;
					for (int i = 0; i < Main.projectile.Length; i++)
					{
						Projectile proj = Main.projectile[i];
						if (Projectile.type == proj.type && proj.active && Projectile.active && proj.owner == Projectile.owner)
						{
							if (Projectile.whoAmI == i)
								found = true;
							if (!found)
								ofTotal++;
							total++;
						}
					}
					if (ofTotal >= 50)
					{
						Projectile.Kill();
					}
					Vector2 rotateCenter = new Vector2(Projectile.ai[0], 0).RotatedBy(MathHelper.ToRadians(modPlayer.orbitalCounter + (ofTotal * 360f / total)));
					rotateCenter += player.Center;
					Vector2 toRotate = rotateCenter - Projectile.Center;
					float dist2 = toRotate.Length();
                    Projectile.velocity *= 0.71f;
					if(dist2 < 2)
                    {
						Projectile.Center = rotateCenter;
					}
					else
					{
                        Projectile.velocity += toRotate.SNormalize() * dist2 * 0.075f + player.velocity * 0.2f;
                    }
				}
				else
				{
					float speed = distance * 0.09f;
					if (speed > 5)
						speed = 5;
					Projectile.velocity *= 0.8125f;
					Projectile.velocity += toPlayer.SNormalize() * speed + player.velocity * 0.2f;
				}
			}
			else
				Projectile.velocity *= 0.94f;
		}
		public override void OnKill(int timeLeft)
		{
			SOTSUtils.PlaySound(SoundID.Item50, Projectile.Center, 0.9f, 1.1f, 0.04f);
			for (int i = 0; i < 9; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.position - new Vector2(5), Projectile.width, Projectile.height, ModContent.DustType<ModIceDust>());
                dust.noGravity = true;
				dust.scale = 1.35f;
				dust.velocity *= 1.12f;
			}
		}
	}
}
		