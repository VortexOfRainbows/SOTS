using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using SOTS.Items.Planetarium.FromChests;
using System.Linq;
using System.Collections.Generic;
using SOTS.Items.Pyramid;
using SOTS.Void;
using SOTS.Items.Celestial;
using SOTS.Items.Planetarium;
using SOTS.Items.ChestItems;
using SOTS.Items;
using SOTS.Items.Fragments;
using SOTS.Items.Inferno;
using Terraria.Utilities;
using SOTS.Projectiles.Permafrost;
using SOTS.Items.DoorItems;
using SOTS.Items.Secrets;
using System;
using SOTS.Items.Planetarium.Furniture;
using SOTS.Items.Fishing;
using SOTS.Items.Chaos;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent;
using Terraria.Localization;
using SOTS.Items.Conduit;
using SOTS.Common;
using SOTS.Items.Tide;
using SOTS.FakePlayer;
using Terraria.UI.Chat;
using SOTS.Items.Wings;
using SOTS.Items.Void;
using SOTS.Items.AbandonedVillage;
using SOTS.Items.Permafrost;
using SOTS.Projectiles.Pyramid.GhostPepper;
using SOTS.Helpers;

namespace SOTS
{
	public class PrefixItem : GlobalItem
	{
        public override bool InstancePerEntity => true;
		public int InventorySlotID;
		public int extraVoid;
		public int extraVoidGain;
		public float voidCostMultiplier;
		public PrefixItem()
		{
			InventorySlotID = -1;
			extraVoidGain = 0;
			extraVoid = 0;
			voidCostMultiplier = 1;
		}
		public override GlobalItem Clone(Item item, Item itemClone)
		{
			PrefixItem myClone = (PrefixItem)base.Clone(item, itemClone);
			myClone.voidCostMultiplier = voidCostMultiplier;
			myClone.extraVoidGain = extraVoidGain;
			myClone.extraVoid = extraVoid;
            myClone.InventorySlotID = InventorySlotID;
            return myClone;
		}
		public override int ChoosePrefix(Item item, UnifiedRandom rand)
		{
			return -1;
		}
		public override void UpdateAccessory(Item item, Player player, bool hideVisual)
		{
			if (extraVoid > 0 && (item.prefix == PrefixType<Awakened>() || item.prefix == PrefixType<Omniscient>()))
			{
				player.VoidPlayer().voidMeterMax2 += extraVoid;
			}
			if (extraVoidGain > 0 && (item.prefix == PrefixType<Chained>() || item.prefix == PrefixType<Soulbound>()))
			{
                player.VoidPlayer().bonusVoidGain += extraVoidGain;
			}
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (!item.social && item.prefix > 0)
			{
				int voidTooltip = extraVoid;
				if (extraVoid > 0 && (item.prefix == PrefixType<Awakened>() || item.prefix == PrefixType<Omniscient>()))
				{
					TooltipLine line = new TooltipLine(Mod, "PrefixAwakened", "+" + voidTooltip + Language.GetTextValue("Mods.SOTS.Prefixes.MaxVoid.DisplayName"))
					{
						IsModifier = true
					};
					tooltips.Add(line);
				}
				if (extraVoidGain > 0 && (item.prefix == PrefixType<Chained>() || item.prefix == PrefixType<Soulbound>()))
				{
					voidTooltip = extraVoidGain;
					TooltipLine line = new TooltipLine(Mod, "PrefixAwakened", "+" + voidTooltip + Language.GetTextValue("Mods.SOTS.Prefixes.RegVoid.DisplayName"))
					{
						IsModifier = true
					};
					tooltips.Add(line);
				}
				if (!item.CountsAsClass(DamageClass.Summon) && item.ModItem is VoidItem vItem)
				{
					int voidAmt = vItem.GetVoid(Main.LocalPlayer);
					if (voidAmt != 0 && voidAmt != 1)
                    {
						int intMax = (int)(voidCostMultiplier * voidAmt);
						float mult = intMax / (float)voidAmt;
						int voidCostTooltip = (int)(100f * (mult - 1f));
						if (voidCostTooltip != 0 && (item.prefix == PrefixType<Famished>() || item.prefix == PrefixType<Precarious>() || item.prefix == PrefixType<Potent>() || item.prefix == PrefixType<Omnipotent>() || item.prefix == PrefixType<Chthonic>()))
						{
							string sign = (voidCostTooltip > 0 ? "+" : "");
							Color baseColor = (voidCostTooltip < 0 ? new Color(120, 190, 120) : new Color(190, 120, 120));
							TooltipLine line = new TooltipLine(Mod, "PrefixAwakened", sign + voidCostTooltip + Language.GetTextValue("Mods.SOTS.Prefixes.CosVoid.DisplayName"))
							{
								OverrideColor = baseColor
							};
							tooltips.Add(line);
						}
					}
				}
			}
			if(!SOTSWorld.downedLux && SOTS.ServerConfig.NerfInsignia && (item.type == ItemType<SpiritInsignia>() || item.type == ItemID.EmpressFlightBooster || item.type == ItemType<GildedBladeWings>()))
			{
				TooltipLine line = new TooltipLine(Mod, "Tooltip0", Language.GetTextValue("Mods.SOTS.Common.InsigniaNerf"))
				{
					OverrideColor = ColorHelper.PastelRainbow
				};
				tooltips.Add(line);
			}
		}
		public override void NetSend(Item item, BinaryWriter writer)
		{
			writer.Write(extraVoidGain);
			writer.Write(extraVoid);
			writer.Write(voidCostMultiplier);
		}
		public override void NetReceive(Item item, BinaryReader reader)
		{
			extraVoidGain = reader.ReadInt32();
			extraVoid = reader.ReadInt32();
			voidCostMultiplier = reader.ReadSingle();
		}
		public static void SetInventorySlot(Item item, int slot)
        {
			item.GetGlobalItem<PrefixItem>().InventorySlotID = slot;
        }
    }
	public class SOTSItem : GlobalItem
	{
		public static int[] DarkBlueRarity;
		public static int[] DarkRedRarity;
		public static int[] DarkGrayRarity;
		public static int[] BrightGreenRarity;
		public static int[] DedicatedOrange;
		public static int[] DedicatedBlue;
		public static int[] DedicatedPurpleRed;
		public static int[] DedicatedPastelPink;
		public static int[] DedicatedMinez;
		public static int[] DedicatedRainbow;
		public static int[] DedicatedBlasfah;
		public static int[] DedicatedHeartPlus;
		public static int[] DedicatedTaco;
		public static int[] PiscesFishItems;
		public static void LoadArrays() //called in SOTS.Load()
		{
			DarkBlueRarity = [ItemType<StarlightAlloy>(), ItemType<HardlightAlloy>(), ItemType<OtherworldlyAlloy>(), ItemType<PotGenerator>(), ItemType<PrecariousCluster>(), ItemType<Calculator>(), ItemType<BookOfVirtues>()]; //Dark Blue
			DarkRedRarity = [ItemType<RefractingCrystal>(), ItemType<CursedApple>(), ItemType<RubyKeystone>()]; //Dark Red
			DarkGrayRarity = [ItemType<TaintedKeystoneShard>(), ItemType<TerminalCluster>(), ItemType<TaintedKeystone>(), ItemType<VoidAnomaly>()]; //Very Dark gray
			BrightGreenRarity = [ItemType<DreamLamp>()];

			DedicatedOrange = [ItemType<TerminatorAcorns>(), ItemType<PlasmaCutterButOnAChain>(), ItemType<CoconutGun>()]; //friends
			DedicatedBlue = [ItemType<Calculator>()]; //friends 2
			DedicatedPurpleRed = [ItemType<CursedApple>(), ItemType<ArcStaffMk2>()]; //James
			DedicatedPastelPink = []; //Tris
			DedicatedMinez = [ItemType<DoorPants>(), ItemType<BandOfDoor>()]; //Minez
			DedicatedRainbow = [ItemType<SubspaceLocket>(), ItemType<DreamLamp>()]; //Vortex
			DedicatedBlasfah = [ItemType<Doomstick>(), ItemType<TheBlaspha>(), ItemType<BookOfVirtues>()]; //Blasfah
			DedicatedHeartPlus = [ItemType<DigitalDaito>(), ItemType<Items.Evil.ToothAche>()]; //Heart Plus Up
			DedicatedTaco = [ItemType<Baguette>(), ItemType<Taco>()]; //Coolio/Taco

			PiscesFishItems = [-6, -5, -4, -3, -2, -1, ItemID.AmanitaFungifin, ItemID.Angelfish, ItemID.Batfish, ItemID.BloodyManowar, ItemID.Bonefish, ItemID.BumblebeeTuna, ItemID.Bunnyfish, ItemID.CapnTunabeard, ItemID.Catfish, ItemID.Cloudfish, ItemID.Clownfish, ItemID.Cursedfish, ItemID.DemonicHellfish, ItemID.Derpfish,
			ItemID.Dirtfish, ItemID.DynamiteFish, ItemID.EaterofPlankton, ItemID.FallenStarfish, ItemID.TheFishofCthulu, ItemID.Fishotron, ItemID.Fishron, ItemID.GuideVoodooFish, ItemID.Harpyfish, ItemID.Hungerfish, ItemID.Ichorfish, ItemID.InfectedScabbardfish, ItemID.Jewelfish, ItemID.MirageFish, ItemID.Mudfish,
			ItemID.MutantFlinxfin, ItemID.Pengfish, ItemID.Pixiefish, ItemID.Slimefish, ItemID.Spiderfish, ItemID.TropicalBarracuda, ItemID.TundraTrout, ItemID.UnicornFish, ItemID.Wyverntail, ItemID.ZombieFish, ItemID.ArmoredCavefish, ItemID.AtlanticCod, ItemID.Bass, ItemID.BlueJellyfish, ItemID.ChaosFish, ItemID.CrimsonTigerfish,
			ItemID.Damselfish, ItemID.DoubleCod, ItemID.Ebonkoi, ItemID.FlarefinKoi, ItemID.FrostMinnow, ItemID.GoldenCarp, ItemID.GreenJellyfish, ItemID.Hemopiranha, ItemID.Honeyfin, ItemID.NeonTetra, ItemID.Obsidifish, ItemID.PinkJellyfish, ItemID.PrincessFish, ItemID.Prismite, ItemID.RedSnapper, ItemID.Salmon, ItemID.Shrimp, ItemID.SpecularFish,
			ItemID.Stinkfish, ItemID.Trout, ItemID.Tuna, ItemID.VariegatedLardfish, ItemType<Curgeon>(), ItemType<PhantomFish>(), ItemType<SeaSnake>(), ItemType<TinyPlanetFish>(), ItemID.ScarabFish, ItemID.ScorpioFish, ItemID.Flounder, ItemID.RockLobster ];
		}
		public static Color[] ConvertToSingleColor(Texture2D texture, Color color)
		{
			int width = texture.Width;
			int height = texture.Height;
			Color[] data = new Color[width * height];
			texture.GetData(data);
			for (int i = 0; i < width * height; i++)
			{
				if (data[i].A >= 255)
					data[i] = color;
			}
			return data;
		}
		public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (item.type == ItemType<Items.Earth.Glowmoth.GlowmothBag>() || item.type == ItemType<Items.Slime.PinkyBag>() || item.type == ItemType<TheAdvisorBossBag>() || item.type == ItemType<CurseBag>() || item.type == ItemType<Items.Permafrost.PolarisBossBag>() || item.type == ItemType<SubspaceBag>() || item.type == ItemType<LuxBag>())
			{
				float alphaMult = 1f;
				if (item.type == ItemType<LuxBag>())
					alphaMult = 0.2f;
				Texture2D texture = TextureAssets.Item[item.type].Value;
				Rectangle frame;

				if (Main.itemAnimations[item.type] != null)
				{
					// In case this item is animated, this picks the correct frame
					frame = Main.itemAnimations[item.type].GetFrame(texture, Main.itemFrameCounter[whoAmI]);
				}
				else
				{
					frame = texture.Frame();
				}

				Vector2 frameOrigin = frame.Size() / 2f;
				Vector2 offset = new Vector2(item.width / 2 - frameOrigin.X, item.height - frame.Height);
				Vector2 drawPos = item.position - Main.screenPosition + frameOrigin + offset;

				float time = Main.GlobalTimeWrappedHourly;
				float timer = item.timeSinceItemSpawned / 240f + time * 0.04f;

				time %= 4f;
				time /= 2f;

				if (time >= 1f)
				{
					time = 2f - time;
				}

				time = time * 0.5f + 0.5f;

				for (float i = 0f; i < 1f; i += 0.25f)
				{
					float radians = (i + timer) * MathHelper.TwoPi;

					spriteBatch.Draw(texture, drawPos + new Vector2(0f, 8f).RotatedBy(radians) * time, frame, new Color(90, 70, 255, 50) * alphaMult, rotation, frameOrigin, scale, SpriteEffects.None, 0);
				}

				for (float i = 0f; i < 1f; i += 0.34f)
				{
					float radians = (i + timer) * MathHelper.TwoPi;

					spriteBatch.Draw(texture, drawPos + new Vector2(0f, 4f).RotatedBy(radians) * time, frame, new Color(140, 120, 255, 77) * alphaMult, rotation, frameOrigin, scale, SpriteEffects.None, 0);
				}
			}
			return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            bool GoldenApple = item.type == ItemType<CursedApple>() && GhostPepper.IsAlternate;
            Color rarityColor = Color.White;
            Color dedicatedColor = Color.White;
            if (DarkBlueRarity.Contains(item.type))
				rarityColor = new Color(0, 130, 235, 255);
			if (DarkRedRarity.Contains(item.type))
                rarityColor = new Color(210, 0, 0);
			if (DarkGrayRarity.Contains(item.type))
                rarityColor = new Color(60, 60, 60);
            if (BrightGreenRarity.Contains(item.type) || GoldenApple)
                rarityColor = (DreamLamp.IsItemForgotten && !GoldenApple) ? new Color(95, 85, 105) : new Color(66, 226, 75);
            if (DedicatedPastelPink.Contains(item.type))
            {
                dedicatedColor = new Color(255, 158, 235);
                rarityColor = new Color(211, 0, 194);
            }
			if (DedicatedOrange.Contains(item.type))
				dedicatedColor = new Color(255, 115, 0);
			if (DedicatedBlue.Contains(item.type))
				dedicatedColor = new Color(0, 130, 235, 255);
			if (DedicatedPurpleRed.Contains(item.type))
				dedicatedColor = ColorHelper.SoulLootingColor;
			if (GoldenApple)
				dedicatedColor = new Color(61, 166, 136); //For my resident hololive fan friend
			if (DedicatedRainbow.Contains(item.type) && (item.type != ItemType<DreamLamp>() || !DreamLamp.IsItemForgotten))
				dedicatedColor = ColorHelper.PastelRainbow;
			if (DedicatedMinez.Contains(item.type))
				dedicatedColor = new Color(255, 153, 51);
			if (DedicatedBlasfah.Contains(item.type))
				dedicatedColor = new Color(90, 12, 240);
			if (DedicatedHeartPlus.Contains(item.type))
				dedicatedColor = new Color(255, 123, 123);
			if (DedicatedTaco.Contains(item.type))
				dedicatedColor = new Color(252, 254, 56);
            if (rarityColor != Color.White)
            {
                foreach (TooltipLine line in tooltips)
                {
                    if (line.Mod == "Terraria" && line.Name == "ItemName")
                    {
                        line.OverrideColor = rarityColor;
                        break;
                    }
                }
            }
            if (dedicatedColor != Color.White)
			{
				TooltipLine line = new TooltipLine(Mod, "Dedicated", Language.GetTextValue("Mods.SOTS.Common.Dedicated"));
				line.OverrideColor = dedicatedColor;
				tooltips.Add(line);
			}
			if(!item.IsAir)
            {
                FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(Main.LocalPlayer);
                int validItemSlots = fPlayer.tesseractPlayerCount;
                bool validSlot = item.GetGlobalItem<PrefixItem>().InventorySlotID >= 40 && item.GetGlobalItem<PrefixItem>().InventorySlotID < 40 + validItemSlots;
                int tesseractDataIBelongIn = item.GetGlobalItem<PrefixItem>().InventorySlotID % 10;
                if (validSlot)
                {
                    TesseractMinionData data = fPlayer.tesseractData[tesseractDataIBelongIn];
                    Color c = Color.Lerp(ColorHelper.RubyColor, Color.Red, 0.4f);
                    string aligned = Language.GetTextValue("Mods.SOTS.Common.Solar" + tesseractDataIBelongIn);
                    string text;
                    if (!data.FoundValidItem)
                    {
                        text = Language.GetTextValue("Mods.SOTS.Common.TesseractInvalid", aligned);
                    }
                    else if (data.ChargeFrames < 0)
                    {
                        text = Language.GetTextValue("Mods.SOTS.Common.TesseractAwaiting", aligned);
                    }
                    else
                    {
                        c = Color.Lerp(ColorHelper.PastelRainbow, ColorHelper.AmethystColor, 0.9f);
                        string duration = data.ChargeFrames == 7200 ? Language.GetTextValue("Mods.SOTS.Common.TesseractUsageContinuous") : Language.GetTextValue("Mods.SOTS.Common.TesseractUsageDuration", MathF.Round(data.ChargeFrames / 60f, 3));
                        string primaryOrSecondary = !data.AltFunctionUse ? Language.GetTextValue("Mods.SOTS.Common.TesseractUsagePrimary") : Language.GetTextValue("Mods.SOTS.Common.TesseractUsageSecondary");
                        text = Language.GetTextValue("Mods.SOTS.Common.TesseractUsage", aligned, primaryOrSecondary, duration);
                    }
                    TooltipLine line = new TooltipLine(Mod, "TesseractInventory", text);
                    line.OverrideColor = c;
                    tooltips.Add(line);
					if(data.FoundValidItem)
                    {
                        TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.Mod == "Terraria");
                        if (tt != null)
                        {
                            string[] splitText = tt.Text.Split(' ');
							string originalType = "";
                            if (!item.CountsAsClass(DamageClass.Summon))
                            {
                                if (splitText.Length > 2) //not TRUE damage, must be a damage type
                                {
									int index = 1;
									if (item.CountsAsClass<VoidMelee>() || item.CountsAsClass<VoidRanged>() || item.CountsAsClass<VoidMagic>())
										index = 3;
                                    originalType = splitText[index] + " + ";
                                }
                            }
                            string damageValue = splitText.First();
                            string damageWord = Language.GetTextValue("Mods.SOTS.Common.Damage");
							tt.Text = Language.GetTextValue("Mods.SOTS.Common.VoidSDouble", damageValue, originalType, damageWord);
                        }
                    }
                }
            }
		}
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.Name == "TesseractInventory")
            {
                Color outer = line.OverrideColor ?? line.Color;
                Color inner = Color.White;
                TextSnippet[] snippets = ChatManager.ParseMessage(line.Text, inner).ToArray();
                ChatManager.ConvertNormalSnippets(snippets);
                ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, line.Font, line.Text, new Vector2(line.X, line.Y), outer, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
                int outSnip;
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, snippets, new Vector2(line.X, line.Y), inner, line.Rotation, line.Origin, line.BaseScale, out outSnip, line.MaxWidth);
                return false;
            }
			if(item.type == ModContent.ItemType<WishingStar>() && (line.Name == "ItemName" || line.Name == "Tooltip1") && WishingStar.IsAlternate)
            {
                Color outer = line.OverrideColor ?? line.Color;
                Color inner = Color.Black;
                TextSnippet[] snippets = ChatManager.ParseMessage(line.Text, inner).ToArray();
                ChatManager.ConvertNormalSnippets(snippets);
                ChatManager.DrawColorCodedStringShadow(Main.spriteBatch, line.Font, line.Text, new Vector2(line.X, line.Y), outer, line.Rotation, line.Origin, line.BaseScale, line.MaxWidth, line.Spread);
                int outSnip;
                ChatManager.DrawColorCodedString(Main.spriteBatch, line.Font, snippets, new Vector2(line.X, line.Y), inner, line.Rotation, line.Origin, line.BaseScale, out outSnip, line.MaxWidth);
				return false;
            }
            return true;
        }
        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
			FakeModPlayer fPlayer = FakeModPlayer.ModPlayer(player);
			int validItemSlots = fPlayer.tesseractPlayerCount;
			bool validSlot = item.GetGlobalItem<PrefixItem>().InventorySlotID >= 40 && item.GetGlobalItem<PrefixItem>().InventorySlotID < 40 + validItemSlots;
			int tesseractDataIBelongIn = item.GetGlobalItem<PrefixItem>().InventorySlotID % 10;
            if (!item.IsAir && validSlot)
            {
                TesseractMinionData data = fPlayer.tesseractData[tesseractDataIBelongIn];
				if(data.FoundValidItem)
				{
					float flat = player.GetDamage<VoidSummon>().Flat;
					float baseD = player.GetDamage<VoidSummon>().Base;
					float multi = player.GetDamage<VoidSummon>().Multiplicative;
					float add = player.GetDamage<VoidSummon>().Additive - 1;
					if(!item.CountsAsClass<VoidGeneric>())
					{
						flat += player.GetDamage<VoidGeneric>().Flat;
                        baseD += player.GetDamage<VoidGeneric>().Base;
                        multi += player.GetDamage<VoidGeneric>().Multiplicative - 1;
                        add += player.GetDamage<VoidGeneric>().Additive - 1;
                    }
					if(!item.CountsAsClass(DamageClass.Summon))
                    {
                        flat += player.GetDamage(DamageClass.Summon).Flat;
                        baseD += player.GetDamage(DamageClass.Summon).Base;
                        multi += player.GetDamage(DamageClass.Summon).Multiplicative - 1;
						add += player.GetDamage(DamageClass.Summon).Additive - 1;
                    }
					damage.Flat += flat;
                    damage.Base += baseD;
                    damage *= multi;
                    damage += add;
                }
            }
        }
        public Tile? FindTATile(Player player)
        {
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Tile? bestTile = null;
			for (int i = -9; i < 10; i++)
            {
				for(int j = -9; j < 10; j++)
                {
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].TileFrameX >= 18 && Main.tile[x + i, y + j].TileFrameX < 36 && Main.tile[x + i, y + j].TileFrameY % 36 >= 18)
						type = 1;
					if (tile.TileType == ModContent.TileType<TransmutationAltarTile>() && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = tile;
						}
					}
				}
            }
			return bestTile;
		}
		public Point16 FindTATileIJ(Player player)
		{
			int x = (int)player.Center.X / 16;
			int y = (int)player.Center.Y / 16;
			Vector2 between = new Vector2(100000, 0);
			Point16 bestTile = new Point16(x, y);
			for (int i = -9; i < 10; i++)
			{
				for (int j = -9; j < 10; j++)
				{
					Tile tile = Framing.GetTileSafely(x + i, y + j);
					int type = 0;
					if (Main.tile[x + i, y + j].TileFrameX >= 18 && Main.tile[x + i, y + j].TileFrameX < 36 && Main.tile[x + i, y + j].TileFrameY % 36 >= 18)
						type = 1;
					if (tile.TileType == ModContent.TileType<TransmutationAltarTile>() && type == 1)
					{
						Vector2 newBetween = new Vector2(i * 16, j * 16);
						if (newBetween.Length() < between.Length())
						{
							between = newBetween;
							bestTile = new Point16(x + i, y + j);
						}
					}
				}
			}
			return bestTile;
		}
        public override void OnCreated(Item item, ItemCreationContext context)
        {
			if(context is RecipeItemCreationContext r)
			{
				Recipe recipe = r.Recipe;
				Player player = Main.LocalPlayer;
				if (recipe.requiredTile.Contains(TileID.DemonAltar)|| recipe.requiredTile.Contains(TileType<TransmutationAltarTile>()))
				{
					//Main.NewText("Passed 1");
					Tile? tile2 = FindTATile(player);
					if (tile2 != null)
					{
						Tile tile = (Tile)tile2;
						Point16 ij = FindTATileIJ(player);
						int left = ij.X - tile.TileFrameX / 18;
						int top = ij.Y - tile.TileFrameY / 18;
						int index = GetInstance<TransmutationAltarStorage>().Find(left, top);
						if (index == -1)
						{
							return;
						}
						Item item2 = recipe.createItem;
						//TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[index];
						Projectile projectile = Main.projectile[Projectile.NewProjectile(player.GetSource_TileInteraction(ij.X, ij.Y), player.Center, Vector2.Zero, ProjectileType<DataTransferProj>(), 0, 0, Main.myPlayer, index, 0)];
						DataTransferProj proj = (DataTransferProj)projectile.ModProjectile;
						proj.itemsArray[0] = item2.type;
						proj.itemAmountsArray[0] = item2.stack;
						int amountOfUniqueItems = 0;
						for (int l = 0; l < recipe.requiredItem.Count; l++)
						{
							if (recipe.requiredItem[l].type > ItemID.None)
							{
								amountOfUniqueItems++;
							}
							else
								break;
						}
						for (int i = 0; i < (amountOfUniqueItems < 20 ? amountOfUniqueItems : 19); i++)
						{
							//Main.NewText("Passed 2: " + i);
							int itemType = recipe.requiredItem[i].type;
							int itemStack = recipe.requiredItem[i].stack;
							//int itemFrames = Terraria.GameContent.TextureAssets.Item[itemType].Value.Height / recipe.requiredItem[i].height;
							proj.itemsArray[i + 1] = itemType;
							proj.itemAmountsArray[i + 1] = itemStack;
						}
						for (int i = amountOfUniqueItems; i < 19; i++)
						{
							proj.itemsArray[i + 1] = 0;
							proj.itemAmountsArray[i + 1] = 0;
						}
						projectile.netUpdate = true;
						//Main.NewText("I am Netmode: " + Main.netMode);
					}
				}
			}
		}
        public override bool CanUseItem(Item item, Player player)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			if (player.HasAmmo(item))
			{
				int polarCannons = modPlayer.polarCannons;
				if ((item.CountsAsClass(DamageClass.Ranged) || item.CountsAsClass(DamageClass.Melee)) && polarCannons > 0 && (!item.autoReuse || player.ownedProjectileCounts[ProjectileType<MiniPolarisCannon>()] <= 0))
				{
					int time = item.useTime;
					if (item.shoot == ProjectileID.None)
						time = item.useAnimation;
					if (item.autoReuse || item.channel)
						time = -2;
					if(player.ownedProjectileCounts[ProjectileType<MiniPolarisCannon>()] == 0)
						for (int i = 0; i < polarCannons; i++)
						{
							Projectile.NewProjectile(player.GetSource_Misc("SOTS:SetBonus_FrostArtifact"), player.Center, Vector2.Zero, ProjectileType<MiniPolarisCannon>(), item.damage, item.knockBack, player.whoAmI, time, item.shoot != ProjectileID.None ? item.useTime : item.useAnimation);
						}
				}
			}
			if (modPlayer.EndothermicAfterburner && item.CountsAsClass(DamageClass.Melee) && !item.noMelee)
			{
				Vector2 offset = new Vector2(24 * -player.direction, 0);
				float mult = item.useAnimation / 70f;
				if (mult > 1)
					mult = 1;
				if(Math.Abs(player.velocity.X) < 9f)
					player.velocity.X += player.direction * 7f * mult;
				Projectile.NewProjectile(player.GetSource_Misc("SOTS:Accessory_EndothermicAfterburner"), player.Center + offset, Vector2.Zero + offset * 0.16f, ProjectileType<EndoBurst>(), (int)(item.damage * 0.7f), 3f, player.whoAmI);
			}
			if (item.type == ItemType<ConduitChassis>() || TypeIsConduitCore(item.type))
			{
				if (!SOTSPlayer.ModPlayer(player).ConduitBelt)
					return false;
			}
			if(modPlayer.PlasmaShrimp)
			{
				if (Main.myPlayer == player.whoAmI && player.statMana > player.statManaMax2 * 0.4f && item.CountsAsClass(DamageClass.Magic))
				{
					for (int i = 0; i < 1000; i++)
					{
						Projectile shrimp = Main.projectile[i];
						if (shrimp.type == ProjectileType<Projectiles.Tide.PlasmaShrimp>() && shrimp.active && shrimp.owner == player.whoAmI)
						{
							Projectiles.Tide.PlasmaShrimp pShrimp = shrimp.ModProjectile as Projectiles.Tide.PlasmaShrimp;
							pShrimp.FireTowards(Main.MouseWorld, (int)(item.damage * 0.5f) + 1);
							break;
						}
					}
				}
			}
			if(modPlayer.WishingStar && WishingStar.IsAlternate)
			{
				if (player.whoAmI == Main.myPlayer && item.CountsAsClass(DamageClass.Magic) && !item.CountsAsClass(GetInstance<VoidGeneric>()))
				{
					VoidPlayer vPlayer = player.VoidPlayer();
					float cost = item.mana / 10f * vPlayer.voidCost;
					bool hasEnoughVoid = !vPlayer.safetySwitch || vPlayer.voidMeter >= cost;
					if (hasEnoughVoid)
                    {
                        SOTSPlayer.CastWishingStar(player, Main.MouseWorld, item.damage);
						VoidItem.DrainMana(player, cost);
                    }
                }
            }
			return base.CanUseItem(item, player);
        }
        public override bool? UseItem(Item item, Player player)
		{
			return base.UseItem(item, player);
        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
		{
			if (item.type == ItemID.EaterOfWorldsBossBag || item.type == ItemID.BrainOfCthulhuBossBag)
				itemLoot.Add(ItemDropRule.Common(ItemType<PyramidKey>()));
		}
        public override bool OnPickup(Item item, Player player)
        {
            if (SOTSPlayer.ModPlayer(player).RubyRing && (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane))
            {
				SOTSPlayer.IncreaseBuffDurations(player, 240, 0.05f, 540, true); //increases buff duration by 5 seconds + 5% of the remaining buff duration, caps at 10 seconds
			}
            return base.OnPickup(item, player);
        }
		public static bool TypeIsConduitCore(int type)
		{
			return type == ItemType<NatureConduit>() || type == ItemType<EarthenConduit>() || type == ItemType<PermafrostConduit>() || type == ItemType<OtherworldConduit>() || type == ItemType<TidalConduit>() || type == ItemType<EvilConduit>() || type == ItemType<InfernoConduit>() || type == ItemType<ChaosConduit>();
        }
        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
			if (item.type == ItemType<ConduitChassis>() || TypeIsConduitCore(item.type)
                || item.type == ItemType<DissolvingNatureBlock>() || item.type == ItemType<DissolvingEarthBlock>() || item.type == ItemType<DissolvingAuroraBlock>() || item.type == ItemType<DissolvingAetherBlock>()
				|| item.type == ItemType<DissolvingBrillianceBlock>() || item.type == ItemType<DissolvingNetherBlock>() || item.type == ItemType<DissolvingUmbraBlock>() || item.type == ItemType<DissolvingDelugeBlock>())
            {
				if(SOTSPlayer.ModPlayer(player).ConduitBelt)
                {
					grabRange = (int)(grabRange * 2.5f);
                }
            }
        }
        public override void UpdateEquip(Item item, Player player)
        {
			if(item.type == ItemType<GildedBladeWings>() || item.type == ItemType<MachinaBooster>())
            {
                 WingUpdate(-10, player, false);
            }
        }
        public override void UpdateVanity(Item item, Player player)
        {
            if (item.type == ItemType<GildedBladeWings>() || item.type == ItemType<MachinaBooster>())
            {
                WingUpdate(-10, player, false);
            }
        }
        public override bool WingUpdate(int wings, Player player, bool inUse)
        {
			if(wings == EquipLoader.GetEquipSlot(Mod, "MachinaBooster", EquipType.Wings) || wings == EquipLoader.GetEquipSlot(Mod, "GildedBladeWings", EquipType.Wings) || wings == -10)
            {
                MachinaBoosterPlayer MachinaBoosterPlayer = player.GetModPlayer<MachinaBoosterPlayer>();
				if (MachinaBoosterPlayer.creativeFlight)
				{
					if(wings == -10)
						MachinaBoosterPlayer.FlightCounter += 2;
					player.wingFrame = 2;
				}
				else if (player.velocity.Y != 0f && (player.controlJump || (MachinaBoosterPlayer.PlayerWasLastOnASlope < 0)))
                {
                    player.wingFrame = 1;
                    if (inUse)
                    {
                        if (wings != -10)
                        {
                            MachinaBoosterPlayer.FlightCounter += 5;
                            if (SOTSWorld.GlobalCounter % 18 == 0)
                            {
                                SOTSUtils.PlaySound(SoundID.Item32, player.Center, 1.0f, -0.1f, 0.05f);
                            }
                        }
                    }
                    else
                    {
                        MachinaBoosterPlayer.FlightCounter += 1;
                    }
                }
                else
                {
                    player.wingFrame = 0;
                    if (MachinaBoosterPlayer.FlightModeFloat <= 0.02f)
                        MachinaBoosterPlayer.FlightCounter = 0;
                }
				if(wings == -10)
                {
                    MachinaBoosterPlayer.PlayerWasLastOnASlope = player.sloping ? 5 : MachinaBoosterPlayer.PlayerWasLastOnASlope - 1;
                    MachinaBoosterPlayer.FlightModeFloat = MathHelper.Lerp(MachinaBoosterPlayer.FlightModeFloat, player.wingFrame, 0.24f);
                }
                return true;
            }
            return false;
        }
		public static bool CountsForLockpick(Item item)
		{
			int t = item.type;
			return t == ItemID.GoldenKey || t == ItemID.ShadowKey || t == ItemID.CrimsonKey || t == ItemID.FrozenKey || t == ItemID.CorruptionKey || t == ItemID.HallowedKey || t == ItemID.JungleKey || t == ItemID.DungeonDesertKey
				|| t == ItemType<OldKey>() || t == ItemType<StrangeKey>() || t == ItemType<SkywareKey>() || t == ItemType<MeteoriteKey>();
		}
        public override bool ConsumeItem(Item item, Player player)
        {
			if(player.SOTSPlayer().Lockpick)
			{
				if(Main.rand.NextBool(3))
				{
					if(CountsForLockpick(item))
						return false;
				}
			}
            return base.ConsumeItem(item, player);
        }
        public override void OnConsumeItem(Item item, Player player)
        {
            if (player.SOTSPlayer().Lockpick && player.whoAmI == Main.myPlayer)
            {
				if(CountsForLockpick(item))
                {
                    int t = item.type;
					int outcomeType = ItemID.GoldBar;
					if (t == ItemID.ShadowKey || t == ItemID.CorruptionKey)
                        outcomeType = ItemID.DemoniteBar;
					if (t == ItemID.CrimsonKey)
						outcomeType = ItemID.CrimtaneBar;
                    if (t == ItemID.JungleKey)
                        outcomeType = ItemID.ChlorophyteBar;
                    if (t == ItemID.DungeonDesertKey)
                        outcomeType = ItemID.FossilOre;
                    if (t == ItemID.HallowedKey)
                        outcomeType = ItemID.HallowedBar;
                    if (t == ItemID.FrozenKey)
                        outcomeType = ItemType<FrigidBar>();
                    if (t == ItemType<OldKey>())
                        outcomeType = ItemType<AncientSteelBar>();
                    if (t == ItemType<StrangeKey>())
                        outcomeType = ItemType<HardlightAlloy>();
                    if (t == ItemType<SkywareKey>())
                        outcomeType = ItemType<StarlightAlloy>();
                    if (t == ItemType<MeteoriteKey>())
                        outcomeType = ItemType<OtherworldlyAlloy>();
                    player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), outcomeType, Main.rand.Next(1, 4));
					if(t == ItemID.CrimsonKey || t == ItemID.CorruptionKey)
                        player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), ItemType<DissolvingUmbra>(), 1);
                    if (t == ItemID.HallowedKey)
                        player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), ItemType<DissolvingBrilliance>(), 1);
                    if (t == ItemID.FrozenKey)
                        player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), ItemType<DissolvingAurora>(), 1);
                    if (t == ItemID.JungleKey)
                        player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), ItemType<DissolvingNature>(), 1);
                    if (t == ItemID.DungeonDesertKey)
                        player.QuickSpawnItem(player.GetSource_Misc("SOTS:Lockpick"), ItemType<DissolvingEarth>(), 1);
                }
            }
            base.OnConsumeItem(item, player);
        }
    }
	public class DataTransferProj : ModProjectile
	{
		public int[] itemsArray = new int[20];
		public int[] itemAmountsArray = new int[20];
		public override void SendExtraAI(BinaryWriter writer)
		{
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemsArray[i]);
			}
			for (int i = 0; i < 20; i++)
			{
				writer.Write(itemAmountsArray[i]);
			}
			base.SendExtraAI(writer);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
		{
			for (int i = 0; i < 20; i++)
			{
				itemsArray[i] = reader.ReadInt32();
			}
			for (int i = 0; i < 20; i++)
			{
				itemAmountsArray[i] = reader.ReadInt32();
			}
			base.ReceiveExtraAI(reader);
        }
        public static DataTransferProj ModProjectile(Projectile proj)
		{
			return (DataTransferProj)GetModProjectile(proj.type);
		}
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Data Transfer Proj"); //Do you enjoy how all my net sycning is done via projectiles?
		}
		public override void SetDefaults()
		{
			Projectile.alpha = 255;
			Projectile.timeLeft = 24;
			Projectile.friendly = false;
			Projectile.tileCollide = false;
			Projectile.width = 36;
			Projectile.height = 36;
			//Projectile.extraUpdates = 4;
		}
		public override bool? CanCutTiles()
		{
			return false;
		}
        public override bool PreAI()
        {
			if(Projectile.owner == Main.myPlayer)
            {
				Projectile.netUpdate = true;
            }
			return true;
        }
        public override void AI()
		{
			Projectile.alpha = 255;
			if(Projectile.timeLeft < 22)
				Projectile.Kill();
		}
		public bool checkArraySame(int[] arr1, int[] arr2)
        {
			if (arr1.Length != arr2.Length)
				return false;

			bool diff = false;
			for(int i = 0; i < arr1.Length; i++)
            {
				if (arr1[i] != arr2[i])
				{
					diff = true;
					break;
				}
            }

			return !diff;
        }
		public override void OnKill(int timeLeft)
		{
			TransmutationAltarStorage entity = (TransmutationAltarStorage)TileEntity.ByID[(int)Projectile.ai[0]];
			if(Main.netMode != NetmodeID.MultiplayerClient)
            {
				if (!checkArraySame(entity.itemAmountsArray, itemAmountsArray) || !checkArraySame(entity.itemsArray, itemsArray))
				{
					entity.itemAmountsArray = itemAmountsArray;
					entity.itemsArray = itemsArray;
					entity.netUpdate = true;

					Vector2 dynamicAddition = new Vector2(3, 0).RotatedBy(MathHelper.ToRadians(Main.GlobalTimeWrappedHourly * 40));
					int amountOfUniqueItems = 1;
					int totalItems = 0;
					for (int l = 1; l < entity.itemsArray.Length; l++)
					{
						if (entity.itemsArray[l] != 0)
						{
							amountOfUniqueItems++;
							totalItems += entity.itemAmountsArray[l];
						}
					}
					Vector2 pos = new Vector2((float)(entity.Position.X * 16 + 24), (float)(entity.Position.Y * 16 + 24));
					pos.Y -= 80 + dynamicAddition.Y + (totalItems + entity.itemAmountsArray[0]) * 0.5f;
					Projectile.NewProjectile(Projectile.GetSource_FromThis(), pos, Vector2.Zero, ModContent.ProjectileType<UndoParticles>(), 0, 1, Main.myPlayer, entity.Position.X, entity.Position.Y);
				}
            }
		}
	}
	public class ItemUseGlow : GlobalItem
	{
		public Texture2D glowTexture = null;
		public int glowOffsetY = 0;
		public int glowOffsetX = 0;
		public override bool InstancePerEntity => true;
        public override GlobalItem Clone(Item item, Item itemClone)
		{
			ItemUseGlow myClone = (ItemUseGlow)base.Clone(item, itemClone);
			myClone.glowOffsetY = glowOffsetY;
			myClone.glowOffsetX = glowOffsetX;
			return myClone;
        }
    }
	public static class ItemHelpers
	{
		public static HashSet<WormholeRecipe> WormholeRecipes;
		public static void DrawInInventoryBobbing(SpriteBatch spriteBatch, Item item, Vector2 position, Rectangle frame, Color drawColor, float scale, float speedMultiplier = 1f, float sinMult = 1f)
		{
			DrawInInventoryBobbing(TextureAssets.Item[item.type].Value, spriteBatch, item, position, frame, drawColor, scale, speedMultiplier, sinMult);
		}
		public static void DrawInWorldBobbing(SpriteBatch spriteBatch, Item item, Vector2 offset, Color color, ref float rotation, ref float scale, float speedMultiplier = 1f, float sinMult = 1f)
		{
			DrawInWorldBobbing(TextureAssets.Item[item.type].Value, spriteBatch, item, offset, color, ref rotation, ref scale, speedMultiplier, sinMult);
        }
        public static void DrawInInventoryBobbing(Texture2D texture, SpriteBatch spriteBatch, Item item, Vector2 position, Rectangle frame, Color drawColor, float scale, float speedMultiplier = 1f, float sinMult = 1f)
        {
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 sinusoid = new Vector2(0, 3 * sinMult * scale * (float)Math.Cos(speedMultiplier * MathHelper.ToRadians(SOTSWorld.GlobalCounter)));
            float rotation = 14 * (float)Math.Sin(0.5f * MathHelper.ToRadians(SOTSWorld.GlobalCounter) * speedMultiplier);
            spriteBatch.Draw(texture, position + sinusoid, frame, drawColor, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
        }
        public static void DrawInWorldBobbing(Texture2D texture, SpriteBatch spriteBatch, Item item, Vector2 offset, Color color, ref float rotation, ref float scale, float speedMultiplier = 1f, float sinMult = 1f)
        {
            scale *= item.scale;
            Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Vector2 sinusoid = new Vector2(0, sinMult * 6 * scale * (float)Math.Cos(speedMultiplier * 2 * MathHelper.ToRadians(SOTSWorld.GlobalCounter))) + new Vector2(0, -3 * scale);
            rotation = 15 * (float)Math.Sin(MathHelper.ToRadians(SOTSWorld.GlobalCounter) * speedMultiplier);
            spriteBatch.Draw(texture, item.Center + offset + sinusoid - Main.screenPosition, null, color, rotation * MathHelper.Pi / 180f, origin, scale, SpriteEffects.None, 0f);
        }
        public static void InitializeWormholeRecipes()
        {
			WormholeRecipes = new HashSet<WormholeRecipe>() { 
				new WormholeRecipe(ItemType<TwilightGel>(), ItemType<SkipSoul>()), 
				new WormholeRecipe("SOTS:AnyGem", ItemType<SkipShard>()),
				new WormholeRecipe(ItemType<RoyalRubyShard>(), ItemType<TaintedKeystoneShard>()),
				new WormholeRecipe(ItemType<TaintedKeystone>(), ItemType<VoidAnomaly>()),
                new WormholeRecipe(ItemType<Riptide>(), ItemType<Atlantis>()),
                new WormholeRecipe(ItemType<BagOfAmmoGathering>(), ItemType<InfinityPouch>()),
                new WormholeRecipe(ItemType<AlmondMilk>(), ItemType<Taco>()),
				new WormholeRecipe(ItemType<WishingStar>(), ItemType<WishingStar>()),
            };
		}
		public static void ConvertItemUsingWormholeRecipe(Item item, int whoAmI)
        {
			foreach(WormholeRecipe wormRecipe in WormholeRecipes)
            {
				if(wormRecipe.ItemIsAnInput(item.type))
                {
					int newWhoAmI = Item.NewItem(new EntitySource_Misc("SOTS:Wormhole"), item.Hitbox, wormRecipe.ItemIDOutput, item.stack, false, item.prefix);
					Item item2 = Main.item[newWhoAmI];
					GlobalEntityItem gen = item2.GetGlobalItem<GlobalEntityItem>();
					gen.TeleportCounter = 1;
					item.active = false;
					if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, whoAmI);
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, newWhoAmI); //The new item should be shared anyway, but this should help it sync the global param
                    }
                    if (item.type == ItemType<WishingStar>() && item.ModItem is WishingStar star)
                    {
						if(star.MyPlayer != -1)
                        {
                            //if (Main.netMode != NetmodeID.Server)
                            //    Main.NewText("I am here", Color.Gray);
                            //else
                            //    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("I am here"), Color.Orange);
							
							/// The server reaches this exclusively in multiplayer...
                            Player p = Main.player[star.MyPlayer];
							if(p.SOTSPlayer().UniqueVisionNumber % 8 != 7)
								p.SOTSPlayer().ResetVisionID(true);
						}
                    }
                }
            }
        }
		public struct WormholeRecipe
        {
			public string RecipeGroupInput;
			public int ItemIDInput;
			public int ItemIDOutput;
			public void InitializeRecipe()
            {
				if (ItemIDInput != -20)
				{
					Recipe.Create(ItemIDOutput, 1)
						.AddCondition(Language.GetText("Mods.SOTS.Common.VoidAnomalyCrafting"), () => Main.LocalPlayer.SOTSPlayer().VoidAnomaly || Main.LocalPlayer.SOTSPlayer().VMincubator)
						.AddIngredient(ItemIDInput)
						.Register();
				}
				else
				{
					Recipe.Create(ItemIDOutput, 1)
						.AddCondition(Language.GetText("Mods.SOTS.Common.VoidAnomalyCrafting"), () => Main.LocalPlayer.SOTSPlayer().VoidAnomaly || Main.LocalPlayer.SOTSPlayer().VMincubator)
						.AddRecipeGroup(RecipeGroupInput, 1).Register();
				}
			}
			public bool ItemIsAnInput(int itemID)
            {
				List<int> items = new List<int>();
				if(ItemIDInput != -20)
					items.Add(ItemIDInput);
				else
                {
					if(RecipeGroupInput.Equals("SOTS:AnyGem"))
                    {
						items = new List<int> {
							ItemID.Ruby,
							ItemID.Amethyst,
							ItemID.Topaz,
							ItemID.Sapphire,
							ItemID.Emerald,
							ItemID.Diamond,
							ItemID.Amber,
						};
                    }
                }
				return items.Contains(itemID);
            }
			public WormholeRecipe(int ItemInput, int ItemOutput)
            {
				ItemIDInput = ItemInput;
				ItemIDOutput = ItemOutput;
				RecipeGroupInput = "";
				InitializeRecipe();
			}
			public WormholeRecipe(string RecipeGroup, int ItemOutput)
			{
				ItemIDInput = -20;
				ItemIDOutput = ItemOutput;
				RecipeGroupInput = RecipeGroup;
				InitializeRecipe();
			}
		}
    }
    public class AnomalyRarity : ModRarity
    {
        public override Color RarityColor => ColorHelper.VoidAnomaly;
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            return Type;
        }
    }
    public class PastelRainbowRarity : ModRarity
    {
        public override Color RarityColor
		{
			get
			{
				return ColorHelper.PastelRainbow;
            }
		}
        public override int GetPrefixedRarity(int offset, float valueMult)
        {
            return Type;
        }
    }
}