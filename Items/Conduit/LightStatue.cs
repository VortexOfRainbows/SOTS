using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Dusts;
using SOTS.Items.Fragments;
using SOTS.Items.Gems;
using SOTS.NPCs.Boss;
using SOTS.Projectiles.Blades;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SOTS.Items.Conduit
{
	public class LightStatue : ModItem
	{
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.CloneDefaults(ItemID.StoneBlock);
			Item.width = 26;
			Item.height = 58;
			Item.rare = ModContent.RarityType<StrangeWhiteRarity>();
			Item.createTile = ModContent.TileType<LightStatueTile>();
		}
        public override void AddRecipes()
        {
            CreateRecipe(1)
                .AddIngredient<DissolvingNihility>(1)
                .AddIngredient(ItemID.Diamond, 7)
                .AddRecipeGroup("SOTS:GoldBar", 10)
                .AddIngredient(ItemID.ArmorStatue)
                .AddTile(TileID.HeavyWorkBench).Register();
        }
    }	
	public class LightStatueTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileObsidianKill[Type] = false;
			Main.tileLighted[Type] = true;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = false;
			Main.tileWaterDeath[Type] = false;
            TileID.Sets.HasOutlines[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style5x4);
			TileObjectData.newTile.LavaPlacement = LiquidPlacement.Allowed;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.Width = 4;
			TileObjectData.newTile.Height = 8;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 16, 16, 16, 16];
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop, 4, 0);
            TileObjectData.newTile.Origin = new Point16(1, 7);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<LightStatueTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(177, 202, 232), name);
            MinPick = 110;
			DustType = DustID.Platinum;
            HitSound = SoundID.Tink;
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
            yield return new Item(ModContent.ItemType<LightStatue>());
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<ConduitCounterTE>().Kill(i, j);
        }
        public sealed override void DrawEffects(int i, int j, SpriteBatch spriteBatch, ref TileDrawInfo drawData)
        {
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
                Main.instance.TilesRenderer.AddSpecialLegacyPoint(i, j);
        }
        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
                DrawDiamonds(i, j, spriteBatch);
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i, j].TileFrameX == 0 && Main.tile[i, j].TileFrameY == 0)
                DrawDiamonds(i, j, spriteBatch);
            return true;
        }
        public static void DrawDiamonds(int i, int j, SpriteBatch spriteBatch)
        {

        }
        public override bool RightClick(int i, int j)
        {
            int left = i - (Main.tile[i, j].TileFrameX / 18) % 4;
            int top = j - (Main.tile[i, j].TileFrameY / 18) % 8;
            int k = ModContent.GetInstance<LightStatueTE>().Find(left, top);
            LightStatueTE entity = (LightStatueTE)TileEntity.ByID[k];
            if(entity.Enabled)
            {
                Main.tile[left, top].TileFrameX = 0;
            }
            else
            {
                Main.tile[left, top].TileFrameX = 72;
            }
            if(Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendTileSquare(Main.myPlayer, left, top, 3);
            Main.NewText(entity.Enabled);
            return true;
        }
        public override void MouseOver(int i, int j)
        {
            Player player = Main.LocalPlayer;
            player.cursorItemIconID = ItemID.Diamond;
            player.noThrow = 2;
            player.cursorItemIconEnabled = true;
        }
        public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings)
        {
            return true;
        }
    }
    public class LightStatueTE : ModTileEntity //Mostly for keeping track of the position of the light statue an whether or not it is enabled. This could be done with the Important Tile system too, but I figured I ought not to.
    {
        public bool Enabled => Main.tile[Position.X, Position.Y].TileFrameX != 0;
        public override bool IsTileValidForEntity(int x, int y)
        {
            Tile t = Main.tile[x, y];
            return t.HasTile && t.TileType == ModContent.TileType<LightStatueTile>();
        }
        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
        {
            if(Main.netMode != NetmodeID.Server)
                Main.NewText($"(i, j): ({i}, {j}), type: {type}");
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            return Place(i, j);
        }
    }
}