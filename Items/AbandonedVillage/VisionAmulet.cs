using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;
using Terraria.Localization;

namespace SOTS.Items.AbandonedVillage
{
	public class VisionAmulet : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/VisionAmuletSheet");
			frame = new Rectangle(38 * GetGem(unique), 44 * GetFrame(unique) + 2, 36, 40);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/VisionAmuletSheet");
			Rectangle frame = new Rectangle(38 * GetGem(unique), 44 * GetFrame(unique) + 2, 36, 40);
			Vector2 origin = Item.Size / 2;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 40;   
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
			Item.hasVanityEffects = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0") //checks the name of the tootip line
				{
					line.Text = GetTooltip(GetGem(unique), GetFrame(unique));
					return;
				}
			}
		}
        public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			GetBonuses(player, GetGem(unique), GetFrame(unique));
			if(!hideVisual)
				modPlayer.VisionVanity = true;
		}
		public int GetFrame(int unique)
        {
			return unique / 8;
        }
		public int GetGem(int unique)
		{
			return unique % 8;
		}
		public void GetBonuses(Player player, int gem, int frame)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			switch (gem)
            {
				case 0: //geo
					player.endurance += 0.1f;
					break;
				case 1: //electro
					player.maxMinions++;
					player.maxTurrets++;
					break;
				case 2: //anemo
					modPlayer.attackSpeedMod += 0.12f;
					break;
				case 3: //cyro
					player.GetCritChance(DamageClass.Generic) += 10;
					break;
				case 4: //pyro
					modPlayer.CritBonusMultiplier += 0.3f;
					break;
				case 5: //hydro
					modPlayer.additionalHeal += 40;
					player.lifeRegen += 2;
					break;
				case 6: //dendro
					player.statLifeMax2 += 20;
                    player.GetDamage(DamageClass.Generic) += 0.1f;
                    break;
				case 7: //masterless
					vPlayer.voidRegenSpeed += 0.2f;
					break;
			}
			switch(frame)
            {
				case 0: //liyue
					player.discountAvailable = true;
					break;
				case 1: //inazuma
					modPlayer.PotionBuffDegradeRate -= 0.2f;
					player.manaCost -= 0.1f;
					break;
				case 2: //mondstadt
					player.jumpSpeedBoost += 2f;
					player.moveSpeed += 0.1f;
					player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
					break;
				case 3: //Sumeru		
					modPlayer.LazyCrafterAmulet = true;
					modPlayer.additionalPotionMana += 40;
					player.statManaMax2 += 40;
					break;
                case 4: //Fontaine
					modPlayer.StatShareAll = true;
                    break;
            }
        }
		public string GetTooltip(int gem, int frame)
		{
			string text = Language.GetTextValue($"Mods.SOTS.VisionAmuletTextList.{gem}");
			text += Language.GetTextValue($"Mods.SOTS.VisionAmuletTextList2.{frame}");
			return text;
		}
	}
}

