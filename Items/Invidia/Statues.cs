using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Invidia
{
	public class RuinedStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 30;
			Item.height = 36;
			Item.rare = ItemRarityID.Green;
			Item.createTile = ModContent.TileType<RuinedStatueTile>();
		}
	}	
	public class RuinedStatueTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 5;
			TileObjectData.newTile.DrawYOffset = 0;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 18 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 4, 0);
            TileObjectData.newTile.Origin = new Point16(1, 4);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(237, 255, 193), name);
			MinPick = 110;
			DustType = ModContent.DustType<EvostoneDust>();
			HitSound = SoundID.Item27;
			MineResist = 0.1f;
		}
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return true;
		}
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 2;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<RuinedStatue>());
        }
    }
    public class SerpentStatue : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.width = 30;
            Item.height = 48;
            Item.rare = ItemRarityID.Green;
            Item.createTile = ModContent.TileType<SerpentStatueTile>();
        }
    }
    public class SerpentStatueTile : ModTile
    {
        public static Texture2D glow = null;
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (glow == null)
                glow = ModContent.Request<Texture2D>("SOTS/Items/Invidia/SerpentStatueTileGlow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Color lC = Lighting.GetColor(i, j);
            float fillPercent = SOTSWorld.MoonPhasePercent * SOTSWorld.MoonPhasePercent * 0.6f + 0.4f * SOTSTile.PlanetariumLightingColorMultiplier(i, j) * SOTSWorld.MoonPhasePercent;
            Color color = Color.Lerp(lC, Color.White, fillPercent) * fillPercent;
            color.A = 0;
            int c = SOTS.Config.lowFidelityMode ? 3 : 6;
            int d = SOTS.Config.lowFidelityMode ? 120 : 60;
            float moonDist = .4f + 1.8f * SOTSWorld.MoonPhasePercent;
            for (int a = 0; a < c; a++)
                SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color * 0.23f * fillPercent, new Vector2(moonDist, 0).RotatedBy(MathHelper.ToRadians(SOTSWorld.GlobalCounter + a * d)));
            SOTSTile.DrawSlopedGlowMask(i, j, Type, glow, color, Vector2.Zero);
        }
        public override void SetStaticDefaults()
        {
            Main.tileObsidianKill[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileWaterDeath[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
            TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.Width = 4;
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.DrawYOffset = 0;
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16, 18 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 4, 0);
            TileObjectData.newTile.Origin = new Point16(1, 5);
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1);
            TileObjectData.addTile(Type);
            LocalizedText name = CreateMapEntryName();
            AddMapEntry(new Color(237, 255, 193), name);
            MinPick = 110;
            DustType = ModContent.DustType<EvostoneDust>();
            HitSound = SoundID.Item27;
            MineResist = 0.1f;
        }
        public override bool CanExplode(int i, int j)
        {
            return true;
        }
        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return true;
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 2;
        }
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
            yield return new Item(ModContent.ItemType<SerpentStatue>());
        }
    }
}