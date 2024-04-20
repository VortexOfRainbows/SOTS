using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class PutridPinkyMask : PlayerDrawLayer
	{
		private Asset<Texture2D> maskTexture;
		public override bool IsHeadLayer => true;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "PutridPinkyMask", EquipType.Head);
		}
		public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			if (maskTexture == null)
			{
				maskTexture = ModContent.Request<Texture2D>("SOTS/Items/Slime/PutridPinkyMaskEye_Head");
			}
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
            float drawX = (int)drawInfo.Center.X;
            float drawY = (int)drawInfo.Position.Y + 12.5f;// + drawPlayer.height;
            Vector2 origin = drawInfo.headVect;
            Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White);
			color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.playerEffect;
			Vector2 addition = new Vector2(2, 0).RotatedBy((Main.MouseWorld - drawPlayer.Center).ToRotation());
			addition.Y *= 0.5f;
			if (addition.X * drawPlayer.direction < 0)
				addition.X *= 0.25f;
			if (Main.myPlayer != drawPlayer.whoAmI)
			{
				addition = new Vector2(2, 0) * drawPlayer.direction;
            }
            DrawData drawData = new DrawData(maskTexture.Value, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.cHead;
			drawInfo.DrawDataCache.Add(drawData);
		}
    }
    public class TinyPlanetoidHat : PlayerDrawLayer
    {
        private Asset<Texture2D> maskTexture;
        public override bool IsHeadLayer => true;
        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return drawInfo.drawPlayer.head == EquipLoader.GetEquipSlot(Mod, "TinyPlanetoid", EquipType.Head);
        }
        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Head);
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
            if (maskTexture == null)
            {
                maskTexture = ModContent.Request<Texture2D>("SOTS/Items/Conduit/TinyPlanetoid_Head");
            }
            float alpha = 1 - drawInfo.shadow;
            Player drawPlayer = drawInfo.drawPlayer;
            float drawX = (int)drawInfo.Center.X;
            float drawY = (int)drawInfo.Position.Y + 12.5f;// + drawPlayer.height;
            Vector2 origin = drawInfo.headVect;
            Vector2 position = new Vector2(drawX, drawY) + drawPlayer.headPosition - Main.screenPosition;
            alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
            Color color = new Color(110, 100, 130, 50);
            color = MachinaBooster.changeColorBasedOnStealth(color, drawInfo);
            Rectangle frame = drawPlayer.bodyFrame;
            float rotation = drawPlayer.bodyRotation;
            SpriteEffects spriteEffects = drawInfo.playerEffect;
			for(int i = 0; i < 6; i++)
			{
                Vector2 offset = new Vector2(2f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 2 + i * 60));
                DrawData drawData = new DrawData(maskTexture.Value, position + offset, frame, color * 1.5f * alpha, rotation, origin, 1f, spriteEffects, 0);
                drawData.shader = drawInfo.cHead;
                drawInfo.DrawDataCache.Add(drawData);
            }
        }
    }
}