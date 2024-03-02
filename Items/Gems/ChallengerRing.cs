using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using SOTS.Items.Earth.Glowmoth;

namespace SOTS.Items.Gems
{
	public class ChallengerRing : ModItem
	{
		private Texture2D ChallengerGems = ModContent.Request<Texture2D>("SOTS/Items/Gems/ChallengerGems").Value;
        private string Glow => this.Texture + "Glow";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = ModContent.Request<Texture2D>(Glow).Value;
            for (int i = 0; i < 7; i++)
            {
                float counter = MathHelper.WrapAngle(MathHelper.ToRadians(SOTSWorld.GlobalCounter) + i * MathHelper.TwoPi / 7f);
                if (counter > 0 && counter < MathHelper.Pi)
                {
                    continue;
                }
                Vector2 circular = new Vector2(18, 0).RotatedBy(counter) * scale;
                float rotation = circular.ToRotation() + MathHelper.Pi / 2f;
                circular.Y *= 0.625f;
                circular = circular.RotatedBy(MathHelper.Pi / 4 * -1);
                Rectangle rect = new Rectangle(18 * i, 0, 16, 16);
                Main.spriteBatch.Draw(ChallengerGems, position + circular , rect, drawColor, rotation, new Vector2(8, 8), scale * 0.625f, SpriteEffects.None, 0f);
            }
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
            for (int i = 0; i < 7; i++)
            {
				float counter = MathHelper.WrapAngle(MathHelper.ToRadians(SOTSWorld.GlobalCounter) + i * MathHelper.TwoPi / 7f);
				if(counter > 0 && counter < MathHelper.Pi)
                {
                    Vector2 circular = new Vector2(18, 0).RotatedBy(counter) * scale;
                    float rotation = circular.ToRotation() + MathHelper.Pi / 2f;
                    circular.Y *= 0.625f;
                    circular = circular.RotatedBy(MathHelper.Pi / 4 * -1);
                    Rectangle rect = new Rectangle(18 * i, 0, 16, 16);
                    Main.spriteBatch.Draw(ChallengerGems, position + circular, rect, drawColor, rotation, new Vector2(8, 8), scale * 0.625f, SpriteEffects.None, 0f);
                }
            }
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
			/*foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					int defenseStat = SOTSPlayer.ModPlayer(Main.LocalPlayer).previousDefense;
					line.Text = Language.GetTextValue("Mods.SOTS.DiamondRingText");
                    line.Text += Language.GetTextValue("Mods.SOTS.DiamondRingText2", Convert.ToString(defenseStat), Convert.ToString(defenseStat / 3));
					return;
				}
			}
			base.ModifyTooltips(tooltips);*/
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
            //SOTSPlayer.ModPlayer(player).DevilRing = true;
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
    }
}