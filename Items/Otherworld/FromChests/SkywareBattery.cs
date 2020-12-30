using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Void;
using System.Runtime.Remoting.Messaging;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SkywareBattery : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Battery");
			Tooltip.SetDefault("Increases void regen by 2 and max void by 50\nRegenerate void when hit\nImmunity to broken armor and ichor");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
            item.width = 18;     
            item.height = 32;
            item.value = Item.sellPrice(0, 4, 50, 0);
			item.rare = ItemRarityID.LightPurple;
			item.accessory = true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/SkywareBatteryEffect");
			Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/SkywareBatteryGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

			texture = mod.GetTexture("Items/Otherworld/FromChests/SkywareBatteryEffect");
			Main.spriteBatch.Draw(texture, item.Center - Main.screenPosition + new Vector2(0, 2), null, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.voidRegen += 0.2f;
			voidPlayer.voidMeterMax2 += 50;

			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			if (modPlayer.onhit == 1)
			{
				voidPlayer.voidMeter += 3 + (modPlayer.onhitdamage / 11);
				VoidPlayer.VoidEffect(player, 3 + (modPlayer.onhitdamage / 11));
			}
			player.buffImmune[BuffID.BrokenArmor] = true;
			player.buffImmune[BuffID.Ichor] = true;

			if (!hideVisual)
				modPlayer.rainbowGlowmasks = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PlatinumBattery", 1);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddIngredient(null, "StarlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "GoldBattery", 1);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddIngredient(null, "StarlightAlloy", 8);
			recipe.AddTile(mod.TileType("HardlightFabricatorTile"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}