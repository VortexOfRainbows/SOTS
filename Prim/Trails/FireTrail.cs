using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using SOTS.Projectiles.Temple;
using System.Collections.Generic;

namespace SOTS.Prim.Trails
{
	class FireTrail : PrimTrail
	{
		int ClockWiseOrCounterClockwise;
		public FireTrail(Projectile projectile, float width = 12, int clockWise = 1)
		{
			Entity = projectile;
			EntityType = projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color = new Color(255, 190, 0, 0);
			Width = width;
			Cap = 24;
			Pixellated = false;
			ClockWiseOrCounterClockwise = clockWise;
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
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().Assets.Request<Texture2D>("TrailTextures/PyrocideTrail" + (ClockWiseOrCounterClockwise == 1 ? "" : "Flipped")).Value);
			effect.Parameters["ColorOne"].SetValue(Color.ToVector4());
			effect.Parameters["ColorTwo"].SetValue(new Vector4(0.9f, 0, 0, 0));
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