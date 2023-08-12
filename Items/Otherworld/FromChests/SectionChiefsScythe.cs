using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SOTS.Void;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Items.Otherworld.Furniture;
using SOTS.Items.OreItems;
using SOTS.Projectiles.Otherworld;
using SOTS.Dusts;

namespace SOTS.Items.Otherworld.FromChests
{
	public class SectionChiefsScythe : VoidItem
	{ 	
		public override void SetStaticDefaults()
		{
			this.SetResearchCost(1);
		}
		public override void SafeSetDefaults()
		{
            Item.damage = 48;  
            Item.DamageType = DamageClass.Melee; 
            Item.width = 58;    
            Item.height = 58;  
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;   
            Item.autoReuse = true; 
            Item.knockBack = 3f;
			Item.value = Item.sellPrice(0, 5, 0, 0);
            Item.rare = ItemRarityID.LightPurple;
            Item.UseSound = SoundID.Item71;
			Item.crit = 11;
			if (!Main.dedServ)
			{
				Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/HardlightScytheGlow").Value;
			}
			Item.shoot = ModContent.ProjectileType<ScytheSlash>();
			Item.shootSpeed = 15f;
		}
		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture = Mod.Assets.Request<Texture2D>("Items/Otherworld/FromChests/HardlightScytheGlow").Value;
			Color color = Color.White;
			Vector2 drawOrigin = new Vector2(Terraria.GameContent.TextureAssets.Item[Item.type].Value.Width * 0.5f, Item.height * 0.5f);
			Main.spriteBatch.Draw(texture, new Vector2((float)(Item.Center.X - (int)Main.screenPosition.X), (float)(Item.Center.Y - (int)Main.screenPosition.Y)), null, color, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
		}
		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(hitbox.Location.ToVector2() - new Vector2(5f), hitbox.Width, hitbox.Height, ModContent.DustType<CopyDust4>(), 0, -2, 200, new Color(), 1f);
				dust.velocity *= 0.4f;
				dust.color = new Color(100, 100, 255, 120);
				dust.noGravity = true;
				dust.fadeIn = 0.1f;
				dust.scale *= 1.5f;
			}
		}
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
			knockback *= 3;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
		{
			if(crit)
			{
				BeadPlayer modPlayer = player.GetModPlayer<BeadPlayer>();
				if (crit && Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(player.GetSource_OnHit(target), player.Center.X, player.Center.Y, 0, 0, ModContent.ProjectileType<SoulofRetaliation>(), damage + modPlayer.soulDamage, 1f, player.whoAmI);
				}
			}
		}
		public override int GetVoid(Player player)
		{
			return  10;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1).AddIngredient(ModContent.ItemType<PlatinumScythe>(), 1).AddIngredient(ModContent.ItemType<HardlightAlloy>(), 12).AddTile(ModContent.TileType<HardlightFabricatorTile>()).Register();
		}
	}
}
