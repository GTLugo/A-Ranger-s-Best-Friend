using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace BestFriend.Content.Items.Weapons;

public class Blank : BaseGun {
  public override string Texture => BestFriend.ASSET_PATH + "Textures/Items/Weapons/Blank";
  public override string Name => "空白";

  public override Dictionary<FireMode, GunStats> Stats { get; } = new() {
    {
      FireMode.Primary, new GunStats {
        damage = 1,
        crit = 0,
        knockback = 0,
        use_time = 0,
        fire_rate = 5,
        full_auto = false,
        ammo_save_chance = 1f,
        converts_bullets = true,
        converted_bullet_type = ProjectileID.ChlorophyteBullet
      }
    }
  };

  public override void SetStaticDefaults() {
    // Tooltip.SetDefault("∞ ranged damage\n\'Blank never loses!\'");
    CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    ItemID.Sets.IsLavaImmuneRegardlessOfRarity[Type] = true;
  }

  public override void SetDefaults() {
    base.SetDefaults();
    Item.width = 40;
    Item.height = 30;
    Item.scale = 0.85f;
    Item.rare = ItemRarityID.Quest;
    Item.value = Item.sellPrice(999, 999, 999, 999);
  }

  public override Vector2? HoldoutOffset() {
    return new Vector2(0, 0);
  }

  public override void ModifyWeaponCrit(Player player, ref float crit) {
    crit = 0;
  }
}