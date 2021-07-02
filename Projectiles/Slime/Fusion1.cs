using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Slime
{    
    public class Fusion1 : ModProjectile 
    {	
		int worm = 0;
		int wait = 1;
		Vector2 oldVelocity;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helix Bolt");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = projectile.GetAlpha(color) * ((projectile.oldPos.Length - k) / (float)projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!projectile.oldPos[k].Equals(projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, projectile.rotation, drawOrigin, (projectile.oldPos.Length - k) / (float)projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
		}
		public override void SetDefaults()
        {
			projectile.CloneDefaults(14);
            aiType = 14; 
			projectile.height = 10;
			projectile.width = 10;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 900;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.magic = false;
			projectile.ranged = true;
			projectile.extraUpdates = 3;
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(end);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			end = reader.ReadBoolean();
		}
		bool end = false;
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 0;
			end = true;
			projectile.velocity *= 0;
			if(projectile.owner == Main.myPlayer)
				projectile.netUpdate = true;
			base.OnHitNPC(target, damage, knockback, crit);
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			end = true;
			projectile.velocity *= 0;
			if (projectile.owner == Main.myPlayer)
				projectile.netUpdate = true;
			return false;
        }
        public override void AI()
		{
			if (projectile.timeLeft <= 30)
				end = true;
			if (end)
			{
				projectile.friendly = false;
				if (projectile.timeLeft > 30)
					projectile.timeLeft = 30;
			}
			if (end || Main.rand.NextBool(18))
			{
				int num2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(250, 100, 190, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (projectile.timeLeft / 60f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
				return;
			}
			if (wait == 1)
			{
				wait++;
				oldVelocity = projectile.velocity;
			}
			worm++;
			if(worm <= 60)
			{
				projectile.velocity.X += oldVelocity.Y / 30f;
				projectile.velocity.Y += -oldVelocity.X / 30f;
			}
			else if(worm >= 61 && worm <= 120)
			{
				projectile.velocity.X += -oldVelocity.Y / 30f;
				projectile.velocity.Y += oldVelocity.X / 30f;
			}
			else
				worm = 0;
		}
	}
}
		