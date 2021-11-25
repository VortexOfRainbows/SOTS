using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Slime
{
	[AutoloadEquip(EquipType.Head)]
	public class PutridPinkyMask : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 20;
			item.vanity = true;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.rare = ItemRarityID.Blue;
		}
	}
	public class PutridPinkyMaskPlayer : ModPlayer
    {
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int bodyLayer = layers.FindIndex(l => l == PlayerLayer.Head);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, PutridPinkyMaskEye);
			}
		}
		public static readonly PlayerLayer PutridPinkyMaskEye = new PlayerLayer("SOTS", "PutridPinkyMaskEye", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");
			if (drawPlayer.head != mod.GetEquipSlot("PutridPinkyMask", EquipType.Head))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Slime/PutridPinkyMaskEye_Head");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White);
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			Vector2 addition = new Vector2(2, 0).RotatedBy((Main.MouseWorld - drawPlayer.Center).ToRotation());
			addition.Y *= 0.5f;
			if (addition.X * drawPlayer.direction < 0)
				addition.X *= 0.25f;
			if(Main.myPlayer != drawPlayer.whoAmI)
            {
				addition = new Vector2(2, 0) * drawPlayer.direction;
            }
			DrawData drawData = new DrawData(texture, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.headArmorShader;
			Main.playerDrawData.Add(drawData);
		});
	}
}