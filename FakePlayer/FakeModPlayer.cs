using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.FakePlayer
{
    public class TesseractMinionData
    {
        public bool FoundValidItem;
        public int ChargeFrames;
        public bool AltFunctionUse;
        public TesseractMinionData()
        {
            Reset();
        }
        public void Reset()
        {
            FoundValidItem = false;
            ChargeFrames = -1;
            AltFunctionUse = false;
        }
    }
    public class FakeModPlayer : ModPlayer
    {
        public override void SetStaticDefaults()
        {

        }
        public TesseractMinionData[] tesseractData = 
            { new TesseractMinionData(), new TesseractMinionData(), 
            new TesseractMinionData(), new TesseractMinionData(), 
            new TesseractMinionData(), new TesseractMinionData(),
            new TesseractMinionData(),new TesseractMinionData(),
            new TesseractMinionData(),new TesseractMinionData() };
        public bool foundItem = false;
        public bool servantActive = false;
        public bool servantIsVanity = false;
        public bool hasHydroFakePlayer = false;
        public static int TesseractPlayerCount(Player player)
        {
            return FakeModPlayer.ModPlayer(player).tesseractPlayerCount;
        }
        public int tesseractPlayerCount => Player.ownedProjectileCounts[ModContent.ProjectileType<TesseractServant>()];
        public static FakeModPlayer ModPlayer(Player player)
        {
            return player.GetModPlayer<FakeModPlayer>();
        }
        public override void PostUpdate()
        {
            if(servantActive)
            {
                int type = ModContent.ProjectileType<SubspaceServant>();
                SOTSPlayer.ModPlayer(Player).runPets(ref Probe, type, 0, 0, false);
            }
            if (hasHydroFakePlayer)
            {
                int type = ModContent.ProjectileType<HydroServant>();
                SOTSPlayer.ModPlayer(Player).runPets(ref Probe2, type, 0, 0, false);
                if(FakePlayer.CheckItemValidityFull(Player, Player.HeldItem, Player.HeldItem, 1))
                {
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        Projectile proj = Main.projectile[i];
                        if (proj.active)
                        {
                            if (proj.ModProjectile is FakePlayerPossessingProjectile fppp && proj.owner == Player.whoAmI)
                            {
                                if (fppp.FakePlayer != null && fppp.FakePlayer.FakePlayerType == 1)
                                {
                                    Vector2 fromOwnerToMe = fppp.FakePlayer.Position - Player.position;
                                    int newDirection = Math.Sign(fromOwnerToMe.X);
                                    if (newDirection != 0)
                                        Player.direction = newDirection;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public override bool CanUseItem(Item item)
        {
            if(hasHydroFakePlayer && FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
            {
                bool isHydroPlayerUsingAnItem = FakePlayer.CheckItemValidityFull(Player, item, item, 1);
                if (isHydroPlayerUsingAnItem)
                {
                    Player.SetDummyItemTime(2); //This is necessary to prevent the owner from switching items mid-use
                    Player.itemTimeMax--;
                    return false;
                }
            }
            return true;
        }
        public override bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, NPC target)
        {
            if (hasHydroFakePlayer && FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
            {
                bool isHydroPlayerUsingAnItem = FakePlayer.CheckItemValidityFull(Player, item, item, 1);
                if (isHydroPlayerUsingAnItem)
                {
                    return false;
                }
            }
            return null;
        }
        public int subspaceServantShader = 0;
        public override void ResetEffects()
        {
            subspaceServantShader = 0;
            servantIsVanity = false;
            servantActive = false;
            hasHydroFakePlayer = false;
            foundItem = false;
        }
        public override void PostUpdateMiscEffects()
        {
            //Main.NewText(tesseractPlayerCount);
            for (int i = 0; i < tesseractData.Length; i++)
            {
                if (i >= tesseractPlayerCount) //Reset states if the fake player is despawned
                {
                    tesseractData[i].Reset();
                }
                else
                    tesseractData[i].FoundValidItem = false;
            }
        }
        public int Probe = -1;
        public int Probe2 = -1;
        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (FakePlayerProjectile.OwnerOfThisUpdateCycle != -1)
            {
                Projectile dummyProj = new Projectile();
                dummyProj.SetDefaults(item.shoot);
                if(dummyProj.aiStyle == ProjAIStyleID.Yoyo && Player.ownedProjectileCounts[item.shoot] > 1)
                {
                    return false;
                }
            }
            else
            {
                if (hasHydroFakePlayer && FakePlayerProjectile.OwnerOfThisUpdateCycle == -1)
                {
                    bool isHydroPlayerUsingAnItem = FakePlayer.CheckItemValidityFull(Player, item, item, 1);
                    if (isHydroPlayerUsingAnItem)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}