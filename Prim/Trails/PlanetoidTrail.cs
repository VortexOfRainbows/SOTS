using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using System;
using static SOTS.NPCs.Anomaly.Planetoid;
using System.Collections.Generic;
using SOTS.NPCs.Anomaly;

namespace SOTS.Prim.Trails
{
	public class PlanetoidTrail : PrimTrail
	{
		public PlanetoidTrail(NPC npc, int width = 12)
		{
			Entity = npc;
			EntityType = npc.type;
			DrawType = PrimTrailManager.DrawNPC;
			Color = new Color(46, 104, 234);
			Width = width;
			Cap = 20;
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
					float percent = (float)i / Points.Count;
					float sinusoid = (float)Math.Sin(percent * MathHelper.Pi);
					widthVar = Width;
					Color colorvar = Color.Lerp(Color, Color.White, sinusoid);
					Vector2 normal = CurveNormal(Points, i);
					Vector2 normalAhead = CurveNormal(Points, i + 1);
					Vector2 firstUp = Points[i] - normal * widthVar;
					Vector2 firstDown = Points[i] + normal * widthVar;
					Vector2 secondUp = Points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = Points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, colorvar * AlphaValue * sinusoid, new Vector2((i / ((float)Points.Count)), 1));
					AddVertex(firstUp, colorvar * AlphaValue * sinusoid, new Vector2((i / ((float)Points.Count)), 0));
					AddVertex(secondDown, colorvar * AlphaValue * sinusoid, new Vector2((i + 1) / ((float)Points.Count), 1));

					AddVertex(secondUp, colorvar * AlphaValue * sinusoid, new Vector2((i + 1) / ((float)Points.Count), 0));
					AddVertex(secondDown, colorvar * AlphaValue * sinusoid, new Vector2((i + 1) / ((float)Points.Count), 1));
					AddVertex(firstUp, colorvar * AlphaValue * sinusoid, new Vector2((i / ((float)Points.Count)), 0));
				}
			}
		}

		public override void SetShaders()
		{
			Effect effect = SOTS.GridTrail;
			effect.Parameters["TrailTexture"].SetValue(ModContent.GetInstance<SOTS>().Assets.Request<Texture2D>("TrailTextures/WhiteBox").Value);
			effect.Parameters["ColorTwo"].SetValue(new Color(213, 66, 232).ToVector4());
			effect.Parameters["ColorOne"].SetValue(new Color(119, 190, 238).ToVector4());
			int bonusPoints = (int)(PointCount / 6 / 8f);
			bonusPoints -= 10;
			if (bonusPoints < 0)
				bonusPoints = 0;
			effect.Parameters["pointCount"].SetValue(10 + bonusPoints);
			PrepareShader(effect, "MainPS", AlphaMult);
		}
		public override void OnUpdate()
		{
			if (!(Entity is NPC npc) || !(npc.ModNPC is Planetoid))
            {
				OnDestroy();
				return;
			}
			Counter++;
			PointCount = Points.Count() * 6;
			if ((!Entity.active && Entity != null) || Destroyed)
				OnDestroy();
		}
		public float AlphaMult = 0f;
		public void ConvertListToPoints(List<GravityWellLine> gwl, float alphaMult)
        {
			Points = new List<Vector2>();
			foreach(GravityWellLine line in gwl)
            {
				Points.Add(line.Position);
            }
			AlphaMult = alphaMult;
		}
		public override void OnDestroy()
		{
			Destroyed = true;
			Width *= 0.9f;
			Width += ((float)Math.Sin(Counter * 2) * 0.3f);
			AlphaMult *= 0.85f;
			if (Width < 0.05f || AlphaMult < 0.01f)
				Dispose();
		}
	}
}