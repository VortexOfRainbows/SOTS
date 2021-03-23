using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Xml.Schema;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Steamworks;
using SOTS.Void;
using SOTS.Items.Void;
using SOTS.NPCs.Boss;
using SOTS.Items.SpecialDrops;
using SOTS.Items;
using SOTS.Buffs;

namespace SOTS.NPCs
{
    public class SOTSNPCs : GlobalNPC
    {
        public override void SetDefaults(NPC npc)
        {
			if(npc.type == NPCID.BlackRecluse || npc.type == NPCID.WallCreeper || npc.type == NPCID.WallCreeperWall || npc.type == NPCID.BlackRecluseWall || npc.type == NPCID.JungleCreeperWall || npc.type == NPCID.JungleCreeper)
            {
				npc.buffImmune[ModContent.BuffType<WebbedNPC>()] = true;
            }
        }
        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			hitBy(npc, player, null, item, ref damage, ref knockback, ref crit);
			base.ModifyHitByItem(npc, player, item, ref damage, ref knockback, ref crit);
		}
        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			hitBy(npc, Main.player[projectile.owner], projectile, null, ref damage, ref knockback, ref crit);
			base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
		public void hitBy(NPC npc, Player player, Projectile projectile, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (modPlayer.assassinate)
			{
				npc.AddBuff(mod.BuffType("Assassination"), 30 * modPlayer.assassinateFlat);
				float mult = 1 - modPlayer.assassinateNum;
				int life = npc.life - (damage - (npc.defense + 1) / 2);
				if ((life < npc.lifeMax * mult || life <= modPlayer.assassinateFlat) && npc.HasBuff(mod.BuffType("Assassination")))
				{
					damage += life + modPlayer.assassinateFlat + ((npc.defense + 1) / 2);
					crit = true;
					for (int i = 0; i < 60; i++)
					{
						Vector2 rotate = new Vector2(npc.width / 2 + 4, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTime * 120 + npc.whoAmI * 7 + 120 * i));
						int num1 = Dust.NewDust(new Vector2(npc.Center.X + rotate.X - 4, npc.Center.Y + rotate.Y - 4), 0, 0, 235);
						Main.dust[num1].noGravity = true;
						Main.dust[num1].scale *= 2f;
						Main.dust[num1].velocity *= 1.5f;
					}
				}
			}
		}
        public override void NPCLoot(NPC npc)
        {
			Player player = Main.player[Main.myPlayer];
			if(npc.target <= 255)
			{
				player = Main.player[npc.target];
			}
            SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");	
			bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;

			if (npc.lifeMax > 5 && !npc.SpawnedFromStatue)
			{
				if ((Main.rand.Next(90) == 0 && Main.expertMode) || (Main.rand.Next(100) == 0 && !Main.expertMode) || (npc.type == NPCID.PigronCorruption || npc.type == NPCID.PigronHallow || npc.type == NPCID.PigronCrimson))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("AlmondMilk"), 1);

				if (Main.rand.NextBool(35))
				{
					//priorities: otherworld > tide > nature > permafrost > earth >  inferno
					//additional: evil & chaos (will not spawn in addition to forest)
					if (player.ZoneSkyHeight || player.ZoneMeteor)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfOtherworld"), Main.rand.Next(2) + 1);
					else if (player.ZoneBeach || player.ZoneDungeon)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfTide"), Main.rand.Next(2) + 1);
					else if (ZoneForest || player.ZoneJungle)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfNature"), Main.rand.Next(2) + 1);
					else if (player.ZoneSnow)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfPermafrost"), Main.rand.Next(2) + 1);
					else if (player.ZoneUndergroundDesert || player.ZoneDesert || player.GetModPlayer<SOTSPlayer>().PyramidBiome || player.ZoneRockLayerHeight)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfEarth"), Main.rand.Next(2) + 1);
					else if (player.ZoneUnderworldHeight)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfInferno"), Main.rand.Next(2) + 1);
				}
				else if (Main.rand.NextBool(34))
				{
					if (player.ZoneCorrupt || player.ZoneCrimson)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfEvil"), Main.rand.Next(2) + 1);
					if (player.ZoneHoly)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfChaos"), Main.rand.Next(2) + 1);
				}

				if (player.ZoneSnow && ((Main.rand.NextBool(90) && Main.expertMode) || (Main.rand.NextBool(100) && !Main.expertMode)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StrawberryIcecream"), 1);
				if (player.ZoneBeach && ((Main.rand.NextBool(120) && Main.expertMode) || (Main.rand.NextBool(150) && !Main.expertMode)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CoconutMilk>(), 1);

				if (npc.type == NPCID.ZombieMushroom || npc.type == NPCID.ZombieMushroomHat || npc.type == NPCID.MushiLadybug || npc.type == NPCID.AnomuraFungus || npc.type == NPCID.FungiBulb || npc.type == NPCID.FungoFish || npc.type == NPCID.GiantFungiBulb)
					if ((Main.rand.NextBool(10) && !Main.expertMode) || (Main.rand.NextBool(9) && Main.expertMode))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CookedMushroom"), 1);

				if (modPlayer.PlanetariumBiome)
					if ((Main.rand.NextBool(90) && Main.expertMode) || (Main.rand.NextBool(100) && !Main.expertMode))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DigitalCornSyrup"), 1);

				if ((npc.type == mod.NPCType("OtherworldlyConstructHead") || npc.type == mod.NPCType("OtherworldlyConstructHead2")) && Main.rand.NextBool(100))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PhaseCannon"), 1);

				if (npc.type == NPCID.WallofFlesh)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HungryHunter"), 1);

				if (npc.type == NPCID.WyvernHead)
				{
					if (Main.rand.NextBool(5))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GiantHarpyFeather, 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfOtherworld"), Main.rand.Next(2) + 1);
				}
				if(npc.type == NPCID.VoodooDemon || npc.type == NPCID.BoneSerpentHead)
				{
					if(Main.rand.NextBool(2) || Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfInferno"), Main.rand.Next(2) + 1);
				}

				if (npc.type == NPCID.GoblinPeon || npc.type == NPCID.GoblinArcher || npc.type == NPCID.GoblinWarrior || npc.type == NPCID.GoblinSorcerer)
				{
					if (Main.rand.Next(5) <= 1 && !Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Goblinsteel>(), 1);
					if (Main.rand.NextBool(2) && Main.expertMode)
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Goblinsteel>(), 1);
				}

				if (npc.type == NPCID.PirateCaptain || npc.type == NPCID.PirateCorsair || npc.type == NPCID.PirateCrossbower || npc.type == NPCID.PirateDeadeye || npc.type == NPCID.Parrot)
				{
					if (Main.rand.NextBool(10))
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Chocolate"), 1);
				}

				if (npc.type == NPCID.ElfCopter && Main.rand.NextBool(12))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("HelicopterParts"), 1);

				if (npc.type == NPCID.UndeadMiner && Main.rand.NextBool(5))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ManicMiner"), 1);

				if (npc.type == NPCID.BlueSlime && Main.rand.NextBool(240))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FireSpitter"), 1);

				if (npc.type == NPCID.Crab && Main.rand.NextBool(18))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CrabClaw"), 1);

				if (npc.type == NPCID.Mothron && NPC.downedPlantBoss && ((Main.rand.NextBool(5) && !Main.expertMode) || (Main.rand.NextBool(4) && Main.expertMode)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BrokenVillainSword"), 1);

				if (npc.type == NPCID.PossessedArmor && Main.rand.NextBool(90))
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedHelmet"), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedChainmail"), 1);
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PossessedGreaves"), 1);
				}

				if (npc.type == NPCID.PinkJellyfish && Main.rand.NextBool(60))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("PinkJellyfishStaff"), 1);
				if ((npc.type == NPCID.BlueJellyfish || npc.type == NPCID.GreenJellyfish) && Main.rand.NextBool(50))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BlueJellyfishStaff"), 1);

				if(npc.type == NPCID.QueenBee && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RoyalJelly>(), 1);
				if (npc.type == ModContent.NPCType<PutridPinkyPhase2>() && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<PeanutButter>(), 1);
				if (npc.type == NPCID.SkeletronHead && (!NPC.downedBoss1 || Main.rand.NextBool(20)))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Baguette>(), 1);
				if (npc.type == NPCID.GreekSkeleton && Main.rand.NextBool(20))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<OlympianAxe>(), 1);

			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) 
		{
			if(player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				if(spawnRate > 60)
				{
					spawnRate = 60;
					maxSpawns = (int)(maxSpawns * 2f);
				}
			}
			if (player.GetModPlayer<SOTSPlayer>().PlanetariumBiome) //spawnrates for this biome have to be very high due to how npc spawning in sky height works. I also manually despawn other sky enemies
			{
				spawnRate = (int)(spawnRate /= 30); //essentially setting it to 20
				if (spawnRate < 1)
					spawnRate = 1;
				maxSpawns = (int)(maxSpawns * 2.5f);
			}
		}
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;
			if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				if (spawnInfo.spawnTileType == (ushort)mod.TileType("PyramidSlabTile"))
				{
					pool[0] = 0f;
					pool.Add(mod.NPCType("SnakePot"), 0.3f);
					pool.Add(mod.NPCType("Snake"), 1f);
					pool.Add(mod.NPCType("LostSoul"), 0.6f);
					pool.Add(mod.NPCType("PyramidTreasureSlime"), 0.1f);
					if (Main.hardMode)
					{
						pool.Add(NPCID.Mummy, 0.5f);
						pool.Add(mod.NPCType("BleedingGhast"), 0.2f);
						pool.Add(mod.NPCType("FlamingGhast"), 0.2f);
					}
				}
			}
			else if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PlanetariumBiome)
			{
				bool correctBlock = (Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY + 3].type == mod.TileType("DullPlatingTile") || Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY + 3].type == mod.TileType("PortalPlatingTile") || Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY + 3].type == mod.TileType("AvaritianPlatingTile")) && Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY + 3].nactive();
				for (int i = 0; i < pool.Count; i++)
					pool[i] = 0f;
				if (correctBlock)
				{
					pool.Add(mod.NPCType("HoloSlime"), 0.4f);
					pool.Add(mod.NPCType("HoloEye"), 0.1f);
					pool.Add(mod.NPCType("HoloBlade"), 0.175f);
					pool.Add(mod.NPCType("TwilightDevil"), 0.03f);
					pool.Add(mod.NPCType("OtherworldlyConstructHead"), 0.015f);
				}
			}
			else if (ZoneForest)
			{
				if (SOTSWorld.downedPinky)
				{
					pool.Add(ModContent.NPCType<FluxSlime>(), SpawnCondition.OverworldDaySlime.Chance * 0.05f);
					pool.Add(mod.NPCType("NatureSlime"), SpawnCondition.OverworldDaySlime.Chance * 0.10f);
				}
				else
				{
					pool.Add(mod.NPCType("NatureSlime"), SpawnCondition.OverworldDaySlime.Chance * 0.15f);
				}
				pool.Add(mod.NPCType("BlueSlimer"), SpawnCondition.OverworldDaySlime.Chance * 0.1f);
				pool.Add(mod.NPCType("TreasureSlime"), SpawnCondition.OverworldDaySlime.Chance * 0.1f);
			}
			else if (player.ZoneCorrupt || player.ZoneCrimson)
			{
				if (SOTSWorld.downedPinky)
				{
					pool.Add(ModContent.NPCType<FluxSlime>(), 0.10f);
				}
			}
		}
	}
}