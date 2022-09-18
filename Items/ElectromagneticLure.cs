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

namespace SOTS.Items
{
	public class ElectromagneticLure : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Electromagnetic Lure");
			Tooltip.SetDefault("Attracts a single biome construct");
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 6));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 40;
			Item.value = Item.sellPrice(0, 1, 0, 0);
			Item.rare = ItemRarityID.Orange;
			Item.maxStack = 30;
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
			NPC.SpawnOnPlayer(player.whoAmI, type);
			SOTSUtils.PlaySound(SoundID.Item122, (int)player.position.X, (int)player.position.Y, 0.8f, 0.1f);
			return true;
		}
	}
}