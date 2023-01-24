using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SOTS.Buffs;
using SOTS.Void;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SOTS.Items.Earth.Glowmoth
{
	public class IlluminantAxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Axe");
            Tooltip.SetDefault("Right click to toss the axe for 150% damage");
            ItemID.Sets.ItemsThatAllowRepeatedRightClick[Type] = true;
            this.SetResearchCost(1);
        }
		public override void SetDefaults()
		{
            Item.damage = 16; 
            Item.DamageType = DamageClass.Melee;  
            Item.width = 54;   
            Item.height = 46;
            Item.useTime = 16; 
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Swing;    
            Item.knockBack = 4f;
            Item.value = Item.sellPrice(0, 1, 0, 0);
            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.axe = 12;
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<Projectiles.Earth.IlluminantAxe>();
            Item.shootSpeed = 12f;
            if (!Main.dedServ)
            {
                Item.GetGlobalItem<ItemUseGlow>().glowTexture = Mod.Assets.Request<Texture2D>("Items/Earth/Glowmoth/IlluminantAxeGlow").Value;
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.NextBool(8))
            {
                int num1 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GlowingMushroom, player.direction * 4, -1f, 150, default(Color), 1.3f);
                Main.dust[num1].velocity *= 0.4f;
                Main.dust[num1].scale = 0.8f;
            }
            else if (Main.rand.NextBool(6))
            {
                int num2 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Dusts.CopyDust4>());
                Dust dust = Main.dust[num2];
                dust.color = VoidPlayer.VibrantColorAttempt(Main.rand.NextFloat(360));
                dust.noGravity = true;
                dust.fadeIn = 0.1f;
                dust.scale *= 1.44f;
                dust.velocity += new Vector2(player.direction * 1, -0.25f);
            }
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            damage = (int)(damage * 1.5f);
            velocity *= (SOTSPlayer.ModPlayer(player).attackSpeedMod);
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return player.ownedProjectileCounts[Item.shoot] < 1 && player.altFunctionUse == 2;
        }
        public override float UseAnimationMultiplier(Player player)
        {
            return UseTimeMultiplier(player);
        }
        public override float UseTimeMultiplier(Player player)
        {
            return 1f / (player.altFunctionUse == 2 ? 0.5f : 1);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                Item.noMelee = true;
                Item.noUseGraphic = true;
                Item.UseSound = SoundID.Item19;
                Item.axe = 0;
            }
            else
            {
                Item.noMelee = false;
                Item.noUseGraphic = false;
                Item.UseSound = SoundID.Item1;
                Item.axe = 16;
                return player.ownedProjectileCounts[Item.shoot] < 1;
            }
            return true;
        }
    }
}
