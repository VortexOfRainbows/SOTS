using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using SOTS.Void;

namespace SOTS.Items.GhostTown
{
	public class VisionAmulet : ModItem
	{
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/GhostTown/VisionAmuletSheet");
			frame = new Rectangle(38 * GetGem(unique), 38 * GetFrame(unique), 36, 38);
			spriteBatch.Draw(texture, position, frame, drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Player player = Main.LocalPlayer;
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			int unique = modPlayer.UniqueVisionNumber;
			Texture2D texture = (Texture2D)ModContent.Request<Texture2D>("SOTS/Items/GhostTown/VisionAmuletSheet");
			Rectangle frame = new Rectangle(38 * GetGem(unique), 38 * GetFrame(unique), 36, 38);
			Vector2 origin = Item.Size / 2;
			spriteBatch.Draw(texture, Item.Center - Main.screenPosition, frame, lightColor, rotation, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Amulet");
			Tooltip.SetDefault("Epic Gamer\n'Resonates with your ambitions'");
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
            Item.width = 36;     
            Item.height = 38;   
            Item.value = Item.sellPrice(gold: 10);
            Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
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
					player.GetDamage(DamageClass.Generic) += 0.1f;
					break;
				case 2: //anemo
					modPlayer.attackSpeedMod += 0.12f;
					break;
				case 3: //cyro
					player.GetCritChance(DamageClass.Magic) += 10;
					player.GetCritChance(DamageClass.Melee) += 10;
					player.GetCritChance(DamageClass.Ranged) += 10;
					player.GetCritChance(DamageClass.Throwing) += 10;
					break;
				case 4: //pyro
					modPlayer.CritBonusMultiplier += 0.3f;
					break;
				case 5: //hydro
					modPlayer.additionalHeal += 40;
					player.lifeRegen += 2;
					break;
				case 6: //dendro
					player.statLifeMax2 += 80;
					break;
				case 7: //masterless
					vPlayer.voidRegenSpeed += 0.2f;
					break;
			}
			switch(frame)
            {
				case 0: //liyue
					player.discount = true;
					break;
				case 1: //inazuma
					modPlayer.inazumaLongerPotions = true;
					player.manaCost -= 0.1f;
					break;
				case 2: //mondstadt
					player.jumpSpeedBoost += 2f;
					player.moveSpeed += 0.1f;
					player.GetAttackSpeed(DamageClass.Melee) += 0.1f;
					break;
			}
        }
		public string GetTooltip(int gem, int frame)
		{
			string text = "";
			switch (gem)
			{
				case 0: //geo
					text += "Reduces damage taken by 10%";
					break;
				case 1: //electro
					text += "Increases max minions and sentries by 1\nIncreases damage by 10%";
					break;
				case 2: //anemo
					text += "Increases attack speed by 12%";
					break;
				case 3: //cyro
					text += "Increases critical strike chance by 10%";
					break;
				case 4: //pyro
					text += "Increases critical strike damage by 30%";
					break;
				case 5: //hydro
					text += "Increases healing recieved from potions by 40\nIncreases life regeneration by 2";
					break;
				case 6: //dendro
					text += "Increases max life by 80";
					break;
				case 7: //masterless
					text += "Increases void regeneration speed by 20%";
					break;
			}
			switch (frame)
			{
				case 0: //liyue
					text += "\nShops prices lowered by 20%";
					break;
				case 1: //inazuma
					text += "\nIncreases potion duration by 20%\nDecreases mana cost by 10%";
					break;
				case 2: //mondstadt
					text += "\nIncreases melee speed and movement speed by 10%\nIncreases jump speed";
					break;
			}
			return text;
		}
	}
}

