using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles
{    
    public class Doomhook : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doomhook");
		}
        public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 8;
			projectile.friendly = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.alpha = 0;
			projectile.penetrate = -1;
			projectile.hide = true;
			projectile.ignoreWater = true;
			projectile.extraUpdates = 1;
		}
		public int targetID = -1;
		public bool hasHit = false;
		public bool goBack = false;
		public bool letGo = false;
        public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(letGo);
			writer.Write(targetID);
			writer.Write(hasHit);
			writer.Write(goBack);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			letGo = reader.ReadBoolean();
			targetID = reader.ReadInt32();
			hasHit = reader.ReadBoolean();
			goBack = reader.ReadBoolean();
		}
        public override bool? CanHitNPC(NPC target)
        {
            return !hasHit ? (bool?)null : false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			targetID = target.whoAmI;
			hasHit = true;
			projectile.netUpdate = true;
			if (target.life <= 0)
            {
				targetID = -1;
            }
			projectile.knockBack = 0;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			float scale = 0.825f;
			Texture2D texture = ModContent.GetTexture("SOTS/Projectiles/DoomChain");
			Vector2 position = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			float num1 = texture.Height * scale;
			Vector2 vector2_4 = mountedCenter - position;
			float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
				flag = false;
			while (flag)
			{
				if (vector2_4.Length() < (double)num1 + 1.0)
				{
					flag = false;
				}
				else
				{
					Vector2 vector2_1 = vector2_4;
					vector2_1.Normalize();
					position += vector2_1 * num1;
					vector2_4 = mountedCenter - position;
					Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
					color2 = projectile.GetAlpha(color2);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, scale, SpriteEffects.None, 0.0f);
				}
			}
			Main.spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Lighting.GetColor((int)projectile.Center.X / 16, (int)(projectile.Center.Y / 16)), projectile.rotation + MathHelper.ToRadians(180), new Vector2(projectile.width/2, projectile.height/2), 1.15f, projectile.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
			return false;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindProjectiles.Add(index);
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			goBack = true;
            return false;
        }
		float speedMult = 1f;
        bool instant = false;
		bool runOnce = true;
		int aiCounter = 0;
		public override void AI()
		{
			if (runOnce)
			{
				if (Main.myPlayer == projectile.owner)
					projectile.netUpdate = true;
				runOnce = false;
            }
			Projectile owner = Main.projectile[(int)projectile.ai[0]];
			Player player = Main.player[projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			aiCounter++;
			speedMult = 1 + 0.005f * aiCounter;
			projectile.ai[1] += 0.4f;
			projectile.rotation = (player.Center - projectile.Center).ToRotation();
			projectile.spriteDirection = (player.Center - projectile.Center).X > 0 ? 1 : -1;
			projectile.tileCollide = !goBack;
			if(projectile.spriteDirection == -1)
            {
				projectile.rotation -= MathHelper.ToRadians(180);
            }
			if(aiCounter > 5)
            {
				if((player.Center - projectile.Center).Length() < 16)
                {
					projectile.Kill();
                }
			}
			if(!Main.mouseRight && Main.myPlayer == projectile.owner)
			{
				letGo = true;
				projectile.netUpdate = true;
			}
			if (aiCounter > 45 || letGo)
            {
				goBack = true;
            }
			if (owner.type == ModContent.ProjectileType<DoomstickHoldOut>() && owner.active)
			{
				Vector2 ownerToPlayer = owner.Center - player.Center;
				Vector2 toPos = Vector2.Zero;
				toPos = new Vector2((12 + (projectile.Center - player.Center).Length()) * speedMult, 0).RotatedBy(ownerToPlayer.ToRotation());
				toPos += player.Center;
				if(goBack)
                {
					toPos = player.Center;
                }
				if (targetID != -1)
				{
					NPC target = Main.npc[targetID];
					if(target.active && !target.friendly && (owner.Center - projectile.Center).Length() < 800 && (owner.Center - player.Center).Length() < 1600 && !letGo)
					{
						Vector2 rotationArea = new Vector2(1, 0).RotatedBy((owner.Center - target.Center).ToRotation());
						float Xmult = rotationArea.X;
						float Ymult = rotationArea.Y;
						float width = target.width * Xmult * 0.5f;
						float height = target.height * Ymult * 0.5f;
						rotationArea = new Vector2(width, height);
						toPos = target.Center + rotationArea;
						target.netUpdate = true;
						if (!hasHit)
                        {
							goBack = true;
							instant = true;
						}
					}
					else
					{
						targetID = -1; 
						hasHit = false;
						instant = false;
					}
				}
				else
                {
					hasHit = false;
					instant = false;
				}

				if(toPos != Vector2.Zero)
				{
					Vector2 goToPos = toPos - projectile.Center;
					float dist = goToPos.Length();
					float speed = 12f + owner.velocity.Length() * 0.4f;
					if (speed > dist)
					{
						speed = dist;
					}
					if (!instant)
						projectile.velocity *= 0.855f;
					if (instant)
						projectile.velocity *= 0.1f;
					projectile.velocity += goToPos.SafeNormalize(Vector2.Zero) * speed * (instant ? 1 : 0.1f) * speedMult;
					projectile.timeLeft = 2;
				}
				//projectile.velocity += owner.velocity;
				projectile.timeLeft = 2;
			}
			else
            {
				projectile.Kill();
			}
			if (Main.myPlayer == projectile.owner)
				projectile.netUpdate = true;
		}
	}
}
		