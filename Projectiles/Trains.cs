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
            Projectile.width = 32;
            Projectile.height = 32; 
            Projectile.timeLeft = 120;
            Projectile.penetrate = -1; 
            Projectile.friendly = true; 
            Projectile.hostile = false; 
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true; 
            Projectile.DamageType = DamageClass.Ranged; 
            Projectile.aiStyle = 0; 
			Projectile.alpha = 255;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 15;
		}
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
			width = 20;
			height = 20;
            return true;
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
			int width = 48;
			hitbox = new Rectangle((int)Projectile.Center.X - width / 2, (int)Projectile.Center.Y - width / 2, width, width);
        }
        List<Vector2> segments = new List<Vector2>();
		List<float> segmentsRotation = new List<float>();
		List<int> targetIDs = new List<int>();
		bool runOnce = true;
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			if(oldVelocity.Y != Projectile.velocity.Y)
            {
				Projectile.velocity.Y = -oldVelocity.Y;
			}
			if (oldVelocity.X != Projectile.velocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}
			return false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(!targetIDs.Contains(target.whoAmI))
				targetIDs.Add(target.whoAmI);
			Projectile.netUpdate = true;
            base.OnHitNPC(target, damage, knockback, crit);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
			behindNPCs.Add(index);
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
		public override bool PreDraw(ref Color lightColor)
		{
			if (runOnce)
				return false;
			DrawWorm(lightColor);
			return false;
		}
		public void DrawWorm(Color lightColor)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 4);
			Rectangle frame = new Rectangle(0, texture.Height / 2, texture.Width, texture.Height / 2);
			Vector2 first = Projectile.Center;
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
				color = Projectile.GetAlpha(Lighting.GetColor((int)segments[i].X / 16, (int)segments[i].Y / 16, Color.White));
				float rotation = segmentsRotation[i];
				int spriteDirection = toOther.X > 0f ? 1 : -1;
				Main.spriteBatch.Draw(texture, segments[i] + Projectile.velocity - Main.screenPosition, frame, color, rotation, origin, 1.00f, spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
				first = segments[i];
			}
			frame = new Rectangle(0, 0, texture.Width, texture.Height / 2);
			color = Projectile.GetAlpha(lightColor);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, color, Projectile.rotation, origin, 1.00f, Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
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
			if (Projectile.timeLeft <= 64)
				Projectile.alpha += 4;
			else if (Projectile.alpha > 0)
            {
				Projectile.alpha -= 10;
            }
			else
            {
				Projectile.alpha = 0;
			}
			counter++;
			if (counter % 3 == 0 && segments.Count < 9)
			{
				Vector2 center = Projectile.Center;
				if(segments.Count > 0)
                {
					center = segments[segments.Count - 1];
				}
				segments.Add(center - Projectile.velocity);
				segmentsRotation.Add(Projectile.rotation);
				runOnce = false;
			}
			Vector2 first = Projectile.Center;
			float firstRot = Projectile.rotation;
			for (int i = 0; i < segments.Count; i++)
			{
				Vector2 pos = segments[i];
				/*
				if (Projectile.timeLeft < Main.rand.Next(3) + 1)
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
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
			if(Projectile.owner == Main.myPlayer)
			{
				SeekEnemies();
				Projectile.netUpdate = true;
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
					dX = target.Center.X - Projectile.Center.X;
					dY = target.Center.Y - Projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					if (distance < minDist)
					{
						bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, target.position, target.width, target.height);
						if (lineOfSight || !Projectile.tileCollide)
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
					dX = toHit.Center.X - Projectile.Center.X;
					dY = toHit.Center.Y - Projectile.Center.Y;
					distance = (float)Math.Sqrt((double)(dX * dX + dY * dY));
					speed /= distance;
					Projectile.velocity *= 0.96f;
					Projectile.velocity += new Vector2(dX * speed, dY * speed);
				}
			}
		}
	}
}