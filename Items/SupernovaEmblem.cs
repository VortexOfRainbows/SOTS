using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Earth.Glowmoth;
using SOTS.Items.Fragments;
using SOTS.Items.Planetarium.FromChests;
using SOTS.Void;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items
{
	public class SupernovaEmblem : ModItem
    {
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            Texture2D texture = Mod.Assets.Request<Texture2D>("Items/SupernovaEmblemGlow").Value;
            Color color = Color.White;
            Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
            Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
        }
        public override Color? GetAlpha(Color lightColor)
		{
			return null;
		}
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SetDefaults()
		{
			Item.maxStack = 1;
            Item.width = 56;
			Item.height = 40;   
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Lime;
            Item.accessory = true;
		}
		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SOTSPlayer modPlayer = player.sotsPlayer();
			modPlayer.SupernovaEmblem = true;
			player.GetDamage(DamageClass.Melee) += 0.15f;

            VoidPlayer voidPlayer = VoidPlayer.ModPlayer(player);
            voidPlayer.bonusVoidGain += 2;
            voidPlayer.voidMeterMax2 += 50;
            voidPlayer.GainVoidOnHurt += 0.12f;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Ichor] = true;
        }
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient<SupernovaHammer>(1).AddIngredient<SkywareBattery>(1).AddIngredient(ItemID.WarriorEmblem, 1).AddIngredient<DissolvingNether>().AddTile(TileID.TinkerersWorkbench).Register();
		}
	}
}