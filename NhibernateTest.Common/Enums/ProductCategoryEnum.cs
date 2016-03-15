using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NhibernateTest
{
    public enum ProductCategoryEnum
    {
        /// <summary>
        /// Low Voltage Motors (1)
        /// </summary>
        [Description("Low Voltage Motors")]
        LowVoltageMotors = 1,
        /// <summary>
        /// Worm Gear Speed Reducer (0)
        /// </summary>
        [Description("Worm Gear Speed Reducer")]
        HelicalWormGearSpeedReducer = 0,
        /// <summary>
        /// Helical Gear Speed Reducer (5)
        /// </summary>
        [Description("Helical Gear Speed Reducer")]
        HelicalGearSpeedReducer = 5,
        /// <summary>
        /// Planet Gear Speed Reducer (2)
        /// </summary>
        [Description("Planetary Gear Speed Reducer")]
        PlanetGearSpeedReducer = 2,
        /// <summary>
        /// Peripheral Products (3)
        /// </summary>
        [Description("Factory Automation")]
        PeripheralProducts = 3,
        /// <summary>
        /// Pneumatic Valve (4)
        /// </summary>
        [Description("Pneumatic Valve")]
        PneumaticValve = 4
    }
}