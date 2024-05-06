using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.SOTS;
using Terraria.Localization;
using SOTS.Items.Chaos;
using System;

namespace SOTS.Items.Wings
{
	[AutoloadEquip(EquipType.Wings)]
	public class GildedBladeWings : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(7200, 9f, 2.5f); //These stats should closely mirror stats from the pillars
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (string key in SOTS.MachinaBoosterHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
					{
						line.Text = Language.GetTextValue("Mods.SOTS.Items.GildedBladeWings.Description", key);
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string Textkey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
					line.Text = Language.GetTextValue("Mods.SOTS.Items.GildedBladeWings.Description", Textkey);
				}
			}
			base.ModifyTooltips(tooltips);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWings").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWingsGlow").Value;
            Color color = new Color(110, 110, 110, 0);
            Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 4; k++)
			{
				Vector2 offset = Main.rand.NextVector2Circular(0.3f, 0.3f);
				Main.spriteBatch.Draw(texture2, position + offset, null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWings").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWingsGlow").Value;
            Color color = new Color(110, 110, 110, 0);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 4; k++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(0.3f, 0.3f);
                Main.spriteBatch.Draw(texture2, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + offset, null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 30;
			Item.value = Item.sellPrice(0, 20, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.expert = true;
			Item.accessory = true;
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (equippedItem.type == ModContent.ItemType<SpiritInsignia>() || 
				incomingItem.type == ModContent.ItemType<SpiritInsignia>() ||
				equippedItem.type == ModContent.ItemType<SpiritSymphony>() || 
				incomingItem.type == ModContent.ItemType<SpiritSymphony>())
                return false;
            return true;
        }
        public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<MachinaBooster>().AddIngredient<SpiritInsignia>().AddTile(TileID.TinkerersWorkbench).Register();
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
        {
			//Below are the boosts from the Spirit Insignia
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 3f;
            vPlayer.voidRegenSpeed += 0.25f;
            player.empressBrooch = true;
            player.moveSpeed += 0.1f;
            modPlayer.SpiritSymphony = true;


            MachinaBoosterPlayer MachinaBoosterPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
			MachinaBoosterPlayer.canCreativeFlight = true;
			player.wingTimeMax = 7200; //May as well have 2 minutes worth of base-time
			player.noFallDmg = true;
            //voidPlayer.bonusVoidGain += 1f;
        }
        public override bool WingUpdate(Player player, bool inUse)
		{
			MachinaBoosterPlayer MachinaBoosterPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
			if (MachinaBoosterPlayer.creativeFlight)
			{
				player.wingFrame = 2;
			}
			else if ((player.controlJump && player.velocity.Y != 0f) || player.velocity.Y != 0f)
			{
				player.wingFrame = 1;
			}
			else
			{
				player.wingFrame = 0;
			}
			return true;
		}
		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
			//These stats are ripped stright from solar wings
            float num2 = 0.85f;
            float num5 = 0.15f;
            float num4 = 1f;
            float num3 = 3f;
            float num = 0.135f;
            ascentWhenFalling = num2;
			ascentWhenRising = num5;
			maxCanAscendMultiplier = num4;
			maxAscentMultiplier = num3;
			constantAscend = num;
		}
	}
}