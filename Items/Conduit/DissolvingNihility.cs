using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.ChestItems;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SOTS.Items.Conduit
{
	public class DissolvingNihility : ModItem
    {
        public string AppropriateNameRightNow => UniqueNumber != -1 ? this.GetLocalizedValue("AltDisplayName") : this.GetLocalizedValue("DisplayName");
        public static int UniqueNumber
        {
            get
            {
                SOTSPlayer sPlayer = Main.LocalPlayer.SOTSPlayer();
                int uniqueNum = -1;
                if (sPlayer.UniqueVisionNumber == 15)
                    uniqueNum = 1;
                if (sPlayer.UniqueVisionNumber == 42)
                    uniqueNum = 2;
                if (sPlayer.UniqueVisionNumber == SOTSPlayer.TotalVisionNumber - 1)
                    uniqueNum = 3;
                return uniqueNum;
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            foreach (TooltipLine line in tooltips)
            {
                if (line.Mod == "Terraria" && line.Name == "Tooltip0")
                {
                    if (UniqueNumber == -1)
                        line.Text = Language.GetTextValue("Mods.SOTS.Items.DissolvingNihility.Tooltip");
                    else
                        line.Text = Language.GetTextValue("Mods.SOTS.Items.DissolvingNihility.RareTooltip" + UniqueNumber);
                    break;
                }
            }
        }
        private static readonly Texture2D Atom = ModContent.Request<Texture2D>("SOTS/Items/Conduit/DissolvingNihility", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
        private static Texture2D Trail => SOTSUtils.WhitePixel;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawAtom(Item, position, scale * 0.9f, 0f, true);
            return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
            DrawAtom(Item, Item.Center - Main.screenPosition, scale, rotation, false);
			return false;
		}
        private static void DrawTrail(Vector2 position, float scale, float rotation, bool Inventory = false, bool front = true)
        {
            Vector2 drawOriginTrail = new Vector2(0, Trail.Height * 0.5f);
            int trailCount = 30;
            if (Inventory)
                trailCount = 20;
            float trailLength = 140;
            for (int i = 3; i >= -3; i--)
            {
                int dir = Math.Abs(i) % 2 * 2 -1;
                for (int j = trailCount; j >= 0; j--)
                {
                    float dist = (24 - MathF.Abs(i) * 4) * scale;
                    float currentRotation = MathHelper.WrapAngle(i * MathHelper.TwoPi / 6f + dir * MathHelper.ToRadians(Main.GameUpdateCount - (j / (float)trailCount * trailLength)));
                    if (currentRotation > 0 && !front)
                        continue;
                    else if(currentRotation <= 0 && front)
                        continue;
                    Vector2 mainOffset = new Vector2(dist, 0).RotatedBy(currentRotation);
                    mainOffset.Y *= 0.2f;
                    Vector2 previous = new Vector2(dist, 0).RotatedBy(i * MathHelper.TwoPi / 6f + dir * MathHelper.ToRadians(Main.GameUpdateCount - ((j - 1) / (float)trailCount * trailLength)));
                    previous.Y *= 0.2f;
                    Vector2 pos = mainOffset.RotatedBy(rotation) + position + new Vector2(0, i * 6 * scale).RotatedBy(rotation);
                    Vector2 toPreviousPosition = previous - mainOffset;
                    float rot = toPreviousPosition.ToRotation();
                    float scale2 = toPreviousPosition.Length();
                    Main.spriteBatch.Draw(Trail, pos, null, Color.Black * (1f - j / (float)trailCount), rot + rotation, drawOriginTrail, scale * new Vector2(scale2 + (j == 0 ? 0.5f : 0), 2f), SpriteEffects.None, 0f);
                    Main.spriteBatch.Draw(Trail, pos, null, Color.White * (1f - j / (float)trailCount), rot + rotation, drawOriginTrail, scale * new Vector2(scale2, 1), SpriteEffects.None, 0f);
                }
            }
        }
        private static void DrawAtom(Item item, Vector2 position, float scale, float rotation, bool Inventory = false)
        {
            float sinusoid = MathF.Sin(MathHelper.ToRadians(Main.GameUpdateCount)) * 15;
            float sinusoid2 = MathF.Sin(MathHelper.ToRadians(Main.GameUpdateCount * 0.67f)) * (Inventory ? 2 : 5);
            Vector2 wave = new Vector2(0, sinusoid2 * scale);
            rotation += MathHelper.ToRadians(sinusoid);
            position += wave;
            float alpha = 1f - (item.alpha / 255f);
            Vector2 drawOrigin = new Vector2(Atom.Width * 0.5f, Atom.Height * 0.5f);
            DrawTrail(position, scale, rotation, Inventory, false);
            for (int k = 0; k < 20; k++)
            {
                Vector2 offset = new Vector2(4f * scale, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 18));
                Main.spriteBatch.Draw(Atom, position + offset, null, Color.Black * 0.2f * alpha, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(Atom, position, null, Color.White, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            DrawTrail(position, scale, rotation, Inventory, true);
        }
		public override void SetStaticDefaults()
        {
            ItemID.Sets.ShimmerTransformToItem[Type] = ModContent.ItemType<RiftCookie>();
            ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(10);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<StrangeWhiteRarity>();
			Item.value = Item.sellPrice(0, 0, 0, 0);
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
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
            if (Item.type == ModContent.ItemType<DissolvingNihility>())
                Item.SetNameOverride(AppropriateNameRightNow);
        }
    }
}