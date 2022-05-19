using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.Graphics.Shaders;
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
			Projectile.netImportant = true;
			Projectile.height = 18;
			Projectile.width = 18;
			Projectile.friendly = true;
			Projectile.timeLeft = 120;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.alpha = 70;
			Projectile.penetrate = -1;
			Projectile.hide = true;
			Projectile.ignoreWater = true;
		}
		private int shader = 0;
		float counter2 = 0;
		float randMult = 1f;
		float[] counterArr = new float[6];
		float[] randSeed1 = new float[6];
		public int targetID = -1;
		public bool hasHit = false;
        public override bool? CanCutTiles()
        {
            return false;
        }
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
        public override bool PreDraw(ref Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				GameShaders.Armor.GetSecondaryShader(shader, player).Apply(null);
			}
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/FluxSlimeVine");
			Vector2 drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Projectile parent = null;
			for (short i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile proj = Main.projectile[i];
				if (proj.active && proj.owner == Projectile.owner && proj.identity == (int)Projectile.ai[0])
				{
					parent = proj;
					break;
				}
			}
			Projectile owner = parent;
			Vector2 ownerCenter = owner.Center;
			Vector2 dynamicScaling = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(aiCounter * 1.1f));
			float moreScaling = 1.00f - 0.05f * Math.Abs(dynamicScaling.X) / 40f;
			Vector2 drawPos = Projectile.Center - Main.screenPosition;
			Vector2 p0 = ownerCenter;
			Vector2 p1 = ownerCenter - baseVelo.RotatedBy(MathHelper.ToRadians(180 + dynamicScaling.X)) * 1.0f * moreScaling;
			Vector2 p2 = Projectile.Center - baseVelo * 1.5f * moreScaling;
			Vector2 p3 = Projectile.Center;
			int segments = 36;
			for (int i = 0; i < segments; i++)
			{
				float t = i / (float)segments;
				Vector2 drawPos2 = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				t = (i + 1) / (float)segments;
				Vector2 drawPosNext = SOTS.CalculateBezierPoint(t, p0, p1, p2, p3);
				float rotation = (drawPos2 - drawPosNext).ToRotation();
				lightColor = Lighting.GetColor((int)drawPos2.X / 16, (int)(drawPos2.Y / 16));
				spriteBatch.Draw(texture, drawPos2 - Main.screenPosition, null, Projectile.GetAlpha(lightColor), rotation - MathHelper.ToRadians(90), drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			}
			if (!runOnce)
			{
				for (int i = 0; i < 6; i++)
				{
					counterArr[i] += randSeed1[i];
					texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PinkyGrappleSpike");
					Vector2 circular = new Vector2(0, (Projectile.width / 2 - 1.5f) * Projectile.scale).RotatedBy(MathHelper.ToRadians(i * 60 + counter2 * 0.3f * randMult) + Projectile.rotation);
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
					spriteBatch.Draw(texture, drawPos + circular, FrameSize, Projectile.GetAlpha(lightColor), circular.ToRotation() - MathHelper.ToRadians(90), new Vector2(texture.Width / 2, 3.5f), Projectile.scale * 0.8f, SpriteEffects.None, 0f);
				}
			}
			texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
			drawOrigin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			spriteBatch.Draw(texture, drawPos, null, Projectile.GetAlpha(lightColor), Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f);
			return false;
        }
        public override void PostDraw(Color lightColor)
		{
			Player player = Main.player[Projectile.owner];
			if (shader != 0)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);
			}
			base.PostDraw(spriteBatch, lightColor);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
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
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;
				for (int i = 0; i < counterArr.Length; i++)
				{
					counterArr[i] = 0;
					randSeed1[i] = Main.rand.NextFloat(0.8f, 1.2f);
				}
				runOnce = false;
				direction = (int)Projectile.ai[1] / 60;
				direction %= 2;
			}
			//Projectile owner = Main.projectile[(int)Projectile.ai[0]];
			Projectile parent = null;
			for (short i = 0; i < Main.maxProjectiles; i++)
            {
				Projectile proj = Main.projectile[i];
				if(proj.active && proj.owner == Projectile.owner && proj.identity == (int)Projectile.ai[0])
                {
					parent = proj;
					break;
                }
            }
			Projectile owner = parent;
			//Main.NewText(owner.identity + " " + owner.whoAmI);
			Player player = Main.player[Projectile.owner];
			shader = player.cPet;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Projectile.ai[1] += 0.4f;
			if (owner != null && owner.type == ModContent.ProjectileType<PetPutridPinkyCrystal>() && owner.active)
			{
				Vector2 length = new Vector2(4, 0).RotatedBy(MathHelper.ToRadians((180 * direction) + modPlayer.orbitalCounter * (1 + 0.5f * direction)));
				Vector2 rotate = new Vector2(24 + length.X, 0).RotatedBy(MathHelper.ToRadians(Projectile.ai[1]));
				Vector2 toPos = owner.Center + rotate;
				if((owner.Center - Projectile.Center).Length() < 64 || hasHit)
					instant = true;

				if (targetID != -1)
				{
					NPC target = Main.npc[targetID];
					if(target.active && !target.friendly && (owner.Center - Projectile.Center).Length() < 800 && (owner.Center - player.Center).Length() < 1600)
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
				if (Main.myPlayer == Projectile.owner)
					Projectile.netUpdate = true;

				Vector2 goToPos = toPos - Projectile.Center;
				float dist = goToPos.Length();
				float speed = 12f + owner.velocity.Length() * 0.4f;
				if (speed > dist)
				{
					speed = dist;
				}
				if(!instant)
					Projectile.velocity *= 0.875f;
				if (instant)
					Projectile.velocity *= 0.1f;
				Projectile.velocity += goToPos.SafeNormalize(Vector2.Zero) * speed * (instant ? 1 : 0.1f);
				baseVelo *= 0.875f;
				baseVelo += Projectile.velocity.SafeNormalize(Vector2.Zero) * (float)Math.Sqrt(Projectile.velocity.Length());
				//Projectile.velocity += owner.velocity;
				Projectile.timeLeft = 2;
			}
			else
			{
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.Kill();
					Projectile.netUpdate = true;
				}
			}
			aiCounter += 1 * Projectile.direction;
		}
	}
}
		