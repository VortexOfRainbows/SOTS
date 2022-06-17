using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class BladeStorm : PlayerDrawLayer
	{
		private Texture2D bladeTexture;
		private Texture2D bladeGlowTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return true;
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (Main.dresserInterfaceDummy == drawInfo.drawPlayer)
				return;
			if (drawInfo.drawPlayer.dead || !drawInfo.drawPlayer.active)
			{
				return;
			}
			if (drawInfo.shadow != 0)
				return;
			if(bladeTexture == null)
				bladeTexture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/SkywardBladeBeam").Value;
			if (bladeGlowTexture == null)
				bladeGlowTexture = Mod.Assets.Request<Texture2D>("Projectiles/Otherworld/SkywardBladeGlowmask").Value;
			Player drawPlayer = drawInfo.drawPlayer;
			SOTSPlayer modPlayer; 
			bool flag = drawPlayer.TryGetModPlayer<SOTSPlayer>(out modPlayer);
			if (flag && modPlayer.skywardBlades > 0 && !drawPlayer.dead)
			{
				float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
				float drawY = (int)drawInfo.Position.Y + drawPlayer.height / 2;
				int amt = modPlayer.skywardBlades;
				//float total = amt * 8;
				Color color2 = Color.White.MultiplyRGBA(Lighting.GetColor((int)drawX / 16, (int)drawY / 16));
				drawX -= Main.screenPosition.X;
				drawY -= Main.screenPosition.Y;
				for (int i = 0; i < amt; i++)
				{
					Color color = color2;
					float number = 0;
					if (i == 0)
						number = 0;
					if (i == 1)
						number = -7.5f;
					if (i == 2)
						number = 7.5f;
					if (i == 3)
						number = -15;
					if (i == 4)
						number = 15;
					Vector2 moveDraw = new Vector2(64, 0).RotatedBy(modPlayer.cursorRadians + MathHelper.ToRadians(number));
					DrawData data = new DrawData(bladeTexture, new Vector2(drawX, drawY) + moveDraw, null, color * ((255 - modPlayer.bladeAlpha) / 255f), modPlayer.cursorRadians - 0.5f * MathHelper.ToRadians(number) + MathHelper.ToRadians(90), new Vector2(bladeTexture.Width / 2f, bladeTexture.Height / 2f), 1f, SpriteEffects.None, 0);
					drawInfo.DrawDataCache.Add(data);

					int recurse = 1;
					if (modPlayer.rainbowGlowmasks)
					{
						color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
						recurse = 2;
					}
					for (int j = 0; j < recurse; j++)
					{
						data = new DrawData(bladeGlowTexture, new Vector2(drawX, drawY) + moveDraw, null, color * ((255 - modPlayer.bladeAlpha) / 255f), modPlayer.cursorRadians - 0.5f * MathHelper.ToRadians(number) + MathHelper.ToRadians(90), new Vector2(bladeGlowTexture.Width / 2f, bladeGlowTexture.Height / 2f), 1f, SpriteEffects.None, 0);
						drawInfo.DrawDataCache.Add(data);
					}
				}
			}
		}
	}
}