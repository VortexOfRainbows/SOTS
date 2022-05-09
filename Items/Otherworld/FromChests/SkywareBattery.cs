using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using SOTS.Items.OreItems;
using SOTS.Items.Otherworld.Furniture;
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
			Tooltip.SetDefault("Increases void gain by 2 and max void by 50\nRegenerate void when hit\nImmunity to broken armor and ichor");
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 18;     
            Item.height = 32;
            Item.value = Item.sellPrice(0, 4, 50, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.player[Main.myPlayer];
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/SkywareBatteryEffect").Value;
			Main.spriteBatch.Draw(texture, position, frame, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), 0f, origin, scale, SpriteEffects.None, 0f);
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/SkywareBatteryGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/SkywareBatteryEffect").Value;
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + new Vector2(0, 2), null, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.bonusVoidGain += 2;
			voidPlayer.voidMeterMax2 += 50;

			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
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
			recipe.AddIngredient(ModContent.ItemType<PlatinumBattery>(), 1);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 8);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<GoldBattery>(), 1);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 8);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}