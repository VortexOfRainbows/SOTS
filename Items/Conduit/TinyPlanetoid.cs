using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SOTS.Items.Conduit
{
	[AutoloadEquip(EquipType.Head)]
	public class TinyPlanetoid : ModItem
    {
        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            Color color = new Color(110, 100, 130, 50);
            for (int k = 0; k < 8; k++)
            {
                Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
                Main.spriteBatch.Draw(texture, position + offset, frame, color * 1.2f * (1f - (Item.alpha / 255f)), 0f, origin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, position, frame, Color.White * 1f, 0f, origin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            Texture2D texture = Terraria.GameContent.TextureAssets.Item[Item.type].Value;
            Color color = new Color(110, 100, 130, 50);
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            for (int k = 0; k < 8; k++)
            {
                Vector2 offset = new Vector2(4f, 0).RotatedBy(MathHelper.ToRadians(Main.GameUpdateCount * 3 + k * 45));
                Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition + offset, null, color * 1.2f * (1f - (Item.alpha / 255f)), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            }
            Main.spriteBatch.Draw(texture, Item.Center - Main.screenPosition, null, Color.White * 1f, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
            return false;
        }
        public override void SetStaticDefaults()
        {
            ItemID.Sets.ItemNoGravity[Type] = true;
            this.SetResearchCost(1);
        }
        public override void SetDefaults()
		{
			Item.width = 22;
			Item.height = 22;
            Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ModContent.RarityType<AnomalyRarity>();
			Item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			VoidPlayer vPlayer = player.VoidPlayer();
			vPlayer.voidMeterMax2 += 50;
            player.gravControl = true;
            Lighting.AddLight(player.Center, ColorHelpers.VoidAnomaly.ToVector3() * 1.25f);
        }
        public override void UpdateVanity(Player player)
        {
            Lighting.AddLight(player.Center, ColorHelpers.VoidAnomaly.ToVector3() * 1.25f);
        }
    }
}