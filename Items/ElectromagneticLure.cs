using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.NPCs.Constructs;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SOTS.Projectiles;
using SOTS.Buffs;
using Terraria.Audio;
using SOTS.Void;

namespace SOTS.Items
{
	public class ElectromagneticLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<ElectromagneticDeterrent>();
			this.SetResearchCost(1);
		}
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			List<int> CapableNPCs = CapableNPCS(Main.LocalPlayer);
			tooltips.Add(new TooltipLine(Mod, "Construct0", "Possible constructs:"));
			if (CapableNPCs.Contains(ModContent.NPCType<NatureConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct1", "Nature Construct") { OverrideColor = ColorHelpers.NatureColor });
			if (CapableNPCs.Contains(ModContent.NPCType<EarthenConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct2", "Earthen Construct") { OverrideColor = ColorHelpers.EarthColor });
			if (CapableNPCs.Contains(ModContent.NPCType<PermafrostConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct3", "Permafrost Construct") { OverrideColor = ColorHelpers.PermafrostColor });
			if (CapableNPCs.Contains(ModContent.NPCType<OtherworldlyConstructHead>()))
				tooltips.Add(new TooltipLine(Mod, "Construct4", "Otherworldly Construct") { OverrideColor = ColorHelpers.OtherworldColor });
			if (CapableNPCs.Contains(ModContent.NPCType<TidalConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct5", "Tidal Construct") { OverrideColor = ColorHelpers.TideColor });
			if (CapableNPCs.Contains(ModContent.NPCType<EvilConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct6", "Evil Construct") { OverrideColor = new Color(ColorHelpers.EvilColor.R + 50, ColorHelpers.EvilColor.G + 50, ColorHelpers.EvilColor.B + 50)  });
			if (CapableNPCs.Contains(ModContent.NPCType<ChaosConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct7", "Chaos Construct") { OverrideColor = ColorHelpers.pastelRainbow });
			if (CapableNPCs.Contains(ModContent.NPCType<InfernoConstruct>()))
				tooltips.Add(new TooltipLine(Mod, "Construct8", "Inferno Construct") { OverrideColor = ColorHelpers.Inferno1 });
			if(CapableNPCs.Count <= 0)
				tooltips.Add(new TooltipLine(Mod, "Construct8", "None") { OverrideColor = new Color(150, 150, 150) });
		}
        public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 9999;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.consumable = true;
			Item.noUseGraphic = true;
		}
        public override void AddRecipes()
		{
			CreateRecipe(1).AddRecipeGroup("SOTS:DissolvingElement", 1).AddRecipeGroup("SOTS:ElementalFragment", 15).AddRecipeGroup("SOTS:ElementalPlating", 15).AddTile(TileID.Anvils).Register();
		}
		public List<int> CapableNPCS(Player player)
		{
			SOTSPlayer sPlayer = player.GetModPlayer<SOTSPlayer>();
			List<int> capable = new List<int>();
			bool ZoneForest = SOTSPlayer.ZoneForest(player);
			if (sPlayer.PlanetariumBiome || player.ZoneSkyHeight || player.ZoneMeteor)
			{
				capable.Add(ModContent.NPCType<OtherworldlyConstructHead>());
			}
			if (ZoneForest)
			{
				capable.Add(ModContent.NPCType<NatureConstruct>());
			}
			if (player.ZoneBeach)
			{
				capable.Add(ModContent.NPCType<TidalConstruct>());
			}
			if ((player.ZoneCrimson || player.ZoneCorrupt) && (player.ZoneRockLayerHeight || player.ZoneDirtLayerHeight) && Main.hardMode)
			{
				capable.Add(ModContent.NPCType<EvilConstruct>());
			}
			if (!player.ZoneBeach)
			{
				if (player.ZoneDesert || player.ZoneUndergroundDesert || (player.ZoneRockLayerHeight && !player.ZoneDungeon && !player.ZoneJungle && !player.ZoneSnow))
				{
					capable.Add(ModContent.NPCType<EarthenConstruct>());
				}
			}
			if (player.ZoneSnow)
			{
				capable.Add(ModContent.NPCType<PermafrostConstruct>());
			}
			if (player.ZoneJungle)
			{
				capable.Add(ModContent.NPCType<NatureConstruct>());
			}
			if (player.ZoneUnderworldHeight && Main.hardMode)
			{
				capable.Add(ModContent.NPCType<InfernoConstruct>());
			}
			if (player.ZoneHallow && player.ZoneOverworldHeight && Main.hardMode)
			{
				capable.Add(ModContent.NPCType<ChaosConstruct>());
			}
			return capable;
		}
		public int GetNPCType(List<int> npcList)
		{
			if (npcList.Count == 0)
				return -1;
			if(npcList.Contains(ModContent.NPCType<EarthenConstruct>()) && 
				npcList.Contains(ModContent.NPCType<EvilConstruct>()))
			{
				if(Main.rand.NextBool(3))
				{
					return ModContent.NPCType<EvilConstruct>();
                }
			}	
			return npcList[Main.rand.Next(npcList.Count)];
		}
		public override bool CanUseItem(Player player)
		{
			if(player.HasBuff(ModContent.BuffType<IntimidatingPresence>()))
            {
				return false;
            }
			List<int> capable = CapableNPCS(player);
			int type = GetNPCType(capable);
			if (type == -1)
				return false;
			for(int i = 0; i < capable.Count; i++)
            {
				int testNPCAvailable = capable[i];
				if (NPC.AnyNPCs(testNPCAvailable))
					return false;
            }
			return true;
		}
        public override bool? UseItem(Player player)
        {
			if(Main.myPlayer == player.whoAmI)
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center + new Vector2(0, -32), new Vector2(0, -4), ModContent.ProjectileType<ConstructFinder>(), 0, 0, Main.myPlayer);
			int type = GetNPCType(CapableNPCS(player));
			if (type == -1)
				return false;
			if (player.whoAmI == Main.myPlayer)
			{
				SOTSUtils.PlaySound(SoundID.Item122, (int)player.position.X, (int)player.position.Y, 0.8f, 0.1f);
				SoundEngine.PlaySound(SoundID.Roar, player.position);
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