using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Chaos
{
	public class SupernovaLaser : ModProjectile
	{
		public override void SetStaticDefaults() 
		{
			DisplayName.SetDefault("Supernova Laser");
		}

		public override void SetDefaults() 
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.timeLeft = 40;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.localNPCHitCooldown = 40;
			projectile.usesLocalNPCImmunity = true;
		}
		bool runOnce = true;
		Color color;
		float scale = 0.8f;
		public const float length = 7f;
		public override bool PreAI() 
		{
			if(runOnce)
			{
				color = VoidPlayer.pastelAttempt(Main.rand.NextFloat(6.28f), true);
				SetPostitions();
				runOnce = false;
				return true;
            }
			return true;
		}
        public override void AI()
        {
			projectile.alpha = (int)(255 * (1 - projectile.timeLeft / 40f));
			projectile.ai[1] ++;
		}
		bool collided = false;
        public void SetPostitions()
        {
			Vector2 direction = new Vector2(length * scale, 0).RotatedBy(projectile.velocity.ToRotation());
			int maxDist = 480;
			Vector2 currentPos = projectile.Center;
			int k = 0;
			while (maxDist > 0)
			{
				k++;
				posList.Add(currentPos);
				currentPos += direction;
				if (!collided && k > 20)
				{
					int npc = FindClosestEnemy(currentPos, k);
					if (npc != -1)
					{
						NPC target = Main.npc[npc];
						if (target.CanBeChasedBy())
						{
							direction = new Vector2(length * scale, 0).RotatedBy(Redirect(direction.ToRotation(), currentPos, target.Center));
							float width = projectile.width * scale;
							float height = projectile.height * scale;
							Rectangle projHitbox = new Rectangle((int)currentPos.X - (int)width / 2, (int)currentPos.Y - (int)height / 2, (int)width, (int)height);
							if (target.Hitbox.Intersects(projHitbox))
							{
								collided = true;
							}
						}
					}
				}
				else if (maxDist > 30 && collided)
					maxDist = 30;
				if (Main.rand.NextBool(3))
				{
					Dust dust = Dust.NewDustDirect(posList[posList.Count - 1] - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
					dust.fadeIn = 0.2f;
					dust.noGravity = true;
					dust.alpha = 100;
					dust.color = color;
					dust.scale *= 1.2f;
					dust.velocity *= 1.5f;
				}
				maxDist--;
			}
		}
		float redirections = 0;
		public float Redirect(float radians, Vector2 pos, Vector2 npc)
		{
			Vector2 toNPC = npc - pos;
			float speed = redirections * 0.05f;
			Vector2 rnVelo = new Vector2(length - (redirections * 0.025f), 0).RotatedBy(radians);
			rnVelo += toNPC.SafeNormalize(Vector2.Zero) * speed;
			float npcRad = rnVelo.ToRotation();
			redirections++;
			return npcRad;
		}
		int currentNPC = -1;
		public int FindClosestEnemy(Vector2 pos, int length)
		{
			if (currentNPC != -1)
			{
				return currentNPC;
			}
			return SOTSNPCs.FindTarget_Basic(pos, length * 4);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 5;
        }
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if(projectile.alpha >= 150)
            {
				return false;
            }
			float width = projectile.width * scale;
			float height = projectile.height * scale;
			for (int i = 0; i < posList.Count; i += 2)
			{
				Vector2 pos = posList[i];
				projHitbox = new Rectangle((int)pos.X - (int)width/2, (int)pos.Y - (int)height/2, (int)width, (int)height);
				if(projHitbox.Intersects(targetHitbox))
                {
					return true;
                }
			}
			return false;
		}
		List<Vector2> posList = new List<Vector2>();
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width/2, texture.Height/2);
			float alpha = 1;
			Vector2 lastPosition = projectile.Center;
			for(int i = 0; i < posList.Count; i++)
			{
				Vector2 drawPos = posList[i];
				if(i > posList.Count - 120)
                {
					alpha = 1 - (i - posList.Count + 120) / 120f;
				}
				Vector2 direction = drawPos - lastPosition;
				lastPosition = drawPos;
				float rotation = i == 0 ? projectile.velocity.ToRotation() : direction.ToRotation();
				for(int j = 0; j < 3; j++)
				{
					Vector2 sinusoid = new Vector2(0, scale * 18 * (float)Math.Sin(MathHelper.ToRadians(i * 2 + projectile.ai[1] * 2 + j * 120))).RotatedBy(rotation);
					Color color = this.color * ((255 - projectile.alpha) / 255f) * alpha * 0.7f;
					color.A = 0;
					spriteBatch.Draw(texture, drawPos - Main.screenPosition + sinusoid, null, color, rotation, origin, new Vector2(scale * 3, scale), SpriteEffects.None, 0f);
				}
			}
			return false;
		}
	}
}