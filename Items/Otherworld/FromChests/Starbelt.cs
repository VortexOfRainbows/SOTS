using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Void;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Waist)]
	public class Starbelt : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starbelt");
			Tooltip.SetDefault("Critical strikes recover mana\nIncreased max mana by 40\n10% increased magic crit chance");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 34;     
            item.height = 26;
            item.value = Item.sellPrice(0, 3, 75, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/StarbeltGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			player.magicCrit += 10;
			player.statManaMax2 += 40;
			modPlayer.CritManasteal += 6 + Main.rand.Next(5);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(null, "DissolvingAether", 1);
			recipe.AddIngredient(null, "StarlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class StarbeltPlayer : ModPlayer
    {
		public static readonly PlayerLayer StarbeltGlowmask = new PlayerLayer("SOTS", "StarbeltGlowmask", PlayerLayer.WaistAcc, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");

			if (drawPlayer.waist != mod.GetEquipSlot("Starbelt", EquipType.Waist))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/Starbelt_WaistGlow");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = Color.White;
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			DrawData drawData = new DrawData(texture, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.waistShader;
			Main.playerDrawData.Add(drawData);
		});
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int waistLayer = layers.FindIndex(l => l == PlayerLayer.WaistAcc);

			if (waistLayer > -1)
			{
				layers.Insert(waistLayer + 1, StarbeltGlowmask);
			}
		}
	}
}