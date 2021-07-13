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
			Main.projFrames[projectile.type] = 2;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 1;
		}
        public override void SetDefaults()
		{
			projectile.width = 10;
			projectile.height = 34;
			projectile.friendly = false;
			projectile.timeLeft = 240;
			projectile.hostile = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.tileCollide = false;
			projectile.hide = false;
			projectile.extraUpdates = 1;
		}
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			if (projHitbox.Intersects(targetHitbox))
				return true;
			for (int k = 0; k < projectile.oldPos.Length - 4; k++)
			{
				float scale = 1f - 0.5f * (k / (float)projectile.oldPos.Length - 4);
				Vector2 oldPos = projectile.oldPos[k];
				int width = (int)(8 * scale);
				Rectangle hitBox = new Rectangle((int)oldPos.X + projectile.width/2 - width/2, (int)oldPos.Y + projectile.height / 2 - width / 2, width, width);
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
			Vector2 from = projectile.Center;
			for (int i = 1; i < 20; i++)
			{
				float alphaMult = 1 - (0.5f * counter / 240f) - ((float)(i - 1) / 20f);
				Vector2 to = projectile.Center + projectile.velocity * i * 6;
				Vector2 toPos = from - to;
				int length = (int)toPos.Length() + 1;
				Texture2D texture2 = ModContent.GetTexture("SOTS/Projectiles/Pyramid/ShadeSpearIndicator");
				spriteBatch.Draw(texture2, from - Main.screenPosition, new Rectangle(0, 0, length, 2), Color.White * alphaMult, projectile.velocity.ToRotation(), new Vector2(1, 1), 1f, SpriteEffects.None, 0f);
				from = to;
			}
		}
		Vector2[] randList = new Vector2[20];
		public void setRand()
        {
			for (int i = 0; i < projectile.oldPos.Length; i++)
			{
				float scale = 1.05f - 0.85f * (i / (float)projectile.oldPos.Length);
				randList[i] = new Vector2(Main.rand.NextFloat(-2, 2), Main.rand.NextFloat(-2, 2)) * scale;
			}
		}
		public void TruePreDraw(SpriteBatch spriteBatch, int iterationValue)
		{
			if(iterationValue == 1)
				DrawTelegraph(spriteBatch);
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin;
				Color color = Color.White;
				float scale = 1f - 0.6f * (k / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos + randList[k], new Rectangle(0, iterationValue * 34, 10, 34), color, projectile.velocity.ToRotation() + MathHelper.Pi / 2, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//TruePreDraw(spriteBatch, lightColor);
			return false;
		}
		int counter = 0;
        public override void AI()
		{
			projectile.velocity *= 1.0225f;
			counter++;
			int parentID = (int)projectile.ai[0];
			if (parentID >= 0 && Main.netMode != NetmodeID.Server)
			{
				NPC npc = Main.npc[parentID];
				if (npc.active && npc.type == ModContent.NPCType<PharaohsCurse>())
				{
					PharaohsCurse curse = npc.modNPC as PharaohsCurse;
				}
				else
				{
					projectile.Kill();
				}
			}
		}
	}
}
		