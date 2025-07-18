using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;
using Terraria.Localization;
using Terraria.DataStructures;

namespace SOTS.Items.Slime
{
	public class ExperimentalInjection : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Terraria.GameContent.TextureAssets.Item[Type].Value;
			Player player = Main.LocalPlayer;
            SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			frame = new(0, 44 * GetGem(modPlayer.FirstTwoChars() % 4), 42, 44);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Type].Value;
            Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			Rectangle frame = new(0, 44 * GetGem(modPlayer.FirstTwoChars() % 4), 42, 44);
			Vector2 origin = Item.Size / 2;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, Color.White, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(120, 4));
            this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 42;     
            Item.height = 44;   
            Item.value = Item.sellPrice(0, 2, 50, 0);
            Item.rare = ItemRarityID.Green;
			Item.accessory = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
            foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					line.Text = GetTooltip(GetGem(player.SOTSPlayer().FirstTwoChars() % 4));
					return;
				}
			}
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			GetBonuses(player, modPlayer.FirstTwoChars() % 4);
		}
		public static int GetGem(int unique)
		{
			return unique % 4;
		}
		public static void GetBonuses(Player player, int gem)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			switch (gem)
            {
				case 0: //red
					modPlayer.FirstStrikeEffect = 1;
					player.statLifeMax2 += 20;
					break;
				case 1: //blue
					modPlayer.FirstStrikeEffect = 2;
                    modPlayer.CritBonusMultiplier += 0.2f;
                    break;
				case 2: //green
					modPlayer.FirstStrikeEffect = 3;
                    break;
				case 3: //yellow
                    player.GetDamage(DamageClass.Generic) += 0.05f;
					player.SOTSPlayer().attackSpeedMod += 0.05f;
					player.moveSpeed += 0.05f;
                    break;
			}
        }
		public static string GetTooltip(int variant)
		{
			string text = Language.GetTextValue($"Mods.SOTS.ExperimentalInjectionTextList.{variant}");
			return text;
		}
	}
}

