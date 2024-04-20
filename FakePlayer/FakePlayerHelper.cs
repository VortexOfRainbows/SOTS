using SOTS.Items;
using SOTS.Items.ChestItems;
using SOTS.Items.Earth;
using SOTS.Items.Evil;
using SOTS.Items.Invidia;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Items.Permafrost;
using SOTS.Items.Temple;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria.UI;
using SOTS.Items.Celestial;
using static Terraria.ModLoader.PlayerDrawLayer;
using Microsoft.CodeAnalysis;
using SOTS.Items.Tide;
using System;

namespace SOTS.FakePlayer
{
    public abstract class FakePlayerPossessingProjectile : ModProjectile
    {
        public Vector2 ItemLocation;
        public float ItemRotation;
        public FakePlayer FakePlayer = null;
        public Vector2 cursorArea;
        public int Direction = 1;
        //public bool ControlUseItem = false;
        public int itemAnimation = 0;
        public int itemAnimationMax = 0;
        public int itemTime = 0;
        public int itemTimeMax = 0;
        public sealed override void SetStaticDefaults()
        {
            FakePlayerHelper.FakePlayerPossessingProjectile.Add(Type);
            Main.projPet[Projectile.type] = true;
            SafeSetStaticDefaults();
        }
        public virtual void SafeSetStaticDefaults()
        {
            
        }
        public sealed override bool? CanCutTiles() => false;
        public sealed override bool MinionContactDamage() => false;
        public sealed override bool ShouldUpdatePosition() => false;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(cursorArea.X);
            writer.Write(cursorArea.Y);
            writer.Write(ItemLocation.X);
            writer.Write(ItemLocation.Y);
            writer.Write(ItemRotation);
            writer.Write(Direction);
            //writer.Write(ControlUseItem); //I don't think this is needed but I'll re-check later
            writer.Write(itemAnimation);
            writer.Write(itemAnimationMax);
            writer.Write(itemTime);
            writer.Write(itemTimeMax);
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            cursorArea.X = reader.ReadSingle();
            cursorArea.Y = reader.ReadSingle();
            ItemLocation.X = reader.ReadSingle();
            ItemLocation.Y = reader.ReadSingle();
            ItemRotation = reader.ReadSingle();
            Direction = reader.ReadInt32();
            //bool na1 = reader.ReadBoolean(); //I don't think this is needed but I'll re-check later
            itemAnimation = reader.ReadInt32();
            itemAnimationMax = reader.ReadInt32();
            itemTime = reader.ReadInt32();
            itemTimeMax = reader.ReadInt32();
        }
        public void UpdateItems(Player player)
        {
            FakePlayer.UniqueMouseX = Main.mouseX;
            FakePlayer.UniqueMouseY = Main.mouseY;

            //FakePlayer.controlUseItem = ControlUseItem;
            FakePlayer.itemLocation = ItemLocation;
            FakePlayer.itemRotation = ItemRotation;
            FakePlayer.WingFrame = (int)(Projectile.ai[1] / 4);
            FakePlayer.SecondPosition = Projectile.position + new Vector2(-5 * Projectile.ai[0], 2);
            Projectile.position += Projectile.velocity;
            Projectile.Center = new Vector2(MathHelper.Clamp(Projectile.Center.X, 160f, (float)(Main.maxTilesX * 16 - 160)), MathHelper.Clamp(Projectile.Center.Y, 160f, (float)(Main.maxTilesY * 16 - 160)));
            if (FakePlayer.BonusItemAnimationTime <= 0 || FakePlayer.itemAnimation > 0)
                FakePlayer.direction = Direction;
            FakePlayer.Position = Projectile.position;
            FakePlayer.Velocity = Projectile.velocity; //this is only used for wing drawing

            if(FakePlayer.itemTimeMax != itemTimeMax || FakePlayer.itemAnimationMax != itemAnimationMax || FakePlayer.itemTime != itemTime || (FakePlayer.itemAnimation != itemAnimation && FakePlayer.itemAnimation + 1 != itemAnimation && itemAnimation != 0))
            {
                FakePlayer.itemAnimation = itemAnimation;
                FakePlayer.itemAnimationMax = itemAnimationMax;
                FakePlayer.itemTime = itemTime;
                FakePlayer.itemTimeMax = itemTimeMax;
            }

            FakePlayer.ItemCheckHack(player);
            if (!FakePlayer.SkipDrawing)
                Direction = FakePlayer.direction;
            if (Main.myPlayer == player.whoAmI)
            {
                //ControlUseItem = FakePlayer.controlUseItem;
            }
            ItemLocation = FakePlayer.itemLocation;
            ItemRotation = FakePlayer.itemRotation;
            itemAnimation = FakePlayer.itemAnimation;
            itemAnimationMax = FakePlayer.itemAnimationMax;
            itemTime = FakePlayer.itemTime;
            itemTimeMax = FakePlayer.itemTimeMax;
            Projectile.position = FakePlayer.Position;
        }
    }
    public static class FakePlayerHelper
    {
        public static HashSet<int> FakePlayerPossessingProjectile;
        public static int[] FakePlayerItemBlacklist;
        public static HashSet<int> HydroPlayerItemBlacklist;
        public static HashSet<int> CloseRangeItemsForFakePlayer;
        public static int[] FakePlayerItemWhitelist;
        public static void Initialize()
        {
            FakePlayerPossessingProjectile = new HashSet<int>();
            FakePlayerItemBlacklist = new int[] { //Items that are disallowed, despite naturally working (because they have various bugs in actual execution)
                //Will add items into here when needed
            };
            FakePlayerItemWhitelist = new int[] { //Items that, despite being utterly useless, I thought were cool for the Servant to use!
                ItemID.LawnMower,
                ItemID.CarbonGuitar,
                ItemID.IvyGuitar,
                ItemID.DrumStick,
                ItemID.Harp,
                ItemID.Bell 
            };
            HydroPlayerItemBlacklist = new HashSet<int> {
                ModContent.ItemType<VorpalKnife>(),
                ModContent.ItemType<OlympianAxe>(),
                ItemID.LawnMower
            };
            CloseRangeItemsForFakePlayer = new HashSet<int> {
                ItemID.Toxikarp,
                ItemID.SpiritFlame,
                ItemID.LawnMower,
                ItemID.FairyQueenMagicItem,
                ModContent.ItemType<LashesOfLightning>(),
                ModContent.ItemType<SharkPog>()
            };
        }
    }
    public class FakePlayerProjectile : GlobalProjectile
    {
        public static int OwnerOfThisUpdateCycle = -1;
        public static int OwnerOfThisDrawCycle = -1;
        public static bool FullBrightThisDrawCycle = false;
        public static int BeginStartNetUpdate = 0;
        public override bool InstancePerEntity => true;
        public int FakeOwnerIdentity = -1;
        public override void SetDefaults(Projectile entity)
        {
            FakeOwnerIdentity = -1;
        }
        public override void SendExtraAI(Projectile projectile, BitWriter bitWriter, BinaryWriter binaryWriter)
        {
            binaryWriter.Write(FakeOwnerIdentity);
        }
        public override void ReceiveExtraAI(Projectile projectile, BitReader bitReader, BinaryReader binaryReader)
        {
            FakeOwnerIdentity = binaryReader.ReadInt32();
        }
        public override void PostAI(Projectile projectile)
        {
            if (FakeOwnerIdentity == -1)
                return;
            //else
            //    Main.NewText(FakeOwnerIdentity);
            if (BeginStartNetUpdate % 5 == 0 && BeginStartNetUpdate < 20)
            {
                projectile.netUpdate = true;
            }
            BeginStartNetUpdate++;
        }
        public void UpdateFakeOwner(Projectile projectile)
        {
            if (FakeOwnerIdentity == -1)
                return;
            projectile.ownerHitCheck = false; //Projectiles owned by fake players should not need collision to hit NPCs
            Projectile fakePlayer = Main.projectile.Where(x => x.identity == FakeOwnerIdentity).First();
            if(fakePlayer == null)
            {
                return;
            }
            bool killMe = !FakePlayerHelper.FakePlayerPossessingProjectile.Contains(fakePlayer.type) || !fakePlayer.active || fakePlayer.owner != projectile.owner;
            if (fakePlayer.ModProjectile is FakePlayerPossessingProjectile fppp)
            {
                if (fppp.FakePlayer != null && fppp.FakePlayer.KillMyOwnedProjectiles)
                {
                    killMe = true;
                }
            }
            if (killMe)
            {
                FakeOwnerIdentity = -1; //This adds it back to the normal update queue... However, this causes bugs... Therefore I have decided to make it... (next line)
                projectile.active = false;
            }
        }
        public FakePlayerPossessingProjectile WhoOwnsMe(Projectile projectile)
        {
            if (FakeOwnerIdentity == -1)
                return null;
            Projectile fakePlayer = Main.projectile.Where(x => x.identity == FakeOwnerIdentity).First();
            if (fakePlayer == null) 
                return null;
            if (FakePlayerHelper.FakePlayerPossessingProjectile.Contains(fakePlayer.type) && fakePlayer.active)
            {
                if (fakePlayer.ModProjectile is FakePlayerPossessingProjectile fppp)
                {
                    return fppp;
                }
            }
            return null;
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source) //OnSpawn happens before the netmessage that syncs projectiles is sent, so it should be safe to not run projectile.netUpdate = true; here!
        {
            if (OwnerOfThisUpdateCycle == -1 || projectile.owner != Main.myPlayer || projectile.hostile)
            {
                FakeOwnerIdentity = -1;
            }
            else
            {
                FakeOwnerIdentity = OwnerOfThisUpdateCycle;
            }
        }
    }
    public class PlayerInventorySlotsManager
    {
        public static Texture2D planetariumTextures => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/PlanetarySymbols").Value;
        public static Texture2D tesseractBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/Box").Value;
        public static Texture2D tesseractBoxA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxAlt").Value;
        public static Texture2D tesseractBoxF => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxFavorite").Value;
        public static Texture2D tesseractBoxFA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxFavoriteAlt").Value;
        public static Texture2D tesseractBoxG => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGray").Value;
        public static Texture2D tesseractBoxGA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGrayAlt").Value;
        public static Texture2D tesseractBoxFG => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxFavoriteGray").Value;
        public static Texture2D tesseractBoxFGA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxFavoriteGrayAlt").Value;
        public static Texture2D tesseractBoxGlow => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGlow").Value;
        public static Texture2D tesseractBoxGlowA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGlowAlt").Value;
        public static Texture2D tesseractBoxFGlow => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGlowFavorite").Value;
        public static Texture2D tesseractBoxFGlowA => ModContent.Request<Texture2D>("SOTS/FakePlayer/TesseractUI/BoxGlowFavoriteAlt").Value;
        public static Texture2D locket => ModContent.Request<Texture2D>("SOTS/Items/Celestial/SubspaceLocket").Value;
        public static Texture2D grayscaleLocket => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceLocket").Value;
        public static Texture2D grayLocketBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceInventoryBox").Value;
        public static Texture2D locketBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceInventoryBox").Value;
        public static Texture2D favoritedBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceInventoryBoxFavorited").Value;
        public static Texture2D grayFavoritedBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceInventoryBoxFavorited").Value;
        public static bool FakeBorderDrawCycle = false;
        public static int RealBorderDrawCycleSlot = -1;
        public static Color SavedInventoryColor;
        public static bool SlotIsWithinTesseractSlotRange(Player player, int slot)
        {
            int bonusSlots = Math.Clamp(FakeModPlayer.TesseractPlayerCount(player), 0, 10);
            return slot >= 40 && slot < 40 + bonusSlots;
        }
        public static bool DrawTesseractSlot(Item item, int slot)
        {
            //Main.NewText(slot);
            Player player = Main.LocalPlayer;
            bool correctSlot = slot >= 0 && (player.inventory[slot] == item && SlotIsWithinTesseractSlotRange(player, slot));
            bool correctItem = item.type == ModContent.ItemType<Tesseract>() && (player.inventory.Contains(item) || player.armor.Contains(item));
            if (correctSlot || correctItem)
            {
                return true;
            }
            return false;
        }
        public static bool DrawSubspaceSlot(Item item)
        {
            Player player = Main.LocalPlayer;
            int slot = 49;
            if(FakeModPlayer.TesseractPlayerCount(player) >= 10)
            {
                slot = 39;
            }
            bool correctSlot = player.inventory[slot] == item;
            bool correctItem = item.type == ModContent.ItemType<SubspaceLocket>() && (player.inventory.Contains(item) || player.armor.Contains(item));
            if ((correctSlot && FakeModPlayer.ModPlayer(player).servantActive && !FakeModPlayer.ModPlayer(player).servantIsVanity) || correctItem)
            {
                return true;
            }
            return false;
        }
        public static Color InventoryBoxStandard => new Color(210, 210, 210, 210);
        public static void PreDrawSlots(Item item, SpriteBatch spriteBatch, Vector2 position, Color drawColor)
        {
            Player player = Main.LocalPlayer;
            FakeModPlayer fmPlayer = FakeModPlayer.ModPlayer(player);
            if (DrawSubspaceSlot(item))
            {
                bool correctItem = item.type == ModContent.ItemType<SubspaceLocket>() && (player.inventory.Contains(item) || player.armor.Contains(item));
                Color color = Color.White;
                Rectangle frame2 = new Rectangle(0, 0, 40, 50);
                Item dummyItem = new Item(ModContent.ItemType<SubspaceLocket>());
                dummyItem.width = 52;
                dummyItem.height = 52;
                ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 52f, ref frame2, out var itemLight, out var finalDrawScale);
                Texture2D textureOfBox = grayLocketBox;
                if((correctItem || fmPlayer.foundItem) && !FakeBorderDrawCycle)
                {
                    textureOfBox = locketBox;
                    if (item.favorited)
                        textureOfBox = favoritedBox;
                }
                else
                {
                    if (item.favorited)
                        textureOfBox = grayFavoritedBox;
                }
                spriteBatch.Draw(textureOfBox, position, null, InventoryBoxStandard, 0f, locketBox.Size() / 2, finalDrawScale, SpriteEffects.None, 0f);
                dummyItem.width = 40;
                dummyItem.height = 50;
                if (!correctItem || FakeBorderDrawCycle)
                {
                    ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 32f, ref frame2, out var itemLight2, out var finalDrawScale2);
                    spriteBatch.Draw(fmPlayer.foundItem ? locket : grayscaleLocket, position, null, InventoryBoxStandard * 0.5f, 0f, grayscaleLocket.Size() / 2, finalDrawScale2 * 0.85f, SpriteEffects.None, 0f);
                }
            }
            else if(DrawTesseractSlot(item, PlayerInventorySlotsManager.RealBorderDrawCycleSlot))
            {
                TesseractMinionData data = fmPlayer.tesseractData[PlayerInventorySlotsManager.RealBorderDrawCycleSlot % 10];
                bool foundItem = data.FoundValidItem;
                bool correctItem = item.type == ModContent.ItemType<Tesseract>() && (player.inventory.Contains(item) || player.armor.Contains(item));
                Color color = Color.White;
                Rectangle frame2 = new Rectangle(0, 0, 40, 50);
                Item dummyItem = new Item(ModContent.ItemType<Tesseract>());
                dummyItem.width = 52;
                dummyItem.height = 52;
                ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 52f, ref frame2, out var itemLight, out var finalDrawScale);
                Texture2D textureOfBox = tesseractBoxG;
                if ((correctItem || foundItem) && !FakeBorderDrawCycle)
                {
                    textureOfBox = (data.AltFunctionUse && !correctItem) ? tesseractBoxA : tesseractBox;
                    if (item.favorited)
                        textureOfBox = (data.AltFunctionUse && !correctItem) ? tesseractBoxFA : tesseractBoxF;
                }
                else
                {
                    if (item.favorited)
                        textureOfBox = tesseractBoxFG;
                }
                spriteBatch.Draw(textureOfBox, position, null, InventoryBoxStandard, 0f, locketBox.Size() / 2, finalDrawScale, SpriteEffects.None, 0f);
                if(foundItem && data.ChargeFrames < 0 && !correctItem)
                {
                    Texture2D glowBox;
                    if (item.favorited)
                    {
                        glowBox = (data.AltFunctionUse && !correctItem) ? tesseractBoxFGlowA : tesseractBoxFGlow;
                    }
                    else
                    {
                        glowBox = (data.AltFunctionUse && !correctItem) ? tesseractBoxGlowA : tesseractBoxGlow;
                    }
                    Color alternateColor = Color.Lerp(InventoryBoxStandard, Color.Red, 0.75f);
                    spriteBatch.Draw(glowBox, position, null, alternateColor, 0f, locketBox.Size() / 2, finalDrawScale, SpriteEffects.None, 0f);
                }
                dummyItem.width = 26;
                dummyItem.height = 32;
                if (!correctItem || FakeBorderDrawCycle)
                {
                    Color alternateColor = Color.Lerp(InventoryBoxStandard, new Color(231, 95, 203), 0.75f);
                    ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 48f, ref frame2, out var itemLight2, out var finalDrawScale2);
                    int frameY = (RealBorderDrawCycleSlot) % 10;
                    int height = planetariumTextures.Height / 10;
                    Rectangle frame = new Rectangle(0, height * frameY, planetariumTextures.Width, height);
                    spriteBatch.Draw(planetariumTextures, position, frame, (foundItem ? alternateColor: InventoryBoxStandard) * 0.5f, 0f, new Vector2(planetariumTextures.Width, planetariumTextures.Height / 10) / 2f, finalDrawScale2 * 1f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}