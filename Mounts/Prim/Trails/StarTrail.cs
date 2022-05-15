using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;

namespace SOTS.Prim.Trails
{
	class StarTrail : PrimTrail
	{
		public Color color2 = new Color(140, 140, 130);
		public StarTrail(Projectile projectile, Color colorStart, Color colorEnd, int width = 12, int length = 18)
		{
			Entity = projectile;
			EntityType = Projectile.type;
			DrawType = PrimTrailManager.DrawProjectile;
			Color = colorStart;
			color2 = colorEnd;
			Width = width;
			Cap = length;
			Pixellated = false;
		}
		public override void SetDefaults() => AlphaValue = 0.6f;
		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			if (PointCount <= 6) return;
			float widthVar;
			for (int i = 0; i < Points.Count; i++)
			{
				if (i != Points.Count - 1)
				{
					widthVar = MathHelper.Lerp(0, Width, (float)Math.Sqrt(i / (float)Points.Count));
					Color colorvar = Color.Lerp(Color, new Color(140, 193, 252), ((float)i / (float)Points.Count));
					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

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
			Effect effect = SOTS.WaterTrail; //we'll see how this works :D
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().Assets.Request<Texture2D>("TrailTextures/Trail_1").Value);
			effect.Parameters["ColorOne"].SetValue(Color.ToVector4());
			PrepareShader(effect, "MainPS", Counter / 12f);
		}
		public override void OnUpdate()
		{
			if (!(Entity is Projectile proj))
				return;
			Counter++;
			PointCount = Points.Count() * 6;

			if (Cap < PointCount / 6)
				Points.RemoveAt(0);

			if ((!Entity.active && Entity != null) || Destroyed)
				OnDestroy();

			else
				Points.Add(Entity.Center); // - new Vector2(Width / 2, Width / 2));
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.8f;
			Width += ((float)Math.Sin(Counter * 2) * 0.3f);
			if (Width < 0.05f)
				Dispose();
		}
	}
}