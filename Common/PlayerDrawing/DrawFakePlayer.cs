using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using SOTS.FakePlayer;
using System.Collections.Generic;
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
			List<FakePlayer.FakePlayer> fPlayers = SubspacePlayer.GetServantFakePlayers(drawInfo.drawPlayer);
			for(int i = 0; i < fPlayers.Count; i++)
			{
                fPlayers[i].DrawFakePlayer(ref drawInfo);
			}
		}
	}
}