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
            AddMapEntry(MapColor, Language.GetText("ItemName.Clock"));
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

            const double TimeShift = 7.5 + 12.0;
            time = (time - TimeShift + 24.0) % 24.0;
            int hours = (int)time;
            int minutes = (int)Math.Floor((time - hours) * 60.0);
            bool use24Hour = ModLoader.TryGetMod("CalamityRuTranslate", out _) && Language.ActiveCulture.Name == "ru-RU";
            string displayText;

            if (use24Hour)
            {
                displayText = Language.GetTextValue("Mods.SOTS.ClockTime.24", hours, minutes);
            }
            else
            {
                int displayHour = hours % 12;
                if (displayHour == 0) displayHour = 12;

                string key = hours >= 12 ? "Mods.SOTS.ClockTime.12PM" : "Mods.SOTS.ClockTime.12AM";
                displayText = Language.GetTextValue(key, displayHour, minutes);
            }

            Main.NewText(displayText, 255, 240, 20);
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