using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using SOTS.Items.Pyramid;
using SOTS.Items.Potions;
using SOTS.Items.Flails;
using Terraria.GameContent.ItemDropRules;
using SOTS.Items.Planetarium;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Buffs.MinionBuffs;
using Microsoft.Xna.Framework.Graphics;
using System;
using SOTS.Dusts;

namespace SOTS.Items.Fishing
{
	public class PlanetariumCrate : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(10);
			ItemID.Sets.IsFishingCrate[Type] = true;
		}
		public override void SetDefaults()
		{
			Item.DefaultToPlaceableTile(ModContent.TileType<PlanetariumCrateTile>());
			Item.width = 12; //The hitbox dimensions are intentionally smaller so that it looks nicer when fished up on a bobber
			Item.height = 12;
			Item.maxStack = 9999;
			Item.rare = ItemRarityID.Orange;
			Item.value = Item.sellPrice(0, 1);
			Item.consumable = true;
		}
		public override bool CanRightClick()
		{
			return true;
		}
        public static void ModItemLoot(ItemLoot itemLoot)
        {
            int[] themedDrops = {
                ModContent.ItemType<StrangeKey>(),
                ModContent.ItemType<SkywareKey>(),
                ModContent.ItemType<MeteoriteKey>()
            };
            itemLoot.Add(ItemDropRule.OneFromOptionsNotScalingWithLuck(1, themedDrops));
            IItemDropRule[] oreTypes = {
                ItemDropRule.Common(ModContent.ItemType<StarlightAlloy>(), 1, 10, 20),
                ItemDropRule.Common(ModContent.ItemType<OtherworldlyAlloy>(), 1, 10, 20),
                ItemDropRule.Common(ModContent.ItemType<HardlightAlloy>(), 1, 10, 20),
            };
            itemLoot.Add(new OneFromRulesRule(1, oreTypes));

            IItemDropRule[] potionTypes = {
                ItemDropRule.Common(ItemID.GravitationPotion, 1, 2, 4),
                ItemDropRule.Common(ItemID.FeatherfallPotion, 1, 2, 4),
                ItemDropRule.Common(ItemID.HunterPotion, 1, 2, 4),
                ItemDropRule.Common(ItemID.IronskinPotion, 1, 2, 4),
                ItemDropRule.Common(ModContent.ItemType<AssassinationPotion>(), 1, 2, 4),
                ItemDropRule.Common(ModContent.ItemType<BluefirePotion>(), 1, 2, 4),
            };
            itemLoot.Add(new OneFromRulesRule(6, potionTypes));

            IItemDropRule[] resourceTypes = {
                ItemDropRule.Common(ModContent.ItemType<TwilightShard>(), 1, 5, 10),
                ItemDropRule.Common(ModContent.ItemType<TwilightGel>(), 1, 10, 25),
            };
            itemLoot.Add(new OneFromRulesRule(6, resourceTypes));

            IItemDropRule[] resourcePotions = {
                ItemDropRule.Common(ItemID.GreaterHealingPotion, 1, 5, 15),
                ItemDropRule.Common(ItemID.GreaterManaPotion, 1, 5, 15),
            };
            itemLoot.Add(new OneFromRulesRule(2, resourcePotions));


            IItemDropRule[] highendBait = {
                ItemDropRule.Common(ItemID.JourneymanBait, 1, 4, 8),
                ItemDropRule.Common(ItemID.MasterBait, 1, 3, 7),
            };
            itemLoot.Add(new OneFromRulesRule(2, highendBait));
            itemLoot.Add(ItemDropRule.Common(ItemID.GoldCoin, 4, 6, 15));
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
		{
            ModItemLoot(itemLoot);
		}
	}
	public class PlanetariumCrateTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileTable[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(122, 243, 255), name);
            DustType = ModContent.DustType<AvaritianDust>();
			TileObjectData.addTile(Type);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Tile tile = Main.tile[i, j];
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fishing/PlanetariumCrateTileGlow").Value;
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            for (int k = 0; k < 5; k++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
                SOTSTile.DrawSlopedGlowMask(i, j, tile.TileType, texture, color * alphaMult * 0.8f, offset);
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
    public class OtherworldCrate : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(10);
            ItemID.Sets.IsFishingCrate[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<OtherworldCrateTile>());
            Item.width = 12; //The hitbox dimensions are intentionally smaller so that it looks nicer when fished up on a bobber
            Item.height = 12;
            Item.maxStack = 9999;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.sellPrice(0, 1);
            Item.consumable = true;
        }
        public override bool CanRightClick()
        {
            return true;
        }
        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            PlanetariumCrate.ModItemLoot(itemLoot);
            IItemDropRule[] oreTypes = {
                ItemDropRule.Common(ItemID.CobaltBar, 1, 5, 16),
                ItemDropRule.Common(ItemID.MythrilBar, 1, 5, 16),
                ItemDropRule.Common(ItemID.AdamantiteBar, 1, 5, 16),
                ItemDropRule.Common(ItemID.PalladiumBar, 1, 5, 16),
                ItemDropRule.Common(ItemID.OrichalcumBar, 1, 5, 16),
                ItemDropRule.Common(ItemID.TitaniumBar, 1, 5, 16),
            };
            itemLoot.Add(new OneFromRulesRule(5, oreTypes));
            itemLoot.Add(ItemDropRule.Common(ItemID.Moondial, 20, 1, 1));
        }
    }
    public class OtherworldCrateTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileTable[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(191, 95, 208), name);
            DustType = ModContent.DustType<AvaritianDust>();
            TileObjectData.addTile(Type);
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            float uniquenessCounter = Main.GlobalTimeWrappedHourly * -100 + (i + j) * 5;
            Tile tile = Main.tile[i, j];
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Fishing/OtherworldCrateTileGlow").Value;
            Color color;
            color = WorldGen.paintColor((int)Main.tile[i, j].TileColor) * (100f / 255f);
            color.A = 0;
            float alphaMult = 0.55f + 0.45f * (float)Math.Sin(MathHelper.ToRadians(uniquenessCounter));
            for (int k = 0; k < 5; k++)
            {
                Vector2 offset = new Vector2(Main.rand.NextFloat(-1, 1f), Main.rand.NextFloat(-1, 1f)) * 0.25f * k;
                SOTSTile.DrawSlopedGlowMask(i, j, tile.TileType, texture, color * alphaMult * 0.8f, offset);
            }
        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            return false;
        }
    }
}