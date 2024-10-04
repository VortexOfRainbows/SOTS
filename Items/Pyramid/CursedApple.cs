using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.DataStructures;
using SOTS.Void;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Projectiles.Pyramid.GhostPepper;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Pyramid
{
	public class CursedApple : ModItem
    {
        public string AppropriateNameRightNow => GhostPepper.IsAlternate ? this.GetLocalizedValue("AltDisplayName") : this.GetLocalizedValue("DisplayName");
        public override string Texture => "SOTS/Items/Pyramid/CursedApple";
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
			if(GhostPepper.IsAlternate)
            {
				Texture2D t = ModContent.Request<Texture2D>("SOTS/Items/Pyramid/GoldenApple").Value;
                DrawInInventoryBobbing(t, spriteBatch, Item, position, new Rectangle(0, 0, t.Width, t.Height), Color.White, scale * 0.85f, 0.75f, 0.75f);
            }
            return !GhostPepper.IsAlternate;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if (GhostPepper.IsAlternate)
            {
                DrawInWorldBobbing(ModContent.Request<Texture2D>("SOTS/Items/Pyramid/GoldenApple").Value, spriteBatch, Item, Vector2.Zero, lightColor, ref rotation, ref scale, 0.75f, 0.75f);
            }
            return !GhostPepper.IsAlternate;
        }
        public override void UpdateInventory(Player player)
        {
            SetOverridenName();
        }
        public override void PostUpdate()
        {
            SetOverridenName();
        }
        public void SetOverridenName()
        {
            if (Item.type == ModContent.ItemType<CursedApple>())
                Item.SetNameOverride(AppropriateNameRightNow);
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria")
                {
                    if (line.Name == "Tooltip0")
                    {
                        if (!GhostPepper.IsAlternate)
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.CursedApple.DefaultTooltip");
                        else
                            line.Text = Language.GetTextValue("Mods.SOTS.Items.CursedApple.AltTooltip");
                    }
                }
            }
        }
        public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.maxStack = 1;
			Item.rare = ItemRarityID.LightPurple;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.accessory = true;
			Item.hasVanityEffects = true;
			Item.shopCustomPrice = Item.buyPrice(1, 0, 0, 0);
		}
		public override void EquipFrameEffects(Player player, EquipType type)
		{
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
			modPlayer.petPepper = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
            SetOverridenName();
			SOTSPlayer modPlayer = SOTSPlayer.ModPlayer(player);
            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
			voidPlayer.soulsOnKill += 2;
			//modPlayer.typhonRange = 120;
			if (!hideVisual)
				modPlayer.petPepper = true;
        }
	}
	public class CursedAppleTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new int[] { 16, 18 };
			LocalizedText name = CreateMapEntryName();
			AddMapEntry(new Color(185, 20, 40), name);
			TileObjectData.addTile(Type);
			HitSound = SoundID.Grass;
			MineResist = 0.5f;
			DustType = DustID.Grass;
		}
        public override IEnumerable<Item> GetItemDrops(int i, int j)
        {
			yield return new Item(ModContent.ItemType<CursedApple>());
        }
	}
}