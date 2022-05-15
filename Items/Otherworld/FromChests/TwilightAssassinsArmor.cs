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
			Item.width = 30;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 8;
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
					if (line.Mod == "Terraria" && line.Name == "Tooltip0")
					{
						line.Text = "Increases your max number of minions and sentries by 1" +
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
				if (line.Mod == "Terraria" && line.Name == "Tooltip0")
				{
					string key = "Unbound";
					line.Text = "Increases your max number of minions and sentries by 1" +
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
            return body.type == Mod.Find<ModItem>("TwilightAssassinsChestplate") .Type&& legs.type == Mod.Find<ModItem>("TwilightAssassinsLeggings").Type;
        }
        public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			player.setBonus = "The Holo Eye now automatically attacks for you\nAutomatic attacks have a 25% chance to destabilize enemies, but the chance of applying it gets lower with each level already applied";
			modPlayer.HoloEyeAutoAttack = true;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsCircletGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsCircletGlow").Value;
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
				modPlayer.HoloEyeDamage += (int)(33 * (1f + (player.GetDamage(DamageClass.Summon) - 1f) + (player.allDamage - 1f)));
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
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 22;
			Item.value = Item.sellPrice(0, 4, 0, 0);
			Item.rare = ItemRarityID.LightPurple;
			Item.defense = 10;
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
			voidPlayer.voidCrit += 10;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsChestplateGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsChestplateGlow").Value;
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
			CreateRecipe(1).AddIngredient(ItemID.HellstoneBar, 12).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 20).AddIngredient(ModContent.ItemType<DissolvingAether>(), 1).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
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
			DisplayName.SetDefault("Twilight Assassin Leggings");
			Tooltip.SetDefault("Increased movement speed by 15%\nBlink Pack decreased cooldown by 20%\nSlightly increased jump speed");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == Mod.Find<ModItem>("TwilightAssassinsLeggings") .Type&& head.type == Mod.Find<ModItem>("TwilightAssassinsCirclet").Type;
		}
		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsLeggingsGlow").Value;
			Color color = new Color(60, 70, 80, 0);
			for (int i = 0; i < 360; i += 30)
			{
				Vector2 addition = new Vector2(-Main.rand.Next(15) * 0.1f + 1.75f, 0).RotatedBy(MathHelper.ToRadians(i));
				Main.spriteBatch.Draw(texture, position + addition, frame, color, 0f, origin, scale, SpriteEffects.None, 0f);
			}
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsLeggingsGlow").Value;
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
				float alpha = 1 - Player.shadow;
				Lighting.AddLight(Player.Center, alpha * glowNum * new Vector3(200, 220, 255) / 255f);
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsChestplate_BodyGlow").Value;
			if(!drawPlayer.Male)
				texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsChestplate_FemaleBodyGlow").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsChestplate_ArmsGlow").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsCirclet_HeadGlow").Value;
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
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/TwilightAssassinsLeggings_LegsGlow").Value;
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