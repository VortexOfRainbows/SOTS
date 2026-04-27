using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class ExcavatorHelmet : PlayerDrawLayer
	{
		private Asset<Texture2D> texture;
		public override bool IsHeadLayer => false;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "ExcavatorHelmet", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.FinchNest); // this is because head layer forces a draw onto the map
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
				return;
			texture ??= ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/ExcavatorHelmet_Head");
			Player drawPlayer = drawInfo.drawPlayer;
			float alpha = 1 - drawInfo.shadow;
			float drawX = (int)drawInfo.Position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.Position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyVect;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			Color color = drawInfo.colorArmorHead;
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			DrawData drawData = new(texture.Value, position + new Vector2(2 * drawPlayer.direction, 0), new Rectangle( drawPlayer.bodyFrame.X, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width + 4, drawPlayer.bodyFrame.Width + 4), color * alpha, drawPlayer.bodyRotation, origin + new Vector2(2, 0), 1f, spriteEffects, 0);
			drawData.shader = drawInfo.cHead;
			drawInfo.DrawDataCache.Add(drawData);
		}
	}
}