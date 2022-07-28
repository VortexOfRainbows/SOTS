using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.OreItems;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SkywareBattery : ModItem	
	{	
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skyware Battery");
			Tooltip.SetDefault("Increases void gain by 2 and max void by 50\nRegenerate void when hit\nImmunity to broken armor and ichor");
			this.SetResearchCost(1);
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
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);

			texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/SkywareBatteryEffect").Value;
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0), rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
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
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PlatinumBattery>(), 1).AddIngredient(ItemID.ArmorPolish, 1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
			CreateRecipe(1).AddIngredient(ModContent.ItemType<GoldBattery>(), 1).AddIngredient(ItemID.ArmorPolish, 1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 8).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}