using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI.Chat;

namespace SOTS.Items.Gems
{
    public class ChallengerRing : ModItem
    {
        private const float orbitSpeed = -0.8f;
        private bool[] GemDisabled = new bool[7];
        public override void SaveData(TagCompound tag)
        {
            tag["Gems"] = GemDisabled;
        }
        public override void LoadData(TagCompound tag)
        {
            bool[] outBools;
            if(tag.TryGet<bool[]>("Gems", out outBools))
                GemDisabled = outBools;
        }
        private Texture2D ChallengerGems = ModContent.Request<Texture2D>("SOTS/Items/Gems/ChallengerGems").Value;
        private Texture2D InvertedChallengerGems = ModContent.Request<Texture2D>("SOTS/Items/Gems/InvertedChallengerGems").Value;
        private string Glow => this.Texture + "Glow";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Glow).Value;
            DrawOrbitalsInInventory(spriteBatch, position, drawColor, scale, false);
            position -= new Vector2(2, 2) * scale;
            for (int k = 0; k < 6; k++)
            {
                Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSWorld.GlobalCounter * 2));
                Color color = new Color(120 - k * 7, 110 - k * 2, 100 + k * 4, 0);
                Main.spriteBatch.Draw(texture, position + circular, null, color * (1f - (Item.alpha / 255f)) * 1.2f, 0f, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(Terraria.GameContent.TextureAssets.Item[Type].Value, position, null, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = ModContent.Request<Texture2D>(Glow).Value;
            Main.spriteBatch.Draw(texture, position, null, Color.Lerp(drawColor, Color.White, 0.5f), 0f, origin, scale, SpriteEffects.None, 0f);
            DrawOrbitalsInInventory(spriteBatch, position, drawColor, scale, true);
        }
        private void DrawOrbitalsInInventory(SpriteBatch spriteBatch, Vector2 position, Color drawColor, float scale, bool front = true)
        {
            for (int i = 0; i < 7; i++)
            {
                float counter = MathHelper.WrapAngle(MathHelper.ToRadians(SOTSWorld.GlobalCounter * orbitSpeed) + i * MathHelper.TwoPi / 7f);
                bool active = (counter > 0 && counter < MathHelper.Pi) == front;
                if (active)
                {
                    Vector2 circular = new Vector2(18, 0).RotatedBy(counter) * scale;
                    float radiansCheck = counter - MathHelper.PiOver2;
                    float maxRadians = MathHelper.Pi / 7f;
                    float bonusAlpha = 1 - Math.Abs(radiansCheck) / maxRadians;
                    float rotation = circular.ToRotation() + MathHelper.Pi / 2f;
                    float zScale = 1 + circular.Y / circular.Length() / 5f;
                    circular.Y *= 0.625f;
                    circular = circular.RotatedBy(MathHelper.Pi / 4 * -1);
                    Rectangle rect = new Rectangle(0, 16 * i, 16, 16);
                    Texture2D texture = GemDisabled[i] ? InvertedChallengerGems : ChallengerGems;
                    bonusAlpha = Math.Clamp(bonusAlpha, 0.25f, 1f);
                    float radialMult = (float)Math.Sqrt(bonusAlpha);
                    for (int k = 0; k < 8; k++)
                    {
                        Vector2 bonus = new Vector2(0, 3 * scale * radialMult).RotatedBy(MathHelper.ToRadians(k * 45 + SOTSWorld.GlobalCounter * 2));
                        spriteBatch.Draw(ChallengerGems, position + circular + bonus, rect, new Color(200, 200, 200, 0) * bonusAlpha, rotation, new Vector2(8, 8), scale * 0.60f * zScale, SpriteEffects.None, 0f);
                    }
                    spriteBatch.Draw(texture, position + circular, rect, drawColor, rotation, new Vector2(8, 8), scale * 0.60f * zScale, SpriteEffects.None, 0f);
                }
            }
        }
        private int SimulateSelectedGem()
        {
            int selectedGem = 0;
            for (int i = 0; i < 7; i++) ///Theres a better way to do this than using a loop. However, I cannot be ass to do the math right now.
            {
                float counter = MathHelper.WrapAngle(MathHelper.ToRadians(SOTSWorld.GlobalCounter * orbitSpeed) + i * MathHelper.TwoPi / 7f);
                float radiansCheck = counter - MathHelper.PiOver2;
                float maxRadians = MathHelper.Pi / 7f;
                if (radiansCheck > -maxRadians && radiansCheck < maxRadians)
                {
                    selectedGem = i;
                    break;
                }
            }
            return selectedGem;
        }
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Glow).Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			for (int k = 0; k < 6; k++)
			{
				Vector2 circular = new Vector2(0, 2).RotatedBy(MathHelper.ToRadians(k * 60 + SOTSWorld.GlobalCounter * 2));
				Color color = new Color(120 - k * 7, 110 - k * 2, 100 + k * 4, 0);
				Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + circular, null, color * (1f - (Item.alpha / 255f)) * 1.2f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
			return true;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Glow).Value;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.Lerp(lightColor, Color.White, 0.5f), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int selected = SimulateSelectedGem();
            float colorSwap = 0.4f;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if(line.Mod == "Terraria")
                {
                    if (line.Name == "Tooltip1") //checks the name of the tootip line
                    {
                        string bonus = selected == 0 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.AmethystColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G0");
                    }
                    if (line.Name == "Tooltip2")
                    {
                        string bonus = selected == 1 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.TopazColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G1");
                    }
                    if (line.Name == "Tooltip3")
                    {
                        string bonus = selected == 2 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.SapphireColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G2");
                    }
                    if (line.Name == "Tooltip4")
                    {
                        string bonus = selected == 3 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.EmeraldColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G3");
                    }
                    if (line.Name == "Tooltip5")
                    {
                        string bonus = selected == 4 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.RubyColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G4");
                        if (selected == 4)
                            line.Text = line.Text.Replace("\n", "\n" + bonus);
                    }
                    if (line.Name == "Tooltip6")
                    {
                        string bonus = selected == 5 ? "> " : "";
                        int defenseStat = SOTSPlayer.ModPlayer(Main.LocalPlayer).previousDefense;
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.DiamondColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G5", Convert.ToString(defenseStat), Convert.ToString(defenseStat / 3));
                        if (selected == 5)
                            line.Text = line.Text.Replace("\n", "\n" + bonus);
                    }
                    if (line.Name == "Tooltip7")
                    {
                        string bonus = selected == 6 ? "> " : "";
                        line.OverrideColor = Color.Lerp(Color.White, ColorHelpers.AmberColor, colorSwap);
                        line.Text = bonus + Language.GetTextValue("Mods.SOTS.Items.ChallengerRing.G6");
                    }
                }
			}
			base.ModifyTooltips(tooltips);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 28;     
            Item.height = 28;   
            Item.value = Item.sellPrice(0, 15, 0, 0);
            Item.rare = ModContent.RarityType<PastelRainbowRarity>();
            Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SOTSPlayer sPlayer = player.SOTSPlayer();
            sPlayer.AmethystRing = !GemDisabled[0];
            sPlayer.TopazRing = !GemDisabled[1];
            if(!GemDisabled[2])
                player.VoidPlayer().VoidGenerateMoney += 1f; 
            sPlayer.EmeraldRing = !GemDisabled[3];
            sPlayer.RubyRing = !GemDisabled[4];
            sPlayer.DiamondRing = !GemDisabled[5];
            sPlayer.AmberRing = !GemDisabled[6];
        }
        public override void AddRecipes()
        {
            CreateRecipe(1)
				.AddIngredient<AmethystRing>()
				.AddIngredient<TopazRing>()
				.AddIngredient<SapphireRing>()
				.AddIngredient<EmeraldRing>()
				.AddIngredient<RubyRing>()
				.AddIngredient<DiamondRing>()
				.AddIngredient<AmberRing>()
				.AddTile(TileID.TinkerersWorkbench).Register();
        }
        public override void RightClick(Player player)
        {
            int gemToModify = SimulateSelectedGem();
            GemDisabled[gemToModify] = !GemDisabled[gemToModify];
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override bool ConsumeItem(Player player)
        {
            return false;
        }
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            int selected = SimulateSelectedGem();
            bool thisLineIsSelected = false;
            bool invertColors = false;
            if (line.Name == "Tooltip1") //amethyst
            {
                thisLineIsSelected = selected == 0;
                invertColors = GemDisabled[0];
            }
            if (line.Name == "Tooltip2")
            {
                thisLineIsSelected = selected == 1;
                invertColors = GemDisabled[1];
            }
            if (line.Name == "Tooltip3")
            {
                thisLineIsSelected = selected == 2;
                invertColors = GemDisabled[2];
            }
            if (line.Name == "Tooltip4")
            {
                thisLineIsSelected = selected == 3;
                invertColors = GemDisabled[3];
            }
            if (line.Name == "Tooltip5")
            {
                thisLineIsSelected = selected == 4;
                invertColors = GemDisabled[4];
            }
            if (line.Name == "Tooltip6")
            {
                thisLineIsSelected = selected == 5;
                invertColors = GemDisabled[5];
            }
            if (line.Name == "Tooltip7")
            {
                thisLineIsSelected = selected == 6;
                invertColors = GemDisabled[6];
            }
            int xOffset = 0;
            if(thisLineIsSelected)
            {
                xOffset = -19;
            }
            Color outer = invertColors ? line.OverrideColor ?? line.Color : Color.Black;
            Color inner = !invertColors ? line.OverrideColor ?? line.Color : Color.Black;
            TextSnippet[] snippets = ChatManager.ParseMessage(line.Text, inner).ToArray();
            ChatManager.ConvertNormalSnippets(snippets);
            ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, line.Font, line.Text, new Vector2(line.X + xOffset, line.Y), outer, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
            int outSnip;
            ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, snippets, new Vector2(line.X + xOffset, line.Y), inner, line.Rotation, line.Origin, line.BaseScale, out outSnip, line.MaxWidth);
            return false;
        }
    }
}