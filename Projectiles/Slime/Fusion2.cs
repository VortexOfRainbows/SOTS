using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.IO;

namespace SOTS.Projectiles.Slime
{    
    public class Fusion2 : ModProjectile 
    {	
		int worm = 0;
		int wait = 1;    
		Vector2 oldVelocity;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helix Bolt");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 30;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Color color = new Color(110, 110, 110, 0);
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				color = Projectile.GetAlpha(color) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length) * 0.5f;
				for (int j = 0; j < 5; j++)
				{
					float x = Main.rand.Next(-10, 11) * 0.1f;
					float y = Main.rand.Next(-10, 11) * 0.1f;
					if (!Projectile.oldPos[k].Equals(Projectile.position))
					{
						Main.spriteBatch.Draw(texture, drawPos + new Vector2(x, y), null, color, Projectile.rotation, drawOrigin, (Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length, SpriteEffects.None, 0f);
					}
				}
			}
			return false;
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
			target.immune[Projectile.owner] = 0;
			end = true;
			Projectile.velocity *= 0;
			if (Projectile.owner == Main.myPlayer)
				Projectile.netUpdate = true;
			base.OnHitNPC(target, damage, knockback, crit);
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			end = true;
			Projectile.velocity *= 0;
			if (Projectile.owner == Main.myPlayer)
				Projectile.netUpdate = true;
			return false;
		}
		public override void SetDefaults()
        {
			Projectile.CloneDefaults(14);
            aiType = 14; 
			Projectile.height = 10;
			Projectile.width = 10;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 900;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.magic = false;
			Projectile.ranged = true;
			Projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			if (Projectile.timeLeft <= 30)
				end = true;
			if (end)
			{
				Projectile.friendly = false;
				if (Projectile.timeLeft > 30)
					Projectile.timeLeft = 30;
			}
			if (end || Main.rand.NextBool(18))
			{
				int num2 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, mod.DustType("CopyDust4"));
				Dust dust = Main.dust[num2];
				dust.color = new Color(130, 110, 250, 40);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 2f;
				int alpha = 255 - (int)(255 * (Projectile.timeLeft / 60f));
				alpha = alpha > 255 ? 255 : alpha;
				alpha = alpha < 0 ? 0 : alpha;
				dust.alpha = alpha;
				return;
			}
			if (wait == 1)
			{
				wait++;
				oldVelocity = Projectile.velocity;
			}
			worm++;
			if (worm <= 60)
			{
				Projectile.velocity.X += -oldVelocity.Y / 30f;
				Projectile.velocity.Y += oldVelocity.X / 30f;
			}
			else if (worm >= 61 && worm <= 120)
			{
				Projectile.velocity.X += oldVelocity.Y / 30f;
				Projectile.velocity.Y += -oldVelocity.X / 30f;
			}
			else
				worm = 0;
		}
	}
}
		