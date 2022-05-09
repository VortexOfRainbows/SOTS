using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using SOTS.Dusts;

namespace SOTS.Projectiles.Pyramid
{    
    public class PhantomFist : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Fist");
		}
        public override void SetDefaults()
        {
			Projectile.aiStyle = 0;
			Projectile.melee = true;
			Projectile.friendly = true;
			Projectile.width = 44;
			Projectile.height = 28;
			Projectile.timeLeft = 70;
			Projectile.penetrate = 6;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.alpha = 40;
            Main.projFrames[Projectile.type] = 5;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 25;
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Projectile.localNPCImmunity[target.whoAmI] = Projectile.localNPCHitCooldown;
			target.immune[Projectile.owner] = 0;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, Projectile.height / 2);
			Color color = Color.Black;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 circular = new Vector2(Main.rand.NextFloat(3.5f, 5), 0).RotatedBy(MathHelper.ToRadians(i));
				color = new Color(150, 100, 200, 0);
				Main.spriteBatch.Draw(texture, Projectile.Center + circular - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color * ((255f - Projectile.alpha) / 255f), Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			}
			color = new Color(48 + 100, 0 + 100, 108 + 100);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, new Rectangle(0, Projectile.height * Projectile.frame, Projectile.width, Projectile.height), color, Projectile.rotation, origin, Projectile.scale, SpriteEffects.None, 0.0f);
			return false;
		}
		public override void AI()
		{ 
			Projectile.rotation = (float)Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X);
			Projectile.velocity *= 0.99f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter = 0;
                Projectile.frame = (Projectile.frame + 1) % 5;
            }
			Projectile.alpha += 3;		
			float minDist = 560;
			int target2 = -1;
			float dX = 0f;
			float dY = 0f;
			float distance = 0;
			float speed = 0.25f;
			if(Projectile.friendly == true && Projectile.hostile == false)
			{
				for(int i = 0; i < Main.npc.Length; i++)
				{
					NPC target = Main.npc[i];
					if(!target.friendly && target.dontTakeDamage == false && target.lifeMax > 5 && target.active && target.CanBeChasedBy())
					{
						dX = target.Center.X - Projectile.Center.X;
						dY = target.Center.Y - Projectile.Center.Y;
						distance = (float) Math.Sqrt((double)(dX * dX + dY * dY));
						if(distance < minDist)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
				
				if(target2 != -1)
				{
					NPC toHit = Main.npc[target2];
					if(toHit.active == true)
					{
						dX = toHit.Center.X - Projectile.Center.X;
						dY = toHit.Center.Y - Projectile.Center.Y;
						distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
						speed /= distance;
					   
						Projectile.velocity += new Vector2(dX * speed, dY * speed);
					}
				}
			}
			if(Main.rand.NextBool(3))
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 0.2f;
				dust.velocity -= 2 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(150, 100, 200, 0);
				dust.alpha = 100;
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 32; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
				dust.velocity *= 1.2f;
				dust.velocity += 5 * Projectile.velocity.SafeNormalize(Vector2.Zero);
				dust.scale *= 2;
				dust.noGravity = true;
				dust.fadeIn = 0.2f;
				dust.color = new Color(150, 100, 200, 0);
				dust.alpha = 100;
			}
			base.Kill(timeLeft);
		}
	}
}
		