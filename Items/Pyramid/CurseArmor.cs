using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.EpicWings;
using SOTS.Void;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Pyramid
{
	[AutoloadEquip(EquipType.Body)]
	public class CursedRobe : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 36;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 6;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Robe");
			Tooltip.SetDefault("Increased maximum mana and void by 80\nReduces mana and void usage by 15%\nSummons a Ruby Monolith to your side\nThe Ruby Monolith increases your void regeneration speed by 10%");
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return head.type == ModContent.ItemType<CursedHood>();
		}
		public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
		{
			robes = true;
			equipSlot = mod.GetEquipSlot("CursedRobe_Legs", EquipType.Legs);
		}

		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			vPlayer.voidMeterMax2 += 80;
			player.statManaMax2 += 80;
			vPlayer.voidCost -= 0.15f;
			player.manaCost -= 0.15f;
			modPlayer.RubyMonolith = true;
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 12);
			recipe.AddRecipeGroup("SOTS:GemRobes", 1);
			recipe.AddIngredient(ModContent.ItemType<RubyKeystone>(), 1);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
	[AutoloadEquip(EquipType.Head)]
	public class CursedHood : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 24;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Pink;
			Item.defense = 4;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Hood");
			Tooltip.SetDefault("Increases magic damage and void damage by 8%\nAlso increases magic crit chance and void crit chance by 5%\nThe closest enemy to you is afflicted with a curse\nUpon taking damage, cursed enemies will Flare, dealing 140% additional damage to it and other nearby enemies\nThis effect has a 2 second cooldown");
		}
		public override void UpdateEquip(Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			VoidPlayer vPlayer = VoidPlayer.ModPlayer(player);
			modPlayer.CurseVision = true;
			vPlayer.voidDamage += 0.08f;
			player.magicDamage += 0.08f;
			vPlayer.voidCrit += 5;
			player.magicCrit += 5;
		}
		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<CursedRobe>();
		}
		public override void UpdateArmorSet(Player player)
		{
			SOTSPlayer modPlayer = player.GetModPlayer<SOTSPlayer>();
			modPlayer.CanCurseSwap = true;
			string theKey = "Unbound";
			foreach (string key in SOTS.ArmorSetHotKey.GetAssignedKeys())
			{
				theKey = key;
			}
			player.setBonus = "Press the '" + theKey + "' key to change the Ruby Monolith into an offensive stance\nWhile in offensive stance, decreases the cooldown of Curse Flaring by 80%\nHowever, increases void drain by 6 instead of increasing void regeneration speed by 10%";
		}
		public override void AddRecipes()
		{
			Recipe recipe = new Recipe(mod);
			recipe.AddIngredient(ModContent.ItemType<CursedMatter>(), 8);
			recipe.AddIngredient(ModContent.ItemType<RoyalRubyShard>(), 20);
			recipe.SetResult(this);
			recipe.AddTile(TileID.Anvils);
			recipe.AddRecipe();
		}
	}
	public class CurseHoodPlayer : ModPlayer
	{
		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			int bodyLayer = layers.FindIndex(l => l == PlayerLayer.Head);
			if (bodyLayer > -1)
			{
				layers.Insert(bodyLayer + 1, CurseHoodGlow);
			}
		}
		public static readonly PlayerLayer CurseHoodGlow = new PlayerLayer("SOTS", "CurseHoodGlow", PlayerLayer.Head, delegate (PlayerDrawInfo drawInfo) {

			// We don't want the glowmask to draw if the player is cloaked or dead
			if (drawInfo.drawPlayer.dead)
			{
				return;
			}
			float alpha = 1 - drawInfo.shadow;
			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = ModLoader.GetMod("SOTS");
			if (drawPlayer.head != mod.GetEquipSlot("CursedHood", EquipType.Head))
			{
				return;
			}
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Pyramid/CursedHood_HeadGlow").Value;
			float drawX = (int)drawInfo.position.X + drawPlayer.width / 2;
			float drawY = (int)drawInfo.position.Y + drawPlayer.height - drawPlayer.bodyFrame.Height / 2 + 4f;
			Vector2 origin = drawInfo.bodyOrigin;
			Vector2 position = new Vector2(drawX, drawY) + drawPlayer.bodyPosition - Main.screenPosition;
			alpha *= (255 - drawPlayer.immuneAlpha) / 255f;
			Color color = TestWingsPlayer.changeColorBasedOnStealth(Color.White, drawPlayer);
			Rectangle frame = drawPlayer.bodyFrame;
			float rotation = drawPlayer.bodyRotation;
			SpriteEffects spriteEffects = drawInfo.spriteEffects;
			DrawData drawData = new DrawData(texture, position, frame, color * alpha, rotation, origin, 1f, spriteEffects, 0);
			drawData.shader = drawInfo.headArmorShader;
			Main.playerDrawData.Add(drawData);
		});
	}
}