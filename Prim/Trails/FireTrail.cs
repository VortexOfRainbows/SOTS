using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Projectiles.Temple;
using System.Collections.Generic;
using SOTS.Projectiles.Evil;
using SOTS.Projectiles.Blades;

namespace SOTS.Prim.Trails
{
	class FireTrail : PrimTrail
	{
		public int ClockWiseOrCounterClockwise;
		public Vector4 Color1;
		public Vector4 Color2;
		public int TextureType;
		public FireTrail(Projectile projectile, int clockWise = 1, int maxTrailLength = 24, int textureType = 0)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color1 = new Color(255, 190, 0, 0).ToVector4();
			Cap = maxTrailLength;
			Pixellated = false;
			ClockWiseOrCounterClockwise = clockWise;
			Color2 = new Vector4(0.9f, 0, 0, 0);
			TextureType = textureType;
		}
		public FireTrail(Projectile projectile, int clockWise, Vector4 firstColor, Vector4 secondColor, int maxTrailLength = 24, int textureType = 0)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color1 = firstColor;
			Cap = maxTrailLength;
			Pixellated = false;
			ClockWiseOrCounterClockwise = clockWise;
			Color2 = secondColor;
			TextureType = textureType;
		}
		public override void SetDefaults() => AlphaValue = 1f;
		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			if (PointCount <= 6) return;

			for (int i = 0; i < Points.Count; i++)
			{
				if (i != Points.Count - 1)
				{
					float widthVar = WidthList[i];
					float widthVar2 = WidthList[i + 1];
					Color colorvar = new Color(255, 0, 0) * (1 - i / (float)Points.Count);
					Vector2 normal = Vector2.Normalize(toOwner[i]) * ClockWiseOrCounterClockwise;
					Vector2 normal2 = Vector2.Normalize(toOwner[i + 1]) * ClockWiseOrCounterClockwise;
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normal2 * widthVar2;
					Vector2 secondDown = Points[i + 1] + normal2 * widthVar2;

					AddVertex(firstDown, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 1));
					AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
					AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));

					AddVertex(secondUp, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 0));
					AddVertex(secondDown, colorvar * AlphaValue, new Vector2((i + 1) / ((float)Points.Count), 1));
					AddVertex(firstUp, colorvar * AlphaValue, new Vector2((i / ((float)Points.Count)), 0));
				}
			}
		}
		public override void SetShaders()
		{
			Effect effect = SOTS.FireTrail;
			string texture = "TrailTextures/PyrocideTrail";
			if (TextureType == 1 || TextureType == 2)
			{
				texture = "TrailTextures/SwordSlash";
			}
			int directionCheck = 1;
			if (TextureType == 2)
				directionCheck = -1;
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().Assets.Request<Texture2D>(texture + (ClockWiseOrCounterClockwise == directionCheck ? "" : "Flipped")).Value);
			effect.Parameters["ColorOne"].SetValue(Color1);
			effect.Parameters["ColorTwo"].SetValue(Color2);
			PrepareShader(effect, "MainPS", 0);
		}
		public Vector2 ownerCenter;
		public List<float> WidthList = new List<float>();
		public List<Vector2> toOwner = new List<Vector2>();
		public override void OnUpdate()
		{
			Counter++;
			if (Entity is Projectile proj && !Destroyed)
			{
				Player projOwner = Main.player[proj.owner];
				PointCount = Points.Count() * 6;

				if (Cap < PointCount / 6)
				{
					Points.RemoveAt(0);
					WidthList.RemoveAt(0);
					toOwner.RemoveAt(0);
				}
				if (proj.ModProjectile is PyrocideSlash pyro && Entity.active && Entity != null)
				{
					WidthList.Add(pyro.GetArcLength() * 0.5f + 16);
					ownerCenter = projOwner.Center;
					toOwner.Add(ownerCenter - proj.Center);
					if (pyro.FetchDirection != ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Vector2.Lerp(ownerCenter - toOwner[toOwner.Count - 1].SafeNormalize(Vector2.Zero) * 32, Entity.Center, 0.5f)); // - new Vector2(Width / 2, Width / 2));
				}
				else if (proj.ModProjectile is ToothAcheSlash ache && Entity.active && Entity != null)
				{
					WidthList.Add(ache.GetArcLength() * 0.5f + 12);
					ownerCenter = projOwner.Center;
					toOwner.Add(ownerCenter - proj.Center);
					if (ache.FetchDirection != ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Vector2.Lerp(ownerCenter - toOwner[toOwner.Count - 1].SafeNormalize(Vector2.Zero) * 38, Entity.Center, 0.5f)); // - new Vector2(Width / 2, Width / 2));
				}
				else if (proj.ModProjectile is VertebraekerSlash vert && Entity.active && Entity != null)
				{
					WidthList.Add(vert.GetArcLength() * 0.5f + 16);
					ownerCenter = projOwner.Center;
					toOwner.Add(ownerCenter - proj.Center);
					if (vert.FetchDirection != ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Vector2.Lerp(ownerCenter - toOwner[toOwner.Count - 1].SafeNormalize(Vector2.Zero) * 38, Entity.Center, 0.5f)); // - new Vector2(Width / 2, Width / 2));
				}
				else if (proj.ModProjectile is ToothAcheThrow throwSword && Entity.active && Entity != null)
				{
					WidthList.Add(36);
					//ownerCenter = projOwner.Center;
					toOwner.Add(new Vector2(1, -1).RotatedBy(proj.rotation));
					if (throwSword.initialDirection != ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Entity.Center + toOwner[toOwner.Count - 1] * 32); // - new Vector2(Width / 2, Width / 2));
				}
				else if (proj.ModProjectile is VertebraekerThrow throwSword2 && Entity.active && Entity != null)
				{
					WidthList.Add(26);
					//ownerCenter = projOwner.Center;
					toOwner.Add(new Vector2(1, -1).RotatedBy(proj.rotation));
					if (throwSword2.initialDirection != ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Entity.Center + toOwner[toOwner.Count - 1] * 24); // - new Vector2(Width / 2, Width / 2));
				}
				else if (proj.ModProjectile is VorpalThrow throwSword3 && Entity.active && Entity != null)
				{
					WidthList.Add(22);
					//ownerCenter = projOwner.Center;
					toOwner.Add(new Vector2(1, -1).RotatedBy(proj.rotation));
					if (throwSword3.initialDirection != -ClockWiseOrCounterClockwise)
						Destroyed = true;
					Points.Add(Entity.Center + toOwner[toOwner.Count - 1] * 14); // - new Vector2(Width / 2, Width / 2));
				}
				else
					OnDestroy();
			}
			else
			{
				OnDestroy();
			}
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			int repeats = 1;
			if (EntityType == ModContent.ProjectileType<ToothAcheThrow>() || EntityType == ModContent.ProjectileType<VertebraekerThrow>() || EntityType == ModContent.ProjectileType<VorpalThrow>())
			{
				repeats = 3;
			}
			for (int i = 0; i < repeats; i++)
			{
				if (Points.Count > 0)
				{
					Points.RemoveAt(0);
					if (WidthList.Count > 0)
						WidthList.RemoveAt(0);
					if (toOwner.Count > 0)
						toOwner.RemoveAt(0);
				}
				else
					Dispose();
			}
		}
	}
}