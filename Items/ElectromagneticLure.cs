using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SOTS.NPCs.Constructs;
using System.Collections.Generic;

namespace SOTS.Items
{
	public class ElectromagneticLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electromagnetic Lure");
			Tooltip.SetDefault("Attracts a single biome construct");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 6));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 40;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.maxStack = 30;
			item.useAnimation = 30;
			item.useTime = 30;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.consumable = true;
			item.noUseGraphic = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SOTS:DissolvingElement", 1);
			recipe.AddRecipeGroup("SOTS:ElementalFragment", 20);
			recipe.AddRecipeGroup("SOTS:ElementalPlating", 20);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
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
			if (player.ZoneHoly && player.ZoneOverworldHeight && Main.hardMode)
			{
				capable.Add(ModContent.NPCType<ChaosConstruct>());
			}
			return capable;
		}
		public int GetNPCType(List<int> npcList)
		{
			if (npcList.Count == 0)
				return -1;
			return npcList[Main.rand.Next(npcList.Count)];
		}
		public override bool CanUseItem(Player player)
		{
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
		public override bool UseItem(Player player)
		{
			int type = GetNPCType(CapableNPCS(player));
			if (type == -1)
				return false;
			NPC.SpawnOnPlayer(player.whoAmI, type);
			Main.PlaySound(SoundID.Item, (int)player.position.X, (int)player.position.Y, 122, 0.8f, 0.1f);
			return true;
		}
	}
}