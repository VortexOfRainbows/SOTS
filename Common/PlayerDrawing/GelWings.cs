using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class GelWings : PlayerDrawLayer
	{
		private Asset<Texture2D> wingsTexture;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.wings == EquipLoader.GetEquipSlot(Mod, "GelWings", EquipType.Wings);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Wings);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (wingsTexture == null)
			{
				wingsTexture = ModContent.Request<Texture2D>("SOTS/Items/Slime/GelWings_Wings2");
			}
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
			/*int drawX = (int)(drawInfo.Position.X - Main.screenPosition.X);
			int drawY = (int)(drawInfo.Position.Y - Main.screenPosition.Y);*/
			Vector2 Position = drawInfo.Position;
			Vector2 origin = new Vector2(wingsTexture.Value.Width / 2, wingsTexture.Value.Height / 12);
			Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction))), (float)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) - 5f * drawPlayer.gravDir));
			Color lightColor = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White);
			Color color = MachinaBooster.changeColorBasedOnStealth(lightColor, drawInfo) * (175f / 255f) * alpha;
			DrawData data = new DrawData(wingsTexture.Value, pos, new Rectangle(0, wingsTexture.Value.Height / 6 * drawPlayer.wingFrame, wingsTexture.Value.Width, wingsTexture.Value.Height / 6), color, 0f, origin, 1f, drawInfo.playerEffect, 0);
			data.shader = drawInfo.cWings;
			drawInfo.DrawDataCache.Add(data);
		}
	}
}