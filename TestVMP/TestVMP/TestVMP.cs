using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;

namespace TestVMP
{
    public class TestVMP : BaseScript
    {
        public TestVMP()
        {
            Tick += OnServerTick;
        }

        private async Task OnServerTick()
        {
            try
            {
                foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
                {
                    if (Game.PlayerPed.Weapons.HasWeapon(weapon)) continue;
                    Game.PlayerPed.Weapons.Give(weapon, 800, false, true);
                }
                await Delay(5000);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
