using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class SuspiciousLookingCandle : ModItem
	{
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 42;
			Item.value = 0;
			Item.rare = ItemRarityID.Blue;
			Item.maxStack = 30;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ItemID.GoldenCandle).AddIngredient<GlowNylon>(5).AddIngredient(ItemID.GlowingMushroom, 10).AddTile(TileID.DemonAltar).Register();
			CreateRecipe(1).AddIngredient(ItemID.PlatinumCandle).AddIngredient<GlowNylon>(5).AddIngredient(ItemID.GlowingMushroom, 10).AddTile(TileID.DemonAltar).Register();
		}
		public override bool CanUseItem(Player player)
		{
			return !NPC.AnyNPCs(ModContent.NPCType<NPCs.Boss.Glowmoth.Glowmoth>()) && player.ZoneGlowshroom;
		}
		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				SoundEngine.PlaySound(SoundID.Roar, player.position);
				int type = ModContent.NPCType<NPCs.Boss.Glowmoth.Glowmoth>();
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					NPC.SpawnOnPlayer(player.whoAmI, type);
				}
				else
				{
					NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
				}
			}
			return true;
		}
	}
}