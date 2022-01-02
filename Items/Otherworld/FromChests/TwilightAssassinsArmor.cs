using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Fragments;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Void;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Otherworld.FromChests
{
	[AutoloadEquip(EquipType.Head)]
	public class TwilightAssassinsCirclet : ModItem
	{	
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
            item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 8;
		}
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
		{
			drawHair = true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Assassin Circlet");
			Tooltip.SetDefault("temp");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys()) //gets the key configured to this hotkey
			{
				foreach (TooltipLine line in tooltips) //goes through each tooltip line
				{
					if (line.mod == "Terraria" && line.Name == "Tooltip0")
					{
						line.text = "Increases your max number of minions and sentries by 1" +
							"\nIncreases minion and melee damage by 7%" +
							"\nIncreased max void by 50" +
							"\nProvides a Holo Eye minion to assist in combat" +
							"\nPress the " + "'" + key + "' key to make it launch a destabilizing beam that applies 4 levels of destabilized, but only once per enemy" +
							"\nDestabilized enemies gain a 5% flat chance to be critically striked" +
							"\nThis is calculated after normal crits, allowing some attacks to double crit" +
							"\nCosts 6 void to launch" +
							"\nHolo Eye remains active in the inventory when favorited or while worn in vanity slots, but cannot attack";
						return;
					}
				}
			}
			foreach (TooltipLine line in tooltips) //goes through each tooltip line
			{
				if (line.mod == "Terraria" && line.Name == "Tooltip0")
				{
					string key = "Unbound";
					line.text = "Increases your max number of minions and sentries by 1" +
						"\nIncreases minion and melee damage by 7%" +
						"\nIncreased max void by 50" +
						"\nProvides a Holo Eye minion to assist in combat" +
						"\nPress the " + "'" + key + "' key to make it launch a destabilizing beam that applies 4 levels of destabilized, but only once per enemy" +
						"\nDestabilized enemies gain a 5% flat chance to be critically striked" +
						"\nThis is calculated after normal crits, allowing some attacks to double crit" +
						"\nCosts 6 void to launch" +
						"\nHolo Eye remains active in the inventory when favorited or while worn in vanity slots, but cannot attack";
				}
			}
			base.ModifyTooltips(tooltips);
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("TwilightAssassinsChestplate") && legs.type == mod.ItemType("TwilightAssassinsLeggings");
        }
        public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			player.setBonus = "The Holo Eye now automatically attacks for you\nAutomatic attacks have a 25% chance to destabilize enemies, but the chance of applying it gets lower with each level already applied";
			modPlayer.HoloEyeAutoAttack = true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsCircletGlow");
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsCircletGlow");
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, Main.itemTexture[item.type].Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void UpdateEquip(Player player)
		{
			TwilightAssassinPlayer TWAP = player.GetModPlayer<TwilightAssassinPlayer>();
			TWAP.glowNum += 0.2f;
			player.maxMinions++;
			player.maxTurrets++;
			player.minionDamage += 0.07f;
			player.meleeDamage += 0.07f;
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidMeterMax2 += 50;
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			if(!modPlayer.HoloEye)
				modPlayer.HoloEyeDamage += (int)(33 * (1f + (player.minionDamage - 1f) + (player.allDamage - 1f)));
			modPlayer.HoloEye = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarlightAlloy>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 12);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAether>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class TwilightAssassinsChestplate : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.value = Item.sellPrice(0, 4, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 10;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = false;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Assassin Chestplate");
			Tooltip.SetDefault("Increased your max number of minions by 1\nIncreased melee and void critical strike chance by 10%\nIncreased life regeneration by 2 and void regeneration speed by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == mod.ItemType("TwilightAssassinsCirclet") && legs.type == mod.ItemType("TwilightAssassinsLeggings");
		}
		public override void UpdateEquip(Player player)
		{
			TwilightAssassinPlayer TWAP = player.GetModPlayer<TwilightAssassinPlayer>();
			TWAP.glowNum += 0.2f;
			player.maxMinions++;
			player.meleeCrit += 10;
			player.lifeRegen += 2;
			VoidPlayer voidPlayer = player.GetModPlayer<VoidPlayer>();
			voidPlayer.voidRegenSpeed += 0.1f;
			voidPlayer.voidCrit += 10;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsChestplateGlow");
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsChestplateGlow");
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, Main.itemTexture[item.type].Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HellstoneBar, 12);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 20);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAether>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class TwilightAssassinsLeggings : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 16;
			item.value = Item.sellPrice(0, 3, 60, 0);
			item.rare = ItemRarityID.LightPurple;
			item.defense = 9;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twilight Assassin Leggings");
			Tooltip.SetDefault("Increased movement speed by 15%\nBlink Pack decreased cooldown by 20%\nSlightly increased jump speed");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == mod.ItemType("TwilightAssassinsLeggings") && head.type == mod.ItemType("TwilightAssassinsCirclet");
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsLeggingsGlow");
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsLeggingsGlow");
			Color color = new Color(60, 70, 80, 0);
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, Main.itemTexture[item.type].Height * 0.5f);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y)) + addition, null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
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
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OtherworldlyAlloy>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HardlightAlloy>(), 16);
			recipe.AddIngredient(ModContent.ItemType<DissolvingAether>(), 1);
			recipe.AddTile(ModContent.TileType<HardlightFabricatorTile>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class TwilightAssassinPlayer : ModPlayer
	{
		public float glowNum = 0f;
        public override void ResetEffects()
		{
			if(glowNum > 0)
            {
				float alpha = 1 - player.shadow;
				Lighting.AddLight(player.Center, alpha * glowNum * new Vector3(200, 220, 255) / 255f);
			}
			glowNum = 0;
            base.ResetEffects();
        }
        public static readonly PlayerLayer TwilightGlowmaskChest = new PlayerLayer("SOTS", "TwilightGlowmaskChest", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");

			if (drawPlayer.body != mod.GetEquipSlot("TwilightAssassinsChestplate", EquipType.Body))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsChestplate_BodyGlow");
			if(!drawPlayer.Male)
				texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsChestplate_FemaleBodyGlow");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			for(int i = 0; i < 360; i += 30)
            {
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(texture, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.bodyArmorShader;
				Main.playerDrawData.Add(drawData);
			}
		});
		public static readonly PlayerLayer TwilightGlowmaskArms = new PlayerLayer("SOTS", "TwilightGlowmaskArms", PlayerLayer.Body, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");

			if (drawPlayer.body != mod.GetEquipSlot("TwilightAssassinsChestplate", EquipType.Body))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsChestplate_ArmsGlow");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(texture, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.bodyArmorShader;
				Main.playerDrawData.Add(drawData);
			}
		});
		public static readonly PlayerLayer TwilightGlowmaskHead = new PlayerLayer("SOTS", "TwilightGlowmaskHead", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");

			if (drawPlayer.head != mod.GetEquipSlot("TwilightAssassinsCirclet", EquipType.Head))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsCirclet_HeadGlow");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(texture, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.headArmorShader;
				Main.playerDrawData.Add(drawData);
			}
		});
		public static readonly PlayerLayer TwilightGlowmaskLegs = new PlayerLayer("SOTS", "TwilightGlowmaskLegs", PlayerLayer.Legs, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");

			if (drawPlayer.legs != mod.GetEquipSlot("TwilightAssassinsLeggings", EquipType.Legs))
			{
				return;
			}
			Texture2D texture = mod.GetTexture("Items/Otherworld/FromChests/TwilightAssassinsLeggings_LegsGlow");
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.legFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = new Color(60, 70, 80, 0) * 0.4f;
			color = TestWingsPlayer.changeColorBasedOnStealth(color, drawPlayer);
			Rectangle frame = drawPlayer.legFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				DrawData drawData = new DrawData(texture, position + addition, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
				drawData.shader = drawInfo.legArmorShader;
				Main.playerDrawData.Add(drawData);
			}
		});
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int bodyLayer = layers.FindIndex(l => l == PlayerLayer.Body);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, TwilightGlowmaskChest);
			}
			bodyLayer = layers.FindIndex(l => l == PlayerLayer.Arms);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, TwilightGlowmaskArms);
			}
			bodyLayer = layers.FindIndex(l => l == PlayerLayer.Head);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, TwilightGlowmaskHead);
			}
			bodyLayer = layers.FindIndex(l => l == PlayerLayer.Legs);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, TwilightGlowmaskLegs);
			}
		}
	}
}