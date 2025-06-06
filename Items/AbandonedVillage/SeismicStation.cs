using Microsoft.Xna.Framework;
using SOTS.Items.Pyramid;
using SOTS.NPCs.Boss.Polaris.NewPolaris;
using SOTS.NPCs.Boss.Polaris;
using SOTS.Projectiles;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Microsoft.Xna.Framework.Graphics;
using SOTS.NPCs.Boss.Excavator;

namespace SOTS.Items.AbandonedVillage
{
    public class SeismicStation : ModItem
    {
        public override void SetStaticDefaults()
        {
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StoneBlock);
            Item.width = 28;
            Item.height = 36;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<SeismicStationTile>();
			Item.rare = ItemRarityID.Orange;
        }
    }
    public class SeismicStationTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = false;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 36;
			TileObjectData.newTile.CoordinateHeights = [18, 16, 16];
			TileObjectData.addTile(Type);
			DustType = DustID.Iron; 
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(251, 129, 13), name);
		}
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
			SOTSTile.DrawSlopedGlowMask(i, j, Type, ModContent.Request<Texture2D>("SOTS/Items/AbandonedVillage/SeismicStationTileGlow").Value, Color.White, Vector2.Zero, false);
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
			r = 0.99f * 0.25f;
			g = 0.51f * 0.25f;
			b = 0.05f * 0.25f;
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
        public override bool RightClick(int i, int j)
        {
			int left = i - Main.tile[i, j].TileFrameX / 18;
			int top = j - Main.tile[i, j].TileFrameY / 18;
			Main.mouseRightRelease = true;
            Player player = Main.LocalPlayer;
			int type = ModContent.ProjectileType<ConstructFinder>();
            foreach (Projectile proj in Main.projectile)
				if(proj.active && proj.type == type)
					return true;
			if (NPC.AnyNPCs(ModContent.NPCType<Excavator>()))
				return true;
			Projectile.NewProjectile(player.GetSource_TileInteraction(i, j), new Vector2(left * 16 + 24, top * 16 + 8) + new Vector2(0, -32), new Vector2(0, -1), type, 0, 0, Main.myPlayer, 0, -1);
            return true;
		}  
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.cursorItemIconID = ModContent.ItemType<SeismicStation>();
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}
		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}