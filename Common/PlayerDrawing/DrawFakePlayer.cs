using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.FakePlayer;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SOTS.Common.PlayerDrawing
{
	public class FakePlayerLayer : PlayerDrawLayer
	{
		public override bool IsHeadLayer => false;
		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			//Main.NewText(SubspacePlayer.ModPlayer(drawInfo.drawPlayer).servantActive);
			return SubspacePlayer.ModPlayer(drawInfo.drawPlayer).servantActive;
		}
		public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.JimsCloak); //this would be the very first layer... Hopefully this works
		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			SubspacePlayer.ModPlayer(drawInfo.drawPlayer).fPlayer.DrawFakePlayer(ref drawInfo);
		}
	}
}