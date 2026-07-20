using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;
using SOTS.Items.Banners;
using SOTS.Items.MusicBoxes;
using SOTS.Items.Slime;
using SOTS.NPCs.Boss;
using Microsoft.Xna.Framework;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Permafrost;
using SOTS.Items.Pyramid;
using SOTS.Items.Tools;
using SOTS.Items;
using SOTS.NPCs.Boss.Advisor;
using SOTS.NPCs.Boss.Glowmoth;
using SOTS.NPCs.Boss.Lux;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.NPCs.Boss.Curse;
using SOTS.Items.Celestial;
using SOTS.NPCs.Boss.Excavator;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Nature;
using SOTS.Items.Invidia;
using SOTS.Items.Earth;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Chaos;
using Terraria.ID;

namespace SOTS
{
    public partial class SOTS
    {
        /// <summary>
        /// <b>Vanilla main bosses:</b><br />
        ///  1.0 = King Slime<br />
        ///  2.0 = Eye of Cthulhu<br />
        ///  3.0 = Eater of Worlds / Brain of Cthulhu<br />
        ///  4.0 = Queen Bee<br />
        ///  5.0 = Skeletron<br />
        ///  6.0 = Deerclops<br />
        ///  7.0 = Wall of Flesh<br />
        ///  8.0 = Queen Slime<br />
        ///  9.0 = The Twins<br />
        /// 10.0 = The Destroyer<br />
        /// 11.0 = Skeletron Prime<br />
        /// 12.0 = Plantera<br />
        /// 13.0 = Golem<br />
        /// 14.0 = Duke Fishron<br />
        /// 15.0 = Empress of Light<br />
        /// 16.0 = Betsy<br />
        /// 17.0 = Lunatic Cultist<br />
        /// 18.0 = Moon Lord
        /// </summary>
        public Dictionary<string, float> BossChecklistValues = new()
        {
            {nameof(Glowmoth), 2.1f},
            {nameof(PutridPinkyPhase2), 4.25f},
            {nameof(PharaohsCurse), 4.5f},
            {nameof(TheAdvisorHead), 6.9f},
            {nameof(NewPolaris), 11.01f},
            {nameof(Lux), 16.5f},
            {nameof(SubspaceSerpentHead), 17.9f},
            {nameof(Excavator), 6.8f}
        };

        private void BossChecklistCompatibility()
        {
            if (!ModLoader.TryGetMod("BossChecklist", out Mod bossChecklist))
                return;

            void Add(string type, string bossName, Func<bool> downed, List<int> npcIDs, Dictionary<string, object> extraInfo)
            {
                bossChecklist.Call(
                        $"Log{type}",
                        this,
                        bossName,
                        BossChecklistValues[bossName],
                        downed,
                        npcIDs,
                        extraInfo);
            }

            Add("Boss",
                nameof(Glowmoth),
                () => SOTSWorld.downedGlowmoth,
                [ModContent.NPCType<Glowmoth>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = ModContent.ItemType<SuspiciousLookingCandle>(),
                    ["collectibles"] = new List<int>() { ModContent.ItemType<MothMusicBox>(), ModContent.ItemType<GlowmothTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Glowmoth.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/GlowmothPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(PutridPinkyPhase2),
                () => SOTSWorld.downedPinky,
                [ModContent.NPCType<PutridPinkyPhase2>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = ModContent.ItemType<JarOfPeanuts>(),
                    ["collectibles"] = new List<int>() { ModContent.ItemType<PutridPinkyMusicBox>(), ModContent.ItemType<PutridPinkyTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.PutridPinkyPhase2.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/NPCs/Boss/PutridPinky1_Display").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(PharaohsCurse),
                () => SOTSWorld.downedCurse,
                [ModContent.NPCType<PharaohsCurse>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = ModContent.ItemType<Sarcophagus>(),
                    ["collectibles"] = new List<int>() { ModContent.ItemType<CurseMusicBox>(), ModContent.ItemType<CurseTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.PharaohsCurse.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PharaohPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(TheAdvisorHead),
                () => SOTSWorld.downedAdvisor,
                [ModContent.NPCType<TheAdvisorHead>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = ModContent.ItemType<WorldgenScanner>(),
                    ["collectibles"] = new List<int>() { ModContent.ItemType<AdvisorMusicBox>(), ModContent.ItemType<AdvisorTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.TheAdvisorHead.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/AdvisorPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(Excavator),
                () => SOTSWorld.downedExcavator,
                [ModContent.NPCType<Excavator>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.Excavator.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Excavator.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = ModContent.ItemType<SeismicStation>(),
                    ["collectibles"] = new List<int>() { ModContent.ItemType<ExcavatorMusicBox>(), ModContent.ItemType<ExcavatorTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Excavator.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/ExcavatorPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2), rect.Y + (rect.Height / 2));
                        sb.Draw(texture, centered, null, color, 0, texture.Size() / 2, 0.425f, SpriteEffects.None, 0);
                    }
                });

            Add("Boss",
                nameof(NewPolaris),
                () => SOTSWorld.downedAmalgamation,
                [ModContent.NPCType<NewPolaris>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.Polaris.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Polaris.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = new List<int>() { ModContent.ItemType<FrostedKey>(), ModContent.ItemType<FrostArtifact>() },
                    ["collectibles"] = new List<int>() { ModContent.ItemType<PolarisMusicBox>(), ModContent.ItemType<PolarisTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Polaris.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/PolarisPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(Lux),
                () => SOTSWorld.downedLux,
                [ModContent.NPCType<Lux>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.Lux.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.Lux.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = new List<int>() { ModContent.ItemType<ElectromagneticLure>() },
                    ["collectibles"] = new List<int>() { ModContent.ItemType<LuxMusicBox>(), ModContent.ItemType<LuxTrophy>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.Lux.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/LuxBossLog").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });

            Add("Boss",
                nameof(SubspaceSerpentHead),
                () => SOTSWorld.downedSubspace,
                [ModContent.NPCType<SubspaceSerpentHead>(), ModContent.NPCType<SubspaceSerpentBody>(), ModContent.NPCType<SubspaceSerpentTail>()],
                new Dictionary<string, object>()
                {
                    ["displayName"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.DisplayName"),
                    ["spawnInfo"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.BossChecklistIntegration.SpawnInfo"),
                    ["spawnItems"] = new List<int>() { ModContent.ItemType<CatalystBomb>() },
                    ["collectibles"] = new List<int>() { ModContent.ItemType<SubspaceSerpentMusicBox>() },
                    ["availability"] = (Func<bool>)(() => true),
                    //["overrideHeadTextures"] = ,
                    ["despawnMessage"] = Language.GetText("Mods.SOTS.NPCs.SubspaceSerpentHead.BossChecklistIntegration.DespawnMessage"),
                    ["customPortrait"] = (SpriteBatch sb, Rectangle rect, Color color) =>
                    {
                        Texture2D texture = ModContent.Request<Texture2D>("SOTS/BossCL/SubspaceSerpentPortrait").Value;
                        Vector2 centered = new Vector2(rect.X + (rect.Width / 2) - (texture.Width / 2), rect.Y + (rect.Height / 2) - (texture.Height / 2));
                        sb.Draw(texture, centered, color);
                    }
                });
        }

        #region Russian localization support
        private void ProjecttRUCompatibility()
        {
            if (!ModLoader.TryGetMod("CalamityRuTranslate", out Mod tru) || Language.ActiveCulture.Name != "ru-RU")
                return;

            tru.Call("AddFeminineItems", this, new[]
            {
                "AncientSteelHalberd",
                "CatalystBomb",
                "VoidspaceEmblem",
                "SpiritSymphony",
                "SpiritInsignia",
                "BagOfAmmoGathering",
                "CoconutGun",
                "PerfectStar",
                "RebarRifle",
                "RockCandy",
                "Sawflake",
                "SpikedClub",
                "SyntheticLiver",
                "TinyPlanet",
                "SkipScythe",
                "VoidAnomaly",
                "BloodstainedCoin",
                "PutridCoin",
                "PolishedCoin",
                "CircuitBoard",
                "CrabClaw",
                "MantisGrip",
                "SpiderCrusher",
                "TorchGun",
                "HarvestersScythe",
                "VibrantCannon",
                "AbyssalFury",
                "DeathSpiral",
                "MidnightPrism",
                "TomeOfTheReaper",
                "ToothAche",
                "NorthStar",
                "BlazingClub",
                "BookOfVirtues",
                "IncineratorGlove",
                "Sharanga",
                "VoidTablet",
                "SporeClub",
                "GoldBattery",
                "GoldGlaive",
                "PlatinumBattery",
                "PlatinumScythe",
                "ChainedPlasma",
                "HardlightGlaive",
                "LashesOfLightning",
                "SectionChiefsScythe",
                "SkywareBattery",
                "PhaseCannon",
                "CryoCannon",
                "HypericeClusterCannon",
                "IcicleImpale",
                "ImperialPike",
                "JeweledGauntlet",
                "PharaohsCane",
                "SpiritGlove",
                "DreamLamp",
                "Traingun",
                "Revolution",
                "PistolShrimp",
                "AncientSteelGreatPickaxe",
                "FrigidPickaxe",
                "MinersPickaxe",
                "OlympianAegis",
                "ZombieHand",
                "SharkPog",
                "Atlantis",
                "SupernovaEmblem",
                "SwallowedPenny",
                "BundleOfSnakes",
                "Lockpick",
                "KeepersBox",
                "SoulOfTheKeeper",
                "WishingStar",
                "JarOfPineapple",
                "FizzleStar",
                "AcidicInjection",
                "LittleWoes",
                "TorchGunMk2",
                "RockingHorse",
                "PintOPunch",
                "FishBomb",
                "Sunbulb"
            });

            tru.Call("AddNeuterItems", this, new[]
            {
                "FoggyClairvoyance",
                "AquaticEclipse",
                "GuideToIllumination",
                "WitchHeart",
                "Shattershine",
                "AmberRing",
                "AmethystRing",
                "DiamondRing",
                "EmeraldRing",
                "RubyRing",
                "SapphireRing",
                "TopazRing",
                "ObsidianEruption",
                "VesperaNanDao",
                "BiomassBarrage",
                "NatureSpell",
                "Poyoyo",
                "BladeNecklace",
                "FrigidJavelin",
                "TwilightBeads",
                "StormSpell",
                "BrachialLance",
                "CursedApple",
                "HeartInAJar",
                "HeartOfTheSea",
                "PrismarineNecklace",
                "PeanutButter",
                "RoyalJelly",
                "GlowJelly",
                "ChallengerRing",
                "RotHeart",
                "GreedierRing"
            });

            tru.Call("AddPluralItems", this, new[]
            {
                "CataclysmSpheres",
                "SubspaceScissors",
                "SnakeEyes",
                "GlowSpores",
                "BerryBombs",
                "SporeBombs",
                "ArclightOrbs",
                "SkywardBlades",
                "TerminatorAcorns",
                "FrigidHourglass",
                "ShoeIce",
                "ShiftingSands",
                "GelWings",
                "FlashsparkBoots",
                "SubspaceBoosters",
                "GildedBladeWings",
                "SteelerWheeler"
            });

            var prefixes = new List<string[]>
            {
                //Accessories
                new[] { "Пробуждённый", "Пробуждённая", "Пробуждённое", "Пробуждённые" },
                new[] { "Всеведущий", "Всеведущая", "Всеведущее", "Всеведущие" },
                new[] { "Скованный", "Скованная", "Скованное", "Скованные" },
                new[] { "Душесвязанный", "Душесвязанная", "Душесвязанное", "Душесвязанные" },
                // Weapons
                new[] { "Истощённый", "Истощённая", "Истощённое", "Истощённые" },
                new[] { "Презренный", "Презренная", "Презренное", "Презренные" },
                new[] { "Могущественный", "Могущественная", "Могущественное", "Могущественные" },
                new[] { "Всемогущий", "Всемогущая", "Всемогущее", "Всемогущие" },
                new[] { "Хтонический", "Хтоническая", "Хтоническое", "Хтонические" }
            };
            tru.Call("RegisterPrefixes", prefixes);

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<NatureWreath>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.NatureWreath.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<VesperaMask>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.VesperaMask.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<FrigidCrown>(), () =>
                "Если надета [i:SOTS/FrigidRobe] леденящая мантия:\n" +
                Language.GetTextValue("Mods.SOTS.Items.FrigidRobe.SetBonus") + "\n" +
                "Если надет [i:SOTS/ShatterShardChestplate] нагрудник ледяного осколка:\n" +
                Language.GetTextValue("Mods.SOTS.Items.ShatterShardChestplate.SetBonus"
            ));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<EarthenHelmet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.EarthenHelmet.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<PatchLeatherHat>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.PatchLeatherHat.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<CursedHood>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.CursedHood.SetBonus", ArmorSetHotKey.GetAssignedKeys().FirstOrDefault() ?? Language.GetTextValue("LegacyMenu.195")));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<VibrantHelmet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.VibrantHelmet.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<ExcavatorHelmet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.ExcavatorHelmet.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<TwilightAssassinsCirclet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.TwilightAssassinsCirclet.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<VoidspaceMask>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.VoidspaceMask.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<FrostArtifactHelmet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.FrostArtifactHelmet.SetBonus"));

            tru.Call("AddArmorSetBonusPreview", ModContent.ItemType<ElementalHelmet>(), () =>
                Language.GetTextValue("Mods.SOTS.Items.ElementalHelmet.SetBonus"));
        }
        #endregion
    }
}
