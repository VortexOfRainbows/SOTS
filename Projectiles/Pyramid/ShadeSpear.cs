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
    public class ShadeSpear : ModProjectile 
    {	          
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Curse");
			Main.projFrames[Projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 1;
		}
        public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 34;
			Projectile.friendly = false;
			Projectile.timeLeft = 240;
			Projectile.hostile = true;
			Projectile.alpha = 255;
			Projectile.penetrate = -1;
			Projectile.netImportant = true;
			Projectile.tileCollide = false;
			Projectile.hide = false;
			Projectile.extraUpdates = 1;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (projHitbox.Intersects(targetHitbox))
				return true;
			for (int k = 0; k < Projectile.oldPos.Length - 4; k++)
			{
				float scale = 1f - 0.5f * (k / (float)Projectile.oldPos.Length - 4);
				Vector2 oldPos = Projectile.oldPos[k];
				int width = (int)(8 * scale);
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
        public void DrawTelegraph(SpriteBatch spriteBatch)
		{
			Vector2 from = Projectile.Center;
			for (int i = 1; i < 20; i++)
			{
				float alphaMult = 1 - (0.5f * counter / 240f) - ((float)(i - 1) / 20f);
				Vector2 to = Projectile.Center + Projectile.velocity * i * 6;
				Vector2 toPos = from - to;
				int length = (int)toPos.Length() + 1;
				Texture2D texture2 = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/Pyramid/ShadeSpearIndicator");
				spriteBatch.Draw(texture2, from - Main.screenPosition, new Rectangle(0, 0, length, 2), Color.White * alphaMult, Projectile.velocity.ToRotation(), new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
				from = to;
			}
		}
		Vector2[] randList = new Vector2[20];
		public void setRand()
        {
			for (int i = 0; i < Projectile.oldPos.Length; i++)
			{
				float scale = 1.05f - 0.85f * (i / (float)Projectile.oldPos.Length);
				randList[i] = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)) * scale;
			}
		}
		public void TruePreDraw(SpriteBatch spriteBatch, int iterationValue)
		{
			if(iterationValue == 1)
				DrawTelegraph(spriteBatch);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.White;
				float scale = 1f - 0.6f * (k / (float)Projectile.oldPos.Length);
				spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, drawPos + randList[k], new Rectangle(0, iterationValue * 34, 10, 34), color, Projectile.velocity.ToRotation() + MathHelper.Pi / 2, drawOrigin, Projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			//TruePreDraw(spriteBatch, lightColor);
			return false;
		}
		int counter = 0;
        public override void AI()
		{
			Projectile.velocity *= 1.0225f;
			counter++;
			int parentID = (int)Projectile.ai[0];
			if (parentID >= 0)
			{
				NPC npc = Main.npc[parentID];
				if (!npc.active || npc.type != ModContent.NPCType<PharaohsCurse>())
				{
					Projectile.Kill();
				}
			}
		}
	}
}
		