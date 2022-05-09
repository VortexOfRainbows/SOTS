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
			Projectile.width = 26;
			Projectile.height = 8;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = true;
			Projectile.hostile = false;
			Projectile.alpha = 0;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.ignoreWater = true;
			Projectile.extraUpdates = 1;
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
			Projectile.netUpdate = true;
			if (target.life <= 0)
            {
				targetID = -1;
            }
			Projectile.knockBack = 0;
		}
        public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			float scale = 0.825f;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Projectiles/DoomChain");
			Vector2 position = Projectile.Center;
			Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
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
					color2 = Projectile.GetAlpha(color2);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, scale, SpriteEffects.None, 0.0f);
				}
			}
			Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, null, Lighting.GetColor((int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16)), Projectile.rotation + MathHelper.ToRadians(180), new Vector2(Projectile.width/2, Projectile.height/2), 1.15f, Projectile.spriteDirection != 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
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
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;
				runOnce = false;
            }
			Projectile owner = Main.projectile[(int)Projectile.ai[0]];
			Player player = Main.player[Projectile.owner];
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			aiCounter++;
			speedMult = 1 + 0.005f * aiCounter;
			Projectile.ai[1] += 0.4f;
			Projectile.rotation = (player.Center - Projectile.Center).ToRotation();
			Projectile.spriteDirection = (player.Center - Projectile.Center).X > 0 ? 1 : -1;
			Projectile.tileCollide = !goBack;
			if(Projectile.spriteDirection == -1)
            {
				Projectile.rotation -= MathHelper.ToRadians(180);
            }
			if(aiCounter > 5)
            {
				if((player.Center - Projectile.Center).Length() < 16)
                {
					Projectile.Kill();
                }
			}
			if(!Main.mouseRight && Main.myPlayer == Projectile.owner)
			{
				letGo = true;
				Projectile.netUpdate = true;
			}
			if (aiCounter > 45 || letGo)
            {
				goBack = true;
            }
			if (owner.type == ModContent.ProjectileType<DoomstickHoldOut>() && owner.active)
			{
				Vector2 ownerToPlayer = owner.Center - player.Center;
				Vector2 toPos = Vector2.Zero;
				toPos = new Vector2((12 + (Projectile.Center - player.Center).Length()) * speedMult, 0).RotatedBy(ownerToPlayer.ToRotation());
				toPos += player.Center;
				if(goBack)
                {
					toPos = player.Center;
                }
				if (targetID != -1)
				{
					NPC target = Main.npc[targetID];
					if(target.active && !target.friendly && (owner.Center - Projectile.Center).Length() < 800 && (owner.Center - player.Center).Length() < 1600 && !letGo)
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
					Vector2 goToPos = toPos - Projectile.Center;
					float dist = goToPos.Length();
					float speed = 12f + owner.velocity.Length() * 0.4f;
					if (speed > dist)
					{
						speed = dist;
					}
					if (!instant)
						Projectile.velocity *= 0.855f;
					if (instant)
						Projectile.velocity *= 0.1f;
					Projectile.velocity += goToPos.SafeNormalize(Vector2.Zero) * speed * (instant ? 1 : 0.1f) * speedMult;
					Projectile.timeLeft = 2;
				}
				//Projectile.velocity += owner.velocity;
				Projectile.timeLeft = 2;
			}
			else
            {
				Projectile.Kill();
			}
			if (Main.myPlayer == Projectile.owner)
				Projectile.netUpdate = true;
		}
	}
}
		