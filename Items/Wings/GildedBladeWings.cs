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
using Microsoft.Xna.Framework.Input;

namespace SOTS.Items.Wings
{
	[AutoloadEquip(EquipType.Wings)]
	public class GildedBladeWings : ModItem
	{ 
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(300, 9f, 2.5f); //These stats should closely mirror stats from the pillars
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips) //goes through each tooltip line
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
                {
                    string Textkey1 = Language.GetTextValue("Mods.SOTS.Common.Unbound");
                    string Textkey2 = Textkey1;
                    foreach (string key in MachinaBoosterHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
					{
						Textkey1 = key;
						break;
                    }
                    foreach (string key in SlowFlightHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
                    {
                        Textkey2 = key;
                        break;
                    }
                    line.Text = Language.GetTextValue("Mods.SOTS.Items.GildedBladeWings.Description", Textkey1, Textkey2);
                }
            }
			base.ModifyTooltips(tooltips);
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWings").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWingsGlow").Value;
            Color color = new Color(110, 110, 110, 0) * 0.25f;
            DrawHalo(spriteBatch, position, scale, 0f);
            Main.spriteBatch.Draw(texture, new Vector2(position.X, position.Y), null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 8; k++)
			{
				Vector2 offset = new Vector2(2, 0).RotatedBy(k * MathHelper.PiOver4 + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2));
				Main.spriteBatch.Draw(texture2, position + offset, null, color * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWings").Value;
            Texture2D texture2 = Mod.Assets.Request<Texture2D>("Items/Wings/GildedBladeWingsGlow").Value;
            Color color = new Color(110, 110, 110, 0) * 0.25f;
            Vector2 drawPos = new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y));
            DrawHalo(spriteBatch, drawPos, scale, rotation);
            Main.spriteBatch.Draw(texture, drawPos, null, lightColor * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			for (int k = 0; k < 8; k++)
            {
                Vector2 offset = new Vector2(2, 0).RotatedBy(k * MathHelper.PiOver4 + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2));
                Main.spriteBatch.Draw(texture2, drawPos + offset, null, color * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
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
			MachinaBoosterPlayer.canCreativeFlight = MachinaBoosterPlayer.CreativeFlightTier2 = true;
			player.wingTimeMax = 300;
			player.noFallDmg = true;
            //voidPlayer.bonusVoidGain += 1f;
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
        private void DrawHalo(SpriteBatch spriteBatch, Vector2 position, float scale, float s_rotation)
        {
            List<DrawData> drawData0 = new List<DrawData>();
            List<DrawData> drawData1 = new List<DrawData>();
            List<DrawData> drawData2 = new List<DrawData>();
            Texture2D pixel = Mod.Assets.Request<Texture2D>("Items/Secrets/WhitePixel", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            int repeats = 40;
            Vector2 center = position;
            Vector2[] points = new Vector2[repeats];
            Vector2[] points2 = new Vector2[repeats / 2];
            Vector2 bonusPoint = Vector2.Zero;
            for (int i = 0; i < repeats; i++)
            {
                float rotation = i / (float)repeats * MathHelper.TwoPi + MathHelper.ToRadians(SOTSWorld.GlobalCounter * 0.75f) + s_rotation;
                float offset = 20;
                int pointN = i % repeats;
                float offsetBonus = 0;
                if (pointN == 39)
                    offsetBonus += 18;
                if (pointN == 2)
                    offsetBonus += 14;
                if (pointN == 5)
                    offsetBonus += 14;
                if (pointN == 10)
                    offsetBonus += 10;
                if (pointN == 14)
                    offsetBonus += 20;
                if (pointN == 17)
                    offsetBonus += 14;
                if (pointN == 20)
                    offsetBonus += 8;
                if (pointN == 27)
                    offsetBonus += 10;
                if (pointN == 31)
                    offsetBonus += 16;
                offset += offsetBonus * (0.7f + 0.3f * MathF.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter * (2 + i / 40f) + i * 24))) * 0.4f;

                if (i % 2 == 0)
                {
                    Vector2 innerCircular = new Vector2(16 * scale, 0).RotatedBy(rotation);
                    points2[i / 2] = center + innerCircular;
                }

                Vector2 circular = new Vector2(offset * scale, 0).RotatedBy(rotation);
                points[i] = center + circular;
                if (pointN == 37)
                {
                    Vector2 bonusPos = new Vector2(5 * scale, 0).RotatedBy(rotation);
                    bonusPoint = points[i] + bonusPos;
                }
            }
            Vector2 previous = points[points.Length - 1];
            for (int i = 0; i < points.Length; i++)
            {
                Color color = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2) + i / (float)repeats * MathHelper.TwoPi, true), 0.8f);
                Color color2 = color * 1.1f;
                color.A = 0;
                Vector2 current = points[i];
                Vector2 toPrevious = previous - current;
                drawData0.Add(new DrawData(pixel, points[i], null, color, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, 0.8f * scale), SpriteEffects.None, 0));
                drawData2.Add(new DrawData(pixel, points[i], null, color2, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scale) + Vector2.One * 0.6f, SpriteEffects.None, 0));
                previous = current;
            }
            previous = points2[points2.Length - 1];
            for (int i = 0; i < points2.Length; i++)
            {
                Color color = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * -1) + i / (float)repeats * MathHelper.TwoPi * 2, true), 0.8f);
                Color color2 = color * 1.1f;
                color.A = 0;
                Vector2 current = points2[i];
                Vector2 toPrevious = previous - current;
                drawData1.Add(new DrawData(pixel, points2[i], null, color, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, 0.8f * scale), SpriteEffects.None, 0));
                drawData2.Add(new DrawData(pixel, points2[i], null, color2, toPrevious.ToRotation(), new Vector2(0, 1), new Vector2(toPrevious.Length() / 2, scale) + Vector2.One * 0.6f, SpriteEffects.None, 0));
                previous = current;
            }
            Color finalColor2 = Color.Lerp(Color.Black, ColorHelpers.pastelAttempt(MathHelper.ToRadians(SOTSWorld.GlobalCounter * 2) + 37f / (float)repeats * MathHelper.TwoPi, true), 0.8f);
            Color finalColor3 = finalColor2;
            finalColor2.A = 0;
            drawData0.Add(new DrawData(pixel, bonusPoint, null, finalColor2, 0f, new Vector2(1, 1), 1.2f * scale, SpriteEffects.None, 0));
            drawData2.Add(new DrawData(pixel, bonusPoint, null, finalColor3, 0f, new Vector2(1, 1), 1.8f * scale, SpriteEffects.None, 0));
            for (int i = 0; i < drawData2.Count; i++)
            {
                DrawData data = drawData2[i];
                spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0f);
            }
            for (int i = 0; i < drawData1.Count; i++)
            {
                DrawData data = drawData1[i];
                spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0f);
            }
            for (int i = 0; i < drawData0.Count; i++)
            {
                DrawData data = drawData0[i];
                spriteBatch.Draw(data.texture, data.position, data.sourceRect, data.color, data.rotation, data.origin, data.scale, data.effect, 0f);
            }
        }
    }
}