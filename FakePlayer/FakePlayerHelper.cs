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

public static class FakePlayerHelper
{
    public static int[] FakePlayerItemBlacklist;
    public static int[] FakePlayerItemWhitelist;
    public static void Initialize()
    {
        FakePlayerItemBlacklist = new int[] { //Items that are disallowed, despite naturally working (because they have various bugs in actual execution)
            ModContent.ItemType<LashesOfLightning>(),
            ModContent.ItemType<SkywardBlades>(),
            ItemID.GolemFist,
            ItemID.Flairon,
            ModContent.ItemType<PhaseCannon>(),
            ModContent.ItemType<HardlightGlaive>(),
            ModContent.ItemType<StarcoreAssaultRifle>(),
            ModContent.ItemType<VibrantPistol>(),
            ModContent.ItemType<SupernovaHammer>(),
            ItemID.MonkStaffT1,
            ModContent.ItemType<FrigidJavelin>(),
            ItemID.Zenith,
            //These are the "Blade" type items, which don't work very well with the Servant
            ModContent.ItemType<DigitalDaito>(),
            ModContent.ItemType<VorpalKnife>(),
            ModContent.ItemType<ToothAche>(),
            ModContent.ItemType<Vertebraeker>(),
            ModContent.ItemType<Pyrocide>(),
            ModContent.ItemType<BetrayersKnife>(),
            ModContent.ItemType<VesperaNanDao>(),
            ModContent.ItemType<Colossus>()        
        };
        FakePlayerItemWhitelist = new int[] { //Items that, despite being utterly useless, I thought were cool for the Servant to use!
            ItemID.LawnMower, 
            ItemID.CarbonGuitar, 
            ItemID.IvyGuitar, 
            ItemID.DrumStick, 
            ItemID.Harp, 
            ItemID.Bell };
    }
}