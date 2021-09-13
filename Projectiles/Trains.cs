using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace SOTS.Projectiles 
{    
    public class Trains : ModProjectile 
    {	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Train Snake");
		}
        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32; 
            projectile.timeLeft = 120;
            projectile.penetrate = -1; 
            projectile.friendly = true; 
            projectile.hostile = false; 
            projectile.tileCollide = true;
            projectile.ignoreWater = true; 
            projectile.ranged = true; 
            projectile.aiStyle = 0; 
			projectile.alpha = 255;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 15;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
			width = 20;
			height = 20;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 48;
			hitbox = new Rectangle((int)projectile.Center.X - width / 2, (int)projectile.Center.Y - width / 2, width, width);
        }
        List<Vector2> segments = new List<Vector2>();
		List<float> segmentsRotation = new List<float>();
		List<int> targetIDs = new List<int>();
		bool runOnce = true;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(oldVelocity.Y != projectile.velocity.Y)
            {
				projectile.velocity.Y = -oldVelocity.Y;
			}
			if (oldVelocity.X != projectile.velocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(!targetIDs.Contains(target.whoAmI))
				targetIDs.Add(target.whoAmI);
			projectile.netUpdate = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
		{
			drawCacheProjsBehindNPCs.Add(index);
		}
		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (runOnce)
				return false;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 here = segments[i];
				int width = 32;
				Rectangle hitbox = new Rectangle((int)here.X - width / 2, (int)here.Y - width / 2, width, width);
				if (hitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return null;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (runOnce)
				return false;
			DrawWorm(spriteBatch, lightColor);
			return false;
		}
		public void DrawWorm(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 4);
			Rectangle frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
			Vector2 first = projectile.Center;
			Color color;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 toOther = first - segments[i];
				if (segmentsRotation[i] != 0)
				{
					var num1090 = MathHelper.WrapAngle(segmentsRotation[i]);
					var spinningpoint60 = toOther;
					var radians64 = (double)(num1090 * 0.1f);
					Vector2 vector = default(Vector2);
					toOther = spinningpoint60.RotatedBy(radians64, vector);
				}
				color = projectile.GetAlpha(Lighting.GetColor((int)segments[i].X / 16, (int)segments[i].Y / 16, Color.White));
				float rotation = segmentsRotation[i];
				int spriteDirection = toOther.X > 0f ? 1 : -1;
				spriteBatch.Draw(texture, segments[i] + projectile.velocity - Main.screenPosition, frame, color, rotation, origin, 1.00f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
				first = segments[i];
			}
			frame = new Rectangle(0, 0, texture.Width, texture.Height / 2);
			color = projectile.GetAlpha(lightColor);
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, frame, color, projectile.rotation, origin, 1.00f, projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
		}
		public void BodyTailMovement(ref Vector2 position, Vector2 prevPosition, ref float segmentsRotation, float segmentsRotation2, int i)
		{
			float width = 26;
			if (i == 0)
				width = 28;
			Vector2 npcCenter = position;
			float dirX = prevPosition.X - npcCenter.X;
			float dirY = prevPosition.Y - npcCenter.Y;
			segmentsRotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
			float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
			float dist = (length - width) / length;
			float posX = dirX * dist;
			float posY = dirY * dist;
			position.X += posX;
			position.Y += posY;
			/*
			float height = 40f;
			if (i != segments.Count - 1 && i != 0)
				height = 36f;
			if (i == segments.Count - 2)
				height = 48f;
			Vector2 toOther = prevPosition - position;
			if (segmentsRotation != segmentsRotation2)
			{
				var num1090 = MathHelper.WrapAngle(segmentsRotation2 - segmentsRotation);
				var radians64 = (double)(num1090 * 0.3f);
				toOther = toOther.RotatedBy(radians64);
			}
			segmentsRotation = toOther.ToRotation() + MathHelper.ToRadians(90);
			position = prevPosition - toOther.SafeNormalize(new Vector2(1, 0)) * height; */
		}
		int counter = 0;
		public override bool PreAI()
		{
			if (projectile.timeLeft <= 64)
				projectile.alpha += 4;
			else if (projectile.alpha > 0)
            {
				projectile.alpha -= 10;
            }
			else
            {
				projectile.alpha = 0;
			}
			counter++;
			if (counter % 3 == 0 && segments.Count < 9)
			{
				Vector2 center = projectile.Center;
				if(segments.Count > 0)
                {
					center = segments[segments.Count - 1];
				}
				segments.Add(center - projectile.velocity);
				segmentsRotation.Add(projectile.rotation);
				runOnce = false;
			}
			Vector2 first = projectile.Center;
			float firstRot = projectile.rotation;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 pos = segments[i];
				/*
				if (projectile.timeLeft < Main.rand.Next(3) + 1)
				{
					for (int k = 0; k < Main.rand.Next(3) + 1; k++)
					{
						int dust2 = Dust.NewDust(new Vector2(pos.X, pos.Y) - new Vector2(5), 0, 0, ModContent.DustType<CopyDust4>());
						Dust dust = Main.dust[dust2];
						dust.color = new Color(100, 255, 100, 0);
						if (Main.rand.NextBool(3))
							dust.color = Color.Black;
						dust.noGravity = true;
						dust.fadeIn = 0.1f;
						dust.scale *= 3.5f;
						dust.velocity *= 2.5f;
					}
				}*/
				float rotation = segmentsRotation[i];
				BodyTailMovement(ref pos, first, ref rotation, firstRot, i);
				segments[i] = pos;
				segmentsRotation[i] = rotation;
				first = pos;
				firstRot = segmentsRotation[i];
			}
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			if(projectile.owner == Main.myPlayer)
			{
				SeekEnemies();
				projectile.netUpdate = true;
            }
			return true;
		}
		public void SeekEnemies()
		{
			float minDist = 480;
			int target2 = -1;
			float dX;
			float dY;
			float distance;
			float speed = 1.2f;
			for (int i = 0; i < Main.npc.Length; i++)
			{
				NPC target = Main.npc[i];
				if (target.CanBeChasedBy() && !targetIDs.Contains(i))
				{
					dX = target.Center.X - projectile.Center.X;
					dY = target.Center.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < minDist)
					{
						bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, target.position, target.width, target.height);
						if (lineOfSight || !projectile.tileCollide)
						{
							minDist = distance;
							target2 = i;
						}
					}
				}
			}
			if (target2 != -1)
			{
				NPC toHit = Main.npc[target2];
				if (toHit.active == true)
				{
					dX = toHit.Center.X - projectile.Center.X;
					dY = toHit.Center.Y - projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
					projectile.velocity *= 0.96f;
					projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
		}
	}
}