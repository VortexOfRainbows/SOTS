using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.GelGear
{
	[AutoloadEquip(EquipType.Wings)]
	public class GelWings : ModItem
	{
		public override void SetStaticDefaults()
		{	
			DisplayName.SetDefault("Gel Wings");
			Tooltip.SetDefault("Allows flight and slow fall\n'It really shouldn't hold up well'");
		}
		public override void SetDefaults()
		{
			item.width = 42;
			item.height = 38;
            item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.LightRed;
			item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 30;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1.35f;
			constantAscend = 0.195f;
		}
        public override bool WingUpdate(Player player, bool inUse)
		{
			if (player.controlJump && player.wingTime > 0 && player.velocity.Y != 0)
			{
				player.wingFrameCounter++;
				if (player.wingFrameCounter >= 20)
					player.wingFrameCounter = 0;
				if(player.wingFrameCounter < 5)
                {
					player.wingFrame = 1;
				}
				else if (player.wingFrameCounter < 10)
				{
					player.wingFrame = 2;
				}
				else if (player.wingFrameCounter < 15)
				{
					player.wingFrame = 3;
				}
				else if (player.wingFrameCounter < 20)
				{
					player.wingFrame = 4;
				}
			}
			else
			{
				player.wingFrameCounter = 0;
				player.wingFrame = 0;
				if (player.velocity.Y != 0)
				{
					player.wingFrame = 1;
					if (player.controlJump && player.velocity.Y > 0)
						player.wingFrame = 3;
				}
			}
			return true;
        }
        public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 8f;
			acceleration *= 1.01f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CorrosiveGel>(), 8);
			recipe.AddIngredient(null, "Wormwood", 24);
			recipe.AddIngredient(ItemID.Feather, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class StarbeltPlayer : ModPlayer
	{
		public static readonly PlayerLayer GelWings = new PlayerLayer("SOTS", "GelWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo) {

			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Mod mod = ModLoader.GetMod("SOTS");
			if (drawPlayer.wings == mod.GetEquipSlot("GelWings", EquipType.Wings))
			{
				Texture2D texture = mod.GetTexture("Items/GelGear/GelWings_Wings2");
				int drawX = (int)(drawPlayer.position.X - Main.screenPosition.X);
				int drawY = (int)(drawPlayer.position.Y - Main.screenPosition.Y);
				Vector2 Position = drawInfo.position;
				Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 12);
				Vector2 pos = new Vector2((float)((int)(Position.X - Main.screenPosition.X + (float)(drawPlayer.width / 2) - (float)(9 * drawPlayer.direction))), (float)(Position.Y - Main.screenPosition.Y + (float)(drawPlayer.height / 2) - 6f * drawPlayer.gravDir));
				Color lightColor = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16, Color.White);
				Color color = TestWingsPlayer.changeColorBasedOnStealth(lightColor, drawPlayer) * (155f / 255f) * alpha;
				DrawData data = new DrawData(texture, pos, new Rectangle(0, texture.Height / 6 * drawPlayer.wingFrame, texture.Width, texture.Height / 6), color, 0f, origin, 1f, drawInfo.spriteEffects, 0);
				data.shader = drawInfo.wingShader;
				Main.playerDrawData.Add(data);
			}
		});
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int wings = layers.FindIndex(l => l == PlayerLayer.Wings);
			if (wings > -1)
			{
				layers.Insert(wings + 1, GelWings);
			}
		}
	}
}