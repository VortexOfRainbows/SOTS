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

namespace SOTS.FakePlayer
{
    public abstract class FakePlayerPossessingProjectile : ModProjectile
    {
        public Vector2 ItemLocation;
        public float ItemRotation;
        public FakePlayer FakePlayer = null;
        public Vector2 cursorArea;
        public int Direction = 1;
        public sealed override void SetStaticDefaults()
        {
            FakePlayerHelper.FakePlayerPossessingProjectile.Add(Type);
            Main.projPet[Projectile.type] = true;
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
        }
        public override void ReceiveExtraAI(BinaryReader reader)
        {
            cursorArea.X = reader.ReadSingle();
            cursorArea.Y = reader.ReadSingle();
            ItemLocation.X = reader.ReadSingle();
            ItemLocation.Y = reader.ReadSingle();
            ItemRotation = reader.ReadSingle();
            Direction = reader.ReadInt32();
        }
        public void UpdateItems(Player player)
        {
            FakePlayer.itemLocation = ItemLocation;
            FakePlayer.itemRotation = ItemRotation;
            FakePlayer.WingFrame = (int)(Projectile.ai[1] / 4);
            FakePlayer.SecondPosition = Projectile.position + new Vector2(-5 * Projectile.ai[0], 2);
            Projectile.position += Projectile.velocity;
            FakePlayer.direction = Direction;
            FakePlayer.Position = Projectile.position;
            FakePlayer.Velocity = Projectile.velocity; //this is only used for wing drawing
            FakePlayer.ItemCheckHack(player);
            Direction = FakePlayer.direction;
            if (Main.myPlayer == player.whoAmI)
            {
                ItemLocation = FakePlayer.itemLocation;
                ItemRotation = FakePlayer.itemRotation;
            }
            Projectile.position = FakePlayer.Position;
        }
    }
    public static class FakePlayerHelper
    {
        public static HashSet<int> FakePlayerPossessingProjectile;
        public static int[] FakePlayerItemBlacklist;
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
            Projectile fakePlayer = Main.projectile.Where(x => x.identity == FakeOwnerIdentity).First();
            if(fakePlayer == null) 
                return;
            if(!FakePlayerHelper.FakePlayerPossessingProjectile.Contains(fakePlayer.type) || !fakePlayer.active || fakePlayer.owner != projectile.owner)
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
            if (OwnerOfThisUpdateCycle == -1 || projectile.owner != Main.myPlayer)
            {
                FakeOwnerIdentity = -1;
            }
            else
                FakeOwnerIdentity = OwnerOfThisUpdateCycle;
        }
    }
    public class PlayerInventorySlotsManager
    {
        public static Texture2D locket => ModContent.Request<Texture2D>("SOTS/Items/Celestial/SubspaceLocket").Value;
        public static Texture2D grayscaleLocket => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceLocket").Value;
        public static Texture2D grayLocketBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceInventoryBox").Value;
        public static Texture2D locketBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceInventoryBox").Value;
        public static Texture2D favoritedBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/SubspaceInventoryBoxFavorited").Value;
        public static Texture2D grayFavoritedBox => ModContent.Request<Texture2D>("SOTS/FakePlayer/GraySubspaceInventoryBoxFavorited").Value;
        public static bool FakeBorderDrawCycle = false;
        public static Color SavedInventoryColor;
        public static bool DrawSubspaceSlot(Item item)
        {
            Player player = Main.LocalPlayer;
            bool correctSlot = player.inventory[49] == item;
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
            bool correctItem = item.type == ModContent.ItemType<SubspaceLocket>() && (player.inventory.Contains(item) || player.armor.Contains(item));
            if (DrawSubspaceSlot(item))
            {
                Color color = Color.White;
                Rectangle frame2 = new Rectangle(0, 0, 40, 50);
                Item dummyItem = new Item(ModContent.ItemType<SubspaceLocket>());
                dummyItem.width = 52;
                dummyItem.height = 52;
                ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 52f, ref frame2, out var itemLight, out var finalDrawScale);
                Texture2D textureOfBox = grayLocketBox;
                if((correctItem || FakeModPlayer.ModPlayer(player).foundItem) && !FakeBorderDrawCycle)
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
                dummyItem.headSlot = 50;
                if (!correctItem || FakeBorderDrawCycle)
                {
                    ItemSlot.DrawItem_GetColorAndScale(dummyItem, Main.inventoryScale, ref color, 32f, ref frame2, out var itemLight2, out var finalDrawScale2);
                    spriteBatch.Draw(FakeModPlayer.ModPlayer(player).foundItem ? locket : grayscaleLocket, position, null, InventoryBoxStandard * 0.5f, 0f, grayscaleLocket.Size() / 2, finalDrawScale2 * 0.85f, SpriteEffects.None, 0f);
                }
            }
        }
    }
}