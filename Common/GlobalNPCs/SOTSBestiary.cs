using SOTS.Biomes;
using SOTS.NPCs.AbandonedVillage;
using SOTS.NPCs.Anomaly;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.NPCs.Boss.Polaris;
using SOTS.NPCs.Boss;
using SOTS.NPCs.Chaos;
using SOTS.NPCs.Constructs;
using SOTS.NPCs.Critters;
using SOTS.NPCs.Gizmos;
using SOTS.NPCs.Phase;
using SOTS.NPCs.Tide;
using SOTS.NPCs.TreasureSlimes;
using SOTS.NPCs;
using System.Collections.Generic;
using Terraria.GameContent.Bestiary;
using Terraria;
using Terraria.ModLoader;
using SOTS.NPCs.Inferno;
using SOTS.NPCs.Boss.Excavator;

namespace SOTS.Common.GlobalNPCs
{
    public static class BestiaryHelper
    {
        public static void AddToBestiary(this BestiaryEntry bestiaryEntry, NPC npc, int Type, IBestiaryInfoElement[] biome, string flavorTextOverride = null)
        {
            if(npc.type == Type)
            {
                FlavorTextBestiaryInfoElement flavorText = flavorTextOverride != null ? new FlavorTextBestiaryInfoElement($"Mods.SOTS.Bestiary.{flavorTextOverride}") : 
                    new FlavorTextBestiaryInfoElement($"Mods.SOTS.Bestiary.{npc.ModNPC.GetType().Name}");
                foreach (IBestiaryInfoElement b in biome)
                {
                    bestiaryEntry.Info.Add(b);
                }
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    flavorText
                });
            }
        }
    }
    public class SOTSBestiary : GlobalNPC
    {
        public override void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            SpawnConditionBestiaryInfoElement Surface = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface;
            SpawnConditionBestiaryInfoElement Underground = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground;
            SpawnConditionBestiaryInfoElement Caverns = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns;
            SpawnConditionBestiaryInfoElement UndergroundGlowingMushroom = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom;
            SpawnConditionBestiaryInfoElement Crimson = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCrimson;
            SpawnConditionBestiaryInfoElement UndergroundCrimson = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCrimson;
            SpawnConditionBestiaryInfoElement Corruption = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption;
            SpawnConditionBestiaryInfoElement UndergroundHallow = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundHallow;
            SpawnConditionBestiaryInfoElement TheHallow = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheHallow;
            SpawnConditionBestiaryInfoElement UndergroundCorruption = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundCorruption;
            SpawnConditionBestiaryInfoElement Sky = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Sky;
            SpawnConditionBestiaryInfoElement SurfaceSnow = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Snow;
            SpawnConditionBestiaryInfoElement UndergroundSnow = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow;
            SpawnConditionBestiaryInfoElement TheDungeon = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon;
            SpawnConditionBestiaryInfoElement Jungle = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle;
            SpawnConditionBestiaryInfoElement UndergroundJungle = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle;
            SpawnConditionBestiaryInfoElement TheUnderworld = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld;
            SpawnConditionBestiaryInfoElement Desert = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert;
            SpawnConditionBestiaryInfoElement UndergroundDesert = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert;
            SpawnConditionBestiaryInfoElement Ocean = BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean;
            ModBiomeBestiaryInfoElement Planetarium = ModContent.GetInstance<PlanetariumBiome>().ModBiomeBestiaryInfoElement;
            ModBiomeBestiaryInfoElement Pyramid = ModContent.GetInstance<PyramidBiome>().ModBiomeBestiaryInfoElement;
            ModBiomeBestiaryInfoElement Anomaly = ModContent.GetInstance<AnomalyBiome>().ModBiomeBestiaryInfoElement;
            ModBiomeBestiaryInfoElement AbandonedVillage = ModContent.GetInstance<AbandonedVillageBiome>().ModBiomeBestiaryInfoElement;
            ModBiomeBestiaryInfoElement Sanctuary = ModContent.GetInstance<SanctuaryBiome>().ModBiomeBestiaryInfoElement;
            if (npc.type == ModContent.NPCType<HoloSlime>() || npc.type == ModContent.NPCType<HoloBlade>() || npc.type == ModContent.NPCType<HoloEye>() ||
                npc.type == ModContent.NPCType<TwilightDevil>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead>()
                || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>() || npc.type == ModContent.NPCType<PhaseEye>() || npc.type == ModContent.NPCType<OtherworldlySpirit>()
                || npc.type == ModContent.NPCType<TheAdvisorHead>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.HoloSlime");

                if (npc.type == ModContent.NPCType<HoloBlade>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.HoloBlade");
                if (npc.type == ModContent.NPCType<HoloEye>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.HoloEye");
                if (npc.type == ModContent.NPCType<PhaseEye>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PhaseEye");
                if (npc.type == ModContent.NPCType<TwilightDevil>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.TwilightDevil");
                if (npc.type == ModContent.NPCType<OtherworldlyConstructHead>() || npc.type == ModContent.NPCType<OtherworldlyConstructHead2>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.OtherworldlyConstructHead");
                if (npc.type == ModContent.NPCType<OtherworldlySpirit>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.OtherworldlySpirit");
                if (npc.type == ModContent.NPCType<TheAdvisorHead>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Advisor");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Planetarium,
                    flavorText
                });
            }
            if (npc.ModNPC != null && npc.ModNPC is TardigradeBrown)
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Tardigrade");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Planetarium,
                    flavorText
                });
            }
            if (npc.type == ModContent.NPCType<PhaseSpeeder>() || npc.type == ModContent.NPCType<PhaseAssaulterHead>() || npc.type == ModContent.NPCType<TwilightScouter>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PhaseSpeeder");
                if (npc.type == ModContent.NPCType<TwilightScouter>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.TwilightScouter");
                if (npc.type == ModContent.NPCType<PhaseAssaulterHead>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PhaseAssaulterHead");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Sky,
                    flavorText
                });
            }
            if (npc.type == ModContent.NPCType<LostSoul>() || npc.type == ModContent.NPCType<Snake>() || npc.type == ModContent.NPCType<SnakePot>() ||
                npc.type == ModContent.NPCType<WallMimic>() || npc.type == ModContent.NPCType<Teratoma>() || npc.type == ModContent.NPCType<Ghast>() || npc.type == ModContent.NPCType<Maligmor>() || npc.type == ModContent.NPCType<NPCs.Boss.Curse.PharaohsCurse>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.LostSoul");
                if (npc.type == ModContent.NPCType<Snake>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Snake");
                if (npc.type == ModContent.NPCType<SnakePot>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.SnakePot");
                if (npc.type == ModContent.NPCType<WallMimic>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.WallMimic");
                if (npc.type == ModContent.NPCType<Teratoma>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Teratoma");
                if (npc.type == ModContent.NPCType<Ghast>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Ghast");
                if (npc.type == ModContent.NPCType<Maligmor>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Maligmor");
                if (npc.type == ModContent.NPCType<NPCs.Boss.Curse.PharaohsCurse>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PharaohsCurse");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Pyramid,
                    flavorText
                });
            }
            if (npc.type == ModContent.NPCType<ArcticGoblin>() || npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>() || npc.type == ModContent.NPCType<PermafrostConstruct>() || npc.type == ModContent.NPCType<PermafrostSpirit>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ArcticGoblin");
                if (npc.type == ModContent.NPCType<Polaris>() || npc.type == ModContent.NPCType<NewPolaris>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Polaris");
                if (npc.type == ModContent.NPCType<PermafrostConstruct>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PermafrostConstruct");
                if (npc.type == ModContent.NPCType<PermafrostSpirit>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PermafrostSpirit");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    SurfaceSnow,
                    flavorText
                });
            }
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<FluxSlime>(), [Surface, Corruption, Crimson]);
            if (npc.type == ModContent.NPCType<PutridPinkyPhase2>() || npc.type == ModContent.NPCType<BlueSlimer>() || npc.type == ModContent.NPCType<NatureConstruct>() || npc.type == ModContent.NPCType<NatureSlime>() || npc.type == ModContent.NPCType<NatureSpirit>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PP");
                if (npc.type == ModContent.NPCType<BlueSlimer>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.BlueSlimer");
                if (npc.type == ModContent.NPCType<NatureConstruct>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.NatureConstruct");
                if (npc.type == ModContent.NPCType<NatureSlime>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.NatureSlime");
                if (npc.type == ModContent.NPCType<NatureSpirit>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.NatureSpirit");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Surface,
                    flavorText
                });
            }

            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<SittingMushroom>(), [Underground, Caverns, UndergroundGlowingMushroom]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Glowmoth>(), [UndergroundGlowingMushroom]);

            if (npc.type == ModContent.NPCType<Lux>() || npc.type == ModContent.NPCType<ChaosConstruct>() || npc.type == ModContent.NPCType<ChaosRubble>() || npc.type == ModContent.NPCType<Chimera>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Lux");
                if (npc.type == ModContent.NPCType<ChaosConstruct>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ChaosConstruct");
                if (npc.type == ModContent.NPCType<ChaosRubble>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.ChaosConstructChassis");
                if (npc.type == ModContent.NPCType<Chimera>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Chimera");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    TheHallow,
                    flavorText
                });
            }
            if (npc.type == ModContent.NPCType<EarthenConstruct>() || npc.type == ModContent.NPCType<EarthenSpirit>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.EarthenConstruct");
                if (npc.type == ModContent.NPCType<EarthenSpirit>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.EarthenSpirit");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Underground,
                    Desert,
                    UndergroundDesert,
                    flavorText
                });
            }
            if (npc.type == ModContent.NPCType<InfernoConstruct>() || npc.type == ModContent.NPCType<SubspaceSerpentHead>() || npc.type == ModContent.NPCType<InfernoSpirit>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.InfernoConstruct");
                if (npc.type == ModContent.NPCType<SubspaceSerpentHead>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.SubspaceSerpent");
                if (npc.type == ModContent.NPCType<InfernoSpirit>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.InfernoSpirit");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    TheUnderworld,
                    flavorText
                });
            }
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<LesserWisp>(), [TheUnderworld]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Ultracap>(), [Anomaly]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Planetoid>(), [Anomaly]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<LunaMoth>(), [Sanctuary]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<SubspaceWorm>(), [Sanctuary]);
            if (npc.type == ModContent.NPCType<Famished>() || npc.type == ModContent.NPCType<EarthenGizmo>() || npc.type == ModContent.NPCType<Throe>() || npc.type == ModContent.NPCType<CorpseBloom>() ||
                npc.type == ModContent.NPCType<Bridgeburner>() || npc.type == ModContent.NPCType<SanguineFoundry>() || npc.type == ModContent.NPCType<CoalCart>() || npc.type == ModContent.NPCType<Pupa>() || npc.type == ModContent.NPCType<PupaFly>())
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Famished");
                if (npc.type == ModContent.NPCType<EarthenGizmo>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.EarthenGizmo");
                if (npc.type == ModContent.NPCType<Throe>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Throe");
                if (npc.type == ModContent.NPCType<CorpseBloom>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.CorpseBloom");
                if (npc.type == ModContent.NPCType<SanguineFoundry>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.SanguineFoundry");
                if (npc.type == ModContent.NPCType<Bridgeburner>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Bridgeburner");
                if (npc.type == ModContent.NPCType<CoalCart>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.CoalCart");
                if (npc.type == ModContent.NPCType<Pupa>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.Pupa");
                if (npc.type == ModContent.NPCType<PupaFly>())
                    flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.PupaFly");
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    AbandonedVillage,
                    flavorText
                });
            }
            
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Fistfull>(), [AbandonedVillage]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<BallOWorms>(), [AbandonedVillage], "BallOWormsFlesh");
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<BallOGuts>(), [AbandonedVillage], "BallOWormsFlesh");

            if (npc.type == ModContent.NPCType<TidalConstruct>() || npc.type == ModContent.NPCType<PhantarayBig>() || npc.type == ModContent.NPCType<PhantarayCore>())
            {
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    Ocean
                });
            }
            if (npc.ModNPC is TreasureSlime)
            {
                FlavorTextBestiaryInfoElement flavorText = new FlavorTextBestiaryInfoElement("Mods.SOTS.Bestiary.TreasureSlime");
                if (npc.type == ModContent.NPCType<BasicTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Surface
                    });
                }
                if (npc.type == ModContent.NPCType<MutagenTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        AbandonedVillage,
                    });
                }
                if (npc.type == ModContent.NPCType<CorruptionTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Corruption,
                        UndergroundCorruption
                    });
                }
                if (npc.type == ModContent.NPCType<CrimsonTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Crimson,
                        UndergroundCrimson
                    });
                }
                if (npc.type == ModContent.NPCType<DungeonTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        TheDungeon
                    });
                }
                if (npc.type == ModContent.NPCType<GoldenTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Underground,
                        Caverns
                    });
                }
                if (npc.type == ModContent.NPCType<IceTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        SurfaceSnow,
                        UndergroundSnow
                    });
                }
                if (npc.type == ModContent.NPCType<JungleTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Jungle,
                        UndergroundJungle
                    });
                }
                if (npc.type == ModContent.NPCType<PyramidTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Pyramid
                    });
                }
                if (npc.type == ModContent.NPCType<ShadowTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        TheUnderworld
                    });
                }
                if (npc.type == ModContent.NPCType<HallowTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        TheHallow,
                        UndergroundHallow
                    });
                }
                if (npc.type == ModContent.NPCType<VoidTreasureSlime>())
                {
                    bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                        Sanctuary
                    });
                }
                bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                    flavorText
                });
            }

            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Collector>(), [Planetarium]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Collector2>(), [Planetarium]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<Excavator>(), [AbandonedVillage]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<EvilSpirit>(), [AbandonedVillage, UndergroundCorruption, UndergroundCrimson, Crimson, Corruption]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<EvilConstruct>(), [AbandonedVillage, UndergroundCorruption, UndergroundCrimson, Crimson, Corruption]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<TidalConstruct>(), [Ocean]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<TidalSpirit>(), [Ocean]);
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<BleedingGhast>(), [Pyramid], "Ghast2");
            bestiaryEntry.AddToBestiary(npc, ModContent.NPCType<FlamingGhast>(), [Pyramid], "Ghast2");
        }
    }
}