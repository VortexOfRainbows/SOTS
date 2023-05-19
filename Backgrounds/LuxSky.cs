using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Lux;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SOTS.Backgrounds
{
	public class LuxSky : CustomSky
	{
		private bool isActive;
		private float intensity;
		private int luxIndex;

		public override void Update(GameTime gameTime)
		{
			if (isActive && intensity < 1f)
			{
				intensity += 0.01f;
			}
			else if (!isActive && intensity > 0f)
			{
				intensity -= 0.01f;
			}
		}

		private bool UpdateLuxIndex()
		{
			if (luxIndex >= 0 && Main.npc[luxIndex].active && Main.npc[luxIndex].type == ModContent.NPCType<Lux>())
			{
				return true;
			}
			luxIndex = -1;
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<Lux>())
				{
					luxIndex = i;
					break;
				}
			}
			return luxIndex >= 0;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			if (maxDepth >= 0 && minDepth < 0)
			{
				spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(240, 240, 240) * intensity);
			}
		}

		public override float GetCloudAlpha()
		{
			return 0f;
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			isActive = false;
		}

		public override void Reset()
		{
			isActive = false;
		}

		public override bool IsActive()
		{
			return isActive || intensity > 0f;
		}
	}
}