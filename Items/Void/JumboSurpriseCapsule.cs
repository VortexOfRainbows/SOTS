using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Items.AbandonedVillage;
using Microsoft.Xna.Framework;
using SOTS.Items.Slime;
using SOTS.Items.Potions;
using SOTS.Items.DoorItems;
using Terraria.Audio;
using SOTS.Items.Furniture.Functional;

namespace SOTS.Items.Void
{
    public class JumboSurpriseCapsule : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(99);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.Geode);
            Item.Size = new Vector2(18, 24);
            Item.rare = ItemRarityID.Blue;
            Item.shoot = ModContent.ProjectileType<JumboSurpriseCapsuleProj>();
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            HydraulicPressTile.CheckIfInsideHydraulic(Item);
        }
        public void Crush()
        {
            Item.active = false;
            SOTSUtils.PlaySound(new SoundStyle("SOTS/Sounds/Tiles/WoodBreaking"), Item.Center, 0.7f, 0.4f, 0.1f);
            int total = Item.stack;
            int totalDust = 10 + total;
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Item.NewItem(Item.GetSource_Misc("SOTS:Crush"), Item.Center, ModContent.ItemType<BrokenJumboSurpriseCapsule>(), total, false);
                for(int i = 0; i < total; i++)
                {
                    Item.NewItem(Item.GetSource_Misc("SOTS:Crush"), Item.Center, JumboSurpriseCapsuleProj.SpawnRandomCapsuleItem(),
                        1, false);
                }
            }
            for (int i = 0; i < totalDust; ++i)
            {
                Dust d = Dust.NewDustDirect(Item.position, Item.width - 5, Item.height - 5, DustID.Tin);
                d.scale *= 1.2f;
            }
        }
    }
    public class JumboSurpriseCapsuleProj : ModProjectile
    {
        public override string Texture => "SOTS/Items/Void/JumboSurpriseCapsule";
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.Geode);
            Projectile.Size = new Vector2(18, 24);
        }
        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            width = 8;
            height = 8;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac);
        }
        public override void AI()
        {
            if (Main.rand.NextBool(5))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width - 5, Projectile.height - 5, DustID.Tin);
                d.velocity *= 0.5f;
            }
        }
        public static int SpawnRandomCapsuleItem()
        {
            return Main.rand.NextFromList(ItemID.Present, ItemID.GoodieBag, ItemID.Geode, ModContent.ItemType<JumboSurpriseEgg>(),
            Main.rand.NextFromList(ItemID.GingerbreadCookie, ItemID.SugarCookie, ItemID.ChocolateChipCookie, ItemID.Apple, ItemID.ApplePie, ItemID.Penguin, ItemID.Seaweed, ItemID.OldShoe,
            ItemID.Bunny, ItemID.BunnyEars, ItemID.FoxEars, ItemID.CatEars, ItemID.Lemon, ItemID.Peach, ItemID.Pigronata, ItemID.BubbleWand, ModContent.ItemType<PeanutBush>(), ItemID.WoodenCrate, ModContent.ItemType<Peanut>(),
            ModContent.ItemType<Sandwich>(), ModContent.ItemType<Baguette>(), ModContent.ItemType<RoyalJelly>(), ModContent.ItemType<GlowJelly>(), ModContent.ItemType<VeganBurger>(), ModContent.ItemType<Taco>(),
            ModContent.ItemType<DoorPants>(), ModContent.ItemType<BandOfDoor>(), ItemID.IronCrate, ItemID.GoldenCrate, ItemID.WaterWalkingBoots, ItemID.LavaCharm, ItemID.NaturesGift, ItemID.JungleRose, ItemID.PinaColada,
            ItemID.ZapinatorGray, ItemID.BlackCounterweight, ItemID.WoodYoyo, ItemID.Rally, ItemID.IronBroadsword, ItemID.Paintbrush, ItemID.PainterPaintballGun, ItemID.MoneyTrough, ItemID.FieryGreatsword));
        }
        public override void OnKill(int timeLeft)
        {
            SOTSUtils.PlaySound(new SoundStyle("SOTS/Sounds/Tiles/WoodBreaking"), Projectile.Center, 0.7f, 0.4f, 0.1f);
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.Center, ModContent.ItemType<BrokenJumboSurpriseCapsule>(), 1, false);
                Item.NewItem(Projectile.GetSource_DropAsItem(), Projectile.Center, SpawnRandomCapsuleItem(),
                    1, false);
            }
            for (int i = 0; i < 10; ++i)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width - 5, Projectile.height - 5, DustID.Tin);
                d.scale *= 1.2f;
            }
        }
    }
    public class BrokenJumboSurpriseCapsule : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(99);
        }
        public override void SetDefaults()
        {
			Item.CloneDefaults(ItemID.FishingSeaweed);
            Item.Size = new  Vector2(28, 24);
            Item.rare = ItemRarityID.Gray;
        }
    }
}