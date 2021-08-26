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
using SOTS.NPCs.Constructs;
using SOTS.Items.Celestial;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Boss.Advisor;
using SOTS.Items.Pyramid;
using SOTS.Items.Otherworld;

namespace SOTS.NPCs
{
    public class SOTSNPCs : GlobalNPC
    {
		public static bool isPolarisNPC(int type)
        {
			return (type == ModContent.NPCType<BulletSnakeHead>() || type == ModContent.NPCType<BulletSnakeWing>() || type == ModContent.NPCType<BulletSnakeBody>() || type == ModContent.NPCType<BulletSnakeEnd>() || type == ModContent.NPCType<Polaris>() || type == ModContent.NPCType<PolarisLaser>() || type == ModContent.NPCType<PolarisCannon>());
		}
        public override bool PreAI(NPC npc)
		{
			if(isPolarisNPC(npc.type))
			{
				if (Main.rand.NextBool(2))
				{
					npc.HitSound = SoundID.NPCHit4;
				}
				else
				{
					npc.HitSound = SoundID.Item50;
				}
				if(npc.DeathSound != SoundID.NPCDeath14)
					npc.DeathSound = SoundID.NPCDeath14;
			}
			return base.PreAI(npc);
        }
        public override void HitEffect(NPC npc, int hitDirection, double damage)
        {
			if(npc.life <= 0)
			{
				if (isPolarisNPC(npc.type))
				{
					for (int i = 0; i < 3; i++)
					{
						Gore.NewGore(npc.position + new Vector2((float)(npc.width * Main.rand.Next(100)) / 100f, (float)(npc.height * Main.rand.Next(100)) / 100f) - Vector2.One * 10f, npc.velocity * 0.5f, Main.rand.Next(61, 64), 1f);
					}
					for (int i = 0; i < 15; i++)
					{
						int num1 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y) - new Vector2(5), npc.width, npc.height, mod.DustType("CopyDust4"));
						Dust dust = Main.dust[num1];
						dust.velocity *= 2f;
						dust.velocity += npc.velocity * 0.2f;
						dust.noGravity = true;
						dust.scale += 1.0f;
						dust.color = new Color(200, 250, 250, 100);
						dust.fadeIn = 0.1f;
						dust.scale *= 1.5f;
						dust.alpha = npc.alpha;
					}
				}
			}
            base.HitEffect(npc, hitDirection, damage);
        }
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
			List<int> nerfBeeNPC = new List<int>() { ModContent.NPCType<PutridHook>() };
			List<int> nerfBeeBoss = new List<int>() { ModContent.NPCType<PutridPinkyPhase2>(), ModContent.NPCType<Boss.Curse.PharaohsCurse>(), ModContent.NPCType<TheAdvisorHead>() };
			List<int> nerfBeeProj = new List<int>() { ProjectileID.Bee, ProjectileID.GiantBee };
			if (nerfBeeProj.Contains(projectile.type))
            {
				if(nerfBeeBoss.Contains(npc.type))
					damage = (int)(damage * 0.8f);
				if(nerfBeeNPC.Contains(npc.type))
					damage = (int)(damage * 0.6f);
			}
			base.ModifyHitByProjectile(npc, projectile, ref damage, ref knockback, ref crit, ref hitDirection);
        }
		public void hitBy(NPC npc, Player player, Projectile projectile, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			SOTSPlayer modPlayer = (SOTSPlayer)player.GetModPlayer(mod, "SOTSPlayer");
			VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			if (modPlayer.assassinate && !npc.boss)
			{
				npc.AddBuff(ModContent.BuffType<Assassination>(), 30 * modPlayer.assassinateFlat);
				float mult = 1 - modPlayer.assassinateNum;
				int life = npc.life - (damage - (npc.defense + 1) / 2);
				if ((life < npc.lifeMax * mult || life <= modPlayer.assassinateFlat) && npc.HasBuff(ModContent.BuffType<Assassination>()))
				{
					damage += 2 * (life + modPlayer.assassinateFlat + ((npc.defense + 1) / 2));
					//crit = true;
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
		public static List<int> Zombies = null;
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
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FragmentOfInferno"), Main.rand.Next(2) + 1);
						if(SOTSWorld.downedSubspace)
							Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SanguiteBar>(), Main.rand.Next(2) + 4);

					}
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

				if (npc.type == NPCID.UndeadMiner && Main.rand.NextBool(50))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ManicMiner>(), 1);

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
				if(Zombies == null)
                {
					Zombies = new List<int>() { NPCID.Zombie, NPCID.ZombieDoctor, NPCID.ZombieElf, NPCID.ZombieElfBeard,
					NPCID.ZombieElfGirl, NPCID.ZombieEskimo, NPCID.ZombieMushroom, NPCID.ZombieMushroomHat, NPCID.ZombiePixie, NPCID.ZombieRaincoat,
					NPCID.ZombieSuperman, NPCID.ZombieSweater, NPCID.ZombieXmas, NPCID.ArmedZombie, NPCID.ArmedZombieCenx, NPCID.ArmedZombieEskimo,
					NPCID.ArmedZombiePincussion, NPCID.ArmedZombieSlimed, NPCID.ArmedZombieSwamp, NPCID.ArmedZombieTwiggy, NPCID.BaldZombie, NPCID.BigBaldZombie, NPCID.BigFemaleZombie,
					NPCID.BigPincushionZombie, NPCID.BigRainZombie, NPCID.BigSlimedZombie, NPCID.BigSwampZombie, NPCID.BigTwiggyZombie, NPCID.BigZombie,
					NPCID.FemaleZombie, NPCID.PincushionZombie, NPCID.SlimedZombie, NPCID.SmallBaldZombie, NPCID.SmallFemaleZombie, NPCID.SmallPincushionZombie,
					NPCID.SmallRainZombie, NPCID.SmallSlimedZombie, NPCID.SmallSwampZombie, NPCID.SmallTwiggyZombie, NPCID.SmallZombie, NPCID.SwampZombie, NPCID.TwiggyZombie};
				}
				if(Zombies.Contains(npc.type) && Main.rand.NextBool(80))
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<ZombieHand>(), 1);
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns) 
		{
			if(player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				spawnRate = (int)(spawnRate * 0.175f); //basically setting to 105
				maxSpawns = (int)(maxSpawns * 1.5f);
			}
			if (player.GetModPlayer<SOTSPlayer>().PlanetariumBiome) //spawnrates for this biome have to be very high due to how npc spawning in sky height works. I also manually despawn other sky enemies
			{
				spawnRate = (int)(spawnRate * 0.2f); //essentially setting it to 60
				maxSpawns = (int)(maxSpawns * 1.4f);
			}
			if (spawnRate < 1)
				spawnRate = 1;
		}
		public static bool CorrectBlockBelowPlanetarium(int i, int j, int dist)
		{
			bool flag = false;
			for (int k = 0; k < dist; k++)
			{
				Tile tile = Framing.GetTileSafely(i, j + k);
				bool correctType = tile.type == ModContent.TileType<DullPlatingTile>() || tile.type == ModContent.TileType<AvaritianPlatingTile>() || tile.type == ModContent.TileType<PortalPlatingTile>();
				if (tile.active() && Main.tileSolid[tile.type] && correctType && tile.nactive())
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
			bool ZoneForest = !player.GetModPlayer<SOTSPlayer>().PyramidBiome && !player.ZoneDesert && !player.ZoneCorrupt && !player.ZoneDungeon && !player.ZoneDungeon && !player.ZoneHoly && !player.ZoneMeteor && !player.ZoneJungle && !player.ZoneSnow && !player.ZoneCrimson && !player.ZoneGlowshroom && !player.ZoneUndergroundDesert && (player.ZoneDirtLayerHeight || player.ZoneOverworldHeight) && !player.ZoneBeach;
			if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PyramidBiome)
			{
				int tileWall = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY - 1].wall;
				bool isValidTile = spawnInfo.spawnTileType == ModContent.TileType<PyramidSlabTile>() || spawnInfo.spawnTileType == ModContent.TileType<PyramidBrickTile>() || spawnInfo.spawnTileType == ModContent.TileType<TrueSandstoneTile>();
				bool isValidWall = tileWall == ModContent.WallType<PyramidWallTile>() || tileWall == ModContent.WallType<TrueSandstoneWallWall>();
				bool isCurseValid = spawnInfo.spawnTileType == ModContent.TileType<CursedTumorTile>() || spawnInfo.spawnTileType == ModContent.TileType<CursedHive>() 
					|| spawnInfo.spawnTileType == ModContent.TileType<MalditeTile>() || tileWall == ModContent.WallType<CursedTumorWallTile>() || tileWall == ModContent.WallType<MalditeWallTile>();
				if (isValidTile || (isValidWall && !isCurseValid))
				{
					pool[0] = 0f;
					pool.Add(mod.NPCType("SnakePot"), 0.35f);
					pool.Add(mod.NPCType("Snake"), 1f);
					pool.Add(mod.NPCType("LostSoul"), 0.6f);
					pool.Add(mod.NPCType("PyramidTreasureSlime"), 0.1f);
					pool.Add(NPCID.SandSlime, 0.35f);
					if(Main.hardMode)
                    {
						pool.Add(NPCID.Mummy, 0.5f);
					}
				}
				else if(isCurseValid)
				{
					pool[0] = 0f;
					if (Main.hardMode)
					{
						pool.Add(mod.NPCType("BleedingGhast"), 0.1f);
						pool.Add(mod.NPCType("FlamingGhast"), 0.1f);
						pool.Add(mod.NPCType("Ghast"), 0.1f);
					}
					else
					{
						pool.Add(mod.NPCType("Ghast"), 0.25f);
					}
					pool.Add(mod.NPCType("LostSoul"), 0.1f);
					pool.Add(mod.NPCType("PyramidTreasureSlime"), 0.025f);
					pool.Add(ModContent.NPCType<Teratoma>(), 0.25f);
					pool.Add(ModContent.NPCType<Maligmor>(), 0.15f);
				}
			}
			else if (spawnInfo.player.GetModPlayer<SOTSPlayer>().PlanetariumBiome)
			{
				bool correctBlock = CorrectBlockBelowPlanetarium(spawnInfo.spawnTileX, spawnInfo.spawnTileY, 5);
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
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<NatureConstruct>(), SpawnCondition.Overworld.Chance * 0.01f);
			}
			else if (player.ZoneCorrupt || player.ZoneCrimson)
			{
				if (SOTSWorld.downedPinky)
				{
					pool.Add(ModContent.NPCType<FluxSlime>(), 0.10f);
				}
			}
			if(player.ZoneBeach && !spawnInfo.player.ZonePeaceCandle) //guarenteed to not spawn when a peace candle is nearby
			{
				if (player.statLifeMax2 >= 120)
				{
					if (NPC.downedBoss1)
					{
						if (NPC.downedBoss3)
						{
							pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.025f);
						}
						else
						{
							pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.015f);
						}
					}
					else
						pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.OceanMonster.Chance * 0.005f);
				}
			}
			else if (!player.ZoneBeach)
			{
				if (player.ZoneDesert || player.ZoneUndergroundDesert || (player.ZoneRockLayerHeight && !player.ZoneDungeon && !player.ZoneJungle && !player.ZoneSnow))
				{
					if(player.statLifeMax2 >= 120)
                    {
						if (player.ZoneCorrupt || player.ZoneHoly || player.ZoneCrimson)
						{
							pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.0025f);
						}
						else if (player.ZoneRockLayerHeight && !player.ZoneUndergroundDesert)
						{
							pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.005f);
						}
						else
							pool.Add(ModContent.NPCType<EarthenConstruct>(), 0.01f);
					}
				}
			}
			if (player.ZoneDungeon)
			{
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<TidalConstruct>(), SpawnCondition.DungeonNormal.Chance * 0.0075f);
			}
			if (spawnInfo.player.ZoneSnow)
			{
				if(!spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson && (NPC.downedBoss1 || NPC.downedGoblins) && spawnInfo.player.ZoneOverworldHeight)
					pool.Add(ModContent.NPCType<ArcticGoblin>(), SpawnCondition.Overworld.Chance * 0.1f);
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<PermafrostConstruct>(), spawnInfo.spawnTileType == TileID.IceBlock || spawnInfo.spawnTileType == TileID.SnowBlock ? 0.02f : 0.015f);
			}
			else if(Main.invasionType == InvasionID.GoblinArmy && spawnInfo.player.ZoneOverworldHeight && spawnInfo.player.ZoneSnow && !spawnInfo.player.ZoneCorrupt && !spawnInfo.player.ZoneCrimson)
			{
				pool.Add(ModContent.NPCType<ArcticGoblin>(), 0.1f);
			}
			else if(player.ZoneJungle)
			{
				if (player.statLifeMax2 >= 120)
					pool.Add(ModContent.NPCType<NatureConstruct>(), (SpawnCondition.SurfaceJungle.Chance * 0.025f) + (SpawnCondition.UndergroundJungle.Chance * 0.0075f));
			}
			if(spawnInfo.player.ZoneUnderworldHeight)
			{
				pool.Add(ModContent.NPCType<LesserWisp>(), SpawnCondition.Underworld.Chance * 0.12f);
			}
		}
	}
}