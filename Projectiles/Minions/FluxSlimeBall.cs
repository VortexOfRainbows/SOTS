using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Projectiles.Minions
{    
    public class FluxSlimeBall : ModProjectile 
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flux Slime Ball");
		}
        public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.height = 18;
			projectile.width = 18;
			projectile.friendly = true;
			projectile.timeLeft = 120;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.alpha = 70;
			projectile.penetrate = -1;
			projectile.hide = true;
			projectile.ignoreWater = true;
		}
		private int shader = 0;
		float counter2 = 0;
		float randMult = 1f;
		float[] counterArr = new float[6];
		float[] randSeed1 = new float[6];
		public int targetID = -1;
		public bool hasHit = false;
        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(targetID);
			writer.Write(hasHit);
		}
        public override void ReceiveExtraAI(BinaryReader reader)
        {
			targetID = reader.ReadInt32();
			hasHit = reader.ReadBoolean();
		}
        public override bool? CanHitNPC(NPC target)
        {
            return !hasHit && target.whoAmI == targetID ? (bool?)null : false;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			hasHit = true;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			}
			Texture2D texture = ModContent.GetTexture("SOTS/NPCs/FluxSlimeVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Projectile owner = Main.projectile[(int)projectile.ai[0]];
			Vector2 ownerCenter = owner.Center;
			Vector2 dynamicScaling = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 1.1f));
			float moreScaling = 1.00f - 0.05f * Math.Abs(dynamicScaling.X) / 40f;
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Vector2 p0 = ownerCenter;
			Vector2 p1 = ownerCenter - baseVelo.RotatedBy(MathHelper.ToRadians(180 + dynamicScaling.X)) * 1.0f * moreScaling;
			Vector2 p2 = projectile.Center - baseVelo * 1.5f * moreScaling;
			Vector2 p3 = projectile.Center;
			int segments = 36;
			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				t = (i + 1) / (float)segments;
				Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				float rotation = (drawPos2 - drawPosNext).ToRotation();
				lightColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
				spriteBatch.Draw(texture, drawPos2 - Main.screenPosition, null, projectile.GetAlpha(lightColor), rotation - MathHelper.ToRadians(90), drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			if (!runOnce)
			{
				for (int i = 0; i < 6; i++)
				{
					counterArr[i] += randSeed1[i];
					texture = ModContent.GetTexture("SOTS/NPCs/Boss/PinkyGrappleSpike");
					Vector2 circular = new Vector2(0, (projectile.width / 2 - 1.5f) * projectile.scale).RotatedBy(MathHelper.ToRadians(i * 60 + counter2 * 0.3f * randMult) + projectile.rotation);
					int frame = 0;
					if (counterArr[i] >= 20)
					{
						frame = 1;
					}
					if (counterArr[i] >= 30)
					{
						frame = 2;
					}
					if (counterArr[i] >= 40)
					{
						frame = 3;
					}
					if (counterArr[i] >= 50)
					{
						frame = 0;
						counterArr[i] = 0;
						randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
					}
					Rectangle FrameSize = new Rectangle(0, texture.Height / 4 * frame, texture.Width, texture.Height / 4);
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, projectile.GetAlpha(lightColor), circular.ToRotation() - MathHelper.ToRadians(90), new Vector2(texture.Width / 2, 3.5f), projectile.scale * 0.8f, SpriteEffects.None, 0f);
				}
			}
			texture = Main.projectileTexture[projectile.type];
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, projectile.GetAlpha(lightColor), projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, lightColor);
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
			drawCacheProjsBehindProjectiles.Add(index);
		}
		bool instant = false;
		Vector2 baseVelo = Vector2.Zero;
		bool runOnce = true;
		int direction = 0;
		int aiCounter = 0;
		public override void AI()
		{
			if (runOnce)
			{
				randMult = Main.rand.NextFloat(0.8f, 1.2f) * (Main.rand.Next(2) * 2 - 1);
				if (Main.myPlayer == projectile.owner)
					projectile.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				runOnce = false;
				direction = (int)projectile.ai[1] / 60;
				direction %= 2;
            }
			Projectile owner = Main.projectile[(int)projectile.ai[0]];
			Player player = Main.player[projectile.owner];
			shader = player.cPet;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			projectile.ai[1] += 0.4f;
			if (owner.type == ModContent.ProjectileType<PetPutridPinkyCrystal>() && owner.active)
			{
				Vector2 length = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians((180 * direction) + modPlayer.orbitalCounter * (1 + 0.5f * direction)));
				Vector2 rotate = new Vector2(24 + length.X, 0).RotatedBy(MathHelper.ToRadians(projectile.ai[1]));
				Vector2 toPos = owner.Center + rotate;
				if((owner.Center - projectile.Center).Length() < 64 || hasHit)
					instant = true;

				if (targetID != -1)
				{
					NPC target = Main.npc[targetID];
					if(target.active && !target.friendly && (owner.Center - projectile.Center).Length() < 800 && (owner.Center - player.Center).Length() < 1600)
					{
						Vector2 rotationArea = new Vector2(1, 0).RotatedBy((owner.Center - target.Center).ToRotation());
						float Xmult = rotationArea.X;
						float Ymult = rotationArea.Y;
						float width = target.width * Xmult * 0.5f;
						float height = target.height * Ymult * 0.5f;
						rotationArea = new Vector2(width, height);
						toPos = target.Center + rotationArea;
						if (!hasHit)
							instant = false;
					}
					else
					{
						targetID = -1; 
						hasHit = false;
					}
				}
				else
                {
					hasHit = false;
				}
				if (Main.myPlayer == projectile.owner)
					projectile.netUpdate = true;

				Vector2 goToPos = toPos - projectile.Center;
				float dist = goToPos.Length();
				float speed = 12f + owner.velocity.Length() * 0.4f;
				if (speed > dist)
				{
					speed = dist;
				}
				if(!instant)
					projectile.velocity *= 0.875f;
				if (instant)
					projectile.velocity *= 0.1f;
				projectile.velocity += goToPos.SafeNormalize(Vector2.Zero) * speed * (instant ? 1 : 0.1f);
				baseVelo *= 0.875f;
				baseVelo += projectile.velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(projectile.velocity.Length());
				//projectile.velocity += owner.velocity;
				projectile.timeLeft = 2;
			}
			else
            {
				projectile.Kill();
			}
			aiCounter += 1 * projectile.direction;
		}
	}
}
		