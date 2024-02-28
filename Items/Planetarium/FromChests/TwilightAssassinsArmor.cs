using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Wings;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace SOTS.Items.Planetarium.FromChests
{
	[AutoloadEquip(EquipType.Head)]
	public class TwilightAssassinsCirclet : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
			ArmorIDs.Head.Sets.DrawFullHair[equipSlotHead] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 8;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.Mod == "Terraria" && line.Name == "Tooltip0")
					{
						line.Text = Language.GetTextValue("Mods.SOTS.TwilightAssassinsCircletText", key);
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string Textkey = Language.GetTextValue("Mods.SOTS.Common.Unbound");
					line.Text = Language.GetTextValue("Mods.SOTS.TwilightAssassinsCircletText2", Textkey);
				}
			}
			base.ModifyTooltips(tooltips);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == Mod.Find<ModItem>("TwilightAssassinsChestplate") .Type&& legs.type == Mod.Find<ModItem>("TwilightAssassinsLeggings").Type;
        }
        public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			player.setBonus = Language.GetTextValue("Mods.SOTS.ArmorSetBonus.TwilightAssassins");
			modPlayer.HoloEyeAutoAttack = true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsCircletGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsCircletGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Item[Item.type].Value.Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void UpdateEquip(Player player)
		{
			TwilightAssassinPlayer TWAP = player.GetModPlayer<TwilightAssassinPlayer>();
			TWAP.glowNum += 0.2f;
			player.maxMinions++;
			player.maxTurrets++;
			player.GetDamage(DamageClass.Summon) += 0.07f;
			player.GetDamage(DamageClass.Melee) += 0.07f;
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidMeterMax2 += 50;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			if(!modPlayer.HoloEye)
				modPlayer.HoloEyeDamage += SOTSPlayer.ApplyDamageClassModWithGeneric(player, DamageClass.Summon, 33);
			modPlayer.HoloEye = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<StarlightAlloy>(), 12).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 12).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class TwilightAssassinsChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
			SetupDrawing();
		}
		private void SetupDrawing()
		{
			if (Main.netMode == NetmodeID.Server)
				return;
			int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
			ArmorIDs.Body.Sets.HidesHands[equipSlotBody] = false;
			ArmorIDs.Body.Sets.showsShouldersWhileJumping[equipSlotBody] = true;
			ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
		}
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == Mod.Find<ModItem>("TwilightAssassinsCirclet") .Type&& legs.type == Mod.Find<ModItem>("TwilightAssassinsLeggings").Type;
		}
		public override void UpdateEquip(Player player)
		{
			TwilightAssassinPlayer TWAP = player.GetModPlayer<TwilightAssassinPlayer>();
			TWAP.glowNum += 0.2f;
			player.maxMinions++;
			player.GetCritChance(DamageClass.Melee) += 10;
			player.lifeRegen += 2;
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidRegenSpeed += 0.1f;
			player.GetCritChance<VoidGeneric>() += 10;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsChestplateGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsChestplateGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Item[Item.type].Value.Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 6).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 20).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class TwilightAssassinsLeggings : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 16;
			Item.value = Item.sellPrice(0, 3, 60, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 9;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("TwilightAssassinsLeggings") .Type&& head.type == Mod.Find<ModItem>("TwilightAssassinsCirclet").Type;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsLeggingsGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Planetarium/FromChests/TwilightAssassinsLeggingsGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Terraria.GameContent.TextureAssets.Item[Item.type].Value.Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void UpdateEquip(Player player)
		{
			TwilightAssassinPlayer TWAP = player.GetModPlayer<TwilightAssassinPlayer>();
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			TWAP.glowNum += 0.2f;
			player.jumpSpeedBoost += 1.75f;
			player.moveSpeed += 0.15f;
			modPlayer.blinkPackMult -= 0.2f;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 12).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 16).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
	public class TwilightAssassinPlayer : ModPlayer
	{
		public float glowNum = 0f;
        public override void ResetEffects()
		{
			if(glowNum > 0)
            {
				float alpha = 1 - Player.stealth;
				Lighting.AddLight(Player.Center, alpha * glowNum * new Vector3(200, 220, 255) / 255f);
			}
			glowNum = 0;
        }
	}
}