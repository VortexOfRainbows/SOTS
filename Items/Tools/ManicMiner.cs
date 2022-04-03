using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using SOTS.Void;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SOTS.Items.Tools
{
	public class ManicMiner : VoidItem
	{
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = mod.GetTexture("Items/Tools/ManicMinerGlow");
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Main.itemTexture[item.type].Width * 0.5f, item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(item.Center.X - (int)Main.screenPosition.X), (float)(item.Center.Y - (int)Main.screenPosition.Y) + 2), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Manic Miner");
			Tooltip.SetDefault("Converts void into short-ranged mining lasers");
		}
		public override void SafeSetDefaults()
		{
			item.width = 48;
			item.height = 18;
			item.useTime = 24;
			item.useAnimation = 24;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5;
            item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = null;
			item.autoReuse = true;            
			item.shoot = ModContent.ProjectileType<Projectiles.Earth.ManicMiner>(); 
            item.shootSpeed = 4f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Tools/ManicMinerGlow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -4;
			}
		}
        public override Vector2? HoldoutOrigin()
        {
            return new Vector2(-4, 0);
        }
        public override int GetVoid(Player player)
		{
			return 5;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			position += new Vector2(speedX, speedY).SafeNormalize(Vector2.Zero) * 28;
			return true; 
		}
	}
}