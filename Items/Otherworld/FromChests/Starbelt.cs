using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

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
			Item.maxStack = 1;
            Item.width = 34;     
            Item.height = 26;
            Item.value = Item.sellPrice(0, 3, 75, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/StarbeltGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			player.GetCritChance(DamageClass.Magic) += 10;
			player.statManaMax2 += 40;
			modPlayer.CritManasteal += 6 + Main.rand.Next(5);
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.ManaCrystal, 1).AddIngredient(null, "DissolvingAether", 1).AddIngredient(null, "StarlightAlloy", 8).AddTile(Mod.Find<ModTile>("HardlightFabricatorTile").Type).Register();
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/Starbelt_WaistGlow").Value;
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