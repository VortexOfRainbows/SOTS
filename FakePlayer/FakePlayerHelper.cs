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

namespace SOTS.FakePlayer
{
    public abstract class FakePlayerPossessingProjectile : ModProjectile
    {
        public FakePlayer FakePlayer = null;
        public sealed override void SetStaticDefaults()
        {
            FakePlayerHelper.FakePlayerPossessingProjectile.Add(Type);
            Main.projPet[Projectile.type] = true;
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
                ItemID.ChargedBlasterCannon
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
        public void UpdateFakeOwner(Projectile projectile)
        {
            if (FakeOwnerIdentity == -1)
                return;
            int whoAmIOfIdentity = Projectile.GetByUUID(projectile.owner, FakeOwnerIdentity);
            if (whoAmIOfIdentity == -1)
            {
                FakeOwnerIdentity = -1;
                return;
            }
            Projectile fakePlayer = Main.projectile[whoAmIOfIdentity];
            if(!FakePlayerHelper.FakePlayerPossessingProjectile.Contains(fakePlayer.type) || !fakePlayer.active || fakePlayer.owner != projectile.owner)
            {
                FakeOwnerIdentity = -1;
            }
        }
        public FakePlayerPossessingProjectile WhoOwnsMe(Projectile projectile)
        {
            if (FakeOwnerIdentity == -1)
                return null;
            int whoAmIOfIdentity = Projectile.GetByUUID(projectile.owner, FakeOwnerIdentity);
            if (whoAmIOfIdentity != -1)
            {
                Projectile fakePlayer = Main.projectile[whoAmIOfIdentity];
                if (FakePlayerHelper.FakePlayerPossessingProjectile.Contains(fakePlayer.type) && fakePlayer.active)
                {
                    if (fakePlayer.ModProjectile is FakePlayerPossessingProjectile fppp)
                    {
                        return fppp;
                    }
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
}