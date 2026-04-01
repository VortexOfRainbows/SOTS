using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static SOTS.SOTSUtils;


namespace SOTS.Items.Furniture
{
    public abstract class Clock<TDrop> : FurnTile where TDrop : ModItem
    {
        protected override int ItemType => ModContent.ItemType<TDrop>();
        protected override void SetStaticDefaults(TileObjectData t)
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Origin = new Point16(0, 4);
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16, 16, 16 };
            AddMapEntry(MapColor, Language.GetText("ItemName.GrandfatherClock"));
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = 0;
        }
        public override void MouseOver(int i, int j)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.noThrow = 2;
            localPlayer.cursorItemIconEnabled = true;
            localPlayer.cursorItemIconID = ModContent.ItemType<TDrop>();
        }
        public override bool RightClick(int x, int y)
        {
            double time = Main.time;
            if (!Main.dayTime)
                time += 54000.0;

            time = time / 86400.0 * 24.0;
            time = time - 7.5 - 12.0;

            if (time < 0.0)
                time += 24.0;
            if (time >= 24.0)
                time -= 24.0;

            int hours = (int)time;
            int minutes = (int)((time - hours) * 60.0);
            bool use24Hour = ModLoader.TryGetMod("CalamityRuTranslate", out _) && Language.ActiveCulture.Name == "ru-RU";
            string displayText;

            if (use24Hour)
            {
                displayText = $"{hours}:{minutes:00}";
            }
            else
            {
                string period = Language.GetTextValue(hours >= 12 ? "GameUI.TimePastMorning" : "GameUI.TimeAtMorning");
                int displayHours = hours % 12;
                if (displayHours == 0)
                    displayHours = 12;

                displayText = $"{displayHours}:{minutes:00} {period}";
            }

            Main.NewText(Language.GetTextValue("CLI.Time", displayText), 255, 240, 20);
            return true;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                Main.SceneMetrics.HasClock = true;
            }
        }
    }
}