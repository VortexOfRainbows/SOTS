using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Helpers;
using SOTS.NPCs.Town;
using System.Text.Json.Serialization.Metadata;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static SOTS.ItemHelpers;

namespace SOTS.Items.Conduit
{
	public class RiftCookie : ModItem
	{
		private Texture2D Atom => ModContent.Request<Texture2D>("SOTS/Items/Conduit/RiftCookieAtom").Value;
        private Texture2D Trail => ModContent.Request<Texture2D>("SOTS/Items/Conduit/RiftCookieAtomTrail").Value;
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            DrawAtom(position, scale * 0.7f, 0f, true);
            return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
            DrawAtom(Item.Center - Main.screenPosition, scale, rotation, false);
			return false;
		}
        private void DrawAtom(Vector2 position, float scale, float rotation, bool Inventory = false)
        {
            Texture2D Atom = this.Atom;
            Texture2D Trail = this.Trail;
            float alpha = (1f - (Item.alpha / 255f));
            Color color = new Color(110, 100, 130, 0);
            Vector2 drawOrigin = new Vector2(Atom.Width * 0.5f, Atom.Height * 0.5f);
            Vector2 drawOriginTrail = new Vector2(Trail.Width * 0.5f, Trail.Height * 0.5f);
            int TrailLength = 30;
            if (Inventory)
                TrailLength = 15;
            for (int j = TrailLength; j >= 0; j--)
            {
                bool mainDraw = j == 0;
                for (int i = 3; i >= 0; i--)
                {
                    float dist = i != 0 ? (24 * scale) : 0;
                    float newScale = i != 0 ? scale * 0.75f : scale;
                    Vector2 mainOffset = new Vector2(0, -dist).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 4 - j * (240f / TrailLength)) + rotation);
                    mainOffset.X *= 0.5f;
                    mainOffset = mainOffset.RotatedBy(i * MathHelper.TwoPi / 3f + rotation + MathHelper.ToRadians(Main.GameUpdateCount));
                    Vector2 pos = mainOffset + position;
                    if (mainDraw)
                    {
                        for (int k = 0; k < 12; k++)
                        {
                            Vector2 offset = new Vector2(5f * newScale, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 2 + k * 30));
                            Main.spriteBatch.Draw(Atom, pos + offset, null, color * 1.2f * alpha, rotation, drawOrigin, newScale, SpriteEffects.None, 0f);
                        }
                        for (int k = 0; k < 12; k++)
                        {
                            Vector2 offset = new Vector2(3f * newScale, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * -2 + k * 30));
                            Main.spriteBatch.Draw(Atom, pos + offset, null, Color.Lerp(color, Color.Black, 0.8f) * 0.75f * alpha, rotation, drawOrigin, newScale, SpriteEffects.None, 0f);
                        }
                        Main.spriteBatch.Draw(Atom, pos, null, Color.White * 1f, rotation, drawOrigin, newScale, SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Vector2 previous = new Vector2(0, -dist).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 4 - (j + 1) * (240f / TrailLength)) + rotation);
                        previous.X *= 0.5f;
                        previous = previous.RotatedBy(i * MathHelper.TwoPi / 3f + rotation + MathHelper.ToRadians(Main.GameUpdateCount));
                        Vector2 toPreviousPosition = previous - mainOffset;
                        float rot = toPreviousPosition.ToRotation();
                        float scale2 = toPreviousPosition.Length();
                        Main.spriteBatch.Draw(Trail, pos, null, Color.White * (1f - j / (float)TrailLength), rot, drawOriginTrail, newScale * 0.5f * new Vector2(scale2, 1), SpriteEffects.None, 0f);
                    }
                }
            }
        }
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ItemNoGravity[Type] = true;
			this.SetResearchCost(10);
		}
		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 32;
			Item.maxStack = 9999;
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.consumable = true;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.reuseDelay = 10;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
		}
		public override void Update(ref float gravity, ref float maxFallSpeed)
		{
			Vector3 vColor = ColorHelper.VoidAnomaly.ToVector3() * 0.2f;
			Lighting.AddLight(Item.position, vColor);
		}
        public override bool CanUseItem(Player player)
        {
            return !Archaeologist.PlayerOnScreen;
        }
        public override bool? UseItem(Player player)
        {
            SOTSUtils.PlaySound(SoundID.NPCDeath39, player.Center, 0.8f, -1.5f);
            Archaeologist.ForceToNewLocation();
            return true;
        }
        public override bool ConsumeItem(Player player)
        {
            return true;
        }
    }
}