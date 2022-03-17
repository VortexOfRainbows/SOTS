using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;

namespace SOTS.Items.Chaos
{
	public class VoidmageIncubator : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = ModContent.GetTexture("SOTS/Items/Chaos/VoidmageIncubatorSheet");
			frame = new Rectangle(0, 48 * GetGem(unique) + 2, 24, 44);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = ModContent.GetTexture("SOTS/Items/Chaos/VoidmageIncubatorSheet");
			Rectangle frame = new Rectangle(0, 48 * GetGem(unique) + 2, 24, 44);
			Vector2 origin = item.Size / 2;
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Voidmage Incubator");
			Tooltip.SetDefault("Epic Gamer\nIncreases void gain by 10 and void regeneration speed by 10%\nGetting hit will freeze time, converting void into health for the duration\nIncreases the potency of Void Shock and Void Recovery");
		}
		public override void SetDefaults()
		{
            item.width = 24;     
            item.height = 44;   
            item.value = Item.sellPrice(gold: 20);
            item.rare = ItemRarityID.Yellow;
			item.accessory = true;
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					line.text = GetTooltip(GetGem(unique));
					return;
				}
			}
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			GetBonuses(player, GetGem(unique));
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.bonusVoidGain += 10f;
			vPlayer.voidRegenSpeed += 0.1f;
			modPlayer.VMincubator = true;
			modPlayer.TimeFreezeImmune = true;
		}
		public int GetGem(int unique)
		{
			return unique % 8;
		}
		public void GetBonuses(Player player, int gem)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			switch (gem)
            {
				case 0: //geo, earth
					player.endurance += 0.05f;
					break;
				case 1: //electro, evil
					player.allDamage += 0.08f;
					break;
				case 2: //anemo, otherworld
					player.moveSpeed += 0.1f;
					player.jumpSpeedBoost += 1.0f;
					break;
				case 3: //cyro, permafrost
					modPlayer.CritBonusMultiplier += 0.2f;
					break;
				case 4: //pyro, inferno
					player.magicCrit += 6;
					player.meleeCrit += 6;
					player.rangedCrit += 6;
					player.thrownCrit += 6;
					break;
				case 5: //hydro, tidal
					player.statLifeMax2 += 40;
					break;
				case 6: //dendro, nature
					player.lifeRegen += 2;
					break;
				case 7: //masterless, chaos
					vPlayer.voidMeterMax2 += 40;
					break;
			}
        }
		public string GetTooltip(int gem)
		{
			string text = "";
			switch (gem)
			{
				case 0: //geo
					text += "Reduces damage taken by 5%";
					break;
				case 1: //electro
					text += "Increases damage by 8%";
					break;
				case 2: //anemo
					text += "Increases movement speed by 10%\nIncreases jump speed";
					break;
				case 3: //cyro
					text += "Increases critical strike damage by 20%";
					break;
				case 4: //pyro
					text += "Increases critical strike chance by 6%";
					break;
				case 5: //hydro
					text += "Increases max life by 40";
					break;
				case 6: //dendro
					text += "Increases life regeneration by 2";
					break;
				case 7: //masterless
					text += "Increases max void by 40";
					break;
			}
			return text;
		}
	}
}

